using System;
using System.Data;
using DomainModel;

namespace DAL.Adapters
{
    /// <summary>
    /// Adaptador para convertir DataRow a entidad Cliente
    /// </summary>
    internal static class ClienteAdapter
    {
        /// <summary>
        /// Convierte un DataRow en una entidad Cliente
        /// </summary>
        public static Cliente Map(DataRow row)
        {
            if (row == null)
                return null;

            var cliente = new Cliente
            {
                IdCliente = row.Field<Guid>("IdCliente"),
                Nombre = row.Field<string>("Nombre"),
                Apellido = row.Field<string>("Apellido"),
                DNI = row.Field<string>("DNI"),
                Telefono = row.Field<string>("Telefono"),
                Email = row.Field<string>("Email"),
                Direccion = row.Field<string>("Direccion"),
                FechaRegistro = row.Field<DateTime>("FechaRegistro"),
                Activo = row.Field<bool>("Activo")
            };

            return cliente;
        }

        /// <summary>
        /// Convierte un DataTable en una lista de entidades Cliente
        /// </summary>
        public static Cliente[] MapAll(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                return new Cliente[0];

            Cliente[] clientes = new Cliente[table.Rows.Count];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                clientes[i] = Map(table.Rows[i]);
            }

            return clientes;
        }
    }
}
