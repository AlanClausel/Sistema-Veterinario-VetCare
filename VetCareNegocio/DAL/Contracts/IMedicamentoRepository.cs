using System;
using DomainModel;

namespace DAL.Contracts
{
    /// <summary>
    /// Contrato para el repositorio de Medicamentos
    /// </summary>
    public interface IMedicamentoRepository
    {
        /// <summary>
        /// Crea un nuevo medicamento
        /// </summary>
        Medicamento Crear(Medicamento medicamento);

        /// <summary>
        /// Actualiza un medicamento existente
        /// </summary>
        bool Actualizar(Medicamento medicamento);

        /// <summary>
        /// Elimina un medicamento (soft delete)
        /// </summary>
        bool Eliminar(Guid idMedicamento);

        /// <summary>
        /// Obtiene un medicamento por ID
        /// </summary>
        Medicamento ObtenerPorId(Guid idMedicamento);

        /// <summary>
        /// Obtiene todos los medicamentos activos
        /// </summary>
        Medicamento[] ObtenerTodos();

        /// <summary>
        /// Busca medicamentos por nombre o presentación
        /// </summary>
        Medicamento[] Buscar(string criterio);

        /// <summary>
        /// Obtiene solo medicamentos con stock disponible
        /// </summary>
        Medicamento[] ObtenerDisponibles();

        /// <summary>
        /// Actualiza el stock de un medicamento
        /// </summary>
        Medicamento ActualizarStock(Guid idMedicamento, int cantidadCambio);
    }
}
