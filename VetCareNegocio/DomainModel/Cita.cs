using System;
using System.Collections.Generic;

namespace DomainModel
{
    /// <summary>
    /// Entidad de dominio que representa una Cita veterinaria.
    /// Gestiona las citas programadas entre mascotas y veterinarios, incluyendo su estado y seguimiento.
    /// </summary>
    public class Cita
    {
        #region Propiedades

        /// <summary>
        /// Identificador único de la cita (GUID)
        /// </summary>
        public Guid IdCita { get; set; }

        /// <summary>
        /// Identificador de la mascota para esta cita (FK a Mascota)
        /// </summary>
        public Guid IdMascota { get; set; }

        /// <summary>
        /// Fecha y hora programada para la cita
        /// </summary>
        public DateTime FechaCita { get; set; }

        /// <summary>
        /// Tipo de consulta a realizar (Ej: Vacunación, Consulta general, Cirugía, etc.)
        /// </summary>
        public string TipoConsulta { get; set; }

        /// <summary>
        /// ID del veterinario (FK a tabla Veterinario) - NUEVO
        /// </summary>
        public Guid? IdVeterinario { get; set; }

        /// <summary>
        /// Nombre del veterinario (campo legacy, se mantiene para compatibilidad).
        /// DEPRECATED: Usar IdVeterinario en nuevas citas
        /// </summary>
        public string Veterinario { get; set; }

        /// <summary>
        /// Estado actual de la cita (Agendada, Confirmada, Completada, Cancelada, NoAsistio)
        /// </summary>
        public EstadoCita Estado { get; set; }

        /// <summary>
        /// Observaciones adicionales sobre la cita
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Fecha y hora en que se registró la cita en el sistema
        /// </summary>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Indica si la cita está activa en el sistema (para baja lógica)
        /// </summary>
        public bool Activo { get; set; }

        // Propiedades de navegación (no mapeadas directamente desde DB)

        /// <summary>
        /// Objeto Mascota asociado a esta cita (propiedad de navegación)
        /// </summary>
        public Mascota Mascota { get; set; }

        /// <summary>
        /// Nombre de la mascota (campo desnormalizado para optimización de consultas)
        /// </summary>
        public string MascotaNombre { get; set; }

        /// <summary>
        /// Nombre del cliente dueño de la mascota (campo desnormalizado)
        /// </summary>
        public string ClienteNombre { get; set; }

        /// <summary>
        /// Apellido del cliente dueño de la mascota (campo desnormalizado)
        /// </summary>
        public string ClienteApellido { get; set; }

        /// <summary>
        /// DNI del cliente dueño de la mascota (campo desnormalizado)
        /// </summary>
        public string ClienteDNI { get; set; }

        /// <summary>
        /// Teléfono del cliente dueño de la mascota (campo desnormalizado)
        /// </summary>
        public string ClienteTelefono { get; set; }

        /// <summary>
        /// Identificador del cliente dueño de la mascota (campo desnormalizado)
        /// </summary>
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

        /// <summary>
        /// Constructor por defecto. Inicializa una nueva cita con valores predeterminados.
        /// Genera un nuevo GUID, establece el estado como Agendada y marca la cita como activa.
        /// </summary>
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
        /// Verifica si la cita puede ser modificada según su estado actual.
        /// Solo se pueden modificar citas en estado Agendada o Confirmada.
        /// </summary>
        /// <returns>True si la cita puede ser modificada, false en caso contrario</returns>
        public bool PuedeSerModificada()
        {
            return Estado == EstadoCita.Agendada || Estado == EstadoCita.Confirmada;
        }

        /// <summary>
        /// Verifica si la cita puede ser cancelada según su estado actual.
        /// Solo se pueden cancelar citas en estado Agendada o Confirmada.
        /// </summary>
        /// <returns>True si la cita puede ser cancelada, false en caso contrario</returns>
        public bool PuedeSerCancelada()
        {
            return Estado == EstadoCita.Agendada || Estado == EstadoCita.Confirmada;
        }

        /// <summary>
        /// Verifica si la cita ya ocurrió comparando la fecha con la fecha actual
        /// </summary>
        /// <returns>True si la fecha de la cita es anterior a la fecha actual, false en caso contrario</returns>
        public bool EsCitaPasada()
        {
            return FechaCita < DateTime.Now;
        }

        /// <summary>
        /// Verifica si la cita está programada para el día de hoy
        /// </summary>
        /// <returns>True si la cita es para hoy, false en caso contrario</returns>
        public bool EsCitaHoy()
        {
            return FechaCita.Date == DateTime.Today;
        }

        #endregion

        #region Validaciones

        /// <summary>
        /// Valida que los datos de la cita cumplan con las reglas de negocio.
        /// Verifica campos obligatorios, longitudes máximas y consistencia de datos.
        /// </summary>
        /// <returns>Lista de mensajes de error. Si está vacía, la cita es válida</returns>
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
        /// <summary>
        /// Cita creada y agendada, pendiente de confirmación
        /// </summary>
        Agendada,

        /// <summary>
        /// Cita confirmada por el cliente
        /// </summary>
        Confirmada,

        /// <summary>
        /// Cita completada con consulta médica finalizada
        /// </summary>
        Completada,

        /// <summary>
        /// Cita cancelada por el cliente o la clínica
        /// </summary>
        Cancelada,

        /// <summary>
        /// Cliente no asistió a la cita programada
        /// </summary>
        NoAsistio
    }
}
