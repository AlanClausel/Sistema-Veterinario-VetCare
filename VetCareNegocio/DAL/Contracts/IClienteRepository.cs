using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    /// <summary>
    /// Repositorio específico para gestión de Clientes
    /// </summary>
    public interface IClienteRepository
    {
        /// <summary>
        /// Crea un nuevo cliente en la base de datos
        /// </summary>
        Cliente Crear(Cliente cliente);

        /// <summary>
        /// Actualiza los datos de un cliente existente
        /// </summary>
        Cliente Actualizar(Cliente cliente);

        /// <summary>
        /// Elimina un cliente (elimina en cascada sus mascotas)
        /// </summary>
        void Eliminar(Guid idCliente);

        /// <summary>
        /// Obtiene un cliente por su ID
        /// </summary>
        Cliente ObtenerPorId(Guid idCliente);

        /// <summary>
        /// Obtiene un cliente por su DNI (único)
        /// </summary>
        Cliente ObtenerPorDNI(string dni);

        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        IEnumerable<Cliente> ObtenerTodos();

        /// <summary>
        /// Busca clientes por nombre, apellido, DNI o email
        /// </summary>
        IEnumerable<Cliente> BuscarPorCriterio(string criterio);

        /// <summary>
        /// Verifica si existe un cliente con un DNI específico
        /// </summary>
        bool ExistePorDNI(string dni);

        /// <summary>
        /// Verifica si existe un cliente con un DNI, excluyendo un ID específico (para edición)
        /// </summary>
        bool ExistePorDNI(string dni, Guid idClienteExcluir);
    }
}
