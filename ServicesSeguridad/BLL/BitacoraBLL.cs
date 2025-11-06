using ServicesSecurity.DAL.Implementations;
using ServicesSecurity.DomainModel.Security;
using ServicesSecurity.Services;
using System;
using System.Collections.Generic;
using BitacoraEntity = ServicesSecurity.DomainModel.Security.Bitacora;
using BitacoraService = ServicesSecurity.Services.Bitacora;

namespace ServicesSecurity.BLL
{
    /// <summary>
    /// Lógica de negocio para gestión de bitácora
    /// </summary>
    public static class BitacoraBLL
    {
        /// <summary>
        /// Registra un nuevo evento en la bitácora
        /// </summary>
        public static void Registrar(BitacoraEntity bitacora)
        {
            try
            {
                if (bitacora == null)
                    throw new ArgumentNullException(nameof(bitacora));

                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(bitacora.Modulo))
                    throw new ArgumentException("El módulo es requerido", nameof(bitacora.Modulo));

                if (string.IsNullOrWhiteSpace(bitacora.Accion))
                    throw new ArgumentException("La acción es requerida", nameof(bitacora.Accion));

                if (string.IsNullOrWhiteSpace(bitacora.Descripcion))
                    throw new ArgumentException("La descripción es requerida", nameof(bitacora.Descripcion));

                if (string.IsNullOrWhiteSpace(bitacora.Criticidad))
                    throw new ArgumentException("La criticidad es requerida", nameof(bitacora.Criticidad));

                // Validar que la criticidad sea válida
                if (bitacora.Criticidad != CriticidadBitacora.Info &&
                    bitacora.Criticidad != CriticidadBitacora.Advertencia &&
                    bitacora.Criticidad != CriticidadBitacora.Error &&
                    bitacora.Criticidad != CriticidadBitacora.Critico)
                {
                    throw new ArgumentException("Criticidad no válida", nameof(bitacora.Criticidad));
                }

                BitacoraRepository.Current.Registrar(bitacora);
            }
            catch (Exception ex)
            {
                // No usar ExceptionManager aquí para evitar recursión si la bitácora está integrada
                throw new Exception("Error al registrar evento en bitácora: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Registra un evento usando parámetros individuales
        /// </summary>
        public static void Registrar(
            Guid? idUsuario,
            string nombreUsuario,
            string modulo,
            string accion,
            string descripcion,
            string criticidad,
            string tabla = null,
            string idRegistro = null,
            string ip = null)
        {
            BitacoraEntity bitacora = new BitacoraEntity(
                idUsuario,
                nombreUsuario,
                modulo,
                accion,
                descripcion,
                criticidad,
                tabla,
                idRegistro,
                ip);

            Registrar(bitacora);
        }

        /// <summary>
        /// Obtiene todos los registros de bitácora
        /// </summary>
        public static IEnumerable<BitacoraEntity> ObtenerTodos(int top = 1000)
        {
            try
            {
                return BitacoraRepository.Current.ObtenerTodos(top);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener registros de bitácora", ex);
            }
        }

        /// <summary>
        /// Obtiene registros de bitácora aplicando filtros
        /// </summary>
        public static IEnumerable<BitacoraEntity> ObtenerPorFiltros(
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            Guid? idUsuario = null,
            string modulo = null,
            string accion = null,
            string criticidad = null,
            int top = 1000)
        {
            try
            {
                return BitacoraRepository.Current.ObtenerPorFiltros(
                    fechaDesde,
                    fechaHasta,
                    idUsuario,
                    modulo,
                    accion,
                    criticidad,
                    top);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener registros filtrados de bitácora", ex);
            }
        }

        /// <summary>
        /// Obtiene los registros de un usuario específico
        /// </summary>
        public static IEnumerable<BitacoraEntity> ObtenerPorUsuario(Guid idUsuario, int top = 100)
        {
            try
            {
                return BitacoraRepository.Current.ObtenerPorUsuario(idUsuario, top);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener registros de bitácora del usuario", ex);
            }
        }

        /// <summary>
        /// Obtiene registros en un rango de fechas
        /// </summary>
        public static IEnumerable<BitacoraEntity> ObtenerPorRangoFechas(
            DateTime fechaDesde,
            DateTime fechaHasta,
            int top = 1000)
        {
            try
            {
                if (fechaDesde > fechaHasta)
                    throw new ArgumentException("La fecha desde no puede ser mayor que la fecha hasta");

                return BitacoraRepository.Current.ObtenerPorRangoFechas(fechaDesde, fechaHasta, top);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener registros por rango de fechas", ex);
            }
        }

        /// <summary>
        /// Elimina registros antiguos de la bitácora (mantenimiento)
        /// Solo usuarios con permisos especiales deberían poder hacer esto
        /// </summary>
        public static int LimpiarRegistrosAntiguos(DateTime fechaLimite)
        {
            try
            {
                if (fechaLimite > DateTime.Now)
                    throw new ArgumentException("La fecha límite no puede ser futura");

                return BitacoraRepository.Current.EliminarAnterioresA(fechaLimite);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al limpiar registros antiguos de bitácora", ex);
            }
        }

        /// <summary>
        /// Obtiene estadísticas de la bitácora
        /// </summary>
        public static Dictionary<string, Dictionary<string, int>> ObtenerEstadisticas(
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            try
            {
                return BitacoraRepository.Current.ObtenerEstadisticas(fechaDesde, fechaHasta);
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener estadísticas de bitácora", ex);
            }
        }
    }
}
