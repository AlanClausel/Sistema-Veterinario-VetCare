using System;

namespace DomainModel
{
    /// <summary>
    /// Entidad Medicamento - Catálogo de medicamentos disponibles en la veterinaria.
    /// Gestiona el inventario, precios y disponibilidad de medicamentos.
    /// </summary>
    public class Medicamento
    {
        /// <summary>
        /// Identificador único del medicamento (GUID)
        /// </summary>
        public Guid IdMedicamento { get; set; }

        /// <summary>
        /// Nombre comercial o genérico del medicamento
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Presentación del medicamento (Ej: "500mg", "Suspensión 250mg/5ml", "10ml inyectable")
        /// </summary>
        public string Presentacion { get; set; }

        /// <summary>
        /// Cantidad disponible en inventario
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Precio unitario del medicamento en la moneda local
        /// </summary>
        public decimal PrecioUnitario { get; set; }

        /// <summary>
        /// Observaciones o notas adicionales sobre el medicamento
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Fecha y hora en que el medicamento fue registrado en el sistema
        /// </summary>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Indica si el medicamento está activo en el sistema (para baja lógica)
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Constructor por defecto. Inicializa un nuevo medicamento con valores predeterminados.
        /// Genera un nuevo GUID, establece la fecha de registro actual y valores iniciales para stock y precio.
        /// </summary>
        public Medicamento()
        {
            IdMedicamento = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
            Activo = true;
            Stock = 0;
            PrecioUnitario = 0;
        }

        /// <summary>
        /// Obtiene el nombre completo del medicamento combinando nombre y presentación
        /// </summary>
        /// <returns>Cadena con el formato "Nombre Presentacion"</returns>
        public string NombreCompleto => $"{Nombre} {Presentacion}";

        /// <summary>
        /// Indica si el medicamento tiene stock disponible
        /// </summary>
        /// <returns>True si el stock es mayor a 0, false en caso contrario</returns>
        public bool DisponibleEnStock => Stock > 0;

        /// <summary>
        /// Valida que los datos del medicamento cumplan con las reglas de negocio.
        /// Verifica campos obligatorios, longitudes mínimas y valores válidos.
        /// </summary>
        /// <returns>Array de mensajes de error. Si está vacío, el medicamento es válido</returns>
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
        /// Reduce el stock del medicamento en la cantidad especificada.
        /// Valida que haya suficiente stock disponible antes de realizar la operación.
        /// </summary>
        /// <param name="cantidad">Cantidad a reducir del stock (debe ser positiva)</param>
        /// <exception cref="ArgumentException">Si la cantidad es negativa</exception>
        /// <exception cref="InvalidOperationException">Si no hay suficiente stock disponible</exception>
        public void ReducirStock(int cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad debe ser positiva");

            if (Stock < cantidad)
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {Stock}, Solicitado: {cantidad}");

            Stock -= cantidad;
        }

        /// <summary>
        /// Aumenta el stock del medicamento en la cantidad especificada.
        /// Útil al recibir nueva mercadería o al realizar devoluciones.
        /// </summary>
        /// <param name="cantidad">Cantidad a agregar al stock (debe ser positiva)</param>
        /// <exception cref="ArgumentException">Si la cantidad es negativa</exception>
        public void AumentarStock(int cantidad)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad debe ser positiva");

            Stock += cantidad;
        }

        /// <summary>
        /// Retorna una representación en cadena del medicamento con su nombre completo y stock
        /// </summary>
        /// <returns>Cadena con el formato "NombreCompleto - Stock: X"</returns>
        public override string ToString()
        {
            return $"{NombreCompleto} - Stock: {Stock}";
        }
    }
}
