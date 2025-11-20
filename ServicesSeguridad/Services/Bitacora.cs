using System;
using System.Diagnostics.Tracing;
using ServicesSecurity.DomainModel.Security;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Servicio centralizado de bitácora para registro de eventos, auditoría y excepciones del sistema.
    /// Implementa un sistema dual de registro: archivos de log (LoggerService) y base de datos (tabla Bitacora).
    /// Utiliza el patrón Singleton para garantizar una única instancia global.
    /// </summary>
    /// <remarks>
    /// ARQUITECTURA DE DOS NIVELES:
    /// 1. Archivos de log (LoggerService): Registro rápido de eventos y debugging
    /// 2. Base de datos (tabla Bitacora): Auditoría persistente con capacidad de búsqueda/filtrado
    ///
    /// CATEGORÍAS DE EVENTOS:
    /// - Login/Logout: Seguimiento de accesos al sistema
    /// - CRUD Operations: Alta, Baja, Modificación de entidades
    /// - Excepciones: Errores y problemas técnicos
    /// - Seguridad: Violaciones DVH, accesos no autorizados
    ///
    /// NIVELES DE CRITICIDAD (según CriticidadBitacora):
    /// - Info: Operaciones normales del sistema
    /// - Advertencia: Situaciones que requieren atención
    /// - Error: Errores técnicos que afectan funcionalidad
    /// - Critico: Problemas de seguridad o integridad de datos
    ///
    /// MANEJO DE ERRORES:
    /// Todos los métodos silencian sus propias excepciones para evitar loops infinitos
    /// si el sistema de bitácora falla.
    /// </remarks>
    /// <example>
    /// // Uso básico - Registrar una operación de negocio
    /// var usuario = LoginService.GetUsuarioLogueado();
    /// Bitacora.Current.RegistrarAlta(
    ///     usuario.IdUsuario,
    ///     usuario.Nombre,
    ///     "Clientes",
    ///     "Cliente",
    ///     nuevoCliente.IdCliente.ToString(),
    ///     $"Cliente creado: {nuevoCliente.NombreCompleto}"
    /// );
    ///
    /// // Registrar una excepción con contexto
    /// try
    /// {
    ///     // Operación riesgosa
    /// }
    /// catch (Exception ex)
    /// {
    ///     Bitacora.Current.RegistrarExcepcion(ex, usuario?.IdUsuario, usuario?.Nombre, "Clientes");
    ///     throw;
    /// }
    /// </example>
    public sealed class Bitacora
    {
        #region Singleton

        /// <summary>
        /// Instancia única del servicio Bitacora (patrón Singleton thread-safe)
        /// </summary>
        private static readonly Bitacora _instance = new Bitacora();

        /// <summary>
        /// Obtiene la instancia global del servicio Bitacora.
        /// Acceso thread-safe garantizado por el compilador de C#.
        /// </summary>
        public static Bitacora Current
        {
            get { return _instance; }
        }

        /// <summary>
        /// Constructor privado para prevenir instanciación externa (patrón Singleton)
        /// </summary>
        private Bitacora()
        {
        }
        #endregion

        #region Métodos Legacy (Archivos de Log)
        /// <summary>
        /// Registra una excepción en la bitácora (solo archivos de log)
        /// </summary>
        public void LogException(Exception ex)
        {
            if (ex != null)
            {
                try
                {
                    // Registrar usando LoggerService
                    LoggerService.WriteLog(
                        $"Exception: {ex.GetType().Name} - {ex.Message}\nStackTrace: {ex.StackTrace}",
                        EventLevel.Error,
                        "System"
                    );

                    // Si hay inner exception, registrarla también
                    if (ex.InnerException != null)
                    {
                        LogException(ex.InnerException);
                    }
                }
                catch
                {
                    // Silenciar errores del logger para evitar loops
                }
            }
        }

        /// <summary>
        /// Registra un evento informativo (solo archivos de log)
        /// </summary>
        public void LogInfo(string message, string user = "System")
        {
            LoggerService.WriteLog(message, EventLevel.Informational, user);
        }

        /// <summary>
        /// Registra una advertencia (solo archivos de log)
        /// </summary>
        public void LogWarning(string message, string user = "System")
        {
            LoggerService.WriteLog(message, EventLevel.Warning, user);
        }

        /// <summary>
        /// Registra un error (solo archivos de log)
        /// </summary>
        public void LogError(string message, string user = "System")
        {
            LoggerService.WriteLog(message, EventLevel.Error, user);
        }

        /// <summary>
        /// Registra un evento crítico (solo archivos de log)
        /// </summary>
        public void LogCritical(string message, string user = "System")
        {
            LoggerService.WriteLog(message, EventLevel.Critical, user);
        }
        #endregion

        #region Métodos de Base de Datos (Nueva Funcionalidad)
        /// <summary>
        /// Registra un evento completo en la base de datos de bitácora
        /// </summary>
        public void RegistrarEvento(
            Guid? idUsuario,
            string nombreUsuario,
            string modulo,
            string accion,
            string descripcion,
            string criticidad,
            string tabla = null,
            string idRegistro = null,
            string ip = null)
        {
            try
            {
                BLL.BitacoraBLL.Registrar(
                    idUsuario,
                    nombreUsuario,
                    modulo,
                    accion,
                    descripcion,
                    criticidad,
                    tabla,
                    idRegistro,
                    ip);
            }
            catch
            {
                // Silenciar errores de bitácora para evitar loops infinitos
                // Si falla el registro en BD, al menos quedará en archivos de log
            }
        }

        /// <summary>
        /// Registrar un evento (alias de RegistrarEvento para compatibilidad)
        /// </summary>
        public void Registrar(
            Guid? idUsuario,
            string nombreUsuario,
            string modulo,
            string accion,
            string descripcion,
            string criticidad,
            string tabla = null,
            string idRegistro = null,
            string ip = null)
        {
            RegistrarEvento(idUsuario, nombreUsuario, modulo, accion, descripcion, criticidad, tabla, idRegistro, ip);
        }

        /// <summary>
        /// Registra un login exitoso
        /// </summary>
        public void RegistrarLogin(Guid idUsuario, string nombreUsuario, string ip = null)
        {
            RegistrarEvento(
                idUsuario,
                nombreUsuario,
                "Sistema",
                AccionBitacora.Login,
                $"Usuario '{nombreUsuario}' inició sesión exitosamente",
                CriticidadBitacora.Info,
                ip: ip);
        }

        /// <summary>
        /// Registra un login fallido
        /// </summary>
        public void RegistrarLoginFallido(string nombreUsuario, string motivo, string ip = null)
        {
            RegistrarEvento(
                null,
                nombreUsuario,
                "Sistema",
                AccionBitacora.LoginFallido,
                $"Intento de login fallido para usuario '{nombreUsuario}': {motivo}",
                CriticidadBitacora.Advertencia,
                ip: ip);
        }

        /// <summary>
        /// Registra un logout
        /// </summary>
        public void RegistrarLogout(Guid idUsuario, string nombreUsuario)
        {
            RegistrarEvento(
                idUsuario,
                nombreUsuario,
                "Sistema",
                AccionBitacora.Logout,
                $"Usuario '{nombreUsuario}' cerró sesión",
                CriticidadBitacora.Info);
        }

        /// <summary>
        /// Registra una operación de alta (INSERT)
        /// </summary>
        public void RegistrarAlta(Guid idUsuario, string nombreUsuario, string modulo, string tabla, string idRegistro, string descripcion)
        {
            RegistrarEvento(
                idUsuario,
                nombreUsuario,
                modulo,
                AccionBitacora.Alta,
                descripcion,
                CriticidadBitacora.Info,
                tabla,
                idRegistro);
        }

        /// <summary>
        /// Registra una operación de baja (DELETE)
        /// </summary>
        public void RegistrarBaja(Guid idUsuario, string nombreUsuario, string modulo, string tabla, string idRegistro, string descripcion)
        {
            RegistrarEvento(
                idUsuario,
                nombreUsuario,
                modulo,
                AccionBitacora.Baja,
                descripcion,
                CriticidadBitacora.Advertencia,
                tabla,
                idRegistro);
        }

        /// <summary>
        /// Registra una operación de modificación (UPDATE)
        /// </summary>
        public void RegistrarModificacion(Guid idUsuario, string nombreUsuario, string modulo, string tabla, string idRegistro, string descripcion)
        {
            RegistrarEvento(
                idUsuario,
                nombreUsuario,
                modulo,
                AccionBitacora.Modificacion,
                descripcion,
                CriticidadBitacora.Info,
                tabla,
                idRegistro);
        }

        /// <summary>
        /// Registra una excepción en la base de datos con contexto completo
        /// </summary>
        public void RegistrarExcepcion(Exception ex, Guid? idUsuario = null, string nombreUsuario = "Sistema", string modulo = "Sistema")
        {
            if (ex == null) return;

            try
            {
                string descripcion = $"{ex.GetType().Name}: {ex.Message}";
                if (ex.StackTrace != null)
                {
                    descripcion += $"\n{ex.StackTrace.Substring(0, Math.Min(ex.StackTrace.Length, 400))}";
                }

                RegistrarEvento(
                    idUsuario,
                    nombreUsuario,
                    modulo,
                    AccionBitacora.Excepcion,
                    descripcion,
                    CriticidadBitacora.Error);

                // También registrar en archivos de log
                LogException(ex);
            }
            catch
            {
                // Silenciar errores
            }
        }

        /// <summary>
        /// Registra un error personalizado
        /// </summary>
        public void RegistrarError(string descripcion, Guid? idUsuario = null, string nombreUsuario = "Sistema", string modulo = "Sistema")
        {
            RegistrarEvento(
                idUsuario,
                nombreUsuario,
                modulo,
                AccionBitacora.Error,
                descripcion,
                CriticidadBitacora.Error);
        }

        /// <summary>
        /// Registra una violación de DVH (Dígito Verificador Horizontal)
        /// </summary>
        public void RegistrarViolacionDVH(string descripcion, Guid? idUsuario = null, string nombreUsuario = "Sistema")
        {
            RegistrarEvento(
                idUsuario,
                nombreUsuario,
                "Seguridad",
                AccionBitacora.ViolacionDVH,
                descripcion,
                CriticidadBitacora.Critico);
        }

        /// <summary>
        /// Registra un intento de acceso no autorizado
        /// </summary>
        public void RegistrarAccesoNoAutorizado(Guid idUsuario, string nombreUsuario, string recurso, string ip = null)
        {
            RegistrarEvento(
                idUsuario,
                nombreUsuario,
                "Seguridad",
                AccionBitacora.AccesoNoAutorizado,
                $"Intento de acceso no autorizado al recurso: {recurso}",
                CriticidadBitacora.Critico,
                ip: ip);
        }
        #endregion
    }
}
