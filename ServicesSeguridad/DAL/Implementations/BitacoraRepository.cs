using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DAL.Implementations.Adapter;
using ServicesSecurity.DAL.Tools;
using ServicesSecurity.DomainModel.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ServicesSecurity.DAL.Implementations
{
    internal class BitacoraRepository : IBitacoraRepository
    {
        #region Singleton
        private readonly static BitacoraRepository _instance = new BitacoraRepository();

        public static BitacoraRepository Current
        {
            get
            {
                return _instance;
            }
        }

        private BitacoraRepository()
        {
            // Initialization code
        }
        #endregion

        public void Registrar(Bitacora bitacora)
        {
            if (bitacora == null)
                throw new ArgumentNullException(nameof(bitacora));

            try
            {
                SqlHelper.ExecuteNonQuery(
                    "Bitacora_Insert",
                    CommandType.StoredProcedure,
                    new SqlParameter[]
                    {
                        new SqlParameter("@IdUsuario", (object)bitacora.IdUsuario ?? DBNull.Value),
                        new SqlParameter("@NombreUsuario", (object)bitacora.NombreUsuario ?? DBNull.Value),
                        new SqlParameter("@Modulo", bitacora.Modulo),
                        new SqlParameter("@Accion", bitacora.Accion),
                        new SqlParameter("@Descripcion", bitacora.Descripcion),
                        new SqlParameter("@Tabla", (object)bitacora.Tabla ?? DBNull.Value),
                        new SqlParameter("@IdRegistro", (object)bitacora.IdRegistro ?? DBNull.Value),
                        new SqlParameter("@Criticidad", bitacora.Criticidad),
                        new SqlParameter("@IP", (object)bitacora.IP ?? DBNull.Value)
                    });
            }
            catch (Exception ex)
            {
                // No usar bitácora aquí para evitar recursión infinita
                // Solo re-lanzar la excepción
                throw new Exception("Error al registrar en bitácora: " + ex.Message, ex);
            }
        }

        public IEnumerable<Bitacora> ObtenerTodos(int top = 1000)
        {
            List<Bitacora> registros = new List<Bitacora>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader(
                    "Bitacora_SelectAll",
                    CommandType.StoredProcedure,
                    new SqlParameter[]
                    {
                        new SqlParameter("@Top", top)
                    }))
                {
                    object[] values = new object[reader.FieldCount];

                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        Bitacora bitacora = BitacoraAdapter.Current.Adapt(values);
                        registros.Add(bitacora);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener registros de bitácora: " + ex.Message, ex);
            }

            return registros;
        }

        public IEnumerable<Bitacora> ObtenerPorFiltros(
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            Guid? idUsuario = null,
            string modulo = null,
            string accion = null,
            string criticidad = null,
            int top = 1000)
        {
            List<Bitacora> registros = new List<Bitacora>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader(
                    "Bitacora_SelectByFiltros",
                    CommandType.StoredProcedure,
                    new SqlParameter[]
                    {
                        new SqlParameter("@FechaDesde", (object)fechaDesde ?? DBNull.Value),
                        new SqlParameter("@FechaHasta", (object)fechaHasta ?? DBNull.Value),
                        new SqlParameter("@IdUsuario", (object)idUsuario ?? DBNull.Value),
                        new SqlParameter("@Modulo", (object)modulo ?? DBNull.Value),
                        new SqlParameter("@Accion", (object)accion ?? DBNull.Value),
                        new SqlParameter("@Criticidad", (object)criticidad ?? DBNull.Value),
                        new SqlParameter("@Top", top)
                    }))
                {
                    object[] values = new object[reader.FieldCount];

                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        Bitacora bitacora = BitacoraAdapter.Current.Adapt(values);
                        registros.Add(bitacora);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener registros filtrados de bitácora: " + ex.Message, ex);
            }

            return registros;
        }

        public IEnumerable<Bitacora> ObtenerPorUsuario(Guid idUsuario, int top = 100)
        {
            List<Bitacora> registros = new List<Bitacora>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader(
                    "Bitacora_SelectByUsuario",
                    CommandType.StoredProcedure,
                    new SqlParameter[]
                    {
                        new SqlParameter("@IdUsuario", idUsuario),
                        new SqlParameter("@Top", top)
                    }))
                {
                    object[] values = new object[reader.FieldCount];

                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        Bitacora bitacora = BitacoraAdapter.Current.Adapt(values);
                        registros.Add(bitacora);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener registros de bitácora por usuario: " + ex.Message, ex);
            }

            return registros;
        }

        public IEnumerable<Bitacora> ObtenerPorRangoFechas(DateTime fechaDesde, DateTime fechaHasta, int top = 1000)
        {
            List<Bitacora> registros = new List<Bitacora>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader(
                    "Bitacora_SelectByRangoFechas",
                    CommandType.StoredProcedure,
                    new SqlParameter[]
                    {
                        new SqlParameter("@FechaDesde", fechaDesde),
                        new SqlParameter("@FechaHasta", fechaHasta),
                        new SqlParameter("@Top", top)
                    }))
                {
                    object[] values = new object[reader.FieldCount];

                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        Bitacora bitacora = BitacoraAdapter.Current.Adapt(values);
                        registros.Add(bitacora);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener registros de bitácora por rango de fechas: " + ex.Message, ex);
            }

            return registros;
        }

        public int EliminarAnterioresA(DateTime fechaLimite)
        {
            try
            {
                object result = SqlHelper.ExecuteScalar(
                    "Bitacora_DeleteOlderThan",
                    CommandType.StoredProcedure,
                    new SqlParameter[]
                    {
                        new SqlParameter("@FechaLimite", fechaLimite)
                    });

                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar registros antiguos de bitácora: " + ex.Message, ex);
            }
        }

        public Dictionary<string, Dictionary<string, int>> ObtenerEstadisticas(
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            Dictionary<string, Dictionary<string, int>> estadisticas = new Dictionary<string, Dictionary<string, int>>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader(
                    "Bitacora_GetEstadisticas",
                    CommandType.StoredProcedure,
                    new SqlParameter[]
                    {
                        new SqlParameter("@FechaDesde", (object)fechaDesde ?? DBNull.Value),
                        new SqlParameter("@FechaHasta", (object)fechaHasta ?? DBNull.Value)
                    }))
                {
                    while (reader.Read())
                    {
                        string modulo = reader["Modulo"].ToString();
                        Dictionary<string, int> stats = new Dictionary<string, int>
                        {
                            { "CantidadEventos", Convert.ToInt32(reader["CantidadEventos"]) },
                            { "EventosCriticos", Convert.ToInt32(reader["EventosCriticos"]) },
                            { "EventosError", Convert.ToInt32(reader["EventosError"]) },
                            { "EventosAdvertencia", Convert.ToInt32(reader["EventosAdvertencia"]) },
                            { "EventosInfo", Convert.ToInt32(reader["EventosInfo"]) }
                        };

                        estadisticas[modulo] = stats;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener estadísticas de bitácora: " + ex.Message, ex);
            }

            return estadisticas;
        }
    }
}
