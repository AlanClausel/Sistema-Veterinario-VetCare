using System;
using System.Data;
using System.Data.SqlClient;
using DAL.Adapters;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;

namespace DAL.Implementations
{
    /// <summary>
    /// Repositorio para la gestión de Consultas Médicas
    /// Implementa el patrón Singleton
    /// </summary>
    public class ConsultaMedicaRepository : IConsultaMedicaRepository
    {
        #region Singleton

        private static readonly ConsultaMedicaRepository _instance = new ConsultaMedicaRepository();
        private static readonly object _lock = new object();

        public static ConsultaMedicaRepository Current
        {
            get { lock (_lock) { return _instance; } }
        }

        private ConsultaMedicaRepository() { }

        #endregion

        public ConsultaMedica Crear(ConsultaMedica consulta)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdConsulta", consulta.IdConsulta),
                new SqlParameter("@IdCita", consulta.IdCita),
                new SqlParameter("@IdVeterinario", consulta.IdVeterinario),
                new SqlParameter("@Sintomas", consulta.Sintomas),
                new SqlParameter("@Diagnostico", consulta.Diagnostico),
                new SqlParameter("@Tratamiento", consulta.Tratamiento ?? (object)DBNull.Value),
                new SqlParameter("@Observaciones", consulta.Observaciones ?? (object)DBNull.Value)
            };

            var dt = SqlHelper.ExecuteDataTable("ConsultaMedica_Insert", CommandType.StoredProcedure, parameters);
            return ConsultaMedicaAdapter.Map(dt.Rows[0]);
        }

        public bool Actualizar(ConsultaMedica consulta)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdConsulta", consulta.IdConsulta),
                new SqlParameter("@Sintomas", consulta.Sintomas),
                new SqlParameter("@Diagnostico", consulta.Diagnostico),
                new SqlParameter("@Tratamiento", consulta.Tratamiento ?? (object)DBNull.Value),
                new SqlParameter("@Observaciones", consulta.Observaciones ?? (object)DBNull.Value)
            };

            int filasAfectadas = SqlHelper.ExecuteNonQuery("ConsultaMedica_Update", CommandType.StoredProcedure, parameters);
            return filasAfectadas > 0;
        }

        public bool Eliminar(Guid idConsulta)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdConsulta", idConsulta)
            };

            int filasAfectadas = SqlHelper.ExecuteNonQuery("ConsultaMedica_Delete", CommandType.StoredProcedure, parameters);
            return filasAfectadas > 0;
        }

        public ConsultaMedica ObtenerPorId(Guid idConsulta)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdConsulta", idConsulta)
            };

            var dt = SqlHelper.ExecuteDataTable("ConsultaMedica_SelectOne", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return ConsultaMedicaAdapter.Map(dt.Rows[0]);
        }

        public ConsultaMedica ObtenerPorCita(Guid idCita)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdCita", idCita)
            };

            var dt = SqlHelper.ExecuteDataTable("ConsultaMedica_SelectByCita", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return ConsultaMedicaAdapter.Map(dt.Rows[0]);
        }

        public ConsultaMedica[] ObtenerPorVeterinario(Guid idVeterinario, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdVeterinario", idVeterinario),
                new SqlParameter("@FechaDesde", fechaDesde.HasValue ? (object)fechaDesde.Value : DBNull.Value),
                new SqlParameter("@FechaHasta", fechaHasta.HasValue ? (object)fechaHasta.Value : DBNull.Value)
            };

            var dt = SqlHelper.ExecuteDataTable("ConsultaMedica_SelectByVeterinario", CommandType.StoredProcedure, parameters);
            return ConsultaMedicaAdapter.MapAll(dt);
        }

        public bool AgregarMedicamento(Guid idConsulta, Guid idMedicamento, int cantidad, string indicaciones)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdConsulta", idConsulta),
                new SqlParameter("@IdMedicamento", idMedicamento),
                new SqlParameter("@Cantidad", cantidad),
                new SqlParameter("@Indicaciones", indicaciones ?? (object)DBNull.Value)
            };

            int filasAfectadas = SqlHelper.ExecuteNonQuery("ConsultaMedicamento_Insert", CommandType.StoredProcedure, parameters);
            return filasAfectadas > 0;
        }

        public bool EliminarMedicamento(Guid idConsulta, Guid idMedicamento)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdConsulta", idConsulta),
                new SqlParameter("@IdMedicamento", idMedicamento)
            };

            int filasAfectadas = SqlHelper.ExecuteNonQuery("ConsultaMedicamento_Delete", CommandType.StoredProcedure, parameters);
            return filasAfectadas > 0;
        }

        public MedicamentoRecetado[] ObtenerMedicamentosPorConsulta(Guid idConsulta)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdConsulta", idConsulta)
            };

            var dt = SqlHelper.ExecuteDataTable("ConsultaMedicamento_SelectByConsulta", CommandType.StoredProcedure, parameters);
            return ConsultaMedicaAdapter.MapAllMedicamentosRecetados(dt);
        }

        public bool FinalizarConsulta(Guid idConsulta, Guid idCita)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdConsulta", idConsulta),
                new SqlParameter("@IdCita", idCita)
            };

            try
            {
                var dt = SqlHelper.ExecuteDataTable("ConsultaMedica_Finalizar", CommandType.StoredProcedure, parameters);
                return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["Resultado"]) == 1;
            }
            catch
            {
                return false;
            }
        }

        public ConsultaMedica[] ObtenerPorMascota(Guid idMascota)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdMascota", idMascota)
            };

            var dt = SqlHelper.ExecuteDataTable("ConsultaMedica_SelectByMascota", CommandType.StoredProcedure, parameters);
            return ConsultaMedicaAdapter.MapAll(dt);
        }
    }
}
