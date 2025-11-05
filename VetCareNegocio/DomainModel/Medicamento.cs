using System;

namespace DomainModel
{
    /// <summary>
    /// Entidad Medicamento - Catálogo de medicamentos disponibles
    /// </summary>
    public class Medicamento
    {
        public Guid IdMedicamento { get; set; }
        public string Nombre { get; set; }
        public string Presentacion { get; set; }  // Ej: "500mg", "Suspensión 250mg/5ml"
        public int Stock { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        // Constructor vacío
        public Medicamento()
        {
            IdMedicamento = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
            Activo = true;
            Stock = 0;
            PrecioUnitario = 0;
        }

        // Propiedades calculadas
        public string NombreCompleto => $"{Nombre} {Presentacion}";
        public bool DisponibleEnStock => Stock > 0;

        /// <summary>
        /// Valida la entidad Medicamento
        /// </summary>
        public string[] Validar()
        {
            var errores = new System.Collections.Generic.List<string>();

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("El nombre del medicamento es requerido");

            if (Nombre != null && Nombre.Length < 3)
                errores.Add("El nombre debe tener al menos 3 caracteres");

            if (Stock < 0)
                errores.Add("El stock no puede ser negativo");

            if (PrecioUnitario < 0)
                errores.Add("El precio no puede ser negativo");

            return errores.ToArray();
        }

        /// <summary>
        /// Reduce el stock del medicamento
        /// </summary>
        public void ReducirStock(int cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad debe ser positiva");

            if (Stock < cantidad)
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {Stock}, Solicitado: {cantidad}");

            Stock -= cantidad;
        }

        /// <summary>
        /// Aumenta el stock del medicamento
        /// </summary>
        public void AumentarStock(int cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad debe ser positiva");

            Stock += cantidad;
        }

        public override string ToString()
        {
            return $"{NombreCompleto} - Stock: {Stock}";
        }
    }
}
