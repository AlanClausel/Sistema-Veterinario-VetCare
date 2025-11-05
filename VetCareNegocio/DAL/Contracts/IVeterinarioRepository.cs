using System;
using System.Collections.Generic;
using DomainModel;

namespace DAL.Contracts
{
    /// <summary>
    /// Repositorio específico para gestión de Veterinarios
    ///
    /// IMPORTANTE:
    /// - Los veterinarios se sincronizan desde SecurityVet
    /// - IdVeterinario coincide con IdUsuario de SecurityVet
    /// - El campo Nombre se sincroniza automáticamente
    /// </summary>
    public interface IVeterinarioRepository
    {
        /// <summary>
        /// Crea un nuevo veterinario en la base de datos
        /// Se usa al asignar rol ROL_Veterinario a un usuario
        /// </summary>
        Veterinario Crear(Veterinario veterinario);

        /// <summary>
        /// Actualiza los datos de un veterinario existente
        /// (Solo actualiza Matricula, Telefono, Email, Observaciones, Activo)
        /// </summary>
        Veterinario Actualizar(Veterinario veterinario);

        /// <summary>
        /// Elimina un veterinario (soft delete - marca como inactivo)
        /// </summary>
        void Eliminar(Guid idVeterinario);

        /// <summary>
        /// Obtiene un veterinario por su ID
        /// </summary>
        Veterinario ObtenerPorId(Guid idVeterinario);

        /// <summary>
        /// Obtiene todos los veterinarios
        /// </summary>
        IEnumerable<Veterinario> ObtenerTodos();

        /// <summary>
        /// Obtiene solo veterinarios activos (para combos en UI)
        /// </summary>
        IEnumerable<Veterinario> ObtenerActivos();

        /// <summary>
        /// Sincroniza el nombre del veterinario desde SecurityVet
        /// Se ejecuta al modificar el nombre del usuario en SecurityVet
        /// </summary>
        Veterinario SincronizarNombre(Guid idVeterinario, string nombreActualizado);

        /// <summary>
        /// Verifica si existe un veterinario con el ID dado
        /// Útil para verificar antes de crear citas
        /// </summary>
        bool Existe(Guid idVeterinario);

        /// <summary>
        /// Verifica si existe un veterinario con una matrícula específica
        /// </summary>
        bool ExistePorMatricula(string matricula);

        /// <summary>
        /// Verifica si existe un veterinario con una matrícula, excluyendo un ID específico (para edición)
        /// </summary>
        bool ExistePorMatricula(string matricula, Guid idVeterinarioExcluir);
    }
}
