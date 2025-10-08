using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DomainModel.Exceptions
{
    public class AutenticacionException : Exception
    {
        public AutenticacionException() : base("Error de autenticación")
        {
        }

        public AutenticacionException(string message) : base(message)
        {
        }

        public AutenticacionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
