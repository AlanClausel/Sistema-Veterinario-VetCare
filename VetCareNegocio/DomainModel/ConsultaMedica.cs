using System;
using System.Collections.Generic;

namespace DomainModel
{
    /// <summary>
    /// Entidad ConsultaMedica - Registro de la consulta médica realizada en una cita
    /// </summary>
    public class ConsultaMedica
    {
        public Guid IdConsulta { get; set; }
        public Guid IdCita { get; set; }
        public Guid IdVeterinario { get; set; }
        public string Sintomas { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaConsulta { get; set; }
        public bool Activo { get; set; }

        // Datos relacionados (no se mapean desde BD, se hidratan por relaciones)
        public Cita Cita { get; set; }
        public Veterinario Veterinario { get; set; }
        public List<MedicamentoRecetado> Medicamentos { get; set; }

        // Constructor vacío
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

        // Constructor con cita
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
        /// Valida la entidad ConsultaMedica
        /// </summary>
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
        /// Agrega un medicamento a la consulta
        /// </summary>
        public void AgregarMedicamento(MedicamentoRecetado medicamento)
        {
            if (medicamento == null)
                throw new ArgumentNullException(nameof(medicamento));

            if (Medicamentos == null)
                Medicamentos = new List<MedicamentoRecetado>();

            Medicamentos.Add(medicamento);
        }

        public override string ToString()
        {
            return $"Consulta {FechaConsulta:dd/MM/yyyy HH:mm} - {Diagnostico}";
        }
    }

    /// <summary>
    /// Clase auxiliar para medicamentos recetados en una consulta
    /// </summary>
    public class MedicamentoRecetado
    {
        public Guid IdConsulta { get; set; }
        public Guid IdMedicamento { get; set; }
        public int Cantidad { get; set; }
        public string Indicaciones { get; set; }

        // Datos del medicamento (hidratados)
        public string NombreMedicamento { get; set; }
        public string Presentacion { get; set; }

        public MedicamentoRecetado()
        {
            Cantidad = 1;
        }

        public string NombreCompleto => $"{NombreMedicamento} {Presentacion}";

        public override string ToString()
        {
            return $"{NombreCompleto} - Cantidad: {Cantidad}";
        }
    }
}
