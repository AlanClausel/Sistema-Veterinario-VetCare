using System;

namespace ServicesSecurity.DomainModel.Security
{
    /// <summary>
    /// Entidad que representa un registro de auditoría del sistema
    /// </summary>
    public class Bitacora
    {
        public Guid IdBitacora { get; set; }
        public Guid? IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaHora { get; set; }
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public string Descripcion { get; set; }
        public string Tabla { get; set; }
        public string IdRegistro { get; set; }
        public string Criticidad { get; set; }
        public string IP { get; set; }

        public Bitacora()
        {
            IdBitacora = Guid.NewGuid();
            FechaHora = DateTime.Now;
        }

        /// <summary>
        /// Constructor para crear un registro de bitácora completo
        /// </summary>
        public Bitacora(
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
            IdBitacora = Guid.NewGuid();
            IdUsuario = idUsuario;
            NombreUsuario = nombreUsuario;
            FechaHora = DateTime.Now;
            Modulo = modulo;
            Accion = accion;
            Descripcion = descripcion;
            Tabla = tabla;
            IdRegistro = idRegistro;
            Criticidad = criticidad;
            IP = ip;
        }

        public override string ToString()
        {
            return $"[{FechaHora:yyyy-MM-dd HH:mm:ss}] {Criticidad} - {Modulo}.{Accion}: {Descripcion}";
        }
    }
}
