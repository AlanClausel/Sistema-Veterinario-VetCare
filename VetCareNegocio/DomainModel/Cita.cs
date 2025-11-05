using System;
using System.Collections.Generic;

namespace DomainModel
{
    /// <summary>
    /// Entidad de dominio que representa una Cita veterinaria
    /// </summary>
    public class Cita
    {
        #region Propiedades

        public Guid IdCita { get; set; }
        public Guid IdMascota { get; set; }
        public DateTime FechaCita { get; set; }
        public string TipoConsulta { get; set; }

        /// <summary>
        /// ID del veterinario (FK a tabla Veterinario) - NUEVO
        /// </summary>
        public Guid? IdVeterinario { get; set; }

        /// <summary>
        /// Nombre del veterinario (campo legacy, se mantiene para compatibilidad)
        /// DEPRECATED: Usar IdVeterinario en nuevas citas
        /// </summary>
        public string Veterinario { get; set; }

        public EstadoCita Estado { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        // Propiedades de navegación (no mapeadas directamente desde DB)
        public Mascota Mascota { get; set; }
        public string MascotaNombre { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteApellido { get; set; }
        public string ClienteDNI { get; set; }
        public string ClienteTelefono { get; set; }
        public Guid IdCliente { get; set; }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Nombre completo del cliente (Apellido, Nombre)
        /// </summary>
        public string ClienteNombreCompleto
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ClienteApellido) && string.IsNullOrWhiteSpace(ClienteNombre))
                    return string.Empty;

                if (string.IsNullOrWhiteSpace(ClienteApellido))
                    return ClienteNombre;

                if (string.IsNullOrWhiteSpace(ClienteNombre))
                    return ClienteApellido;

                return $"{ClienteApellido}, {ClienteNombre}";
            }
        }

        /// <summary>
        /// Formato de fecha para mostrar en grids (dd/MM/yyyy HH:mm)
        /// </summary>
        public string FechaCitaFormateada => FechaCita.ToString("dd/MM/yyyy HH:mm");

        /// <summary>
        /// Solo la hora de la cita (HH:mm)
        /// </summary>
        public string HoraCita => FechaCita.ToString("HH:mm");

        /// <summary>
        /// Solo la fecha de la cita (dd/MM/yyyy)
        /// </summary>
        public string FechaSoloFecha => FechaCita.ToString("dd/MM/yyyy");

        /// <summary>
        /// Estado como string para mostrar en UI
        /// </summary>
        public string EstadoDescripcion
        {
            get
            {
                switch (Estado)
                {
                    case EstadoCita.Agendada:
                        return "Agendada";
                    case EstadoCita.Confirmada:
                        return "Confirmada";
                    case EstadoCita.Completada:
                        return "Completada";
                    case EstadoCita.Cancelada:
                        return "Cancelada";
                    case EstadoCita.NoAsistio:
                        return "No Asistió";
                    default:
                        return Estado.ToString();
                }
            }
        }

        /// <summary>
        /// Información completa de la cita para mostrar en diálogos
        /// </summary>
        public string ResumenCita
        {
            get
            {
                return $"Cliente: {ClienteNombreCompleto}\n" +
                       $"Mascota: {MascotaNombre}\n" +
                       $"Fecha: {FechaCitaFormateada}\n" +
                       $"Tipo: {TipoConsulta}\n" +
                       $"Veterinario: {Veterinario}\n" +
                       $"Estado: {EstadoDescripcion}";
            }
        }

        #endregion

        #region Constructores

        public Cita()
        {
            IdCita = Guid.NewGuid();
            FechaCita = DateTime.Now;
            Estado = EstadoCita.Agendada;
            FechaRegistro = DateTime.Now;
            Activo = true;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Verifica si la cita puede ser modificada según su estado
        /// </summary>
        public bool PuedeSerModificada()
        {
            return Estado == EstadoCita.Agendada || Estado == EstadoCita.Confirmada;
        }

        /// <summary>
        /// Verifica si la cita puede ser cancelada
        /// </summary>
        public bool PuedeSerCancelada()
        {
            return Estado == EstadoCita.Agendada || Estado == EstadoCita.Confirmada;
        }

        /// <summary>
        /// Verifica si la cita ya ocurrió (fecha pasada)
        /// </summary>
        public bool EsCitaPasada()
        {
            return FechaCita < DateTime.Now;
        }

        /// <summary>
        /// Verifica si la cita es del día de hoy
        /// </summary>
        public bool EsCitaHoy()
        {
            return FechaCita.Date == DateTime.Today;
        }

        #endregion

        #region Validaciones

        /// <summary>
        /// Valida que los datos de la cita sean correctos
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (IdMascota == Guid.Empty)
                errores.Add("Debe seleccionar una mascota");

            if (FechaCita == DateTime.MinValue)
                errores.Add("La fecha de la cita es requerida");

            if (FechaCita < DateTime.Now.AddMinutes(-5) && Estado == EstadoCita.Agendada)
                errores.Add("No se puede agendar una cita en el pasado");

            if (string.IsNullOrWhiteSpace(TipoConsulta))
                errores.Add("El tipo de consulta es requerido");

            if (TipoConsulta != null && TipoConsulta.Length > 100)
                errores.Add("El tipo de consulta no puede exceder 100 caracteres");

            // Validar que tenga veterinario asignado (nuevo campo o legacy)
            if (!IdVeterinario.HasValue && string.IsNullOrWhiteSpace(Veterinario))
                errores.Add("El veterinario es requerido");

            if (Veterinario != null && Veterinario.Length > 150)
                errores.Add("El nombre del veterinario no puede exceder 150 caracteres");

            if (Observaciones != null && Observaciones.Length > 500)
                errores.Add("Las observaciones no pueden exceder 500 caracteres");

            return errores;
        }

        #endregion
    }

    /// <summary>
    /// Enumeración de estados posibles de una cita
    /// </summary>
    public enum EstadoCita
    {
        Agendada,
        Confirmada,
        Completada,
        Cancelada,
        NoAsistio
    }
}
