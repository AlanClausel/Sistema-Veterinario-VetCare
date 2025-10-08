using System;
using System.Collections.Generic;

namespace DomainModel
{
    /// <summary>
    /// Representa un cliente (dueño de mascotas) en el sistema veterinario
    /// </summary>
    public class Cliente
    {
        public Guid IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        /// <summary>
        /// Lista de mascotas asociadas a este cliente
        /// </summary>
        public List<Mascota> Mascotas { get; set; }

        public Cliente()
        {
            IdCliente = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
            Activo = true;
            Mascotas = new List<Mascota>();
        }

        /// <summary>
        /// Nombre completo del cliente
        /// </summary>
        public string NombreCompleto
        {
            get { return $"{Apellido}, {Nombre}"; }
        }

        public override string ToString()
        {
            return $"{NombreCompleto} - DNI: {DNI}";
        }
    }
}
