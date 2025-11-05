using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DAL.Adapters;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;

namespace DAL.Implementations
{
    /// <summary>
    /// Implementación del repositorio de Clientes
    /// Usa patrón Singleton y Stored Procedures
    /// </summary>
    public class ClienteRepository : IClienteRepository
    {
        #region Singleton

        private static readonly ClienteRepository _instance = new ClienteRepository();

        public static ClienteRepository Current
        {
            get { return _instance; }
        }

        private ClienteRepository()
        {
            // Constructor privado para Singleton
        }

        #endregion

        #region Implementación IClienteRepository

        public Cliente Crear(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", cliente.IdCliente),
                new SqlParameter("@Nombre", cliente.Nombre),
                new SqlParameter("@Apellido", cliente.Apellido),
                new SqlParameter("@DNI", cliente.DNI),
                new SqlParameter("@Telefono", (object)cliente.Telefono ?? DBNull.Value),
                new SqlParameter("@Email", (object)cliente.Email ?? DBNull.Value),
                new SqlParameter("@Direccion", (object)cliente.Direccion ?? DBNull.Value),
                new SqlParameter("@Activo", cliente.Activo)
            };

            var dt = SqlHelper.ExecuteDataTable("Cliente_Insert", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return ClienteAdapter.Map(dt.Rows[0]);

            return null;
        }

        public Cliente Actualizar(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", cliente.IdCliente),
                new SqlParameter("@Nombre", cliente.Nombre),
                new SqlParameter("@Apellido", cliente.Apellido),
                new SqlParameter("@DNI", cliente.DNI),
                new SqlParameter("@Telefono", (object)cliente.Telefono ?? DBNull.Value),
                new SqlParameter("@Email", (object)cliente.Email ?? DBNull.Value),
                new SqlParameter("@Direccion", (object)cliente.Direccion ?? DBNull.Value),
                new SqlParameter("@Activo", cliente.Activo)
            };

            var dt = SqlHelper.ExecuteDataTable("Cliente_Update", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return ClienteAdapter.Map(dt.Rows[0]);

            return null;
        }

        public void Eliminar(Guid idCliente)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", idCliente)
            };

            SqlHelper.ExecuteNonQuery("Cliente_Delete", CommandType.StoredProcedure, parameters);
        }

        public Cliente ObtenerPorId(Guid idCliente)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", idCliente)
            };

            var dt = SqlHelper.ExecuteDataTable("Cliente_SelectOne", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return ClienteAdapter.Map(dt.Rows[0]);

            return null;
        }

        public Cliente ObtenerPorDNI(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                return null;

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@DNI", dni)
            };

            var dt = SqlHelper.ExecuteDataTable("Cliente_SelectByDNI", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return ClienteAdapter.Map(dt.Rows[0]);

            return null;
        }

        public IEnumerable<Cliente> ObtenerTodos()
        {
            var dt = SqlHelper.ExecuteDataTable("Cliente_SelectAll", CommandType.StoredProcedure);
            return ClienteAdapter.MapAll(dt);
        }

        public IEnumerable<Cliente> BuscarPorCriterio(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return ObtenerTodos();

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Criterio", criterio)
            };

            var dt = SqlHelper.ExecuteDataTable("Cliente_Search", CommandType.StoredProcedure, parameters);
            return ClienteAdapter.MapAll(dt);
        }

        public bool ExistePorDNI(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                return false;

            var cliente = ObtenerPorDNI(dni);
            return cliente != null;
        }

        public bool ExistePorDNI(string dni, Guid idClienteExcluir)
        {
            if (string.IsNullOrWhiteSpace(dni))
                return false;

            var cliente = ObtenerPorDNI(dni);
            return cliente != null && cliente.IdCliente != idClienteExcluir;
        }

        #endregion
    }
}
