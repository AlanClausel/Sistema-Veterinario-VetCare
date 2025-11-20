using System;
using System.Data;
using DomainModel;

namespace DAL.Adapters
{
    /// <summary>
    /// Adaptador estático para transformar datos tabulares (DataRow/DataTable) en entidades Cliente del dominio.
    /// Implementa el patrón Adapter para desacoplar la capa de datos (ADO.NET) del modelo de dominio.
    /// </summary>
    /// <remarks>
    /// PATRÓN ADAPTER:
    /// Este adaptador convierte la representación tabular de datos (DataRow) retornada por stored procedures
    /// en objetos de dominio fuertemente tipados (Cliente), facilitando:
    /// - Separación de responsabilidades entre DAL y DomainModel
    /// - Facilidad de testing mediante objetos del dominio
    /// - Type safety en toda la aplicación
    /// - Manejo de valores NULL de base de datos
    ///
    /// CONVENCIONES:
    /// - Los nombres de columnas del DataRow deben coincidir EXACTAMENTE con los nombres de propiedades
    /// - Todos los campos son obligatorios excepto Telefono, Email y Direccion (pueden ser NULL)
    /// - El mapeo es directo 1:1 sin transformaciones de datos
    /// </remarks>
    internal static class ClienteAdapter
    {
        /// <summary>
        /// Convierte un DataRow individual en una entidad Cliente del dominio.
        /// Mapea cada columna del DataRow a la propiedad correspondiente del objeto Cliente.
        /// </summary>
        /// <param name="row">
        /// DataRow con los datos del cliente obtenidos desde stored procedures.
        /// Debe contener todas las columnas esperadas: IdCliente, Nombre, Apellido, DNI,
        /// Telefono, Email, Direccion, FechaRegistro, Activo.
        /// </param>
        /// <returns>
        /// Objeto Cliente poblado con los datos del DataRow, o null si el DataRow es null.
        /// </returns>
        /// <exception cref="InvalidCastException">
        /// Si alguna columna no puede ser convertida al tipo esperado
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Si alguna columna requerida no existe en el DataRow
        /// </exception>
        /// <example>
        /// var row = dataTable.Rows[0];
        /// Cliente cliente = ClienteAdapter.Map(row);
        /// Console.WriteLine(cliente.NombreCompleto);
        /// </example>
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
        /// Convierte un DataTable completo en un array de entidades Cliente.
        /// Itera sobre todas las filas del DataTable y aplica el mapeo individual a cada una.
        /// Optimizado para procesamiento en lote de resultados de consultas.
        /// </summary>
        /// <param name="table">
        /// DataTable con cero o más filas de clientes obtenidas desde stored procedures.
        /// Cada fila debe tener la estructura esperada para el mapeo.
        /// </param>
        /// <returns>
        /// Array de objetos Cliente mapeados desde las filas del DataTable.
        /// Retorna un array vacío si el DataTable es null o no tiene filas.
        /// El tamaño del array coincide exactamente con el número de filas.
        /// </returns>
        /// <exception cref="InvalidCastException">
        /// Si alguna columna en cualquier fila no puede ser convertida al tipo esperado
        /// </exception>
        /// <remarks>
        /// Este método es más eficiente que usar LINQ o listas para conversiones masivas,
        /// ya que pre-aloca el array con el tamaño exacto y evita redimensionamientos.
        /// </remarks>
        /// <example>
        /// DataTable dt = SqlHelper.ExecuteDataTable("Cliente_SelectAll", CommandType.StoredProcedure);
        /// Cliente[] clientes = ClienteAdapter.MapAll(dt);
        /// foreach (var cliente in clientes)
        /// {
        ///     Console.WriteLine(cliente.NombreCompleto);
        /// }
        /// </example>
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
