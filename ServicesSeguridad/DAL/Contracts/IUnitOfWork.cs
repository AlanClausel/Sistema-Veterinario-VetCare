using System;
using System.Data.SqlClient;

namespace ServicesSecurity.DAL.Contracts
{
    /// <summary>
    /// Patrón Unit of Work para coordinar múltiples operaciones en una transacción atómica
    /// Permite ejecutar múltiples operaciones de repositorio dentro de una sola transacción
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Obtiene la conexión SQL actual de la unidad de trabajo
        /// </summary>
        SqlConnection Connection { get; }

        /// <summary>
        /// Obtiene la transacción SQL actual de la unidad de trabajo
        /// </summary>
        SqlTransaction Transaction { get; }

        /// <summary>
        /// Inicia una nueva transacción
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Confirma todas las operaciones de la transacción y persiste los cambios
        /// </summary>
        void Commit();

        /// <summary>
        /// Revierte todas las operaciones de la transacción y descarta los cambios
        /// </summary>
        void Rollback();
    }
}
