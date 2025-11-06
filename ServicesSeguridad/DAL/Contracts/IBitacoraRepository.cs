using ServicesSecurity.DomainModel.Security;
using System;
using System.Collections.Generic;

namespace ServicesSecurity.DAL.Contracts
{
    /// <summary>
    /// Contrato para el repositorio de Bitacora
    /// La bitácora es principalmente de solo lectura (insert + consultas)
    /// </summary>
    public interface IBitacoraRepository
    {
        /// <summary>
        /// Registra un nuevo evento en la bitácora
        /// </summary>
        void Registrar(Bitacora bitacora);

        /// <summary>
        /// Obtiene todos los registros de bitácora con un límite opcional
        /// </summary>
        /// <param name="top">Cantidad máxima de registros a devolver</param>
        IEnumerable<Bitacora> ObtenerTodos(int top = 1000);

        /// <summary>
        /// Obtiene registros de bitácora aplicando filtros opcionales
        /// </summary>
        IEnumerable<Bitacora> ObtenerPorFiltros(
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            Guid? idUsuario = null,
            string modulo = null,
            string accion = null,
            string criticidad = null,
            int top = 1000);

        /// <summary>
        /// Obtiene los registros de bitácora de un usuario específico
        /// </summary>
        IEnumerable<Bitacora> ObtenerPorUsuario(Guid idUsuario, int top = 100);

        /// <summary>
        /// Obtiene registros de bitácora en un rango de fechas
        /// </summary>
        IEnumerable<Bitacora> ObtenerPorRangoFechas(DateTime fechaDesde, DateTime fechaHasta, int top = 1000);

        /// <summary>
        /// Elimina registros de bitácora anteriores a una fecha (para mantenimiento)
        /// </summary>
        /// <returns>Cantidad de registros eliminados</returns>
        int EliminarAnterioresA(DateTime fechaLimite);

        /// <summary>
        /// Obtiene estadísticas de la bitácora agrupadas por módulo
        /// </summary>
        Dictionary<string, Dictionary<string, int>> ObtenerEstadisticas(
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);
    }
}
