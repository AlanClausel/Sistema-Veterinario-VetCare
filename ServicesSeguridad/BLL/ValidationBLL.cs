using System;
using ServicesSecurity.DomainModel.Exceptions;

namespace ServicesSecurity.BLL
{
    public static class ValidationBLL
    {
        /// <summary>
        /// Valida que un campo de texto no esté vacío o nulo
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <param name="fieldName">Nombre del campo para el mensaje de error</param>
        /// <exception cref="ValidacionException">Si el campo está vacío</exception>
        public static void ValidarCampoRequerido(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidacionException($"El campo '{fieldName}' es requerido");
            }
        }

        /// <summary>
        /// Valida credenciales de login
        /// </summary>
        /// <param name="usuario">Nombre de usuario</param>
        /// <param name="contraseña">Contraseña</param>
        /// <exception cref="ValidacionException">Si algún campo está vacío</exception>
        public static void ValidarCredencialesLogin(string usuario, string contraseña)
        {
            ValidarCampoRequerido(usuario, "Usuario");
            ValidarCampoRequerido(contraseña, "Contraseña");
        }

        /// <summary>
        /// Valida la longitud mínima de un campo
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <param name="fieldName">Nombre del campo</param>
        /// <param name="minLength">Longitud mínima</param>
        /// <exception cref="ValidacionException">Si no cumple la longitud mínima</exception>
        public static void ValidarLongitudMinima(string value, string fieldName, int minLength)
        {
            if (value != null && value.Length < minLength)
            {
                throw new ValidacionException($"El campo '{fieldName}' debe tener al menos {minLength} caracteres");
            }
        }
    }
}
