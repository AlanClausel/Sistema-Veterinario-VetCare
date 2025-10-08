using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DomainModel.Exceptions
{
    public class WordNotFoundException : Exception
    {
        public WordNotFoundException() : base("Palabra no encontrada en el archivo de idioma")
        {
        }

        public WordNotFoundException(string word) : base($"La palabra '{word}' no fue encontrada en el archivo de idioma")
        {
        }

        public WordNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
