using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using System.Linq;
using DAL.Adapters;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;
using ServicesSecurity.Services;

namespace DAL.Implementations
{
    /// <summary>
    /// Implementación del repositorio de Citas usando Singleton pattern
    /// </summary>
    public sealed class CitaRepository : ICitaRepository
    {
        #region Singleton

        private static readonly CitaRepository _instance = new CitaRepository();
        private static readonly object _lock = new object();

        private CitaRepository() { }

        public static CitaRepository Current
        {
            get
            {
                lock (_lock)
                {
                    return _instance;
                }
            }
        }

        #endregion

        #region Implementación ICitaRepository

        public Guid Insert(Cita cita)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@IdCita", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output },
                    new SqlParameter("@IdMascota", cita.IdMascota),
                    new SqlParameter("@FechaCita", cita.FechaCita),
                    new SqlParameter("@TipoConsulta", cita.TipoConsulta ?? string.Empty),
                    new SqlParameter("@IdVeterinario", cita.IdVeterinario.HasValue ? (object)cita.IdVeterinario.Value : DBNull.Value),
                    new SqlParameter("@Veterinario", cita.Veterinario ?? string.Empty),
                    new SqlParameter("@Estado", CitaAdapter.EstadoToString(cita.Estado)),
                    new SqlParameter("@Observaciones", (object)cita.Observaciones ?? DBNull.Value)
                };

                SqlHelper.ExecuteNonQuery("SP_Cita_Insert", CommandType.StoredProcedure, parameters.ToArray());

                var idCita = (Guid)parameters[0].Value;

                LoggerService.WriteLog($"Cita creada: {idCita} - Mascota: {cita.IdMascota} - Veterinario ID: {cita.IdVeterinario} - Fecha: {cita.FechaCita}", EventLevel.Informational, string.Empty);

                return idCita;
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al insertar cita: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al crear la cita en la base de datos", ex);
            }
        }

        public bool Update(Cita cita)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@RETURN_VALUE", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue },
                    new SqlParameter("@IdCita", cita.IdCita),
                    new SqlParameter("@IdMascota", cita.IdMascota),
                    new SqlParameter("@FechaCita", cita.FechaCita),
                    new SqlParameter("@TipoConsulta", cita.TipoConsulta ?? string.Empty),
                    new SqlParameter("@IdVeterinario", cita.IdVeterinario.HasValue ? (object)cita.IdVeterinario.Value : DBNull.Value),
                    new SqlParameter("@Veterinario", cita.Veterinario ?? string.Empty),
                    new SqlParameter("@Estado", CitaAdapter.EstadoToString(cita.Estado)),
                    new SqlParameter("@Observaciones", (object)cita.Observaciones ?? DBNull.Value)
                };

                SqlHelper.ExecuteNonQuery("SP_Cita_Update", CommandType.StoredProcedure, parameters.ToArray());

                // Leer el valor de RETURN del stored procedure
                int rowsAffected = (int)parameters[0].Value;

                if (rowsAffected > 0)
                    LoggerService.WriteLog($"Cita actualizada: {cita.IdCita} - Veterinario ID: {cita.IdVeterinario}", EventLevel.Informational, string.Empty);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al actualizar cita {cita.IdCita}: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al actualizar la cita en la base de datos", ex);
            }
        }

        public bool UpdateEstado(Guid idCita, EstadoCita estado)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@RETURN_VALUE", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue },
                    new SqlParameter("@IdCita", idCita),
                    new SqlParameter("@Estado", CitaAdapter.EstadoToString(estado))
                };

                SqlHelper.ExecuteNonQuery("SP_Cita_UpdateEstado", CommandType.StoredProcedure, parameters.ToArray());

                // Leer el valor de RETURN del stored procedure
                int rowsAffected = (int)parameters[0].Value;

                if (rowsAffected > 0)
                    LoggerService.WriteLog($"Estado de cita actualizado: {idCita} - Nuevo estado: {estado}", EventLevel.Informational, string.Empty);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al actualizar estado de cita {idCita}: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al actualizar el estado de la cita", ex);
            }
        }

        public bool Delete(Guid idCita)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@RETURN_VALUE", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue },
                    new SqlParameter("@IdCita", idCita)
                };

                SqlHelper.ExecuteNonQuery("SP_Cita_Delete", CommandType.StoredProcedure, parameters.ToArray());

                // Leer el valor de RETURN del stored procedure
                int rowsAffected = (int)parameters[0].Value;

                if (rowsAffected > 0)
                    LoggerService.WriteLog($"Cita eliminada (soft delete): {idCita}", EventLevel.Informational, string.Empty);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al eliminar cita {idCita}: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al eliminar la cita", ex);
            }
        }

        public Cita SelectOne(Guid idCita)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@IdCita", idCita)
                };

                DataTable dt = SqlHelper.ExecuteDataTable("SP_Cita_SelectOne", CommandType.StoredProcedure, parameters.ToArray());

                if (dt != null && dt.Rows.Count > 0)
                {
                    return CitaAdapter.DataRowToCita(dt.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al obtener cita {idCita}: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al obtener la cita", ex);
            }
        }

        public List<Cita> SelectAll()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable("SP_Cita_SelectAll", CommandType.StoredProcedure);
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al obtener todas las citas: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al obtener las citas", ex);
            }
        }

        public List<Cita> SelectByMascota(Guid idMascota)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@IdMascota", idMascota)
                };

                DataTable dt = SqlHelper.ExecuteDataTable("SP_Cita_SelectByMascota", CommandType.StoredProcedure, parameters.ToArray());
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al obtener citas de mascota {idMascota}: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al obtener las citas de la mascota", ex);
            }
        }

        public List<Cita> SelectByCliente(Guid idCliente)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@IdCliente", idCliente)
                };

                DataTable dt = SqlHelper.ExecuteDataTable("SP_Cita_SelectByCliente", CommandType.StoredProcedure, parameters.ToArray());
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al obtener citas de cliente {idCliente}: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al obtener las citas del cliente", ex);
            }
        }

        public List<Cita> SelectByFecha(DateTime fecha)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Fecha", fecha.Date)
                };

                DataTable dt = SqlHelper.ExecuteDataTable("SP_Cita_SelectByFecha", CommandType.StoredProcedure, parameters.ToArray());
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al obtener citas por fecha {fecha:yyyy-MM-dd}: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al obtener las citas por fecha", ex);
            }
        }

        public List<Cita> SelectByVeterinario(string veterinario)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Veterinario", veterinario)
                };

                DataTable dt = SqlHelper.ExecuteDataTable("SP_Cita_SelectByVeterinario", CommandType.StoredProcedure, parameters.ToArray());
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al obtener citas del veterinario {veterinario}: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al obtener las citas del veterinario", ex);
            }
        }

        public List<Cita> SelectByEstado(EstadoCita estado)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Estado", CitaAdapter.EstadoToString(estado))
                };

                DataTable dt = SqlHelper.ExecuteDataTable("SP_Cita_SelectByEstado", CommandType.StoredProcedure, parameters.ToArray());
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al obtener citas por estado {estado}: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al obtener las citas por estado", ex);
            }
        }

        public List<Cita> Search(DateTime? fecha = null, string veterinario = null, EstadoCita? estado = null)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Fecha", fecha.HasValue ? (object)fecha.Value.Date : DBNull.Value),
                    new SqlParameter("@Veterinario", string.IsNullOrWhiteSpace(veterinario) ? (object)DBNull.Value : veterinario),
                    new SqlParameter("@Estado", estado.HasValue ? (object)CitaAdapter.EstadoToString(estado.Value) : DBNull.Value)
                };

                DataTable dt = SqlHelper.ExecuteDataTable("SP_Cita_Search", CommandType.StoredProcedure, parameters.ToArray());
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al buscar citas: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al buscar citas", ex);
            }
        }

        public List<Cita> SelectByFechaRango(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@FechaDesde", fechaDesde.Date),
                    new SqlParameter("@FechaHasta", fechaHasta.Date)
                };

                DataTable dt = SqlHelper.ExecuteDataTable("SP_Cita_SelectByFechaRango", CommandType.StoredProcedure, parameters.ToArray());
                return ConvertDataTableToList(dt);
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al obtener citas por rango de fechas {fechaDesde:yyyy-MM-dd} a {fechaHasta:yyyy-MM-dd}: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                throw new InvalidOperationException("Error al obtener las citas por rango de fechas", ex);
            }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Convierte un DataTable en una lista de Citas
        /// </summary>
        private List<Cita> ConvertDataTableToList(DataTable dt)
        {
            var citas = new List<Cita>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        var cita = CitaAdapter.DataRowToCita(row);
                        if (cita != null)
                            citas.Add(cita);
                    }
                    catch (Exception ex)
                    {
                        LoggerService.WriteLog($"Error al convertir fila a Cita: {ex.Message}", EventLevel.Warning, string.Empty);
                        // Continuar con la siguiente fila
                    }
                }
            }

            return citas;
        }

        #endregion
    }
}
