using System;
using System.Data;
using DomainModel;

namespace DAL.Adapters
{
    /// <summary>
    /// Adapter para mapear DataRow a Medicamento
    /// </summary>
    internal static class MedicamentoAdapter
    {
        /// <summary>
        /// Mapea un DataRow a un objeto Medicamento
        /// </summary>
        public static Medicamento Map(DataRow row)
        {
            if (row == null)
                return null;

            return new Medicamento
            {
                IdMedicamento = row.Field<Guid>("IdMedicamento"),
                Nombre = row.Field<string>("Nombre"),
                Presentacion = row.Field<string>("Presentacion"),
                Stock = row.Field<int>("Stock"),
                PrecioUnitario = row.Field<decimal>("PrecioUnitario"),
                Observaciones = row.Field<string>("Observaciones"),
                FechaRegistro = row.Field<DateTime>("FechaRegistro"),
                Activo = row.Field<bool>("Activo")
            };
        }

        /// <summary>
        /// Mapea un DataTable completo a un array de Medicamento
        /// </summary>
        public static Medicamento[] MapAll(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                return new Medicamento[0];

            var medicamentos = new Medicamento[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                medicamentos[i] = Map(table.Rows[i]);
            }

            return medicamentos;
        }
    }
}
