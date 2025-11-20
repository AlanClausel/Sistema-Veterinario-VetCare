using System;

namespace DomainModel
{
    /// <summary>
    /// Representa una mascota en el sistema veterinario.
    /// Contiene información médica y personal de cada animal bajo cuidado.
    /// </summary>
    public class Mascota
    {
        /// <summary>
        /// Identificador único de la mascota (GUID)
        /// </summary>
        public Guid IdMascota { get; set; }

        /// <summary>
        /// Identificador del cliente dueño de la mascota (FK a Cliente)
        /// </summary>
        public Guid IdCliente { get; set; }

        /// <summary>
        /// Nombre de la mascota
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Especie de la mascota (Ej: Perro, Gato, Ave, Reptil, etc.)
        /// </summary>
        public string Especie { get; set; }

        /// <summary>
        /// Raza específica de la mascota
        /// </summary>
        public string Raza { get; set; }

        /// <summary>
        /// Fecha de nacimiento de la mascota
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Sexo de la mascota (Ej: Macho, Hembra)
        /// </summary>
        public string Sexo { get; set; }

        /// <summary>
        /// Peso de la mascota en kilogramos
        /// </summary>
        public decimal Peso { get; set; }

        /// <summary>
        /// Color o colores del pelaje/plumaje de la mascota
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Observaciones o notas adicionales sobre la mascota
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Indica si la mascota está activa en el sistema (para baja lógica)
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Constructor por defecto. Inicializa una nueva mascota con valores predeterminados.
        /// Genera un nuevo GUID, marca la mascota como activa y establece la fecha de nacimiento actual.
        /// </summary>
        public Mascota()
        {
            IdMascota = Guid.NewGuid();
            Activo = true;
            FechaNacimiento = DateTime.Now;
        }

        /// <summary>
        /// Calcula y retorna la edad de la mascota en años completos
        /// </summary>
        /// <returns>Edad en años de la mascota</returns>
        public int EdadEnAnios
        {
            get
            {
                var edad = DateTime.Now.Year - FechaNacimiento.Year;
                if (DateTime.Now < FechaNacimiento.AddYears(edad))
                    edad--;
                return edad;
            }
        }

        /// <summary>
        /// Retorna una representación en cadena de la mascota con su nombre, especie y raza
        /// </summary>
        /// <returns>Cadena con el formato "Nombre (Especie - Raza)"</returns>
        public override string ToString()
        {
            return $"{Nombre} ({Especie} - {Raza})";
        }
    }
}
