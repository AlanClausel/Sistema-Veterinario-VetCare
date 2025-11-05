using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para Medicamentos
    /// </summary>
    public class MedicamentoBLL
    {
        private readonly IMedicamentoRepository _medicamentoRepository;

        #region Singleton

        private static readonly MedicamentoBLL _instance = new MedicamentoBLL();

        public static MedicamentoBLL Current
        {
            get { return _instance; }
        }

        private MedicamentoBLL()
        {
            _medicamentoRepository = MedicamentoRepository.Current;
        }

        #endregion

        #region Casos de Uso - Gestión de Medicamentos

        /// <summary>
        /// UC: Registrar un nuevo medicamento
        /// </summary>
        public Medicamento RegistrarMedicamento(Medicamento medicamento)
        {
            ValidarMedicamento(medicamento);

            return _medicamentoRepository.Crear(medicamento);
        }

        /// <summary>
        /// UC: Actualizar un medicamento existente
        /// </summary>
        public bool ActualizarMedicamento(Medicamento medicamento)
        {
            ValidarMedicamento(medicamento);

            var medicamentoExistente = _medicamentoRepository.ObtenerPorId(medicamento.IdMedicamento);
            if (medicamentoExistente == null)
                throw new InvalidOperationException($"No existe un medicamento con ID {medicamento.IdMedicamento}");

            return _medicamentoRepository.Actualizar(medicamento);
        }

        /// <summary>
        /// UC: Eliminar un medicamento (soft delete)
        /// </summary>
        public bool EliminarMedicamento(Guid idMedicamento)
        {
            var medicamento = _medicamentoRepository.ObtenerPorId(idMedicamento);
            if (medicamento == null)
                throw new InvalidOperationException($"No existe un medicamento con ID {idMedicamento}");

            return _medicamentoRepository.Eliminar(idMedicamento);
        }

        /// <summary>
        /// UC: Obtener un medicamento por ID
        /// </summary>
        public Medicamento ObtenerMedicamentoPorId(Guid idMedicamento)
        {
            return _medicamentoRepository.ObtenerPorId(idMedicamento);
        }

        /// <summary>
        /// UC: Listar todos los medicamentos activos
        /// </summary>
        public IEnumerable<Medicamento> ListarTodosLosMedicamentos()
        {
            return _medicamentoRepository.ObtenerTodos();
        }

        /// <summary>
        /// UC: Buscar medicamentos por nombre o presentación
        /// </summary>
        public IEnumerable<Medicamento> BuscarMedicamentos(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return ListarTodosLosMedicamentos();

            return _medicamentoRepository.Buscar(criterio);
        }

        /// <summary>
        /// UC: Listar solo medicamentos con stock disponible
        /// </summary>
        public IEnumerable<Medicamento> ListarMedicamentosDisponibles()
        {
            return _medicamentoRepository.ObtenerDisponibles();
        }

        #endregion

        #region Casos de Uso - Gestión de Stock

        /// <summary>
        /// UC: Aumentar el stock de un medicamento
        /// </summary>
        public Medicamento AumentarStock(Guid idMedicamento, int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(cantidad));

            var medicamento = _medicamentoRepository.ObtenerPorId(idMedicamento);
            if (medicamento == null)
                throw new InvalidOperationException($"No existe un medicamento con ID {idMedicamento}");

            return _medicamentoRepository.ActualizarStock(idMedicamento, cantidad);
        }

        /// <summary>
        /// UC: Reducir el stock de un medicamento
        /// </summary>
        public Medicamento ReducirStock(Guid idMedicamento, int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(cantidad));

            var medicamento = _medicamentoRepository.ObtenerPorId(idMedicamento);
            if (medicamento == null)
                throw new InvalidOperationException($"No existe un medicamento con ID {idMedicamento}");

            if (medicamento.Stock < cantidad)
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {medicamento.Stock}, Solicitado: {cantidad}");

            return _medicamentoRepository.ActualizarStock(idMedicamento, -cantidad);
        }

        /// <summary>
        /// UC: Obtener medicamentos con stock bajo (menos de 10 unidades)
        /// </summary>
        public IEnumerable<Medicamento> ObtenerMedicamentosConStockBajo()
        {
            return _medicamentoRepository.ObtenerTodos()
                .Where(m => m.Stock < 10 && m.Stock > 0)
                .OrderBy(m => m.Stock);
        }

        /// <summary>
        /// UC: Obtener medicamentos sin stock
        /// </summary>
        public IEnumerable<Medicamento> ObtenerMedicamentosSinStock()
        {
            return _medicamentoRepository.ObtenerTodos()
                .Where(m => m.Stock == 0)
                .OrderBy(m => m.Nombre);
        }

        #endregion

        #region Validaciones Privadas

        /// <summary>
        /// Valida las reglas de negocio para un medicamento
        /// </summary>
        private void ValidarMedicamento(Medicamento medicamento)
        {
            if (medicamento == null)
                throw new ArgumentNullException(nameof(medicamento));

            var errores = medicamento.Validar();
            if (errores.Any())
                throw new ArgumentException($"Errores de validación:\n{string.Join("\n", errores)}");
        }

        #endregion
    }
}
