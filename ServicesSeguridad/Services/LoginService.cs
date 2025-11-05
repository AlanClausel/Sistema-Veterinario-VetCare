using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.DomainModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.Services
{
    public static class LoginService
    {
        /// <summary>
        /// Usuario actualmente logueado en el sistema
        /// </summary>
        private static Usuario _usuarioLogueado;

        /// <summary>
        /// Obtiene el usuario actualmente logueado
        /// </summary>
        public static Usuario GetUsuarioLogueado()
        {
            return _usuarioLogueado;
        }

        /// <summary>
        /// Carga los permisos (Familias y Patentes) del usuario
        /// </summary>
        private static void CargarPermisosUsuario(Usuario usuario)
        {
            if (usuario == null) return;

            usuario.Permisos.Clear();

            // Cargar Familias asignadas al usuario (incluye roles)
            var familias = DAL.Implementations.UsuarioFamiliaRepository.Current.SelectAll()
                .Where(uf => uf.idUsuario == usuario.IdUsuario).ToList();

            foreach (var uf in familias)
            {
                var familia = DAL.Implementations.FamiliaRepository.Current.SelectOne(uf.idFamilia);
                if (familia != null)
                {
                    // IMPORTANTE: Cargar recursivamente los hijos de la familia (otras familias y patentes)
                    CargarHijosDeFamilia(familia);

                    usuario.Permisos.Add(familia);
                }
            }

            // Cargar Patentes asignadas directamente al usuario
            var patentes = DAL.Implementations.UsuarioPatenteRepository.Current.SelectAll()
                .Where(up => up.idUsuario == usuario.IdUsuario).ToList();

            foreach (var up in patentes)
            {
                var patente = DAL.Implementations.PatenteRepository.Current.SelectOne(up.idPatente);
                if (patente != null)
                {
                    usuario.Permisos.Add(patente);
                }
            }
        }

        /// <summary>
        /// Carga recursivamente los hijos de una Familia (Familias hijas y Patentes)
        /// NOTA: Este método funciona de forma post-order: primero carga y procesa familias hijas,
        /// luego patentes de esta familia.
        /// </summary>
        private static void CargarHijosDeFamilia(Familia familia)
        {
            if (familia == null) return;

            // PASO 1: Obtener IDs de las familias hijas (sin agregarlas todavía)
            var idsFamiliasHijas = new List<Guid>();

            try
            {
                using (var reader = DAL.Tools.SqlHelper.ExecuteReader(
                    "Familia_Familia_SelectParticular",
                    System.Data.CommandType.StoredProcedure,
                    new System.Data.SqlClient.SqlParameter[] {
                        new System.Data.SqlClient.SqlParameter("@IdFamiliaPadre", familia.IdComponent)
                    }))
                {
                    while (reader.Read())
                    {
                        idsFamiliasHijas.Add(Guid.Parse(reader[0].ToString()));
                    }
                }
            }
            catch
            {
                // Si falla, continuar sin familias hijas
            }

            // PASO 2: Cargar recursivamente cada familia hija ANTES de agregarla
            foreach (var idFamiliaHija in idsFamiliasHijas)
            {
                var familiaHija = DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamiliaHija);
                if (familiaHija != null)
                {
                    // IMPORTANTE: Cargar los hijos de esta familia ANTES de agregarla al padre
                    CargarHijosDeFamilia(familiaHija);

                    // Ahora sí agregarla al padre (ya tiene sus hijos cargados)
                    familia.Add(familiaHija);
                }
            }

            // PASO 3: Cargar Patentes de esta familia
            try
            {
                DAL.Implementations.FamiliaPatenteRepository.Current.GetChildren(familia);
            }
            catch
            {
                // Si falla, continuar sin patentes
            }
        }

        /// <summary>
        /// Autentica un usuario con nombre y contraseña
        /// </summary>
        /// <param name="nombre">Nombre de usuario</param>
        /// <param name="password">Contraseña en texto plano</param>
        /// <returns>Usuario autenticado con sus permisos cargados</returns>
        /// <exception cref="UsuarioNoEncontradoException">Si el usuario no existe</exception>
        /// <exception cref="ContraseñaInvalidaException">Si la contraseña es incorrecta</exception>
        public static Usuario Login(string nombre, string password)
        {
            try
            {
                // Buscar usuario por nombre
                Usuario usuarioDB = DAL.Implementations.UsuarioRepository.Current.SelectOneByName(nombre);

                if (usuarioDB == null)
                {
                    throw new UsuarioNoEncontradoException(nombre);
                }

                // Verificar contraseña hasheada
                string passwordHash = CryptographyService.HashPassword(password);

                if (usuarioDB.Clave != passwordHash)
                {
                    throw new ContraseñaInvalidaException();
                }

                // Cargar permisos del usuario
                CargarPermisosUsuario(usuarioDB);

                // Guardar usuario logueado en memoria
                _usuarioLogueado = usuarioDB;

                return usuarioDB;
            }
            catch (AutenticacionException)
            {
                // Re-lanzar excepciones de autenticación
                throw;
            }
            catch (IntegridadException iex)
            {
                // Re-lanzar excepciones de integridad de datos
                // No convertir a AutenticacionException - es un problema de seguridad diferente
                Bitacora.Current.LogCritical($"Intento de login con datos comprometidos: {nombre}");
                throw;
            }
            catch (Exception ex)
            {
                // Registrar y manejar otras excepciones
                Bitacora.Current.LogError($"Error inesperado en Login para usuario '{nombre}': {ex.GetType().Name} - {ex.Message}");
                ExceptionManager.Current.Handle(ex);
                throw new AutenticacionException("Error al procesar la autenticación", ex);
            }
        }
        public static Patente SelectOnePatente(Guid id)
        {
            return DAL.Implementations.PatenteRepository.Current.SelectOne(id);
        }

        public static Usuario SelectOneUsuario(Guid id)
        {
            return DAL.Implementations.UsuarioRepository.Current.SelectOne(id);
        }

        public static IEnumerable<Patente> SelectAllPatentes()
        {
            return DAL.Implementations.PatenteRepository.Current.SelectAll();
        }

        public static Familia SelectOneFamilia(Guid id)
        {
            return DAL.Implementations.FamiliaRepository.Current.SelectOne(id);
        }

    }
}
