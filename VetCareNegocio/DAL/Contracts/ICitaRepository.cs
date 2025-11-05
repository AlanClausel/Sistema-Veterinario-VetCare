using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    /// <summary>
    /// Contrato para el repositorio de Citas
    /// Define las operaciones específicas para gestionar citas veterinarias
    /// </summary>
    public interface ICitaRepository
    {
        /// <summary>
        /// Inserta una nueva cita en la base de datos
        /// </summary>
        /// <param name="cita">Cita a insertar</param>
        /// <returns>ID de la cita creada</returns>
        Guid Insert(Cita cita);

        /// <summary>
        /// Actualiza una cita existente
        /// </summary>
        /// <param name="cita">Cita con los datos actualizados</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool Update(Cita cita);

        /// <summary>
        /// Actualiza solo el estado de una cita
        /// </summary>
        /// <param name="idCita">ID de la cita</param>
        /// <param name="estado">Nuevo estado</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdateEstado(Guid idCita, EstadoCita estado);

        /// <summary>
        /// Elimina lógicamente una cita (marca como inactiva)
        /// </summary>
        /// <param name="idCita">ID de la cita a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        bool Delete(Guid idCita);

        /// <summary>
        /// Obtiene una cita por su ID con información completa
        /// </summary>
        /// <param name="idCita">ID de la cita</param>
        /// <returns>Cita encontrada o null</returns>
        Cita SelectOne(Guid idCita);

        /// <summary>
        /// Obtiene todas las citas activas
        /// </summary>
        /// <returns>Lista de todas las citas</returns>
        List<Cita> SelectAll();

        /// <summary>
        /// Obtiene todas las citas de una mascota específica
        /// </summary>
        /// <param name="idMascota">ID de la mascota</param>
        /// <returns>Lista de citas de la mascota</returns>
        List<Cita> SelectByMascota(Guid idMascota);

        /// <summary>
        /// Obtiene todas las citas de un cliente (todas sus mascotas)
        /// </summary>
        /// <param name="idCliente">ID del cliente</param>
        /// <returns>Lista de citas del cliente</returns>
        List<Cita> SelectByCliente(Guid idCliente);

        /// <summary>
        /// Obtiene todas las citas de una fecha específica
        /// </summary>
        /// <param name="fecha">Fecha a buscar</param>
        /// <returns>Lista de citas del día</returns>
        List<Cita> SelectByFecha(DateTime fecha);

        /// <summary>
        /// Obtiene todas las citas de un veterinario específico
        /// </summary>
        /// <param name="veterinario">Nombre del veterinario</param>
        /// <returns>Lista de citas del veterinario</returns>
        List<Cita> SelectByVeterinario(string veterinario);

        /// <summary>
        /// Obtiene todas las citas por estado
        /// </summary>
        /// <param name="estado">Estado a filtrar</param>
        /// <returns>Lista de citas con el estado especificado</returns>
        List<Cita> SelectByEstado(EstadoCita estado);

        /// <summary>
        /// Búsqueda combinada de citas con múltiples filtros
        /// </summary>
        /// <param name="fecha">Fecha a buscar (opcional)</param>
        /// <param name="veterinario">Nombre del veterinario (opcional)</param>
        /// <param name="estado">Estado a filtrar (opcional)</param>
        /// <returns>Lista de citas que cumplen los criterios</returns>
        List<Cita> Search(DateTime? fecha = null, string veterinario = null, EstadoCita? estado = null);

        /// <summary>
        /// Obtiene todas las citas entre dos fechas (rango)
        /// </summary>
        /// <param name="fechaDesde">Fecha inicio del rango (inclusive)</param>
        /// <param name="fechaHasta">Fecha fin del rango (inclusive)</param>
        /// <returns>Lista de citas en el rango especificado</returns>
        List<Cita> SelectByFechaRango(DateTime fechaDesde, DateTime fechaHasta);
    }
}
