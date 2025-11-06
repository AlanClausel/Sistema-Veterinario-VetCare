namespace ServicesSecurity.DomainModel.Security
{
    /// <summary>
    /// Tipos de acciones que se registran en la bitácora
    /// </summary>
    public static class AccionBitacora
    {
        // Acciones de autenticación
        public const string Login = "Login";
        public const string LoginFallido = "LoginFallido";
        public const string Logout = "Logout";
        public const string CambioPassword = "CambioPassword";

        // Acciones de CRUD
        public const string Alta = "Alta";
        public const string Baja = "Baja";
        public const string Modificacion = "Modificacion";
        public const string Consulta = "Consulta";

        // Acciones de permisos
        public const string AsignacionPermiso = "AsignacionPermiso";
        public const string RevocacionPermiso = "RevocacionPermiso";
        public const string CambioRol = "CambioRol";

        // Acciones de sistema
        public const string Inicio = "Inicio";
        public const string Error = "Error";
        public const string Excepcion = "Excepcion";
        public const string ViolacionDVH = "ViolacionDVH";
        public const string AccesoNoAutorizado = "AccesoNoAutorizado";

        // Acciones de negocio
        public const string AgendarCita = "AgendarCita";
        public const string CancelarCita = "CancelarCita";
        public const string FinalizarConsulta = "FinalizarConsulta";
        public const string MovimientoStock = "MovimientoStock";
    }
}
