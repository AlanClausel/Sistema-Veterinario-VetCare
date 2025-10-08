using System;
using System.Diagnostics.Tracing;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Servicio de bitácora para registro de eventos y excepciones
    /// Singleton pattern
    /// </summary>
    public sealed class Bitacora
    {
        #region Singleton
        private static readonly Bitacora _instance = new Bitacora();

        public static Bitacora Current
        {
            get { return _instance; }
        }

        private Bitacora()
        {
        }
        #endregion

        /// <summary>
        /// Registra una excepción en la bitácora
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
        /// Registra un evento informativo
        /// </summary>
        public void LogInfo(string message, string user = "System")
        {
            LoggerService.WriteLog(message, EventLevel.Informational, user);
        }

        /// <summary>
        /// Registra una advertencia
        /// </summary>
        public void LogWarning(string message, string user = "System")
        {
            LoggerService.WriteLog(message, EventLevel.Warning, user);
        }

        /// <summary>
        /// Registra un error
        /// </summary>
        public void LogError(string message, string user = "System")
        {
            LoggerService.WriteLog(message, EventLevel.Error, user);
        }

        /// <summary>
        /// Registra un evento crítico
        /// </summary>
        public void LogCritical(string message, string user = "System")
        {
            LoggerService.WriteLog(message, EventLevel.Critical, user);
        }
    }
}
