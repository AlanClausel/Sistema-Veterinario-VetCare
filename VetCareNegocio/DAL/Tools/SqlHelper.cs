using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Tools
{
    /// <summary>
    /// Helper para acceso a base de datos VetCareDB
    /// </summary>
    internal static class SqlHelper
    {
        private static readonly string conString;

        static SqlHelper()
        {
            conString = ConfigurationManager.ConnectionStrings["VetCareConString"].ConnectionString;
        }

        /// <summary>
        /// Ejecuta un comando que no retorna datos (INSERT, UPDATE, DELETE)
        /// </summary>
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
        /// Ejecuta un comando y retorna un valor único
        /// </summary>
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
        /// Ejecuta una consulta y retorna un DataReader
        /// </summary>
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
        /// Ejecuta una consulta y retorna un DataTable
        /// </summary>
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
        /// Convierte valores null a DBNull.Value para SQL
        /// </summary>
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
