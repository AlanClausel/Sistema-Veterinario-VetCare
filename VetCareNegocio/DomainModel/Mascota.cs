using System;

namespace DomainModel
{
    /// <summary>
    /// Representa una mascota en el sistema veterinario
    /// </summary>
    public class Mascota
    {
        public Guid IdMascota { get; set; }
        public Guid IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }  // Perro, Gato, Ave, etc.
        public string Raza { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }  // Macho, Hembra
        public decimal Peso { get; set; }  // En kilogramos
        public string Color { get; set; }
        public string Observaciones { get; set; }
        public bool Activo { get; set; }

        public Mascota()
        {
            IdMascota = Guid.NewGuid();
            Activo = true;
            FechaNacimiento = DateTime.Now;
        }

        /// <summary>
        /// Calcula la edad de la mascota en años
        /// </summary>
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

        public override string ToString()
        {
            return $"{Nombre} ({Especie} - {Raza})";
        }
    }
}
