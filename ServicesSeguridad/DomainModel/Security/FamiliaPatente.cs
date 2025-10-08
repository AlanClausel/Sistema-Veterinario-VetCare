using System;

namespace ServicesSecurity.DomainModel.Security
{
    /// <summary>
    /// Clase intermedia para la relación muchos-a-muchos entre Familia y Patente
    /// </summary>
    public class FamiliaPatente
    {
        public Guid idFamilia { get; set; }
        public Guid idPatente { get; set; }
    }
}
