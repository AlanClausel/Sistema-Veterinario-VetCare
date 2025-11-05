using System;
using System.Data;
using DomainModel;

namespace DAL.Adapters
{
    /// <summary>
    /// Adaptador para convertir DataRow a entidad Veterinario
    /// </summary>
    internal static class VeterinarioAdapter
    {
        /// <summary>
        /// Convierte un DataRow en una entidad Veterinario
        /// </summary>
        public static Veterinario Map(DataRow row)
        {
            if (row == null)
                return null;

            var veterinario = new Veterinario
            {
                IdVeterinario = row.Field<Guid>("IdVeterinario"),
                Nombre = row.Field<string>("Nombre"),
                Matricula = row.Field<string>("Matricula"),
                Telefono = row.Field<string>("Telefono"),
                Email = row.Field<string>("Email"),
                Observaciones = row.Field<string>("Observaciones"),
                FechaAlta = row.Field<DateTime>("FechaAlta"),
                Activo = row.Field<bool>("Activo")
            };

            return veterinario;
        }

        /// <summary>
        /// Convierte un DataTable en una lista de entidades Veterinario
        /// </summary>
        public static Veterinario[] MapAll(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                return new Veterinario[0];

            Veterinario[] veterinarios = new Veterinario[table.Rows.Count];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                veterinarios[i] = Map(table.Rows[i]);
            }

            return veterinarios;
        }
    }
}
