using System;

namespace ServicesSecurity.DomainModel.Security
{
    /// <summary>
    /// Clase intermedia para la relación muchos-a-muchos entre Usuario y Familia
    /// </summary>
    public class UsuarioFamilia
    {
        public Guid idUsuario { get; set; }
        public Guid idFamilia { get; set; }
    }
}
