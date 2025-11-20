using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    /// <summary>
    /// Contrato del repositorio para la gestión de persistencia de Clientes.
    /// Define las operaciones CRUD y consultas específicas para la entidad Cliente.
    /// Implementa el patrón Repository para abstraer el acceso a datos.
    /// </summary>
    public interface IClienteRepository
    {
        /// <summary>
        /// Crea un nuevo cliente en la base de datos mediante el SP Cliente_Insert.
        /// </summary>
        /// <param name="cliente">Entidad Cliente a crear</param>
        /// <returns>Cliente creado con datos actualizados desde la BD, o null si falla</returns>
        /// <exception cref="ArgumentNullException">Si cliente es null</exception>
        Cliente Crear(Cliente cliente);

        /// <summary>
        /// Actualiza los datos de un cliente existente mediante el SP Cliente_Update.
        /// </summary>
        /// <param name="cliente">Entidad Cliente con datos actualizados</param>
        /// <returns>Cliente actualizado con datos desde la BD, o null si falla</returns>
        /// <exception cref="ArgumentNullException">Si cliente es null</exception>
        Cliente Actualizar(Cliente cliente);

        /// <summary>
        /// Elimina un cliente de forma lógica (Activo=0) mediante el SP Cliente_Delete.
        /// ADVERTENCIA: También elimina en cascada todas las mascotas asociadas.
        /// </summary>
        /// <param name="idCliente">Identificador único del cliente a eliminar</param>
        void Eliminar(Guid idCliente);

        /// <summary>
        /// Obtiene un cliente específico por su identificador único mediante el SP Cliente_SelectOne.
        /// </summary>
        /// <param name="idCliente">Identificador único del cliente</param>
        /// <returns>Cliente encontrado, o null si no existe</returns>
        Cliente ObtenerPorId(Guid idCliente);

        /// <summary>
        /// Obtiene un cliente por su DNI (campo único) mediante el SP Cliente_SelectByDNI.
        /// Útil para validaciones de unicidad y búsquedas por documento.
        /// </summary>
        /// <param name="dni">Documento Nacional de Identidad del cliente</param>
        /// <returns>Cliente encontrado, o null si no existe</returns>
        Cliente ObtenerPorDNI(string dni);

        /// <summary>
        /// Obtiene todos los clientes activos del sistema mediante el SP Cliente_SelectAll.
        /// </summary>
        /// <returns>Colección de todos los clientes</returns>
        IEnumerable<Cliente> ObtenerTodos();

        /// <summary>
        /// Busca clientes que coincidan con el criterio en nombre, apellido, DNI o email
        /// mediante el SP Cliente_Search.
        /// Implementa búsqueda parcial (LIKE) en múltiples campos.
        /// </summary>
        /// <param name="criterio">Texto de búsqueda parcial</param>
        /// <returns>Colección de clientes que coinciden con el criterio</returns>
        IEnumerable<Cliente> BuscarPorCriterio(string criterio);

        /// <summary>
        /// Verifica si existe un cliente con el DNI especificado.
        /// Útil para validaciones de unicidad antes de crear/editar.
        /// </summary>
        /// <param name="dni">Documento Nacional de Identidad a verificar</param>
        /// <returns>True si existe un cliente con ese DNI, false en caso contrario</returns>
        bool ExistePorDNI(string dni);

        /// <summary>
        /// Verifica si existe otro cliente con el DNI especificado, excluyendo un ID específico.
        /// Útil para validar unicidad de DNI al editar un cliente existente.
        /// </summary>
        /// <param name="dni">Documento Nacional de Identidad a verificar</param>
        /// <param name="idClienteExcluir">ID del cliente que se está editando (para excluir de la búsqueda)</param>
        /// <returns>True si existe otro cliente con ese DNI, false en caso contrario</returns>
        bool ExistePorDNI(string dni, Guid idClienteExcluir);
    }
}
