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
    /// Implementación del repositorio de Mascotas
    /// Usa patrón Singleton y Stored Procedures
    /// </summary>
    public class MascotaRepository : IMascotaRepository
    {
        #region Singleton

        private static readonly MascotaRepository _instance = new MascotaRepository();

        public static MascotaRepository Current
        {
            get { return _instance; }
        }

        private MascotaRepository()
        {
            // Constructor privado para Singleton
        }

        #endregion

        #region Implementación IMascotaRepository

        public Mascota Crear(Mascota mascota)
        {
            if (mascota == null)
                throw new ArgumentNullException(nameof(mascota));

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdMascota", mascota.IdMascota),
                new SqlParameter("@IdCliente", mascota.IdCliente),
                new SqlParameter("@Nombre", mascota.Nombre),
                new SqlParameter("@Especie", mascota.Especie),
                new SqlParameter("@Raza", (object)mascota.Raza ?? DBNull.Value),
                new SqlParameter("@FechaNacimiento", mascota.FechaNacimiento),
                new SqlParameter("@Sexo", mascota.Sexo),
                new SqlParameter("@Peso", mascota.Peso > 0 ? (object)mascota.Peso : DBNull.Value),
                new SqlParameter("@Color", (object)mascota.Color ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)mascota.Observaciones ?? DBNull.Value),
                new SqlParameter("@Activo", mascota.Activo)
            };

            var dt = SqlHelper.ExecuteDataTable("Mascota_Insert", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return MascotaAdapter.Map(dt.Rows[0]);

            return null;
        }

        public Mascota Actualizar(Mascota mascota)
        {
            if (mascota == null)
                throw new ArgumentNullException(nameof(mascota));

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdMascota", mascota.IdMascota),
                new SqlParameter("@IdCliente", mascota.IdCliente),
                new SqlParameter("@Nombre", mascota.Nombre),
                new SqlParameter("@Especie", mascota.Especie),
                new SqlParameter("@Raza", (object)mascota.Raza ?? DBNull.Value),
                new SqlParameter("@FechaNacimiento", mascota.FechaNacimiento),
                new SqlParameter("@Sexo", mascota.Sexo),
                new SqlParameter("@Peso", mascota.Peso > 0 ? (object)mascota.Peso : DBNull.Value),
                new SqlParameter("@Color", (object)mascota.Color ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)mascota.Observaciones ?? DBNull.Value),
                new SqlParameter("@Activo", mascota.Activo)
            };

            var dt = SqlHelper.ExecuteDataTable("Mascota_Update", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return MascotaAdapter.Map(dt.Rows[0]);

            return null;
        }

        public void Eliminar(Guid idMascota)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdMascota", idMascota)
            };

            SqlHelper.ExecuteNonQuery("Mascota_Delete", CommandType.StoredProcedure, parameters);
        }

        public Mascota ObtenerPorId(Guid idMascota)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdMascota", idMascota)
            };

            var dt = SqlHelper.ExecuteDataTable("Mascota_SelectOne", CommandType.StoredProcedure, parameters);

            if (dt.Rows.Count > 0)
                return MascotaAdapter.Map(dt.Rows[0]);

            return null;
        }

        public IEnumerable<Mascota> ObtenerPorCliente(Guid idCliente)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", idCliente)
            };

            var dt = SqlHelper.ExecuteDataTable("Mascota_SelectByCliente", CommandType.StoredProcedure, parameters);
            return MascotaAdapter.MapAll(dt);
        }

        public IEnumerable<Mascota> ObtenerActivasPorCliente(Guid idCliente)
        {
            var mascotas = ObtenerPorCliente(idCliente);
            return mascotas.Where(m => m.Activo);
        }

        public IEnumerable<Mascota> ObtenerTodas()
        {
            var dt = SqlHelper.ExecuteDataTable("Mascota_SelectAll", CommandType.StoredProcedure);
            return MascotaAdapter.MapAll(dt);
        }

        public IEnumerable<Mascota> BuscarPorCriterio(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return ObtenerTodas();

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Criterio", criterio)
            };

            var dt = SqlHelper.ExecuteDataTable("Mascota_Search", CommandType.StoredProcedure, parameters);
            return MascotaAdapter.MapAll(dt);
        }

        public int ContarPorCliente(Guid idCliente)
        {
            var mascotas = ObtenerPorCliente(idCliente);
            return mascotas.Count();
        }

        #endregion
    }
}
