using System;
using System.Diagnostics.Tracing;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Gestor centralizado de excepciones del sistema
    /// Singleton pattern
    /// </summary>
    public sealed class ExceptionManager
    {
        #region Singleton
        private static readonly ExceptionManager _instance = new ExceptionManager();

        public static ExceptionManager Current
        {
            get { return _instance; }
        }

        private ExceptionManager()
        {
        }
        #endregion

        /// <summary>
        /// Maneja una excepción registrándola en el log
        /// </summary>
        public void Handle(Exception ex)
        {
            if (ex != null)
            {
                // Registrar en bitácora
                Bitacora.Current.LogException(ex);

                // Log adicional para excepciones críticas
                LoggerService.WriteLog(
                    $"Exception: {ex.Message} | StackTrace: {ex.StackTrace}",
                    EventLevel.Error,
                    "System"
                );
            }
        }

        /// <summary>
        /// Maneja una excepción con información del emisor
        /// </summary>
        public void Handle(Exception ex, object sender)
        {
            if (ex != null)
            {
                string senderInfo = sender?.GetType().Name ?? "Unknown";

                Bitacora.Current.LogException(ex);

                LoggerService.WriteLog(
                    $"[{senderInfo}] Exception: {ex.Message}",
                    EventLevel.Error,
                    senderInfo
                );
            }
        }
    }
}
