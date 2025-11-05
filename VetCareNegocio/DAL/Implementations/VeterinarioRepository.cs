using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Adapters;
using DAL.Contracts;
using DAL.Tools;
using DomainModel;

namespace DAL.Implementations
{
    /// <summary>
    /// Implementación del repositorio de Veterinarios
    /// Usa patrón Singleton y Stored Procedures
    ///
    /// IMPORTANTE:
    /// - Los veterinarios se sincronizan desde SecurityVet
    /// - IdVeterinario coincide con IdUsuario de SecurityVet
    /// - El campo Nombre se sincroniza automáticamente
    /// </summary>
    public class VeterinarioRepository : IVeterinarioRepository
    {
        #region Singleton

        private static readonly VeterinarioRepository _instance = new VeterinarioRepository();

        public static VeterinarioRepository Current
        {
            get { return _instance; }
        }

        private VeterinarioRepository()
        {
            // Constructor privado para Singleton
        }

        #endregion

        #region Implementación IVeterinarioRepository

        public Veterinario Crear(Veterinario veterinario)
        {
            if (veterinario == null)
                throw new ArgumentNullException(nameof(veterinario));

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdVeterinario", veterinario.IdVeterinario),
                new SqlParameter("@Nombre", veterinario.Nombre),
                new SqlParameter("@Matricula", (object)veterinario.Matricula ?? DBNull.Value),
                new SqlParameter("@Telefono", (object)veterinario.Telefono ?? DBNull.Value),
                new SqlParameter("@Email", (object)veterinario.Email ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)veterinario.Observaciones ?? DBNull.Value),
                new SqlParameter("@Activo", veterinario.Activo)
            };

            var dt = SqlHelper.ExecuteDataTable("Veterinario_Insert", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return VeterinarioAdapter.Map(dt.Rows[0]);

            return null;
        }

        public Veterinario Actualizar(Veterinario veterinario)
        {
            if (veterinario == null)
                throw new ArgumentNullException(nameof(veterinario));

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdVeterinario", veterinario.IdVeterinario),
                new SqlParameter("@Nombre", veterinario.Nombre),
                new SqlParameter("@Matricula", (object)veterinario.Matricula ?? DBNull.Value),
                new SqlParameter("@Telefono", (object)veterinario.Telefono ?? DBNull.Value),
                new SqlParameter("@Email", (object)veterinario.Email ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)veterinario.Observaciones ?? DBNull.Value),
                new SqlParameter("@Activo", veterinario.Activo)
            };

            var dt = SqlHelper.ExecuteDataTable("Veterinario_Update", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return VeterinarioAdapter.Map(dt.Rows[0]);

            return null;
        }

        public void Eliminar(Guid idVeterinario)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdVeterinario", idVeterinario)
            };

            // Soft delete - marca como inactivo
            SqlHelper.ExecuteNonQuery("Veterinario_Delete", CommandType.StoredProcedure, parameters);
        }

        public Veterinario ObtenerPorId(Guid idVeterinario)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdVeterinario", idVeterinario)
            };

            var dt = SqlHelper.ExecuteDataTable("Veterinario_SelectOne", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return VeterinarioAdapter.Map(dt.Rows[0]);

            return null;
        }

        public IEnumerable<Veterinario> ObtenerTodos()
        {
            var dt = SqlHelper.ExecuteDataTable("Veterinario_SelectAll", CommandType.StoredProcedure);
            return VeterinarioAdapter.MapAll(dt);
        }

        public IEnumerable<Veterinario> ObtenerActivos()
        {
            var dt = SqlHelper.ExecuteDataTable("Veterinario_SelectActivos", CommandType.StoredProcedure);
            return VeterinarioAdapter.MapAll(dt);
        }

        public Veterinario SincronizarNombre(Guid idVeterinario, string nombreActualizado)
        {
            if (string.IsNullOrWhiteSpace(nombreActualizado))
                throw new ArgumentException("El nombre actualizado no puede estar vacío", nameof(nombreActualizado));

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdVeterinario", idVeterinario),
                new SqlParameter("@NombreActualizado", nombreActualizado)
            };

            var dt = SqlHelper.ExecuteDataTable("Veterinario_SincronizarNombre", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return VeterinarioAdapter.Map(dt.Rows[0]);

            return null;
        }

        public bool Existe(Guid idVeterinario)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdVeterinario", idVeterinario)
            };

            var result = SqlHelper.ExecuteScalar("Veterinario_Existe", CommandType.StoredProcedure, parameters);

            if (result != null && result != DBNull.Value)
                return Convert.ToBoolean(result);

            return false;
        }

        public bool ExistePorMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
                return false;

            // Buscar en todos los veterinarios
            var veterinarios = ObtenerTodos();

            foreach (var vet in veterinarios)
            {
                if (!string.IsNullOrWhiteSpace(vet.Matricula) &&
                    vet.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ExistePorMatricula(string matricula, Guid idVeterinarioExcluir)
        {
            if (string.IsNullOrWhiteSpace(matricula))
                return false;

            // Buscar en todos los veterinarios excluyendo el ID especificado
            var veterinarios = ObtenerTodos();

            foreach (var vet in veterinarios)
            {
                if (vet.IdVeterinario != idVeterinarioExcluir &&
                    !string.IsNullOrWhiteSpace(vet.Matricula) &&
                    vet.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
