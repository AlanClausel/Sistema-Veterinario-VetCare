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
        /// Carga los permisos (Familias y Patentes) del usuario
        /// </summary>
        private static void CargarPermisosUsuario(Usuario usuario)
        {
            if (usuario == null) return;

            usuario.Permisos.Clear();

            // Cargar Familias asignadas al usuario (incluye roles)
            var familias = DAL.Implementations.UsuarioFamiliaRepository.Current.SelectAll()
                .Where(uf => uf.idUsuario == usuario.IdUsuario);

            foreach (var uf in familias)
            {
                var familia = DAL.Implementations.FamiliaRepository.Current.SelectOne(uf.idFamilia);
                if (familia != null)
                {
                    usuario.Permisos.Add(familia);
                }
            }

            // Cargar Patentes asignadas directamente al usuario
            var patentes = DAL.Implementations.UsuarioPatenteRepository.Current.SelectAll()
                .Where(up => up.idUsuario == usuario.IdUsuario);

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
