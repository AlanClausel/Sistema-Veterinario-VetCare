using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ServicesSecurity.DAL.Contracts;

namespace ServicesSecurity.DAL.Implementations
{
    /// <summary>
    /// Implementación del patrón Unit of Work para la base de datos SecurityVet
    /// Coordina múltiples operaciones de repositorio en una sola transacción
    /// </summary>
    public class SecurityUnitOfWork : IUnitOfWork
    {
        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private bool _disposed = false;

        public SqlConnection Connection
        {
            get { return _connection; }
        }

        public SqlTransaction Transaction
        {
            get { return _transaction; }
        }

        /// <summary>
        /// Constructor que inicializa la conexión a SecurityVet
        /// </summary>
        public SecurityUnitOfWork()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ServicesConString"].ConnectionString;
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        /// <summary>
        /// Inicia una nueva transacción
        /// </summary>
        public void BeginTransaction()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Ya existe una transacción activa en esta Unit of Work");

            _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        /// Confirma la transacción y persiste todos los cambios
        /// </summary>
        public void Commit()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No hay una transacción activa para confirmar");

            try
            {
                _transaction.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Revierte la transacción y descarta todos los cambios
        /// </summary>
        public void Rollback()
        {
            if (_transaction == null)
                return;

            try
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Libera los recursos de la conexión y transacción
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    if (_connection != null)
                    {
                        if (_connection.State == ConnectionState.Open)
                            _connection.Close();
                        _connection.Dispose();
                        _connection = null;
                    }
                }

                _disposed = true;
            }
        }
    }
}
