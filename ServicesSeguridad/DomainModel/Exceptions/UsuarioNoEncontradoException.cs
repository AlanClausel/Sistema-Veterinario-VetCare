using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DomainModel.Exceptions
{
    public class UsuarioNoEncontradoException : AutenticacionException
    {
        public UsuarioNoEncontradoException()
            : base("El usuario ingresado no existe")
        {
        }

        public UsuarioNoEncontradoException(string nombreUsuario)
            : base($"El usuario '{nombreUsuario}' no existe")
        {
        }
    }
}
