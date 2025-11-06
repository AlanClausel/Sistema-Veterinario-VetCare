namespace ServicesSecurity.DomainModel.Security
{
    /// <summary>
    /// Niveles de criticidad para los eventos de bitácora
    /// </summary>
    public static class CriticidadBitacora
    {
        /// <summary>
        /// Información general, eventos normales del sistema
        /// </summary>
        public const string Info = "Info";

        /// <summary>
        /// Advertencias, situaciones que requieren atención pero no son críticas
        /// </summary>
        public const string Advertencia = "Advertencia";

        /// <summary>
        /// Errores que afectan la funcionalidad pero son recuperables
        /// </summary>
        public const string Error = "Error";

        /// <summary>
        /// Eventos críticos que requieren atención inmediata (violaciones de seguridad, pérdida de datos, etc.)
        /// </summary>
        public const string Critico = "Critico";
    }
}
