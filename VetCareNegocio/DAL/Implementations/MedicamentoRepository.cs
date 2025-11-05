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
    /// Repositorio para la gestión de Medicamentos
    /// Implementa el patrón Singleton
    /// </summary>
    public class MedicamentoRepository : IMedicamentoRepository
    {
        #region Singleton

        private static readonly MedicamentoRepository _instance = new MedicamentoRepository();
        private static readonly object _lock = new object();

        public static MedicamentoRepository Current
        {
            get { lock (_lock) { return _instance; } }
        }

        private MedicamentoRepository() { }

        #endregion

        public Medicamento Crear(Medicamento medicamento)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdMedicamento", medicamento.IdMedicamento),
                new SqlParameter("@Nombre", medicamento.Nombre),
                new SqlParameter("@Presentacion", medicamento.Presentacion ?? (object)DBNull.Value),
                new SqlParameter("@Stock", medicamento.Stock),
                new SqlParameter("@PrecioUnitario", medicamento.PrecioUnitario),
                new SqlParameter("@Observaciones", medicamento.Observaciones ?? (object)DBNull.Value)
            };

            var dt = SqlHelper.ExecuteDataTable("Medicamento_Insert", CommandType.StoredProcedure, parameters);
            return MedicamentoAdapter.Map(dt.Rows[0]);
        }

        public bool Actualizar(Medicamento medicamento)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdMedicamento", medicamento.IdMedicamento),
                new SqlParameter("@Nombre", medicamento.Nombre),
                new SqlParameter("@Presentacion", medicamento.Presentacion ?? (object)DBNull.Value),
                new SqlParameter("@Stock", medicamento.Stock),
                new SqlParameter("@PrecioUnitario", medicamento.PrecioUnitario),
                new SqlParameter("@Observaciones", medicamento.Observaciones ?? (object)DBNull.Value),
                new SqlParameter("@Activo", medicamento.Activo)
            };

            int filasAfectadas = SqlHelper.ExecuteNonQuery("Medicamento_Update", CommandType.StoredProcedure, parameters);
            return filasAfectadas > 0;
        }

        public bool Eliminar(Guid idMedicamento)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdMedicamento", idMedicamento)
            };

            int filasAfectadas = SqlHelper.ExecuteNonQuery("Medicamento_Delete", CommandType.StoredProcedure, parameters);
            return filasAfectadas > 0;
        }

        public Medicamento ObtenerPorId(Guid idMedicamento)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdMedicamento", idMedicamento)
            };

            var dt = SqlHelper.ExecuteDataTable("Medicamento_SelectOne", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MedicamentoAdapter.Map(dt.Rows[0]);
        }

        public Medicamento[] ObtenerTodos()
        {
            var dt = SqlHelper.ExecuteDataTable("Medicamento_SelectAll", CommandType.StoredProcedure);
            return MedicamentoAdapter.MapAll(dt);
        }

        public Medicamento[] Buscar(string criterio)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Criterio", criterio ?? string.Empty)
            };

            var dt = SqlHelper.ExecuteDataTable("Medicamento_Search", CommandType.StoredProcedure, parameters);
            return MedicamentoAdapter.MapAll(dt);
        }

        public Medicamento[] ObtenerDisponibles()
        {
            var dt = SqlHelper.ExecuteDataTable("Medicamento_SelectDisponibles", CommandType.StoredProcedure);
            return MedicamentoAdapter.MapAll(dt);
        }

        public Medicamento ActualizarStock(Guid idMedicamento, int cantidadCambio)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdMedicamento", idMedicamento),
                new SqlParameter("@CantidadCambio", cantidadCambio)
            };

            var dt = SqlHelper.ExecuteDataTable("Medicamento_ActualizarStock", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MedicamentoAdapter.Map(dt.Rows[0]);
        }
    }
}
