using System;
using System.Collections.Generic;

namespace DomainModel
{
    /// <summary>
    /// Entidad ConsultaMedica - Registro de la consulta médica realizada en una cita.
    /// Documenta los síntomas, diagnóstico, tratamiento y medicamentos recetados durante la consulta.
    /// </summary>
    public class ConsultaMedica
    {
        /// <summary>
        /// Identificador único de la consulta médica (GUID)
        /// </summary>
        public Guid IdConsulta { get; set; }

        /// <summary>
        /// Identificador de la cita asociada (FK a Cita)
        /// </summary>
        public Guid IdCita { get; set; }

        /// <summary>
        /// Identificador del veterinario que realizó la consulta (FK a Veterinario)
        /// </summary>
        public Guid IdVeterinario { get; set; }

        /// <summary>
        /// Descripción de los síntomas presentados por la mascota
        /// </summary>
        public string Sintomas { get; set; }

        /// <summary>
        /// Diagnóstico emitido por el veterinario
        /// </summary>
        public string Diagnostico { get; set; }

        /// <summary>
        /// Descripción del tratamiento prescrito
        /// </summary>
        public string Tratamiento { get; set; }

        /// <summary>
        /// Observaciones adicionales sobre la consulta
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Fecha y hora en que se realizó la consulta
        /// </summary>
        public DateTime FechaConsulta { get; set; }

        /// <summary>
        /// Indica si la consulta está activa en el sistema (para baja lógica)
        /// </summary>
        public bool Activo { get; set; }

        // Datos relacionados (no se mapean desde BD, se hidratan por relaciones)

        /// <summary>
        /// Objeto Cita asociado a esta consulta (propiedad de navegación)
        /// </summary>
        public Cita Cita { get; set; }

        /// <summary>
        /// Objeto Veterinario que realizó la consulta (propiedad de navegación)
        /// </summary>
        public Veterinario Veterinario { get; set; }

        /// <summary>
        /// Lista de medicamentos recetados en esta consulta
        /// </summary>
        public List<MedicamentoRecetado> Medicamentos { get; set; }

        /// <summary>
        /// Constructor por defecto. Inicializa una nueva consulta médica con valores predeterminados.
        /// Genera un nuevo GUID, establece la fecha actual y valores por defecto para campos NOT NULL.
        /// </summary>
        public ConsultaMedica()
        {
            IdConsulta = Guid.NewGuid();
            FechaConsulta = DateTime.Now;
            Activo = true;
            Medicamentos = new List<MedicamentoRecetado>();
            // Inicializar campos NOT NULL con valores por defecto
            Sintomas = string.Empty;
            Diagnostico = string.Empty;
            Tratamiento = string.Empty;
            Observaciones = string.Empty;
        }

        /// <summary>
        /// Constructor con parámetros. Inicializa una nueva consulta médica asociada a una cita específica.
        /// </summary>
        /// <param name="idCita">Identificador de la cita asociada</param>
        /// <param name="idVeterinario">Identificador del veterinario que realiza la consulta</param>
        public ConsultaMedica(Guid idCita, Guid idVeterinario)
        {
            IdConsulta = Guid.NewGuid();
            IdCita = idCita;
            IdVeterinario = idVeterinario;
            FechaConsulta = DateTime.Now;
            Activo = true;
            Medicamentos = new List<MedicamentoRecetado>();
            // Inicializar campos NOT NULL con valores por defecto
            Sintomas = string.Empty;
            Diagnostico = string.Empty;
            Tratamiento = string.Empty;
            Observaciones = string.Empty;
        }

        /// <summary>
        /// Valida que los datos de la consulta médica cumplan con las reglas de negocio.
        /// Verifica campos obligatorios y longitudes mínimas.
        /// </summary>
        /// <returns>Array de mensajes de error. Si está vacío, la consulta es válida</returns>
        public string[] Validar()
        {
            var errores = new List<string>();

            if (IdCita == Guid.Empty)
                errores.Add("La consulta debe estar asociada a una cita");

            if (IdVeterinario == Guid.Empty)
                errores.Add("La consulta debe tener un veterinario asignado");

            if (string.IsNullOrWhiteSpace(Sintomas))
                errores.Add("Los síntomas son requeridos");

            if (Sintomas != null && Sintomas.Trim().Length < 3)
                errores.Add("Los síntomas deben tener al menos 3 caracteres");

            if (string.IsNullOrWhiteSpace(Diagnostico))
                errores.Add("El diagnóstico es requerido");

            if (Diagnostico != null && Diagnostico.Trim().Length < 3)
                errores.Add("El diagnóstico debe tener al menos 3 caracteres");

            return errores.ToArray();
        }

        /// <summary>
        /// Agrega un medicamento recetado a la lista de medicamentos de la consulta
        /// </summary>
        /// <param name="medicamento">Objeto MedicamentoRecetado a agregar</param>
        /// <exception cref="ArgumentNullException">Si el medicamento es null</exception>
        public void AgregarMedicamento(MedicamentoRecetado medicamento)
        {
            if (medicamento == null)
                throw new ArgumentNullException(nameof(medicamento));

            if (Medicamentos == null)
                Medicamentos = new List<MedicamentoRecetado>();

            Medicamentos.Add(medicamento);
        }

        /// <summary>
        /// Retorna una representación en cadena de la consulta con su fecha y diagnóstico
        /// </summary>
        /// <returns>Cadena con el formato "Consulta DD/MM/YYYY HH:mm - Diagnostico"</returns>
        public override string ToString()
        {
            return $"Consulta {FechaConsulta:dd/MM/yyyy HH:mm} - {Diagnostico}";
        }
    }

    /// <summary>
    /// Clase auxiliar que representa un medicamento recetado en una consulta médica.
    /// Asocia un medicamento con la consulta, incluyendo cantidad e indicaciones de uso.
    /// </summary>
    public class MedicamentoRecetado
    {
        /// <summary>
        /// Identificador de la consulta médica asociada (FK a ConsultaMedica)
        /// </summary>
        public Guid IdConsulta { get; set; }

        /// <summary>
        /// Identificador del medicamento recetado (FK a Medicamento)
        /// </summary>
        public Guid IdMedicamento { get; set; }

        /// <summary>
        /// Cantidad de unidades recetadas del medicamento
        /// </summary>
        public int Cantidad { get; set; }

        /// <summary>
        /// Indicaciones de uso del medicamento (dosis, frecuencia, duración, etc.)
        /// </summary>
        public string Indicaciones { get; set; }

        // Datos del medicamento (hidratados desde la tabla Medicamento)

        /// <summary>
        /// Nombre del medicamento (campo desnormalizado para optimización)
        /// </summary>
        public string NombreMedicamento { get; set; }

        /// <summary>
        /// Presentación del medicamento (campo desnormalizado para optimización)
        /// </summary>
        public string Presentacion { get; set; }

        /// <summary>
        /// Constructor por defecto. Inicializa la cantidad en 1.
        /// </summary>
        public MedicamentoRecetado()
        {
            Cantidad = 1;
        }

        /// <summary>
        /// Obtiene el nombre completo del medicamento combinando nombre y presentación
        /// </summary>
        /// <returns>Cadena con el formato "NombreMedicamento Presentacion"</returns>
        public string NombreCompleto => $"{NombreMedicamento} {Presentacion}";

        /// <summary>
        /// Retorna una representación en cadena del medicamento recetado con su cantidad
        /// </summary>
        /// <returns>Cadena con el formato "NombreCompleto - Cantidad: X"</returns>
        public override string ToString()
        {
            return $"{NombreCompleto} - Cantidad: {Cantidad}";
        }
    }
}
