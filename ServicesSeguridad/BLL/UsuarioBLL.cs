using System;
using System.Collections.Generic;
using System.Linq;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.DomainModel.Exceptions;
using ServicesSecurity.Services;

namespace ServicesSecurity.BLL
{
    public static class UsuarioBLL
    {
        /// <summary>
        /// Obtiene todos los usuarios del sistema
        /// </summary>
        public static IEnumerable<Usuario> ObtenerTodosLosUsuarios()
        {
            try
            {
                return ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectAll();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener los usuarios", ex);
            }
        }

        /// <summary>
        /// Obtiene un usuario por ID
        /// </summary>
        public static Usuario ObtenerUsuarioPorId(Guid id)
        {
            try
            {
                var usuario = ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectOne(id);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {id}");
                }
                return usuario;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un usuario por nombre
        /// </summary>
        public static Usuario ObtenerUsuarioPorNombre(string nombre)
        {
            try
            {
                ValidarCampoRequerido(nombre, "Nombre de usuario");

                var usuario = ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectOneByName(nombre);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException(nombre);
                }
                return usuario;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="email">Email del usuario</param>
        /// <param name="password">Contraseña</param>
        /// <param name="idFamiliaRol">ID de la Familia que representa el rol</param>
        /// <param name="idioma">Idioma preferido del usuario (opcional, default: es-AR)</param>
        public static void CrearUsuario(string nombre, string email, string password, Guid idFamiliaRol, string idioma = "es-AR")
        {
            try
            {
                // Validaciones
                ValidarCampoRequerido(nombre, "Nombre de usuario");
                ValidarCampoRequerido(password, "Contraseña");
                ValidarLongitudMinima(password, "Contraseña", 6);

                // Verificar que la familia de rol exista y sea válida
                var familiaRol = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamiliaRol);
                if (familiaRol == null)
                {
                    throw new ValidacionException("El rol seleccionado no existe");
                }
                if (!familiaRol.EsRol)
                {
                    throw new ValidacionException("La familia seleccionada no representa un rol válido");
                }

                // Verificar que el usuario no exista
                var usuarioExistente = ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectOneByName(nombre);
                if (usuarioExistente != null)
                {
                    throw new ValidacionException($"Ya existe un usuario con el nombre '{nombre}'");
                }

                // Crear el usuario
                var nuevoUsuario = new Usuario
                {
                    IdUsuario = Guid.NewGuid(),
                    Nombre = nombre,
                    Email = email,
                    Password = password,
                    Clave = CryptographyService.HashPassword(password),
                    Activo = true,
                    IdiomaPreferido = idioma
                };

                ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.Insert(nuevoUsuario);

                // Asignar el rol (familia) al usuario
                AsignarFamilia(nuevoUsuario.IdUsuario, idFamiliaRol);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="idUsuario">ID del usuario a actualizar</param>
        /// <param name="nombre">Nuevo nombre</param>
        /// <param name="email">Nuevo email</param>
        /// <param name="password">Nueva contraseña (opcional, dejar vacío para no cambiar)</param>
        /// <param name="idFamiliaRol">ID de la nueva Familia de rol (opcional, usar Guid.Empty para no cambiar)</param>
        public static void ActualizarUsuario(Guid idUsuario, string nombre, string email, string password, Guid? idFamiliaRol)
        {
            try
            {
                // Validaciones
                ValidarCampoRequerido(nombre, "Nombre de usuario");

                if (!string.IsNullOrWhiteSpace(password))
                {
                    ValidarLongitudMinima(password, "Contraseña", 6);
                }

                // Obtener usuario actual
                var usuario = ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                // Verificar que el nuevo nombre no esté en uso por otro usuario
                if (usuario.Nombre != nombre)
                {
                    var usuarioConMismoNombre = ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectOneByName(nombre);
                    if (usuarioConMismoNombre != null && usuarioConMismoNombre.IdUsuario != idUsuario)
                    {
                        throw new ValidacionException($"Ya existe otro usuario con el nombre '{nombre}'");
                    }
                }

                // Actualizar datos
                usuario.Nombre = nombre;
                usuario.Email = email;

                // Solo actualizar contraseña si se proporcionó una nueva
                if (!string.IsNullOrWhiteSpace(password))
                {
                    usuario.Password = password;
                    usuario.Clave = CryptographyService.HashPassword(password);
                }

                ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.Update(usuario);

                // Actualizar rol si se proporcionó uno nuevo
                if (idFamiliaRol.HasValue && idFamiliaRol.Value != Guid.Empty)
                {
                    CambiarRol(idUsuario, idFamiliaRol.Value);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Cambia el rol de un usuario (quita el rol anterior y asigna uno nuevo)
        /// </summary>
        public static void CambiarRol(Guid idUsuario, Guid idNuevaFamiliaRol)
        {
            try
            {
                // Verificar que la nueva familia de rol exista y sea válida
                var nuevaFamiliaRol = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(idNuevaFamiliaRol);
                if (nuevaFamiliaRol == null)
                {
                    throw new ValidacionException("El rol seleccionado no existe");
                }
                if (!nuevaFamiliaRol.EsRol)
                {
                    throw new ValidacionException("La familia seleccionada no representa un rol válido");
                }

                // Quitar rol anterior (quitar todas las familias de rol)
                var familiasUsuario = ObtenerFamiliasDelUsuario(idUsuario);
                foreach (var familia in familiasUsuario.Where(f => f.EsRol))
                {
                    QuitarFamilia(idUsuario, familia.IdComponent);
                }

                // Asignar nuevo rol
                AsignarFamilia(idUsuario, idNuevaFamiliaRol);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al cambiar el rol del usuario", ex);
            }
        }

        /// <summary>
        /// Elimina un usuario del sistema
        /// </summary>
        public static void EliminarUsuario(Guid idUsuario)
        {
            try
            {
                var usuario = ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.Delete(idUsuario);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de Familias que representan roles disponibles
        /// Familias con nombres "ROL_Administrador", "ROL_Veterinario", etc.
        /// </summary>
        public static IEnumerable<Familia> ObtenerRolesDisponibles()
        {
            try
            {
                var todasFamilias = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectAll();
                return todasFamilias.Where(f => f.EsRol).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener los roles disponibles", ex);
            }
        }

        /// <summary>
        /// Obtiene el rol (Familia) de un usuario
        /// </summary>
        public static Familia ObtenerRolDelUsuario(Guid idUsuario)
        {
            try
            {
                var usuario = ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }
                return usuario.ObtenerFamiliaRol();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        #region Gestión de Familias

        /// <summary>
        /// Obtiene todas las familias del sistema
        /// </summary>
        public static IEnumerable<Familia> ObtenerTodasLasFamilias()
        {
            try
            {
                return ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectAll();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener las familias", ex);
            }
        }

        /// <summary>
        /// Obtiene las familias asignadas a un usuario
        /// </summary>
        public static IEnumerable<Familia> ObtenerFamiliasDelUsuario(Guid idUsuario)
        {
            try
            {
                var usuario = ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                var usuarioFamilias = ServicesSecurity.DAL.Implementations.UsuarioFamiliaRepository.Current
                    .GetChildren(usuario);

                List<Familia> familias = new List<Familia>();
                foreach (var uf in usuarioFamilias)
                {
                    var familia = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(uf.idFamilia);
                    if (familia != null)
                    {
                        familias.Add(familia);
                    }
                }

                return familias;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Asigna una familia a un usuario
        /// </summary>
        public static void AsignarFamilia(Guid idUsuario, Guid idFamilia)
        {
            try
            {
                var usuarioFamilia = new ServicesSecurity.DomainModel.Security.UsuarioFamilia
                {
                    idUsuario = idUsuario,
                    idFamilia = idFamilia
                };

                ServicesSecurity.DAL.Implementations.UsuarioFamiliaRepository.Current.Insert(usuarioFamilia);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al asignar familia al usuario", ex);
            }
        }

        /// <summary>
        /// Quita una familia de un usuario
        /// </summary>
        public static void QuitarFamilia(Guid idUsuario, Guid idFamilia)
        {
            try
            {
                var usuarioFamilia = new ServicesSecurity.DomainModel.Security.UsuarioFamilia
                {
                    idUsuario = idUsuario,
                    idFamilia = idFamilia
                };

                ServicesSecurity.DAL.Implementations.UsuarioFamiliaRepository.Current.DeleteRelacion(usuarioFamilia);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al quitar familia del usuario", ex);
            }
        }

        #endregion

        #region Gestión de Patentes

        /// <summary>
        /// Obtiene todas las patentes del sistema
        /// </summary>
        public static IEnumerable<Patente> ObtenerTodasLasPatentes()
        {
            try
            {
                return ServicesSecurity.DAL.Implementations.PatenteRepository.Current.SelectAll();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener las patentes", ex);
            }
        }

        /// <summary>
        /// Obtiene las patentes asignadas directamente a un usuario
        /// </summary>
        public static IEnumerable<Patente> ObtenerPatentesDelUsuario(Guid idUsuario)
        {
            try
            {
                var usuario = ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                var usuarioPatentes = ServicesSecurity.DAL.Implementations.UsuarioPatenteRepository.Current
                    .GetChildren(usuario);

                List<Patente> patentes = new List<Patente>();
                foreach (var up in usuarioPatentes)
                {
                    var patente = ServicesSecurity.DAL.Implementations.PatenteRepository.Current.SelectOne(up.idPatente);
                    if (patente != null)
                    {
                        patentes.Add(patente);
                    }
                }

                return patentes;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        /// <summary>
        /// Asigna una patente directamente a un usuario
        /// </summary>
        public static void AsignarPatente(Guid idUsuario, Guid idPatente)
        {
            try
            {
                var usuarioPatente = new ServicesSecurity.DomainModel.Security.UsuarioPatente
                {
                    idUsuario = idUsuario,
                    idPatente = idPatente
                };

                ServicesSecurity.DAL.Implementations.UsuarioPatenteRepository.Current.Insert(usuarioPatente);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al asignar patente al usuario", ex);
            }
        }

        /// <summary>
        /// Quita una patente de un usuario
        /// </summary>
        public static void QuitarPatente(Guid idUsuario, Guid idPatente)
        {
            try
            {
                var usuarioPatente = new ServicesSecurity.DomainModel.Security.UsuarioPatente
                {
                    idUsuario = idUsuario,
                    idPatente = idPatente
                };

                ServicesSecurity.DAL.Implementations.UsuarioPatenteRepository.Current.DeleteRelacion(usuarioPatente);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al quitar patente del usuario", ex);
            }
        }

        #endregion

        #region Gestión de Idioma

        /// <summary>
        /// Cambia el idioma preferido de un usuario
        /// </summary>
        public static void CambiarIdiomaPreferido(Guid idUsuario, string idioma)
        {
            try
            {
                var usuario = ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.SelectOne(idUsuario);
                if (usuario == null)
                {
                    throw new UsuarioNoEncontradoException($"Usuario con ID {idUsuario}");
                }

                usuario.IdiomaPreferido = idioma;
                ServicesSecurity.DAL.Implementations.UsuarioRepository.Current.Update(usuario);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al cambiar idioma preferido del usuario", ex);
            }
        }

        #endregion

        #region Métodos de Validación Privados

        private static void ValidarCampoRequerido(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidacionException($"El campo '{fieldName}' es requerido");
            }
        }

        private static void ValidarLongitudMinima(string value, string fieldName, int minLength)
        {
            if (value != null && value.Length < minLength)
            {
                throw new ValidacionException($"El campo '{fieldName}' debe tener al menos {minLength} caracteres");
            }
        }

        #endregion
    }
}
