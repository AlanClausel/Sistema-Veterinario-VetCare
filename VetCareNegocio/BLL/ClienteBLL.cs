using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para Clientes
    /// Implementa casos de uso y validaciones de negocio
    /// </summary>
    public class ClienteBLL
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMascotaRepository _mascotaRepository;

        #region Singleton

        private static readonly ClienteBLL _instance = new ClienteBLL();

        public static ClienteBLL Current
        {
            get { return _instance; }
        }

        private ClienteBLL()
        {
            _clienteRepository = ClienteRepository.Current;
            _mascotaRepository = MascotaRepository.Current;
        }

        #endregion

        #region Casos de Uso - Crear Cliente

        /// <summary>
        /// Caso de uso: Registrar un nuevo cliente en el sistema
        /// </summary>
        public Cliente RegistrarCliente(Cliente cliente)
        {
            // Validaciones de negocio
            ValidarCliente(cliente);

            // Validar DNI único
            if (_clienteRepository.ExistePorDNI(cliente.DNI))
            {
                throw new InvalidOperationException($"Ya existe un cliente con DNI {cliente.DNI}");
            }

            // Generar nuevo ID si no tiene
            if (cliente.IdCliente == Guid.Empty)
            {
                cliente.IdCliente = Guid.NewGuid();
            }

            // Establecer fecha de registro
            cliente.FechaRegistro = DateTime.Now;
            cliente.Activo = true;

            // Persistir en base de datos
            return _clienteRepository.Crear(cliente);
        }

        #endregion

        #region Casos de Uso - Actualizar Cliente

        /// <summary>
        /// Caso de uso: Modificar los datos de un cliente existente
        /// </summary>
        public Cliente ModificarCliente(Cliente cliente)
        {
            // Validaciones de negocio
            ValidarCliente(cliente);

            // Verificar que el cliente existe
            var clienteExistente = _clienteRepository.ObtenerPorId(cliente.IdCliente);
            if (clienteExistente == null)
            {
                throw new InvalidOperationException($"No existe un cliente con ID {cliente.IdCliente}");
            }

            // Validar DNI único (excluyendo el cliente actual)
            if (_clienteRepository.ExistePorDNI(cliente.DNI, cliente.IdCliente))
            {
                throw new InvalidOperationException($"Ya existe otro cliente con DNI {cliente.DNI}");
            }

            // Mantener fecha de registro original
            cliente.FechaRegistro = clienteExistente.FechaRegistro;

            // Persistir cambios
            return _clienteRepository.Actualizar(cliente);
        }

        #endregion

        #region Casos de Uso - Eliminar Cliente

        /// <summary>
        /// Caso de uso: Eliminar un cliente del sistema
        /// ADVERTENCIA: Eliminará en cascada todas las mascotas del cliente
        /// </summary>
        public void EliminarCliente(Guid idCliente)
        {
            // Verificar que el cliente existe
            var cliente = _clienteRepository.ObtenerPorId(idCliente);
            if (cliente == null)
            {
                throw new InvalidOperationException($"No existe un cliente con ID {idCliente}");
            }

            // Validar reglas de negocio antes de eliminar
            var cantidadMascotas = _mascotaRepository.ContarPorCliente(idCliente);
            if (cantidadMascotas > 0)
            {
                // Opcional: Puedes lanzar excepción o permitir eliminación en cascada
                // throw new InvalidOperationException($"No se puede eliminar el cliente porque tiene {cantidadMascotas} mascotas asociadas");
            }

            // Eliminar cliente (y mascotas en cascada por FK en BD)
            _clienteRepository.Eliminar(idCliente);
        }

        /// <summary>
        /// Caso de uso: Dar de baja lógica a un cliente (no lo elimina físicamente)
        /// </summary>
        public Cliente DesactivarCliente(Guid idCliente)
        {
            var cliente = _clienteRepository.ObtenerPorId(idCliente);
            if (cliente == null)
            {
                throw new InvalidOperationException($"No existe un cliente con ID {idCliente}");
            }

            cliente.Activo = false;
            return _clienteRepository.Actualizar(cliente);
        }

        /// <summary>
        /// Caso de uso: Reactivar un cliente
        /// </summary>
        public Cliente ActivarCliente(Guid idCliente)
        {
            var cliente = _clienteRepository.ObtenerPorId(idCliente);
            if (cliente == null)
            {
                throw new InvalidOperationException($"No existe un cliente con ID {idCliente}");
            }

            cliente.Activo = true;
            return _clienteRepository.Actualizar(cliente);
        }

        #endregion

        #region Casos de Uso - Consultar Clientes

        /// <summary>
        /// Caso de uso: Obtener un cliente por su ID
        /// </summary>
        public Cliente ObtenerClientePorId(Guid idCliente)
        {
            return _clienteRepository.ObtenerPorId(idCliente);
        }

        /// <summary>
        /// Caso de uso: Buscar un cliente por su DNI
        /// </summary>
        public Cliente BuscarClientePorDNI(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede estar vacío", nameof(dni));

            return _clienteRepository.ObtenerPorDNI(dni);
        }

        /// <summary>
        /// Caso de uso: Buscar un cliente por DNI con sus mascotas cargadas
        /// </summary>
        public Cliente BuscarClientePorDNIConMascotas(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede estar vacío", nameof(dni));

            var cliente = _clienteRepository.ObtenerPorDNI(dni);
            if (cliente == null)
                return null;

            // Cargar mascotas del cliente
            var mascotas = _mascotaRepository.ObtenerPorCliente(cliente.IdCliente);
            cliente.Mascotas = mascotas.ToList();

            return cliente;
        }

        /// <summary>
        /// Caso de uso: Listar todos los clientes del sistema
        /// </summary>
        public IEnumerable<Cliente> ListarTodosLosClientes()
        {
            return _clienteRepository.ObtenerTodos();
        }

        /// <summary>
        /// Caso de uso: Listar solo clientes activos
        /// </summary>
        public IEnumerable<Cliente> ListarClientesActivos()
        {
            return _clienteRepository.ObtenerTodos().Where(c => c.Activo);
        }

        /// <summary>
        /// Caso de uso: Buscar clientes por criterio (nombre, apellido, DNI, email)
        /// </summary>
        public IEnumerable<Cliente> BuscarClientes(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return ListarTodosLosClientes();

            return _clienteRepository.BuscarPorCriterio(criterio);
        }

        #endregion

        #region Casos de Uso - Cliente con Mascotas

        /// <summary>
        /// Caso de uso: Obtener un cliente con todas sus mascotas
        /// </summary>
        public Cliente ObtenerClienteConMascotas(Guid idCliente)
        {
            var cliente = _clienteRepository.ObtenerPorId(idCliente);
            if (cliente == null)
                return null;

            // Cargar mascotas del cliente
            var mascotas = _mascotaRepository.ObtenerPorCliente(idCliente);
            cliente.Mascotas = mascotas.ToList();

            return cliente;
        }

        /// <summary>
        /// Caso de uso: Obtener estadísticas de un cliente
        /// </summary>
        public ClienteEstadisticas ObtenerEstadisticasCliente(Guid idCliente)
        {
            var cliente = _clienteRepository.ObtenerPorId(idCliente);
            if (cliente == null)
                throw new InvalidOperationException($"No existe un cliente con ID {idCliente}");

            var mascotas = _mascotaRepository.ObtenerPorCliente(idCliente).ToList();

            return new ClienteEstadisticas
            {
                Cliente = cliente,
                CantidadMascotas = mascotas.Count,
                MascotasActivas = mascotas.Count(m => m.Activo),
                MascotasInactivas = mascotas.Count(m => !m.Activo)
            };
        }

        #endregion

        #region Validaciones Privadas

        /// <summary>
        /// Valida las reglas de negocio para un cliente
        /// </summary>
        private void ValidarCliente(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            // Validar nombre
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new ArgumentException("El nombre es obligatorio");

            if (cliente.Nombre.Length < 2)
                throw new ArgumentException("El nombre debe tener al menos 2 caracteres");

            // Validar apellido
            if (string.IsNullOrWhiteSpace(cliente.Apellido))
                throw new ArgumentException("El apellido es obligatorio");

            if (cliente.Apellido.Length < 2)
                throw new ArgumentException("El apellido debe tener al menos 2 caracteres");

            // Validar DNI
            if (string.IsNullOrWhiteSpace(cliente.DNI))
                throw new ArgumentException("El DNI es obligatorio");

            if (cliente.DNI.Length < 6)
                throw new ArgumentException("El DNI debe tener al menos 6 caracteres");

            // Validar email (si se proporciona)
            if (!string.IsNullOrWhiteSpace(cliente.Email))
            {
                if (!EsEmailValido(cliente.Email))
                    throw new ArgumentException("El formato del email no es válido");
            }

            // Validar teléfono (longitud mínima si se proporciona)
            if (!string.IsNullOrWhiteSpace(cliente.Telefono))
            {
                if (cliente.Telefono.Length < 7)
                    throw new ArgumentException("El teléfono debe tener al menos 7 caracteres");
            }
        }

        /// <summary>
        /// Valida formato de email
        /// </summary>
        private bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// DTO para estadísticas de cliente
    /// </summary>
    public class ClienteEstadisticas
    {
        public Cliente Cliente { get; set; }
        public int CantidadMascotas { get; set; }
        public int MascotasActivas { get; set; }
        public int MascotasInactivas { get; set; }
    }
}
