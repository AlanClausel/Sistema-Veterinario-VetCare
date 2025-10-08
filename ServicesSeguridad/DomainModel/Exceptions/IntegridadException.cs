using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DomainModel.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se detecta una violación de integridad de datos
    /// Por ejemplo, cuando el DVH (Dígito Verificador Horizontal) no coincide
    /// </summary>
    public class IntegridadException : Exception
    {
        public IntegridadException() : base("Se detectó una violación de integridad de datos")
        {
        }

        public IntegridadException(string message) : base(message)
        {
        }

        public IntegridadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
