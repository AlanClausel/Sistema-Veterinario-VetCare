using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;

namespace BLL
{
    /// <summary>
    /// Capa de lógica de negocio para Veterinarios
    /// Implementa casos de uso y sincronización con SecurityVet
    ///
    /// IMPORTANTE:
    /// - Los veterinarios se gestionan desde "Gestión de Usuarios"
    /// - Este BLL maneja la sincronización automática desde SecurityVet
    /// - El campo Nombre se actualiza automáticamente desde SecurityVet
    /// </summary>
    public class VeterinarioBLL
    {
        private readonly IVeterinarioRepository _veterinarioRepository;

        #region Singleton

        private static readonly VeterinarioBLL _instance = new VeterinarioBLL();

        public static VeterinarioBLL Current
        {
            get { return _instance; }
        }

        private VeterinarioBLL()
        {
            _veterinarioRepository = VeterinarioRepository.Current;
        }

        #endregion

        #region Casos de Uso - Crear Veterinario (desde módulo Seguridad)

        /// <summary>
        /// Caso de uso: Crear veterinario al asignar rol ROL_Veterinario
        /// Se ejecuta automáticamente desde UsuarioBLL al asignar el rol
        /// </summary>
        /// <param name="idUsuario">ID del usuario de SecurityVet</param>
        /// <param name="nombreUsuario">Nombre del usuario de SecurityVet</param>
        public Veterinario CrearDesdeUsuario(Guid idUsuario, string nombreUsuario)
        {
            // Verificar que no exista ya
            if (_veterinarioRepository.Existe(idUsuario))
            {
                // Ya existe, retornar el existente
                return _veterinarioRepository.ObtenerPorId(idUsuario);
            }

            // Crear nuevo veterinario
            var veterinario = new Veterinario(idUsuario, nombreUsuario);

            return _veterinarioRepository.Crear(veterinario);
        }

        #endregion

        #region Casos de Uso - Sincronización desde SecurityVet

        /// <summary>
        /// Caso de uso: Sincronizar nombre del veterinario desde SecurityVet
        /// Se ejecuta al modificar el nombre del usuario en SecurityVet
        /// </summary>
        public Veterinario SincronizarNombreDesdeSecurityVet(Guid idUsuario, string nombreActualizado)
        {
            if (string.IsNullOrWhiteSpace(nombreActualizado))
                throw new ArgumentException("El nombre actualizado no puede estar vacío", nameof(nombreActualizado));

            // Verificar que existe el veterinario
            if (!_veterinarioRepository.Existe(idUsuario))
            {
                // No existe, crear nuevo
                return CrearDesdeUsuario(idUsuario, nombreActualizado);
            }

            // Actualizar nombre
            return _veterinarioRepository.SincronizarNombre(idUsuario, nombreActualizado);
        }

        /// <summary>
        /// Caso de uso: Obtener nombre actualizado desde SecurityVet
        /// Útil para sincronización lazy (al consultar)
        /// </summary>
        public string ObtenerNombreDesdeSecurityVet(Guid idUsuario)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ServicesConString"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                    return null;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT Nombre FROM Usuario WHERE IdUsuario = @IdUsuario", connection))
                    {
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        var result = command.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch
            {
                // Si falla la conexión a SecurityVet, retornar null
                return null;
            }
        }

        /// <summary>
        /// Caso de uso: Obtener veterinario con sincronización automática del nombre
        /// Verifica y actualiza el nombre desde SecurityVet si es necesario
        /// </summary>
        public Veterinario ObtenerVeterinarioConSincronizacion(Guid idVeterinario)
        {
            var veterinario = _veterinarioRepository.ObtenerPorId(idVeterinario);

            if (veterinario != null)
            {
                // Sincronizar nombre desde SecurityVet
                var nombreActual = ObtenerNombreDesdeSecurityVet(idVeterinario);

                if (!string.IsNullOrWhiteSpace(nombreActual) && veterinario.Nombre != nombreActual)
                {
                    // Nombre cambió, actualizar
                    veterinario = _veterinarioRepository.SincronizarNombre(idVeterinario, nombreActual);
                }
            }

            return veterinario;
        }

        #endregion

        #region Casos de Uso - Actualizar Veterinario

        /// <summary>
        /// Caso de uso: Actualizar datos del veterinario (Matricula, Telefono, Email, etc.)
        /// NO actualiza el nombre (se sincroniza desde SecurityVet)
        /// </summary>
        public Veterinario ActualizarDatosVeterinario(Veterinario veterinario)
        {
            ValidarVeterinario(veterinario);

            // Verificar que existe
            var veterinarioExistente = _veterinarioRepository.ObtenerPorId(veterinario.IdVeterinario);
            if (veterinarioExistente == null)
            {
                throw new InvalidOperationException($"No existe un veterinario con ID {veterinario.IdVeterinario}");
            }

            // Validar matrícula única (si se proporciona)
            if (!string.IsNullOrWhiteSpace(veterinario.Matricula))
            {
                if (_veterinarioRepository.ExistePorMatricula(veterinario.Matricula, veterinario.IdVeterinario))
                {
                    throw new InvalidOperationException($"Ya existe otro veterinario con matrícula {veterinario.Matricula}");
                }
            }

            // El nombre siempre se sincroniza desde SecurityVet
            var nombreActual = ObtenerNombreDesdeSecurityVet(veterinario.IdVeterinario);
            if (!string.IsNullOrWhiteSpace(nombreActual))
            {
                veterinario.Nombre = nombreActual;
            }
            else
            {
                // Si no se puede obtener de SecurityVet, mantener el existente
                veterinario.Nombre = veterinarioExistente.Nombre;
            }

            // Mantener fecha de alta original
            veterinario.FechaAlta = veterinarioExistente.FechaAlta;

            return _veterinarioRepository.Actualizar(veterinario);
        }

        #endregion

        #region Casos de Uso - Desactivar Veterinario

        /// <summary>
        /// Caso de uso: Desactivar un veterinario (soft delete)
        /// </summary>
        public void DesactivarVeterinario(Guid idVeterinario)
        {
            var veterinario = _veterinarioRepository.ObtenerPorId(idVeterinario);
            if (veterinario == null)
            {
                throw new InvalidOperationException($"No existe un veterinario con ID {idVeterinario}");
            }

            _veterinarioRepository.Eliminar(idVeterinario);
        }

        /// <summary>
        /// Caso de uso: Reactivar un veterinario
        /// </summary>
        public Veterinario ActivarVeterinario(Guid idVeterinario)
        {
            var veterinario = _veterinarioRepository.ObtenerPorId(idVeterinario);
            if (veterinario == null)
            {
                throw new InvalidOperationException($"No existe un veterinario con ID {idVeterinario}");
            }

            veterinario.Activo = true;
            return _veterinarioRepository.Actualizar(veterinario);
        }

        #endregion

        #region Casos de Uso - Consultar Veterinarios

        /// <summary>
        /// Caso de uso: Obtener un veterinario por su ID
        /// </summary>
        public Veterinario ObtenerVeterinarioPorId(Guid idVeterinario)
        {
            return _veterinarioRepository.ObtenerPorId(idVeterinario);
        }

        /// <summary>
        /// Caso de uso: Listar todos los veterinarios
        /// </summary>
        public IEnumerable<Veterinario> ListarTodosLosVeterinarios()
        {
            return _veterinarioRepository.ObtenerTodos();
        }

        /// <summary>
        /// Caso de uso: Listar solo veterinarios activos (para combos en UI)
        /// </summary>
        public IEnumerable<Veterinario> ListarVeterinariosActivos()
        {
            return _veterinarioRepository.ObtenerActivos();
        }

        /// <summary>
        /// Caso de uso: Verificar si un usuario es veterinario
        /// </summary>
        public bool EsVeterinario(Guid idUsuario)
        {
            return _veterinarioRepository.Existe(idUsuario);
        }

        #endregion

        #region Validaciones Privadas

        /// <summary>
        /// Valida las reglas de negocio para un veterinario
        /// </summary>
        private void ValidarVeterinario(Veterinario veterinario)
        {
            if (veterinario == null)
                throw new ArgumentNullException(nameof(veterinario));

            if (veterinario.IdVeterinario == Guid.Empty)
                throw new ArgumentException("El ID del veterinario es requerido");

            // Validar email (si se proporciona)
            if (!string.IsNullOrWhiteSpace(veterinario.Email))
            {
                if (!EsEmailValido(veterinario.Email))
                    throw new ArgumentException("El formato del email no es válido");
            }

            // Validar matrícula (longitud si se proporciona)
            if (!string.IsNullOrWhiteSpace(veterinario.Matricula))
            {
                if (veterinario.Matricula.Length < 3)
                    throw new ArgumentException("La matrícula debe tener al menos 3 caracteres");
            }

            // Validar teléfono (longitud mínima si se proporciona)
            if (!string.IsNullOrWhiteSpace(veterinario.Telefono))
            {
                if (veterinario.Telefono.Length < 7)
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
}
