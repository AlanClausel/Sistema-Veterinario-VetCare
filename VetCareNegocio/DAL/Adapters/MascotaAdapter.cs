using System;
using System.Data;
using DomainModel;

namespace DAL.Adapters
{
    /// <summary>
    /// Adaptador para convertir DataRow a entidad Mascota
    /// </summary>
    internal static class MascotaAdapter
    {
        /// <summary>
        /// Convierte un DataRow en una entidad Mascota
        /// </summary>
        public static Mascota Map(DataRow row)
        {
            if (row == null)
                return null;

            var mascota = new Mascota
            {
                IdMascota = row.Field<Guid>("IdMascota"),
                IdCliente = row.Field<Guid>("IdCliente"),
                Nombre = row.Field<string>("Nombre"),
                Especie = row.Field<string>("Especie"),
                Raza = row.Field<string>("Raza"),
                FechaNacimiento = row.Field<DateTime>("FechaNacimiento"),
                Sexo = row.Field<string>("Sexo"),
                Peso = row.Field<decimal?>("Peso") ?? 0,
                Color = row.Field<string>("Color"),
                Observaciones = row.Field<string>("Observaciones"),
                Activo = row.Field<bool>("Activo")
            };

            return mascota;
        }

        /// <summary>
        /// Convierte un DataTable en una lista de entidades Mascota
        /// </summary>
        public static Mascota[] MapAll(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                return new Mascota[0];

            Mascota[] mascotas = new Mascota[table.Rows.Count];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                mascotas[i] = Map(table.Rows[i]);
            }

            return mascotas;
        }
    }
}
