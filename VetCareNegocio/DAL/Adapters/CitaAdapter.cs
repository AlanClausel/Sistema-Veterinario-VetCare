using System;
using System.Data;
using DomainModel;

namespace DAL.Adapters
{
    /// <summary>
    /// Adaptador para convertir DataRow a entidad Cita y viceversa
    /// </summary>
    public static class CitaAdapter
    {
        /// <summary>
        /// Convierte un DataRow en una entidad Cita
        /// </summary>
        /// <param name="row">Fila de datos de la consulta SQL</param>
        /// <returns>Instancia de Cita con los datos del DataRow</returns>
        public static Cita DataRowToCita(DataRow row)
        {
            if (row == null)
                return null;

            var cita = new Cita
            {
                IdCita = row["IdCita"] != DBNull.Value ? (Guid)row["IdCita"] : Guid.Empty,
                IdMascota = row["IdMascota"] != DBNull.Value ? (Guid)row["IdMascota"] : Guid.Empty,
                FechaCita = row["FechaCita"] != DBNull.Value ? (DateTime)row["FechaCita"] : DateTime.MinValue,
                TipoConsulta = row["TipoConsulta"] != DBNull.Value ? row["TipoConsulta"].ToString() : string.Empty,
                IdVeterinario = row.Table.Columns.Contains("IdVeterinario") && row["IdVeterinario"] != DBNull.Value
                    ? (Guid?)row["IdVeterinario"]
                    : null,
                Veterinario = row["Veterinario"] != DBNull.Value ? row["Veterinario"].ToString() : string.Empty,
                Estado = row["Estado"] != DBNull.Value ? ParseEstado(row["Estado"].ToString()) : EstadoCita.Agendada,
                Observaciones = row["Observaciones"] != DBNull.Value ? row["Observaciones"].ToString() : string.Empty,
                FechaRegistro = row["FechaRegistro"] != DBNull.Value ? (DateTime)row["FechaRegistro"] : DateTime.MinValue,
                Activo = row["Activo"] != DBNull.Value && Convert.ToBoolean(row["Activo"])
            };

            // Propiedades adicionales de navegación (si existen en el DataRow)
            if (row.Table.Columns.Contains("MascotaNombre") && row["MascotaNombre"] != DBNull.Value)
                cita.MascotaNombre = row["MascotaNombre"].ToString();

            if (row.Table.Columns.Contains("ClienteNombre") && row["ClienteNombre"] != DBNull.Value)
                cita.ClienteNombre = row["ClienteNombre"].ToString();

            if (row.Table.Columns.Contains("ClienteApellido") && row["ClienteApellido"] != DBNull.Value)
                cita.ClienteApellido = row["ClienteApellido"].ToString();

            if (row.Table.Columns.Contains("ClienteDNI") && row["ClienteDNI"] != DBNull.Value)
                cita.ClienteDNI = row["ClienteDNI"].ToString();

            if (row.Table.Columns.Contains("ClienteTelefono") && row["ClienteTelefono"] != DBNull.Value)
                cita.ClienteTelefono = row["ClienteTelefono"].ToString();

            if (row.Table.Columns.Contains("IdCliente") && row["IdCliente"] != DBNull.Value)
                cita.IdCliente = (Guid)row["IdCliente"];

            // Si vienen datos de la mascota, crear la entidad
            if (row.Table.Columns.Contains("Especie") && row.Table.Columns.Contains("Raza"))
            {
                cita.Mascota = new Mascota
                {
                    IdMascota = cita.IdMascota,
                    Nombre = cita.MascotaNombre,
                    Especie = row["Especie"] != DBNull.Value ? row["Especie"].ToString() : string.Empty,
                    Raza = row["Raza"] != DBNull.Value ? row["Raza"].ToString() : string.Empty,
                    IdCliente = cita.IdCliente
                };
            }

            return cita;
        }

        /// <summary>
        /// Convierte una entidad Cita a parámetros de SQL
        /// (utilizado para crear SqlParameters en el Repository)
        /// </summary>
        /// <param name="cita">Instancia de Cita</param>
        /// <returns>Array de objetos para parámetros</returns>
        public static object[] CitaToParameters(Cita cita)
        {
            if (cita == null)
                throw new ArgumentNullException(nameof(cita));

            return new object[]
            {
                cita.IdCita,
                cita.IdMascota,
                cita.FechaCita,
                cita.TipoConsulta ?? string.Empty,
                cita.IdVeterinario.HasValue ? (object)cita.IdVeterinario.Value : DBNull.Value,
                cita.Veterinario ?? string.Empty,
                EstadoToString(cita.Estado),
                cita.Observaciones ?? string.Empty
            };
        }

        /// <summary>
        /// Convierte un string de estado a enum EstadoCita
        /// </summary>
        private static EstadoCita ParseEstado(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                return EstadoCita.Agendada;

            switch (estado.Trim())
            {
                case "Agendada":
                    return EstadoCita.Agendada;
                case "Confirmada":
                    return EstadoCita.Confirmada;
                case "Completada":
                    return EstadoCita.Completada;
                case "Cancelada":
                    return EstadoCita.Cancelada;
                case "NoAsistio":
                    return EstadoCita.NoAsistio;
                default:
                    // Intenta parsear como enum
                    if (Enum.TryParse<EstadoCita>(estado, true, out var result))
                        return result;
                    return EstadoCita.Agendada;
            }
        }

        /// <summary>
        /// Convierte un enum EstadoCita a string para la base de datos
        /// </summary>
        public static string EstadoToString(EstadoCita estado)
        {
            switch (estado)
            {
                case EstadoCita.Agendada:
                    return "Agendada";
                case EstadoCita.Confirmada:
                    return "Confirmada";
                case EstadoCita.Completada:
                    return "Completada";
                case EstadoCita.Cancelada:
                    return "Cancelada";
                case EstadoCita.NoAsistio:
                    return "NoAsistio";
                default:
                    return "Agendada";
            }
        }
    }
}
