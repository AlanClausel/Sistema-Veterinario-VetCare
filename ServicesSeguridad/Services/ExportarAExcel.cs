using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Clase para exportar datos a Excel/CSV
    /// </summary>
    public static class ExportarAExcel
    {
        /// <summary>
        /// Exporta una lista de objetos Bitacora a un archivo Excel (CSV)
        /// </summary>
        public static void Exportar(IEnumerable<DomainModel.Security.Bitacora> registros, string rutaArchivo)
        {
            if (registros == null || !registros.Any())
                throw new ArgumentException("No hay registros para exportar");

            var sb = new StringBuilder();

            // Encabezados
            sb.AppendLine("Fecha y Hora,Usuario,Módulo,Acción,Descripción,Criticidad,Tabla,IP");

            // Datos
            foreach (var registro in registros)
            {
                sb.AppendLine($"\"{registro.FechaHora:dd/MM/yyyy HH:mm:ss}\"," +
                             $"\"{EscaparCsv(registro.NombreUsuario)}\"," +
                             $"\"{EscaparCsv(registro.Modulo)}\"," +
                             $"\"{EscaparCsv(registro.Accion)}\"," +
                             $"\"{EscaparCsv(registro.Descripcion)}\"," +
                             $"\"{EscaparCsv(registro.Criticidad)}\"," +
                             $"\"{EscaparCsv(registro.Tabla)}\"," +
                             $"\"{EscaparCsv(registro.IP)}\"");
            }

            // Cambiar extensión a .csv si es .xlsx
            if (rutaArchivo.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                rutaArchivo = rutaArchivo.Substring(0, rutaArchivo.Length - 5) + ".csv";
            }

            // Guardar archivo
            File.WriteAllText(rutaArchivo, sb.ToString(), Encoding.UTF8);
        }

        private static string EscaparCsv(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return "";

            // Escapar comillas dobles duplicándolas
            return valor.Replace("\"", "\"\"");
        }
    }
}
