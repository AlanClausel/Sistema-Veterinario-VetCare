using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DomainModel.Exceptions
{
    public class ValidacionException : Exception
    {
        public ValidacionException() : base("Error de validación")
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
