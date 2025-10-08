using System;

namespace ServicesSecurity.DomainModel.Security
{
    /// <summary>
    /// Clase intermedia para la relación jerárquica entre Familias (Composite Pattern)
    /// </summary>
    public class FamiliaFamilia
    {
        public Guid idFamiliaPadre { get; set; }
        public Guid idFamiliaHijo { get; set; }
    }
}
