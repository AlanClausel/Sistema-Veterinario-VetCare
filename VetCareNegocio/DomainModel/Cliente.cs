using System;
using System.Collections.Generic;

namespace DomainModel
{
    /// <summary>
    /// Representa un cliente (dueño de mascotas) en el sistema veterinario.
    /// Contiene información personal y de contacto del propietario de las mascotas.
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Identificador único del cliente (GUID)
        /// </summary>
        public Guid IdCliente { get; set; }

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido del cliente
        /// </summary>
        public string Apellido { get; set; }

        /// <summary>
        /// Documento Nacional de Identidad del cliente (debe ser único)
        /// </summary>
        public string DNI { get; set; }

        /// <summary>
        /// Número de teléfono de contacto
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Dirección de correo electrónico
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Dirección física del domicilio del cliente
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Fecha y hora en que el cliente fue registrado en el sistema
        /// </summary>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Indica si el cliente está activo en el sistema (para baja lógica)
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Lista de mascotas asociadas a este cliente (propiedad de navegación)
        /// </summary>
        public List<Mascota> Mascotas { get; set; }

        /// <summary>
        /// Constructor por defecto. Inicializa un nuevo cliente con valores predeterminados.
        /// Genera un nuevo GUID, establece la fecha de registro actual y marca el cliente como activo.
        /// </summary>
        public Cliente()
        {
            IdCliente = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
            Activo = true;
            Mascotas = new List<Mascota>();
        }

        /// <summary>
        /// Obtiene el nombre completo del cliente en formato "Apellido, Nombre"
        /// </summary>
        /// <returns>Cadena con el formato "Apellido, Nombre"</returns>
        public string NombreCompleto
        {
            get { return $"{Apellido}, {Nombre}"; }
        }

        /// <summary>
        /// Retorna una representación en cadena del cliente con su nombre completo y DNI
        /// </summary>
        /// <returns>Cadena con el formato "NombreCompleto - DNI: XXX"</returns>
        public override string ToString()
        {
            return $"{NombreCompleto} - DNI: {DNI}";
        }
    }
}
