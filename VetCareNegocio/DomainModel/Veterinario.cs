using System;
using System.Collections.Generic;

namespace DomainModel
{
    /// <summary>
    /// Entidad de dominio que representa un Veterinario
    ///
    /// IMPORTANTE:
    /// - IdVeterinario coincide con IdUsuario de la tabla Usuario en SecurityVet
    /// - El campo Nombre se sincroniza automáticamente desde SecurityVet
    /// - Los veterinarios se gestionan desde "Gestión de Usuarios"
    /// - Este registro se crea automáticamente al asignar el rol ROL_Veterinario
    /// </summary>
    public class Veterinario
    {
        #region Propiedades

        /// <summary>
        /// Identificador único (coincide con IdUsuario de SecurityVet)
        /// </summary>
        public Guid IdVeterinario { get; set; }

        /// <summary>
        /// Nombre completo (sincronizado desde SecurityVet - solo lectura)
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Número de matrícula profesional (opcional)
        /// </summary>
        public string Matricula { get; set; }

        /// <summary>
        /// Teléfono de contacto del consultorio (opcional)
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Email profesional (opcional, puede diferir del email de login)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Observaciones o notas adicionales
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Fecha de alta del veterinario
        /// </summary>
        public DateTime FechaAlta { get; set; }

        /// <summary>
        /// Indica si el veterinario está activo
        /// </summary>
        public bool Activo { get; set; }

        #endregion

        #region Propiedades Calculadas

        /// <summary>
        /// Información completa del veterinario para mostrar en combos
        /// </summary>
        public string NombreConMatricula
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Matricula))
                    return Nombre;

                return $"{Nombre} (MP {Matricula})";
            }
        }

        /// <summary>
        /// Información completa del veterinario para mostrar en diálogos
        /// </summary>
        public string InformacionCompleta
        {
            get
            {
                var info = $"Nombre: {Nombre}\n";

                if (!string.IsNullOrWhiteSpace(Matricula))
                    info += $"Matrícula: {Matricula}\n";

                if (!string.IsNullOrWhiteSpace(Telefono))
                    info += $"Teléfono: {Telefono}\n";

                if (!string.IsNullOrWhiteSpace(Email))
                    info += $"Email: {Email}\n";

                info += $"Estado: {(Activo ? "Activo" : "Inactivo")}";

                return info;
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Veterinario()
        {
            IdVeterinario = Guid.NewGuid();
            FechaAlta = DateTime.Now;
            Activo = true;
        }

        /// <summary>
        /// Constructor para crear veterinario desde un Usuario
        /// </summary>
        /// <param name="idUsuario">ID del usuario de SecurityVet</param>
        /// <param name="nombreUsuario">Nombre del usuario de SecurityVet</param>
        public Veterinario(Guid idUsuario, string nombreUsuario)
        {
            IdVeterinario = idUsuario;
            Nombre = nombreUsuario;
            FechaAlta = DateTime.Now;
            Activo = true;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Verifica si el veterinario tiene datos completos para atender citas
        /// </summary>
        public bool TieneDatosCompletos()
        {
            return !string.IsNullOrWhiteSpace(Nombre) && Activo;
        }

        /// <summary>
        /// Actualiza el nombre desde SecurityVet (para sincronización)
        /// </summary>
        /// <param name="nombreActualizado">Nombre actualizado desde SecurityVet</param>
        public void ActualizarNombre(string nombreActualizado)
        {
            if (!string.IsNullOrWhiteSpace(nombreActualizado))
            {
                Nombre = nombreActualizado;
            }
        }

        #endregion

        #region Validaciones

        /// <summary>
        /// Valida que los datos del veterinario sean correctos
        /// </summary>
        public List<string> Validar()
        {
            var errores = new List<string>();

            if (IdVeterinario == Guid.Empty)
                errores.Add("El ID del veterinario es requerido");

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre es requerido");

            if (Nombre != null && Nombre.Length > 150)
                errores.Add("El nombre no puede exceder 150 caracteres");

            if (Matricula != null && Matricula.Length > 50)
                errores.Add("La matrícula no puede exceder 50 caracteres");

            if (Telefono != null && Telefono.Length > 20)
                errores.Add("El teléfono no puede exceder 20 caracteres");

            if (Email != null)
            {
                if (Email.Length > 100)
                    errores.Add("El email no puede exceder 100 caracteres");

                // Validación básica de formato de email
                if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains("@"))
                    errores.Add("El formato del email no es válido");
            }

            if (Observaciones != null && Observaciones.Length > 500)
                errores.Add("Las observaciones no pueden exceder 500 caracteres");

            return errores;
        }

        #endregion

        #region Override

        public override string ToString()
        {
            return NombreConMatricula;
        }

        public override bool Equals(object obj)
        {
            if (obj is Veterinario otroVeterinario)
            {
                return IdVeterinario == otroVeterinario.IdVeterinario;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return IdVeterinario.GetHashCode();
        }

        #endregion
    }
}
