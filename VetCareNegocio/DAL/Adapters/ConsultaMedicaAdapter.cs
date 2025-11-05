using System;
using System.Data;
using DomainModel;

namespace DAL.Adapters
{
    /// <summary>
    /// Adapter para mapear DataRow a ConsultaMedica
    /// </summary>
    internal static class ConsultaMedicaAdapter
    {
        /// <summary>
        /// Mapea un DataRow a un objeto ConsultaMedica
        /// </summary>
        public static ConsultaMedica Map(DataRow row)
        {
            if (row == null)
                return null;

            return new ConsultaMedica
            {
                IdConsulta = row.Field<Guid>("IdConsulta"),
                IdCita = row.Field<Guid>("IdCita"),
                IdVeterinario = row.Field<Guid>("IdVeterinario"),
                Sintomas = row.Field<string>("Sintomas"),
                Diagnostico = row.Field<string>("Diagnostico"),
                Tratamiento = row.Field<string>("Tratamiento"),
                Observaciones = row.Field<string>("Observaciones"),
                FechaConsulta = row.Field<DateTime>("FechaConsulta"),
                Activo = row.Field<bool>("Activo")
            };
        }

        /// <summary>
        /// Mapea un DataTable completo a un array de ConsultaMedica
        /// </summary>
        public static ConsultaMedica[] MapAll(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                return new ConsultaMedica[0];

            var consultas = new ConsultaMedica[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                consultas[i] = Map(table.Rows[i]);
            }

            return consultas;
        }

        /// <summary>
        /// Mapea un DataRow a un objeto MedicamentoRecetado
        /// (para resultados de ConsultaMedicamento_SelectByConsulta)
        /// </summary>
        public static MedicamentoRecetado MapMedicamentoRecetado(DataRow row)
        {
            if (row == null)
                return null;

            return new MedicamentoRecetado
            {
                IdConsulta = row.Field<Guid>("IdConsulta"),
                IdMedicamento = row.Field<Guid>("IdMedicamento"),
                Cantidad = row.Field<int>("Cantidad"),
                Indicaciones = row.Field<string>("Indicaciones"),
                NombreMedicamento = row.Field<string>("NombreMedicamento"),
                Presentacion = row.Field<string>("Presentacion")
            };
        }

        /// <summary>
        /// Mapea un DataTable completo a un array de MedicamentoRecetado
        /// </summary>
        public static MedicamentoRecetado[] MapAllMedicamentosRecetados(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                return new MedicamentoRecetado[0];

            var medicamentos = new MedicamentoRecetado[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                medicamentos[i] = MapMedicamentoRecetado(table.Rows[i]);
            }

            return medicamentos;
        }
    }
}
