using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para Consultas Médicas
    /// </summary>
    public class ConsultaMedicaBLL
    {
        private readonly IConsultaMedicaRepository _consultaRepository;
        private readonly ICitaRepository _citaRepository;

        #region Singleton

        private static readonly ConsultaMedicaBLL _instance = new ConsultaMedicaBLL();

        public static ConsultaMedicaBLL Current
        {
            get { return _instance; }
        }

        private ConsultaMedicaBLL()
        {
            _consultaRepository = ConsultaMedicaRepository.Current;
            _citaRepository = CitaRepository.Current;
        }

        #endregion

        #region Casos de Uso - Crear Consulta

        /// <summary>
        /// UC: Iniciar una nueva consulta médica desde una cita
        /// </summary>
        public ConsultaMedica IniciarConsulta(Guid idCita, Guid idVeterinario)
        {
            // Validar que la cita existe
            var cita = _citaRepository.SelectOne(idCita)
                ?? throw new InvalidOperationException("La cita especificada no existe");

            // Validar que la cita no tiene ya una consulta
            if (_consultaRepository.ObtenerPorCita(idCita) != null)
                throw new InvalidOperationException("Esta cita ya tiene una consulta médica registrada");

            // Validar que la cita está en estado válido para consulta
            if (cita.Estado != EstadoCita.Agendada && cita.Estado != EstadoCita.Confirmada)
                throw new InvalidOperationException($"No se puede iniciar consulta para una cita en estado {cita.EstadoDescripcion}");

            var consulta = new ConsultaMedica(idCita, idVeterinario);
            return _consultaRepository.Crear(consulta);
        }

        /// <summary>
        /// UC: Guardar los datos de una consulta (sin finalizar)
        /// </summary>
        public bool GuardarConsulta(ConsultaMedica consulta)
        {
            ValidarConsulta(consulta);

            _ = _consultaRepository.ObtenerPorId(consulta.IdConsulta)
                ?? throw new InvalidOperationException($"No existe una consulta con ID {consulta.IdConsulta}");

            return _consultaRepository.Actualizar(consulta);
        }

        /// <summary>
        /// UC: Finalizar una consulta médica
        /// Actualiza el estado de la cita a Completada y reduce el stock de medicamentos
        /// </summary>
        public bool FinalizarConsulta(Guid idConsulta)
        {
            var consulta = _consultaRepository.ObtenerPorId(idConsulta)
                ?? throw new InvalidOperationException("La consulta especificada no existe");

            // Validar que la consulta tiene los datos mínimos
            var errores = consulta.Validar();
            if (errores.Any())
                throw new InvalidOperationException($"No se puede finalizar la consulta:\n{string.Join("\n", errores)}");

            // Finalizar consulta (actualiza cita y reduce stock)
            bool resultado = _consultaRepository.FinalizarConsulta(idConsulta, consulta.IdCita);

            if (!resultado)
                throw new InvalidOperationException("Error al finalizar la consulta. Verifique que todos los medicamentos tengan stock suficiente.");

            return resultado;
        }

        #endregion

        #region Casos de Uso - Medicamentos de la Consulta

        /// <summary>
        /// UC: Agregar un medicamento a la consulta
        /// </summary>
        public bool AgregarMedicamento(Guid idConsulta, Guid idMedicamento, int cantidad, string indicaciones)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(cantidad));

            _ = _consultaRepository.ObtenerPorId(idConsulta)
                ?? throw new InvalidOperationException("La consulta especificada no existe");

            // Verificar que el medicamento existe
            var medicamento = MedicamentoBLL.Current.ObtenerMedicamentoPorId(idMedicamento)
                ?? throw new InvalidOperationException("El medicamento especificado no existe");

            // Verificar que hay stock disponible (sin reservar todavía)
            if (medicamento.Stock < cantidad)
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {medicamento.Stock}, Solicitado: {cantidad}");

            return _consultaRepository.AgregarMedicamento(idConsulta, idMedicamento, cantidad, indicaciones);
        }

        /// <summary>
        /// UC: Eliminar un medicamento de la consulta
        /// </summary>
        public bool EliminarMedicamento(Guid idConsulta, Guid idMedicamento)
        {
            _ = _consultaRepository.ObtenerPorId(idConsulta)
                ?? throw new InvalidOperationException("La consulta especificada no existe");

            return _consultaRepository.EliminarMedicamento(idConsulta, idMedicamento);
        }

        /// <summary>
        /// UC: Obtener los medicamentos recetados en una consulta
        /// </summary>
        public IEnumerable<MedicamentoRecetado> ObtenerMedicamentosDeConsulta(Guid idConsulta)
        {
            return _consultaRepository.ObtenerMedicamentosPorConsulta(idConsulta);
        }

        #endregion

        #region Casos de Uso - Consultar

        /// <summary>
        /// UC: Obtener una consulta por ID con sus medicamentos
        /// </summary>
        public ConsultaMedica ObtenerConsultaCompleta(Guid idConsulta)
        {
            var consulta = _consultaRepository.ObtenerPorId(idConsulta);
            if (consulta == null)
                return null;

            // Hidratar medicamentos
            var medicamentos = _consultaRepository.ObtenerMedicamentosPorConsulta(idConsulta);
            consulta.Medicamentos = medicamentos.ToList();

            return consulta;
        }

        /// <summary>
        /// UC: Obtener la consulta de una cita específica
        /// </summary>
        public ConsultaMedica ObtenerConsultaPorCita(Guid idCita)
        {
            var consulta = _consultaRepository.ObtenerPorCita(idCita);
            if (consulta == null)
                return null;

            // Hidratar medicamentos
            var medicamentos = _consultaRepository.ObtenerMedicamentosPorConsulta(consulta.IdConsulta);
            consulta.Medicamentos = medicamentos.ToList();

            return consulta;
        }

        /// <summary>
        /// UC: Verificar si una cita ya tiene consulta médica
        /// </summary>
        public bool CitaTieneConsulta(Guid idCita)
        {
            return _consultaRepository.ObtenerPorCita(idCita) != null;
        }

        /// <summary>
        /// UC: Obtener todas las consultas de un veterinario
        /// </summary>
        public IEnumerable<ConsultaMedica> ObtenerConsultasPorVeterinario(Guid idVeterinario, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            return _consultaRepository.ObtenerPorVeterinario(idVeterinario, fechaDesde, fechaHasta);
        }

        /// <summary>
        /// UC: Obtener el historial completo de consultas de una mascota
        /// </summary>
        public IEnumerable<ConsultaMedica> ObtenerHistorialMascota(Guid idMascota)
        {
            var consultas = _consultaRepository.ObtenerPorMascota(idMascota);

            // Hidratar medicamentos para cada consulta
            foreach (var consulta in consultas)
            {
                var medicamentos = _consultaRepository.ObtenerMedicamentosPorConsulta(consulta.IdConsulta);
                consulta.Medicamentos = medicamentos.ToList();
            }

            return consultas;
        }

        #endregion

        #region Validaciones Privadas

        /// <summary>
        /// Valida las reglas de negocio para una consulta
        /// </summary>
        private void ValidarConsulta(ConsultaMedica consulta)
        {
            _ = consulta ?? throw new ArgumentNullException(nameof(consulta));

            var errores = consulta.Validar();
            if (errores.Any())
                throw new ArgumentException($"Errores de validación:\n{string.Join("\n", errores)}");
        }

        #endregion
    }
}
