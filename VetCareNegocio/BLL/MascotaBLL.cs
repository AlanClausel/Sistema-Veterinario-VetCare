using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;
using ServicesSecurity.Services;
using ServicesSecurity.DomainModel.Security;
using BitacoraService = ServicesSecurity.Services.Bitacora;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para Mascotas
    /// Implementa casos de uso y validaciones de negocio
    /// </summary>
    public class MascotaBLL
    {
        private readonly IMascotaRepository _mascotaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ICitaRepository _citaRepository;

        #region Singleton

        private static readonly MascotaBLL _instance = new MascotaBLL();

        public static MascotaBLL Current
        {
            get { return _instance; }
        }

        private MascotaBLL()
        {
            _mascotaRepository = MascotaRepository.Current;
            _citaRepository = CitaRepository.Current;
            _clienteRepository = ClienteRepository.Current;
        }

        #endregion

        #region Casos de Uso - Crear Mascota

        /// <summary>
        /// Caso de uso: Registrar una nueva mascota en el sistema
        /// </summary>
        public Mascota RegistrarMascota(Mascota mascota)
        {
            try
            {
                // Validaciones de negocio
                ValidarMascota(mascota);

                // Verificar que el cliente (dueño) existe
                var cliente = _clienteRepository.ObtenerPorId(mascota.IdCliente);
                if (cliente == null)
                {
                    throw new InvalidOperationException($"No existe un cliente con ID {mascota.IdCliente}");
                }

                // Verificar que el cliente esté activo
                if (!cliente.Activo)
                {
                    throw new InvalidOperationException($"No se puede registrar una mascota para un cliente inactivo");
                }

                // Generar nuevo ID si no tiene
                if (mascota.IdMascota == Guid.Empty)
                {
                    mascota.IdMascota = Guid.NewGuid();
                }

                // Establecer como activa
                mascota.Activo = true;

                // Persistir en base de datos
                var nuevaMascota = _mascotaRepository.Crear(mascota);

                // Registrar en bitácora
                var usuario = LoginService.GetUsuarioLogueado();
                if (usuario != null)
                {
                    BitacoraService.Current.RegistrarAlta(
                        usuario.IdUsuario,
                        usuario.Nombre,
                        "Mascotas",
                        "Mascota",
                        nuevaMascota.IdMascota.ToString(),
                        $"Mascota registrada: {nuevaMascota.Nombre} ({nuevaMascota.Especie}), dueño: {cliente.Nombre} {cliente.Apellido}"
                    );
                }

                return nuevaMascota;
            }
            catch (Exception ex)
            {
                var usuario = LoginService.GetUsuarioLogueado();
                BitacoraService.Current.RegistrarExcepcion(ex, usuario?.IdUsuario, usuario?.Nombre, "Mascotas");
                throw;
            }
        }

        #endregion

        #region Casos de Uso - Actualizar Mascota

        /// <summary>
        /// Caso de uso: Modificar los datos de una mascota existente
        /// </summary>
        public Mascota ModificarMascota(Mascota mascota)
        {
            try
            {
                // Validaciones de negocio
                ValidarMascota(mascota);

                // Verificar que la mascota existe
                var mascotaExistente = _mascotaRepository.ObtenerPorId(mascota.IdMascota);
                if (mascotaExistente == null)
                {
                    throw new InvalidOperationException($"No existe una mascota con ID {mascota.IdMascota}");
                }

                // Verificar que el cliente (dueño) existe
                var cliente = _clienteRepository.ObtenerPorId(mascota.IdCliente);
                if (cliente == null)
                {
                    throw new InvalidOperationException($"No existe un cliente con ID {mascota.IdCliente}");
                }

                // Persistir cambios
                var mascotaActualizada = _mascotaRepository.Actualizar(mascota);

                // Registrar en bitácora
                var usuario = LoginService.GetUsuarioLogueado();
                if (usuario != null)
                {
                    BitacoraService.Current.RegistrarModificacion(
                        usuario.IdUsuario,
                        usuario.Nombre,
                        "Mascotas",
                        "Mascota",
                        mascotaActualizada.IdMascota.ToString(),
                        $"Mascota modificada: {mascotaActualizada.Nombre} ({mascotaActualizada.Especie})"
                    );
                }

                return mascotaActualizada;
            }
            catch (Exception ex)
            {
                var usuario = LoginService.GetUsuarioLogueado();
                BitacoraService.Current.RegistrarExcepcion(ex, usuario?.IdUsuario, usuario?.Nombre, "Mascotas");
                throw;
            }
        }

        /// <summary>
        /// Caso de uso: Transferir una mascota a otro dueño
        /// </summary>
        public Mascota TransferirMascota(Guid idMascota, Guid idNuevoDueno)
        {
            try
            {
                // Verificar que la mascota existe
                var mascota = _mascotaRepository.ObtenerPorId(idMascota);
                if (mascota == null)
                {
                    throw new InvalidOperationException($"No existe una mascota con ID {idMascota}");
                }

                // Obtener dueño anterior para bitácora
                var duenoAnterior = _clienteRepository.ObtenerPorId(mascota.IdCliente);

                // Verificar que el nuevo dueño existe y está activo
                var nuevoDueno = _clienteRepository.ObtenerPorId(idNuevoDueno);
                if (nuevoDueno == null)
                {
                    throw new InvalidOperationException($"No existe un cliente con ID {idNuevoDueno}");
                }

                if (!nuevoDueno.Activo)
                {
                    throw new InvalidOperationException("No se puede transferir a un cliente inactivo");
                }

                // Realizar transferencia
                mascota.IdCliente = idNuevoDueno;
                var mascotaActualizada = _mascotaRepository.Actualizar(mascota);

                // Registrar en bitácora
                var usuario = LoginService.GetUsuarioLogueado();
                if (usuario != null)
                {
                    BitacoraService.Current.RegistrarModificacion(
                        usuario.IdUsuario,
                        usuario.Nombre,
                        "Mascotas",
                        "Mascota",
                        mascotaActualizada.IdMascota.ToString(),
                        $"Mascota transferida: {mascotaActualizada.Nombre}, de {duenoAnterior?.Nombre} {duenoAnterior?.Apellido} a {nuevoDueno.Nombre} {nuevoDueno.Apellido}"
                    );
                }

                return mascotaActualizada;
            }
            catch (Exception ex)
            {
                var usuario = LoginService.GetUsuarioLogueado();
                BitacoraService.Current.RegistrarExcepcion(ex, usuario?.IdUsuario, usuario?.Nombre, "Mascotas");
                throw;
            }
        }

        #endregion

        #region Casos de Uso - Eliminar Mascota

        /// <summary>
        /// Caso de uso: Eliminar una mascota del sistema (eliminación física)
        /// </summary>
        public void EliminarMascota(Guid idMascota)
        {
            try
            {
                // Verificar que la mascota existe
                var mascota = _mascotaRepository.ObtenerPorId(idMascota);
                if (mascota == null)
                {
                    throw new InvalidOperationException($"No existe una mascota con ID {idMascota}");
                }

                // Guardar datos antes de eliminar para bitácora
                string nombre = mascota.Nombre;
                string especie = mascota.Especie;

                // Verificar si la mascota tiene citas activas
                var citasActivas = _citaRepository.SelectByMascota(idMascota)
                    .Where(c => c.Estado == EstadoCita.Agendada || c.Estado == EstadoCita.Confirmada)
                    .ToList();

                if (citasActivas.Any())
                {
                    throw new InvalidOperationException(
                        $"No se puede eliminar la mascota '{mascota.Nombre}' porque tiene {citasActivas.Count} cita(s) activa(s).\n\n" +
                        $"Primero debe cancelar o completar las siguientes citas:\n" +
                        string.Join("\n", citasActivas.Select(c => $"- {c.FechaCitaFormateada} ({c.Estado})"))
                    );
                }

                // Eliminar mascota
                _mascotaRepository.Eliminar(idMascota);

                // Registrar en bitácora
                var usuario = LoginService.GetUsuarioLogueado();
                if (usuario != null)
                {
                    BitacoraService.Current.RegistrarBaja(
                        usuario.IdUsuario,
                        usuario.Nombre,
                        "Mascotas",
                        "Mascota",
                        idMascota.ToString(),
                        $"Mascota eliminada: {nombre} ({especie})"
                    );
                }
            }
            catch (Exception ex)
            {
                var usuario = LoginService.GetUsuarioLogueado();
                BitacoraService.Current.RegistrarExcepcion(ex, usuario?.IdUsuario, usuario?.Nombre, "Mascotas");
                throw;
            }
        }

        /// <summary>
        /// Caso de uso: Dar de baja lógica a una mascota (fallecimiento, pérdida, etc.)
        /// </summary>
        public Mascota DesactivarMascota(Guid idMascota)
        {
            try
            {
                var mascota = _mascotaRepository.ObtenerPorId(idMascota);
                if (mascota == null)
                {
                    throw new InvalidOperationException($"No existe una mascota con ID {idMascota}");
                }

                mascota.Activo = false;
                var mascotaActualizada = _mascotaRepository.Actualizar(mascota);

                // Registrar en bitácora
                var usuario = LoginService.GetUsuarioLogueado();
                if (usuario != null)
                {
                    BitacoraService.Current.RegistrarBaja(
                        usuario.IdUsuario,
                        usuario.Nombre,
                        "Mascotas",
                        "Mascota",
                        mascotaActualizada.IdMascota.ToString(),
                        $"Mascota desactivada: {mascotaActualizada.Nombre} ({mascotaActualizada.Especie})"
                    );
                }

                return mascotaActualizada;
            }
            catch (Exception ex)
            {
                var usuario = LoginService.GetUsuarioLogueado();
                BitacoraService.Current.RegistrarExcepcion(ex, usuario?.IdUsuario, usuario?.Nombre, "Mascotas");
                throw;
            }
        }

        /// <summary>
        /// Caso de uso: Reactivar una mascota
        /// </summary>
        public Mascota ActivarMascota(Guid idMascota)
        {
            try
            {
                var mascota = _mascotaRepository.ObtenerPorId(idMascota);
                if (mascota == null)
                {
                    throw new InvalidOperationException($"No existe una mascota con ID {idMascota}");
                }

                mascota.Activo = true;
                var mascotaActualizada = _mascotaRepository.Actualizar(mascota);

                // Registrar en bitácora
                var usuario = LoginService.GetUsuarioLogueado();
                if (usuario != null)
                {
                    BitacoraService.Current.RegistrarAlta(
                        usuario.IdUsuario,
                        usuario.Nombre,
                        "Mascotas",
                        "Mascota",
                        mascotaActualizada.IdMascota.ToString(),
                        $"Mascota reactivada: {mascotaActualizada.Nombre} ({mascotaActualizada.Especie})"
                    );
                }

                return mascotaActualizada;
            }
            catch (Exception ex)
            {
                var usuario = LoginService.GetUsuarioLogueado();
                BitacoraService.Current.RegistrarExcepcion(ex, usuario?.IdUsuario, usuario?.Nombre, "Mascotas");
                throw;
            }
        }

        #endregion

        #region Casos de Uso - Consultar Mascotas

        /// <summary>
        /// Caso de uso: Obtener una mascota por su ID
        /// </summary>
        public Mascota ObtenerMascotaPorId(Guid idMascota)
        {
            return _mascotaRepository.ObtenerPorId(idMascota);
        }

        /// <summary>
        /// Caso de uso: Listar todas las mascotas del sistema
        /// </summary>
        public IEnumerable<Mascota> ListarTodasLasMascotas()
        {
            return _mascotaRepository.ObtenerTodas();
        }

        /// <summary>
        /// Caso de uso: Listar solo mascotas activas
        /// </summary>
        public IEnumerable<Mascota> ListarMascotasActivas()
        {
            return _mascotaRepository.ObtenerTodas().Where(m => m.Activo);
        }

        /// <summary>
        /// Caso de uso: Listar mascotas de un cliente específico
        /// </summary>
        public IEnumerable<Mascota> ListarMascotasPorCliente(Guid idCliente)
        {
            // Verificar que el cliente existe
            var cliente = _clienteRepository.ObtenerPorId(idCliente);
            if (cliente == null)
            {
                throw new InvalidOperationException($"No existe un cliente con ID {idCliente}");
            }

            return _mascotaRepository.ObtenerPorCliente(idCliente);
        }

        /// <summary>
        /// Caso de uso: Listar mascotas activas de un cliente específico
        /// </summary>
        public IEnumerable<Mascota> ListarMascotasActivasPorCliente(Guid idCliente)
        {
            return _mascotaRepository.ObtenerActivasPorCliente(idCliente);
        }

        /// <summary>
        /// Caso de uso: Buscar mascotas por criterio (nombre, especie, raza)
        /// </summary>
        public IEnumerable<Mascota> BuscarMascotas(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return ListarTodasLasMascotas();

            return _mascotaRepository.BuscarPorCriterio(criterio);
        }

        #endregion

        #region Casos de Uso - Consultas Avanzadas

        /// <summary>
        /// Caso de uso: Obtener mascotas agrupadas por especie
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticasPorEspecie()
        {
            var mascotas = _mascotaRepository.ObtenerTodas();
            return mascotas
                .Where(m => m.Activo)
                .GroupBy(m => m.Especie)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Caso de uso: Obtener mascotas próximas a cumplir años (próximos 30 días)
        /// </summary>
        public IEnumerable<Mascota> ObtenerMascotasProximoCumpleanos()
        {
            var hoy = DateTime.Now;
            var mascotas = _mascotaRepository.ObtenerTodas().Where(m => m.Activo);

            return mascotas.Where(m =>
            {
                var proximoCumple = new DateTime(hoy.Year, m.FechaNacimiento.Month, m.FechaNacimiento.Day);
                if (proximoCumple < hoy)
                    proximoCumple = proximoCumple.AddYears(1);

                var diasHastaCumple = (proximoCumple - hoy).Days;
                return diasHastaCumple >= 0 && diasHastaCumple <= 30;
            });
        }

        /// <summary>
        /// Caso de uso: Obtener información detallada de una mascota con su dueño
        /// </summary>
        public MascotaDetalle ObtenerDetalleMascota(Guid idMascota)
        {
            var mascota = _mascotaRepository.ObtenerPorId(idMascota);
            if (mascota == null)
                throw new InvalidOperationException($"No existe una mascota con ID {idMascota}");

            var cliente = _clienteRepository.ObtenerPorId(mascota.IdCliente);

            return new MascotaDetalle
            {
                Mascota = mascota,
                Dueno = cliente,
                Edad = mascota.EdadEnAnios
            };
        }

        #endregion

        #region Validaciones Privadas

        /// <summary>
        /// Valida las reglas de negocio para una mascota
        /// </summary>
        private void ValidarMascota(Mascota mascota)
        {
            if (mascota == null)
                throw new ArgumentNullException(nameof(mascota));

            // Validar cliente
            if (mascota.IdCliente == Guid.Empty)
                throw new ArgumentException("Debe especificar el dueño de la mascota");

            // Validar nombre
            if (string.IsNullOrWhiteSpace(mascota.Nombre))
                throw new ArgumentException("El nombre de la mascota es obligatorio");

            if (mascota.Nombre.Length < 2)
                throw new ArgumentException("El nombre debe tener al menos 2 caracteres");

            // Validar especie
            if (string.IsNullOrWhiteSpace(mascota.Especie))
                throw new ArgumentException("La especie es obligatoria");

            if (mascota.Especie.Length < 2)
                throw new ArgumentException("La especie debe tener al menos 2 caracteres");

            // Validar sexo
            if (string.IsNullOrWhiteSpace(mascota.Sexo))
                throw new ArgumentException("El sexo es obligatorio");

            if (mascota.Sexo != "Macho" && mascota.Sexo != "Hembra")
                throw new ArgumentException("El sexo debe ser 'Macho' o 'Hembra'");

            // Validar fecha de nacimiento
            if (mascota.FechaNacimiento > DateTime.Now)
                throw new ArgumentException("La fecha de nacimiento no puede ser futura");

            if (mascota.FechaNacimiento.Year < 1900)
                throw new ArgumentException("La fecha de nacimiento no es válida");

            // Validar peso (si se proporciona)
            if (mascota.Peso < 0)
                throw new ArgumentException("El peso no puede ser negativo");

            if (mascota.Peso > 1000)
                throw new ArgumentException("El peso parece incorrecto (máximo 1000 kg)");
        }

        #endregion
    }

    /// <summary>
    /// DTO para detalle completo de mascota
    /// </summary>
    public class MascotaDetalle
    {
        public Mascota Mascota { get; set; }
        public Cliente Dueno { get; set; }
        public int Edad { get; set; }
    }
}
