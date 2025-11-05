using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    /// <summary>
    /// Repositorio específico para gestión de Mascotas
    /// </summary>
    public interface IMascotaRepository
    {
        /// <summary>
        /// Crea una nueva mascota en la base de datos
        /// </summary>
        Mascota Crear(Mascota mascota);

        /// <summary>
        /// Actualiza los datos de una mascota existente
        /// </summary>
        Mascota Actualizar(Mascota mascota);

        /// <summary>
        /// Elimina una mascota
        /// </summary>
        void Eliminar(Guid idMascota);

        /// <summary>
        /// Obtiene una mascota por su ID
        /// </summary>
        Mascota ObtenerPorId(Guid idMascota);

        /// <summary>
        /// Obtiene todas las mascotas de un cliente específico
        /// </summary>
        IEnumerable<Mascota> ObtenerPorCliente(Guid idCliente);

        /// <summary>
        /// Obtiene todas las mascotas activas de un cliente
        /// </summary>
        IEnumerable<Mascota> ObtenerActivasPorCliente(Guid idCliente);

        /// <summary>
        /// Obtiene todas las mascotas
        /// </summary>
        IEnumerable<Mascota> ObtenerTodas();

        /// <summary>
        /// Busca mascotas por nombre, especie o raza
        /// </summary>
        IEnumerable<Mascota> BuscarPorCriterio(string criterio);

        /// <summary>
        /// Obtiene la cantidad de mascotas de un cliente
        /// </summary>
        int ContarPorCliente(Guid idCliente);
    }
}
