using System;
using DomainModel;

namespace DAL.Contracts
{
    /// <summary>
    /// Contrato para el repositorio de Consultas Médicas
    /// </summary>
    public interface IConsultaMedicaRepository
    {
        /// <summary>
        /// Crea una nueva consulta médica
        /// </summary>
        ConsultaMedica Crear(ConsultaMedica consulta);

        /// <summary>
        /// Actualiza una consulta médica existente
        /// </summary>
        bool Actualizar(ConsultaMedica consulta);

        /// <summary>
        /// Elimina una consulta médica (soft delete)
        /// </summary>
        bool Eliminar(Guid idConsulta);

        /// <summary>
        /// Obtiene una consulta médica por ID
        /// </summary>
        ConsultaMedica ObtenerPorId(Guid idConsulta);

        /// <summary>
        /// Obtiene la consulta médica de una cita específica
        /// </summary>
        ConsultaMedica ObtenerPorCita(Guid idCita);

        /// <summary>
        /// Obtiene todas las consultas de un veterinario
        /// </summary>
        ConsultaMedica[] ObtenerPorVeterinario(Guid idVeterinario, DateTime? fechaDesde = null, DateTime? fechaHasta = null);

        /// <summary>
        /// Agrega un medicamento a una consulta
        /// </summary>
        bool AgregarMedicamento(Guid idConsulta, Guid idMedicamento, int cantidad, string indicaciones);

        /// <summary>
        /// Elimina un medicamento de una consulta
        /// </summary>
        bool EliminarMedicamento(Guid idConsulta, Guid idMedicamento);

        /// <summary>
        /// Obtiene los medicamentos de una consulta
        /// </summary>
        MedicamentoRecetado[] ObtenerMedicamentosPorConsulta(Guid idConsulta);

        /// <summary>
        /// Finaliza una consulta y actualiza el estado de la cita
        /// </summary>
        bool FinalizarConsulta(Guid idConsulta, Guid idCita);

        /// <summary>
        /// Obtiene el historial de consultas de una mascota
        /// </summary>
        ConsultaMedica[] ObtenerPorMascota(Guid idMascota);
    }
}
