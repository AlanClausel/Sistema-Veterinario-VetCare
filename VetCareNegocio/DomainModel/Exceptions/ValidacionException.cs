using System;

namespace DomainModel.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando falla una validación de reglas de negocio
    /// </summary>
    public class ValidacionException : Exception
    {
        public ValidacionException() : base()
        {
        }

        public ValidacionException(string message) : base(message)
        {
        }

        public ValidacionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
