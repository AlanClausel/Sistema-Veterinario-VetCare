using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Tools
{
    /// <summary>
    /// Clase de utilidad estática para acceso a la base de datos VetCareDB.
    /// Proporciona métodos optimizados para ejecutar comandos SQL y stored procedures.
    /// Gestiona automáticamente la apertura/cierre de conexiones y conversión de valores null.
    /// </summary>
    /// <remarks>
    /// IMPORTANTE:
    /// - Usa la cadena de conexión "VetCareConString" del App.config
    /// - Todos los métodos están optimizados para usar stored procedures
    /// - Implementa el patrón using para garantizar la liberación de recursos
    /// - Convierte automáticamente null a DBNull.Value
    /// - Es thread-safe (todos los métodos son estáticos y sin estado)
    /// </remarks>
    internal static class SqlHelper
    {
        /// <summary>
        /// Cadena de conexión a la base de datos VetCareDB obtenida del archivo de configuración
        /// </summary>
        private static readonly string conString;

        /// <summary>
        /// Constructor estático. Inicializa la cadena de conexión desde App.config al cargar la clase.
        /// </summary>
        /// <exception cref="ConfigurationErrorsException">Si no existe la cadena de conexión "VetCareConString"</exception>
        static SqlHelper()
        {
            conString = ConfigurationManager.ConnectionStrings["VetCareConString"].ConnectionString;
        }

        /// <summary>
        /// Ejecuta un comando SQL que no retorna datos (INSERT, UPDATE, DELETE).
        /// Ideal para operaciones de modificación de datos mediante stored procedures.
        /// Gestiona automáticamente la conexión y convierte valores null.
        /// </summary>
        /// <param name="commandText">Nombre del stored procedure o comando SQL</param>
        /// <param name="commandType">Tipo de comando (StoredProcedure o Text)</param>
        /// <param name="parameters">Parámetros SQL del comando (opcionales)</param>
        /// <returns>Número de filas afectadas por el comando</returns>
        /// <exception cref="SqlException">Si ocurre un error de SQL durante la ejecución</exception>
        /// <example>
        /// int filas = SqlHelper.ExecuteNonQuery("Cliente_Delete",
        ///     CommandType.StoredProcedure,
        ///     new SqlParameter("@IdCliente", idCliente));
        /// </example>
        public static int ExecuteNonQuery(string commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            CheckNullables(parameters);

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Ejecuta un comando SQL y retorna un único valor escalar (primera columna de la primera fila).
        /// Útil para consultas agregadas (COUNT, SUM, MAX) o para obtener IDs generados.
        /// Gestiona automáticamente la conexión y convierte valores null.
        /// </summary>
        /// <param name="commandText">Nombre del stored procedure o comando SQL</param>
        /// <param name="commandType">Tipo de comando (StoredProcedure o Text)</param>
        /// <param name="parameters">Parámetros SQL del comando (opcionales)</param>
        /// <returns>Valor escalar resultante, o null si no hay resultados</returns>
        /// <exception cref="SqlException">Si ocurre un error de SQL durante la ejecución</exception>
        /// <example>
        /// object count = SqlHelper.ExecuteScalar("SELECT COUNT(*) FROM Cliente", CommandType.Text);
        /// int totalClientes = Convert.ToInt32(count);
        /// </example>
        public static object ExecuteScalar(string commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            CheckNullables(parameters);

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL y retorna un SqlDataReader para lectura secuencial de datos.
        /// Ideal para procesar grandes volúmenes de datos de forma eficiente (streaming).
        /// La conexión permanece abierta y se cierra automáticamente al cerrar el DataReader.
        /// </summary>
        /// <param name="commandText">Nombre del stored procedure o comando SQL</param>
        /// <param name="commandType">Tipo de comando (StoredProcedure o Text)</param>
        /// <param name="parameters">Parámetros SQL del comando (opcionales)</param>
        /// <returns>SqlDataReader con los resultados (debe ser cerrado por el llamador)</returns>
        /// <exception cref="SqlException">Si ocurre un error de SQL durante la ejecución</exception>
        /// <remarks>
        /// IMPORTANTE: El DataReader debe ser cerrado/disposed para liberar la conexión.
        /// Se usa CommandBehavior.CloseConnection para cerrar automáticamente la conexión.
        /// </remarks>
        /// <example>
        /// using (var reader = SqlHelper.ExecuteReader("Cliente_SelectAll", CommandType.StoredProcedure))
        /// {
        ///     while (reader.Read())
        ///     {
        ///         // Procesar cada fila
        ///     }
        /// } // La conexión se cierra automáticamente aquí
        /// </example>
        public static SqlDataReader ExecuteReader(string commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            CheckNullables(parameters);

            SqlConnection conn = new SqlConnection(conString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                conn.Open();
                // CloseConnection cierra la conexión cuando se cierra el DataReader
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL y retorna un DataTable con todos los resultados.
        /// Ideal para consultas que retornan conjuntos de datos completos de tamaño moderado.
        /// Gestiona automáticamente la conexión, convierte valores null y carga todos los datos en memoria.
        /// </summary>
        /// <param name="commandText">Nombre del stored procedure o comando SQL</param>
        /// <param name="commandType">Tipo de comando (StoredProcedure o Text)</param>
        /// <param name="parameters">Parámetros SQL del comando (opcionales)</param>
        /// <returns>DataTable con todas las filas resultantes (puede estar vacío si no hay datos)</returns>
        /// <exception cref="SqlException">Si ocurre un error de SQL durante la ejecución</exception>
        /// <remarks>
        /// Este método carga todos los resultados en memoria. Para grandes volúmenes, considerar ExecuteReader.
        /// </remarks>
        /// <example>
        /// DataTable dt = SqlHelper.ExecuteDataTable("Cliente_SelectAll", CommandType.StoredProcedure);
        /// foreach (DataRow row in dt.Rows)
        /// {
        ///     var cliente = ClienteAdapter.Map(row);
        /// }
        /// </example>
        public static DataTable ExecuteDataTable(string commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            CheckNullables(parameters);

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Convierte valores null de C# a DBNull.Value para compatibilidad con SQL Server.
        /// Se ejecuta automáticamente antes de cada comando para evitar errores de tipos.
        /// </summary>
        /// <param name="parameters">Array de parámetros SQL a validar y convertir</param>
        /// <remarks>
        /// Este método asegura que los parámetros con valor null se conviertan correctamente
        /// a DBNull.Value, que es el valor esperado por SQL Server para campos NULL.
        /// </remarks>
        private static void CheckNullables(SqlParameter[] parameters)
        {
            if (parameters == null)
                return;

            foreach (SqlParameter item in parameters)
            {
                if (item.Value == null)
                {
                    item.Value = DBNull.Value;
                }
            }
        }
    }
}
