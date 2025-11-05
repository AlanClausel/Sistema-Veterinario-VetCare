using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;

namespace BLL
{
    /// <summary>
    /// Lógica de negocio para la gestión de Citas
    /// Implementa casos de uso y validaciones de negocio
    /// </summary>
    public sealed class CitaBLL
    {
        #region Singleton

        private static readonly CitaBLL _instance = new CitaBLL();
        private static readonly object _lock = new object();

        private CitaBLL()
        {
            _citaRepository = CitaRepository.Current;
            _mascotaRepository = MascotaRepository.Current;
        }

        public static CitaBLL Current
        {
            get
            {
                lock (_lock)
                {
                    return _instance;
                }
            }
        }

        #endregion

        #region Propiedades

        private readonly ICitaRepository _citaRepository;
        private readonly IMascotaRepository _mascotaRepository;

        #endregion

        #region Casos de Uso - Crear/Registrar

        /// <summary>
        /// UC: Agendar una nueva cita
        /// Validaciones: Mascota debe existir y estar activa, fecha válida, no debe haber conflictos de horario
        /// </summary>
        public Cita AgendarCita(Cita cita)
        {
            // Validaciones de negocio
            ValidarCita(cita);

            // Verificar que la mascota existe y está activa
            var mascota = _mascotaRepository.ObtenerPorId(cita.IdMascota)
                ?? throw new ArgumentException("La mascota especificada no existe");

            if (!mascota.Activo)
                throw new InvalidOperationException("No se puede agendar cita para una mascota inactiva");

            // Verificar que la fecha no esté en el pasado (con margen de 5 minutos)
            if (cita.FechaCita < DateTime.Now.AddMinutes(-5))
                throw new ArgumentException("No se puede agendar una cita en el pasado");

            // Verificar conflictos de horario para el veterinario
            ValidarConflictoHorario(cita.Veterinario, cita.FechaCita);

            // Verificar conflictos de horario para la mascota
            ValidarConflictoHorarioMascota(cita.IdMascota, cita.FechaCita);

            // Establecer estado inicial
            cita.Estado = EstadoCita.Agendada;
            cita.FechaRegistro = DateTime.Now;
            cita.Activo = true;

            // Insertar en la base de datos
            var idCita = _citaRepository.Insert(cita);
            cita.IdCita = idCita;

            // Retornar la cita completa con información del cliente
            return _citaRepository.SelectOne(idCita);
        }

        #endregion

        #region Casos de Uso - Actualizar

        /// <summary>
        /// UC: Modificar una cita existente
        /// Solo se pueden modificar citas en estado Agendada o Confirmada
        /// </summary>
        public bool ModificarCita(Cita cita)
        {
            // Validaciones de negocio
            ValidarCita(cita);

            // Obtener cita actual
            var citaActual = _citaRepository.SelectOne(cita.IdCita)
                ?? throw new ArgumentException("La cita especificada no existe");

            // Verificar que la cita puede ser modificada
            if (!citaActual.PuedeSerModificada())
                throw new InvalidOperationException($"No se puede modificar una cita en estado {citaActual.EstadoDescripcion}");

            // Verificar que la mascota existe y está activa
            var mascota = _mascotaRepository.ObtenerPorId(cita.IdMascota)
                ?? throw new ArgumentException("La mascota especificada no existe");

            if (!mascota.Activo)
                throw new InvalidOperationException("No se puede asignar cita a una mascota inactiva");

            // Verificar conflictos de horario si se cambió la fecha/hora
            if (citaActual.FechaCita != cita.FechaCita || citaActual.Veterinario != cita.Veterinario)
            {
                ValidarConflictoHorario(cita.Veterinario, cita.FechaCita, cita.IdCita);
            }

            if (citaActual.FechaCita != cita.FechaCita || citaActual.IdMascota != cita.IdMascota)
            {
                ValidarConflictoHorarioMascota(cita.IdMascota, cita.FechaCita, cita.IdCita);
            }

            // Actualizar
            bool resultado = _citaRepository.Update(cita);

            return resultado;
        }

        /// <summary>
        /// UC: Actualizar el estado de una cita
        /// </summary>
        public bool ActualizarEstadoCita(Guid idCita, EstadoCita nuevoEstado)
        {
            // Obtener cita actual
            var cita = _citaRepository.SelectOne(idCita)
                ?? throw new ArgumentException("La cita especificada no existe");

            // Validar transición de estado
            ValidarTransicionEstado(cita.Estado, nuevoEstado);

            // Actualizar estado
            bool resultado = _citaRepository.UpdateEstado(idCita, nuevoEstado);

            return resultado;
        }

        /// <summary>
        /// UC: Confirmar una cita (cambiar estado a Confirmada)
        /// </summary>
        public bool ConfirmarCita(Guid idCita)
        {
            return ActualizarEstadoCita(idCita, EstadoCita.Confirmada);
        }

        /// <summary>
        /// UC: Completar una cita (cambiar estado a Completada)
        /// </summary>
        public bool CompletarCita(Guid idCita)
        {
            return ActualizarEstadoCita(idCita, EstadoCita.Completada);
        }

        /// <summary>
        /// UC: Marcar que el cliente no asistió a la cita
        /// </summary>
        public bool MarcarNoAsistio(Guid idCita)
        {
            return ActualizarEstadoCita(idCita, EstadoCita.NoAsistio);
        }

        #endregion

        #region Casos de Uso - Cancelar

        /// <summary>
        /// UC: Cancelar una cita
        /// Solo se pueden cancelar citas en estado Agendada o Confirmada
        /// </summary>
        public bool CancelarCita(Guid idCita)
        {
            var cita = _citaRepository.SelectOne(idCita)
                ?? throw new ArgumentException("La cita especificada no existe");

            if (!cita.PuedeSerCancelada())
                throw new InvalidOperationException($"No se puede cancelar una cita en estado {cita.EstadoDescripcion}");

            bool resultado = _citaRepository.UpdateEstado(idCita, EstadoCita.Cancelada);

            return resultado;
        }

        /// <summary>
        /// UC: Eliminar una cita (soft delete)
        /// </summary>
        public bool EliminarCita(Guid idCita)
        {
            var cita = _citaRepository.SelectOne(idCita)
                ?? throw new ArgumentException("La cita especificada no existe");

            bool resultado = _citaRepository.Delete(idCita);

            return resultado;
        }

        #endregion

        #region Casos de Uso - Consultar

        /// <summary>
        /// UC: Obtener una cita por ID con toda su información
        /// </summary>
        public Cita ObtenerCita(Guid idCita)
        {
            return _citaRepository.SelectOne(idCita)
                ?? throw new ArgumentException("La cita especificada no existe");
        }

        /// <summary>
        /// UC: Listar todas las citas activas
        /// </summary>
        public List<Cita> ListarTodasLasCitas()
        {
            return _citaRepository.SelectAll();
        }

        /// <summary>
        /// UC: Obtener todas las citas de una mascota
        /// </summary>
        public List<Cita> ObtenerCitasPorMascota(Guid idMascota)
        {
            if (idMascota == Guid.Empty)
                throw new ArgumentException("El ID de mascota no es válido");

            return _citaRepository.SelectByMascota(idMascota);
        }

        /// <summary>
        /// UC: Obtener todas las citas de un cliente
        /// </summary>
        public List<Cita> ObtenerCitasPorCliente(Guid idCliente)
        {
            if (idCliente == Guid.Empty)
                throw new ArgumentException("El ID de cliente no es válido");

            return _citaRepository.SelectByCliente(idCliente);
        }

        /// <summary>
        /// UC: Obtener citas del día (hoy)
        /// </summary>
        public List<Cita> ObtenerCitasDelDia()
        {
            return _citaRepository.SelectByFecha(DateTime.Today);
        }

        /// <summary>
        /// UC: Obtener citas de una fecha específica
        /// </summary>
        public List<Cita> ObtenerCitasPorFecha(DateTime fecha)
        {
            return _citaRepository.SelectByFecha(fecha);
        }

        /// <summary>
        /// UC: Obtener citas de un veterinario
        /// </summary>
        public List<Cita> ObtenerCitasPorVeterinario(string veterinario)
        {
            if (string.IsNullOrWhiteSpace(veterinario))
                throw new ArgumentException("El nombre del veterinario es requerido");

            return _citaRepository.SelectByVeterinario(veterinario);
        }

        /// <summary>
        /// UC: Obtener citas por estado
        /// </summary>
        public List<Cita> ObtenerCitasPorEstado(EstadoCita estado)
        {
            return _citaRepository.SelectByEstado(estado);
        }

        /// <summary>
        /// UC: Buscar citas con múltiples filtros
        /// </summary>
        public List<Cita> BuscarCitas(DateTime? fecha = null, string veterinario = null, EstadoCita? estado = null)
        {
            return _citaRepository.Search(fecha, veterinario, estado);
        }

        /// <summary>
        /// UC: Obtener citas pendientes (Agendadas o Confirmadas) del día
        /// </summary>
        public List<Cita> ObtenerCitasPendientesDelDia()
        {
            var citasHoy = ObtenerCitasDelDia();
            return citasHoy.Where(c => c.Estado == EstadoCita.Agendada || c.Estado == EstadoCita.Confirmada)
                          .OrderBy(c => c.FechaCita)
                          .ToList();
        }

        /// <summary>
        /// UC: Obtener citas en un rango de fechas (útil para reportes)
        /// </summary>
        /// <param name="fechaDesde">Fecha inicio (inclusive)</param>
        /// <param name="fechaHasta">Fecha fin (inclusive)</param>
        /// <returns>Lista de citas en el rango especificado</returns>
        public List<Cita> ObtenerCitasPorRango(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde > fechaHasta)
                throw new ArgumentException("La fecha desde no puede ser mayor que la fecha hasta");

            return _citaRepository.SelectByFechaRango(fechaDesde, fechaHasta);
        }

        /// <summary>
        /// UC: Obtener citas de la semana actual
        /// </summary>
        public List<Cita> ObtenerCitasDeLaSemana()
        {
            var hoy = DateTime.Today;
            var inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek); // Domingo
            var finSemana = inicioSemana.AddDays(6); // Sábado

            return ObtenerCitasPorRango(inicioSemana, finSemana);
        }

        /// <summary>
        /// UC: Obtener citas en un rango de fechas filtradas por veterinario (útil para reportes)
        /// </summary>
        /// <param name="fechaDesde">Fecha inicio (inclusive)</param>
        /// <param name="fechaHasta">Fecha fin (inclusive)</param>
        /// <param name="veterinario">Nombre del veterinario (opcional: si es null o vacío, trae todas)</param>
        /// <returns>Lista de citas en el rango especificado, opcionalmente filtradas por veterinario</returns>
        public List<Cita> ObtenerCitasPorRangoYVeterinario(DateTime fechaDesde, DateTime fechaHasta, string veterinario = null)
        {
            if (fechaDesde > fechaHasta)
                throw new ArgumentException("La fecha desde no puede ser mayor que la fecha hasta");

            var citas = _citaRepository.SelectByFechaRango(fechaDesde, fechaHasta);

            // Si se especificó un veterinario, filtrar por ese veterinario
            if (!string.IsNullOrWhiteSpace(veterinario))
            {
                citas = citas.Where(c => c.Veterinario != null &&
                                         c.Veterinario.Equals(veterinario, StringComparison.OrdinalIgnoreCase))
                            .ToList();
            }

            return citas;
        }

        #endregion

        #region Casos de Uso - Estadísticas y Reportes

        /// <summary>
        /// UC: Obtener veterinarios activos con todos sus datos (para combos en formularios)
        /// Retorna objetos Veterinario completos con IdVeterinario
        /// </summary>
        public List<Veterinario> ObtenerVeterinariosActivos()
        {
            return VeterinarioBLL.Current.ListarVeterinariosActivos()
                                          .OrderBy(v => v.Nombre)
                                          .ToList();
        }

        /// <summary>
        /// UC: Obtener lista de veterinarios activos (legacy - solo nombres)
        /// [OBSOLETO] Usar ObtenerVeterinariosActivos() para obtener objetos completos con IdVeterinario
        /// </summary>
        [Obsolete("Usar ObtenerVeterinariosActivos() para obtener objetos completos con IdVeterinario")]
        public List<string> ObtenerListaVeterinarios()
        {
            return ObtenerVeterinariosActivos().Select(v => v.Nombre).ToList();
        }

        /// <summary>
        /// UC: Obtener lista de tipos de consulta únicos (para filtros)
        /// </summary>
        public List<string> ObtenerListaTiposConsulta()
        {
            var todasLasCitas = _citaRepository.SelectAll();
            return todasLasCitas.Select(c => c.TipoConsulta)
                               .Distinct()
                               .OrderBy(t => t)
                               .ToList();
        }

        #endregion

        #region Validaciones Privadas

        /// <summary>
        /// Valida los datos básicos de una cita
        /// </summary>
        private void ValidarCita(Cita cita)
        {
            if (cita == null)
                throw new ArgumentNullException(nameof(cita), "La cita no puede ser nula");

            var errores = cita.Validar();
            if (errores.Any())
                throw new ArgumentException($"Errores de validación:\n{string.Join("\n", errores)}");
        }

        /// <summary>
        /// Valida que la transición de estado sea válida
        /// </summary>
        private void ValidarTransicionEstado(EstadoCita estadoActual, EstadoCita nuevoEstado)
        {
            // Reglas de transición de estado
            switch (estadoActual)
            {
                case EstadoCita.Agendada:
                    // Desde Agendada se puede ir a cualquier otro estado
                    break;

                case EstadoCita.Confirmada:
                    // Desde Confirmada se puede ir a Completada, Cancelada o NoAsistio
                    if (nuevoEstado == EstadoCita.Agendada)
                        throw new InvalidOperationException("No se puede regresar de Confirmada a Agendada");
                    break;

                case EstadoCita.Completada:
                    // Una cita completada no se puede cambiar de estado
                    throw new InvalidOperationException("No se puede cambiar el estado de una cita completada");

                case EstadoCita.Cancelada:
                    // Una cita cancelada solo se puede reagendar
                    if (nuevoEstado != EstadoCita.Agendada)
                        throw new InvalidOperationException("Una cita cancelada solo se puede reagendar");
                    break;

                case EstadoCita.NoAsistio:
                    // Una cita donde no asistió el cliente solo se puede reagendar
                    if (nuevoEstado != EstadoCita.Agendada)
                        throw new InvalidOperationException("Una cita donde no asistió el cliente solo se puede reagendar");
                    break;
            }
        }

        /// <summary>
        /// Valida conflictos de horario para el veterinario
        /// Verifica si hay otra cita en el mismo horario para el mismo veterinario
        /// </summary>
        private void ValidarConflictoHorario(string veterinario, DateTime fechaCita, Guid? idCitaExcluir = null)
        {
            var citasVeterinario = _citaRepository.SelectByVeterinario(veterinario);

            // Buscar citas en un rango de ±30 minutos
            var citasConflicto = citasVeterinario.Where(c =>
                c.IdCita != (idCitaExcluir ?? Guid.Empty) &&
                (c.Estado == EstadoCita.Agendada || c.Estado == EstadoCita.Confirmada) &&
                Math.Abs((c.FechaCita - fechaCita).TotalMinutes) < 30
            ).ToList();

            if (citasConflicto.Any())
            {
                var citaConflicto = citasConflicto.First();
                throw new InvalidOperationException(
                    $"Conflicto de horario: El veterinario {veterinario} ya tiene una cita " +
                    $"a las {citaConflicto.HoraCita} (Mascota: {citaConflicto.MascotaNombre})"
                );
            }
        }

        /// <summary>
        /// Valida conflictos de horario para la mascota
        /// Verifica si la mascota ya tiene otra cita en el mismo horario
        /// </summary>
        private void ValidarConflictoHorarioMascota(Guid idMascota, DateTime fechaCita, Guid? idCitaExcluir = null)
        {
            var todasLasCitas = _citaRepository.SelectAll();

            // Buscar citas de la misma mascota en un rango de ±30 minutos
            var citasConflicto = todasLasCitas.Where(c =>
                c.IdCita != (idCitaExcluir ?? Guid.Empty) &&
                c.IdMascota == idMascota &&
                (c.Estado == EstadoCita.Agendada || c.Estado == EstadoCita.Confirmada) &&
                Math.Abs((c.FechaCita - fechaCita).TotalMinutes) < 30
            ).ToList();

            if (citasConflicto.Any())
            {
                var citaConflicto = citasConflicto.First();
                throw new InvalidOperationException(
                    $"Conflicto de horario: La mascota {citaConflicto.MascotaNombre} ya tiene una cita " +
                    $"agendada a las {citaConflicto.HoraCita} con el veterinario {citaConflicto.Veterinario}"
                );
            }
        }

        #endregion
    }
}
