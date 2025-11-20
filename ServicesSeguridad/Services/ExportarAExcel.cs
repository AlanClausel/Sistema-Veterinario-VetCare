using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Servicio genérico para exportación de datos a diferentes formatos (CSV, Excel, etc).
    /// Implementa patrón Singleton para garantizar una única instancia en toda la aplicación.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Este servicio proporciona funcionalidad centralizada para exportar datos desde:
    /// - Listas genéricas de objetos
    /// - DataGridView de Windows Forms
    /// - Diccionarios personalizados con selectores de columnas
    /// - Objetos específicos como Bitacora (método legacy)
    /// </para>
    ///
    /// <para><b>Formatos soportados actualmente:</b></para>
    /// <list type="bullet">
    /// <item>CSV (Comma-Separated Values) con encoding UTF-8</item>
    /// </list>
    ///
    /// <para><b>Características:</b></para>
    /// <list type="bullet">
    /// <item>Escape automático de caracteres especiales (comillas, comas)</item>
    /// <item>Soporte para valores nulos</item>
    /// <item>Encabezados personalizables o automáticos</item>
    /// <item>Validación de datos antes de exportar</item>
    /// </list>
    ///
    /// <para><b>Uso típico:</b></para>
    /// <code>
    /// // Exportar desde DataGridView
    /// ExportarAExcel.Current.ExportarDataGridViewACSV(dgvCitas, "C:\\reportes\\citas.csv");
    ///
    /// // Exportar lista genérica con columnas personalizadas
    /// var columnas = new Dictionary&lt;string, Func&lt;Cita, string&gt;&gt;
    /// {
    ///     { "Fecha", c => c.FechaCita.ToString("dd/MM/yyyy") },
    ///     { "Cliente", c => $"{c.ClienteNombre} {c.ClienteApellido}" }
    /// };
    /// ExportarAExcel.Current.ExportarACSV(listaCitas, "citas.csv", columnas);
    /// </code>
    /// </remarks>
    public sealed class ExportarAExcel
    {
        #region Singleton

        private static readonly ExportarAExcel _instance = new ExportarAExcel();
        private static readonly object _lock = new object();

        /// <summary>
        /// Obtiene la instancia única del servicio de exportación.
        /// </summary>
        public static ExportarAExcel Current
        {
            get
            {
                lock (_lock)
                {
                    return _instance;
                }
            }
        }

        private ExportarAExcel()
        {
        }

        #endregion

        #region Métodos Legacy (Retrocompatibilidad)

        /// <summary>
        /// [LEGACY] Exporta una lista de objetos Bitacora a un archivo Excel (CSV).
        /// Mantiene compatibilidad con código existente.
        /// </summary>
        /// <param name="registros">Lista de registros de Bitacora</param>
        /// <param name="rutaArchivo">Ruta del archivo de destino</param>
        [Obsolete("Usar ExportarACSV<T> con columnas personalizadas para mayor flexibilidad")]
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

        #endregion

        #region Exportación Genérica a CSV

        /// <summary>
        /// Exporta una lista genérica de objetos a un archivo CSV con columnas personalizadas.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a exportar</typeparam>
        /// <param name="datos">Lista de objetos a exportar</param>
        /// <param name="rutaArchivo">Ruta completa del archivo CSV de destino</param>
        /// <param name="columnas">Diccionario que define nombre de columna y selector de valor</param>
        /// <exception cref="ArgumentNullException">Si datos o columnas son null</exception>
        /// <exception cref="ArgumentException">Si la lista está vacía o no hay columnas definidas</exception>
        /// <exception cref="IOException">Si hay error al escribir el archivo</exception>
        /// <remarks>
        /// <para>
        /// Este método permite máxima flexibilidad al definir qué propiedades exportar
        /// y cómo formatear cada valor mediante funciones lambda.
        /// </para>
        /// <para><b>Ejemplo de uso:</b></para>
        /// <code>
        /// var columnas = new Dictionary&lt;string, Func&lt;Cliente, string&gt;&gt;
        /// {
        ///     { "DNI", c => c.DNI },
        ///     { "Nombre Completo", c => $"{c.Nombre} {c.Apellido}" },
        ///     { "Email", c => c.Email ?? "Sin email" },
        ///     { "Fecha Alta", c => c.FechaAlta.ToString("dd/MM/yyyy") }
        /// };
        ///
        /// ExportarAExcel.Current.ExportarACSV(clientes, "clientes.csv", columnas);
        /// </code>
        /// </remarks>
        public void ExportarACSV<T>(
            List<T> datos,
            string rutaArchivo,
            Dictionary<string, Func<T, string>> columnas)
        {
            if (datos == null)
                throw new ArgumentNullException(nameof(datos), "La lista de datos no puede ser nula");

            if (columnas == null || columnas.Count == 0)
                throw new ArgumentException("Debe definir al menos una columna para exportar", nameof(columnas));

            if (datos.Count == 0)
                throw new ArgumentException("No hay datos para exportar", nameof(datos));

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("La ruta del archivo no puede estar vacía", nameof(rutaArchivo));

            try
            {
                using (var writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
                {
                    // Escribir encabezados
                    var headers = string.Join(",", columnas.Keys.Select(h => $"\"{EscaparCsv(h)}\""));
                    writer.WriteLine(headers);

                    // Escribir filas
                    foreach (var item in datos)
                    {
                        var valores = columnas.Values.Select(selector =>
                        {
                            try
                            {
                                var valor = selector(item);
                                return $"\"{EscaparCsv(valor ?? string.Empty)}\"";
                            }
                            catch
                            {
                                return "\"\""; // En caso de error, celda vacía
                            }
                        });

                        writer.WriteLine(string.Join(",", valores));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"Error al exportar a CSV: {ex.Message}", ex);
            }
        }

        #endregion

        #region Exportación desde DataGridView

        /// <summary>
        /// Exporta todo el contenido visible de un DataGridView a un archivo CSV.
        /// </summary>
        /// <param name="dgv">DataGridView con los datos a exportar</param>
        /// <param name="rutaArchivo">Ruta completa del archivo CSV de destino</param>
        /// <param name="incluirColumnasInvisibles">Si true, exporta también columnas ocultas (por defecto false)</param>
        /// <exception cref="ArgumentNullException">Si dgv es null</exception>
        /// <exception cref="ArgumentException">Si el DataGridView no tiene filas</exception>
        /// <exception cref="IOException">Si hay error al escribir el archivo</exception>
        /// <remarks>
        /// <para>
        /// Exporta automáticamente todas las columnas visibles del DataGridView.
        /// Respeta el formato del HeaderText de cada columna.
        /// </para>
        /// <para><b>Ejemplo de uso:</b></para>
        /// <code>
        /// // Exportar solo columnas visibles
        /// ExportarAExcel.Current.ExportarDataGridViewACSV(dgvCitas, "reporte_citas.csv");
        ///
        /// // Exportar todas las columnas (incluidas ocultas)
        /// ExportarAExcel.Current.ExportarDataGridViewACSV(dgvCitas, "reporte_completo.csv", true);
        /// </code>
        /// </remarks>
        public void ExportarDataGridViewACSV(
            DataGridView dgv,
            string rutaArchivo,
            bool incluirColumnasInvisibles = false)
        {
            if (dgv == null)
                throw new ArgumentNullException(nameof(dgv), "El DataGridView no puede ser nulo");

            if (dgv.Rows.Count == 0)
                throw new ArgumentException("El DataGridView no contiene filas para exportar", nameof(dgv));

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("La ruta del archivo no puede estar vacía", nameof(rutaArchivo));

            try
            {
                using (var writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
                {
                    // Obtener columnas a exportar
                    var columnasAExportar = dgv.Columns.Cast<DataGridViewColumn>()
                        .Where(c => incluirColumnasInvisibles || c.Visible)
                        .OrderBy(c => c.DisplayIndex)
                        .ToList();

                    if (columnasAExportar.Count == 0)
                        throw new ArgumentException("No hay columnas visibles para exportar");

                    // Escribir encabezados
                    var headers = columnasAExportar.Select(c => $"\"{EscaparCsv(c.HeaderText)}\"");
                    writer.WriteLine(string.Join(",", headers));

                    // Escribir filas
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue; // Saltar fila de inserción

                        var valores = columnasAExportar.Select(col =>
                        {
                            var celda = row.Cells[col.Index];
                            var valor = celda.Value?.ToString() ?? string.Empty;
                            return $"\"{EscaparCsv(valor)}\"";
                        });

                        writer.WriteLine(string.Join(",", valores));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"Error al exportar DataGridView a CSV: {ex.Message}", ex);
            }
        }

        #endregion

        #region Exportación Simple

        /// <summary>
        /// Exporta datos simples proporcionando encabezados y filas de valores.
        /// Útil para exportaciones rápidas sin necesidad de objetos complejos.
        /// </summary>
        /// <param name="encabezados">Lista de nombres de columnas</param>
        /// <param name="filas">Lista de filas, donde cada fila es una lista de valores en el mismo orden que los encabezados</param>
        /// <param name="rutaArchivo">Ruta completa del archivo CSV de destino</param>
        /// <exception cref="ArgumentNullException">Si encabezados o filas son null</exception>
        /// <exception cref="ArgumentException">Si no hay encabezados o filas</exception>
        /// <exception cref="IOException">Si hay error al escribir el archivo</exception>
        /// <remarks>
        /// <para><b>Ejemplo de uso:</b></para>
        /// <code>
        /// var encabezados = new List&lt;string&gt; { "ID", "Nombre", "Email" };
        /// var filas = new List&lt;List&lt;string&gt;&gt;
        /// {
        ///     new List&lt;string&gt; { "1", "Juan Pérez", "juan@example.com" },
        ///     new List&lt;string&gt; { "2", "María López", "maria@example.com" }
        /// };
        ///
        /// ExportarAExcel.Current.ExportarDatosSimples(encabezados, filas, "usuarios.csv");
        /// </code>
        /// </remarks>
        public void ExportarDatosSimples(
            List<string> encabezados,
            List<List<string>> filas,
            string rutaArchivo)
        {
            if (encabezados == null || encabezados.Count == 0)
                throw new ArgumentException("Debe proporcionar encabezados", nameof(encabezados));

            if (filas == null || filas.Count == 0)
                throw new ArgumentException("No hay filas para exportar", nameof(filas));

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("La ruta del archivo no puede estar vacía", nameof(rutaArchivo));

            try
            {
                using (var writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
                {
                    // Escribir encabezados
                    var headers = string.Join(",", encabezados.Select(h => $"\"{EscaparCsv(h)}\""));
                    writer.WriteLine(headers);

                    // Escribir filas
                    foreach (var fila in filas)
                    {
                        var valores = fila.Select(v => $"\"{EscaparCsv(v ?? string.Empty)}\"");
                        writer.WriteLine(string.Join(",", valores));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"Error al exportar datos simples a CSV: {ex.Message}", ex);
            }
        }

        #endregion

        #region Métodos Auxiliares Privados

        /// <summary>
        /// Escapa caracteres especiales en valores CSV según el estándar RFC 4180.
        /// </summary>
        /// <param name="valor">Valor a escapar</param>
        /// <returns>Valor escapado para CSV</returns>
        /// <remarks>
        /// Escapa comillas dobles duplicándolas según el estándar CSV.
        /// Ejemplo: "Juan "El Grande" Pérez" se convierte en "Juan ""El Grande"" Pérez"
        /// </remarks>
        private static string EscaparCsv(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            // Escapar comillas dobles duplicándolas (estándar CSV RFC 4180)
            return valor.Replace("\"", "\"\"");
        }

        #endregion

        #region Validación y Utilidades

        /// <summary>
        /// Valida que la ruta de archivo tenga extensión .csv, si no la agrega automáticamente.
        /// </summary>
        /// <param name="rutaArchivo">Ruta del archivo a validar</param>
        /// <returns>Ruta con extensión .csv garantizada</returns>
        public string NormalizarRutaCSV(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("La ruta del archivo no puede estar vacía", nameof(rutaArchivo));

            if (!rutaArchivo.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                // Si termina en .xlsx u otra extensión, reemplazarla
                var extension = Path.GetExtension(rutaArchivo);
                if (!string.IsNullOrEmpty(extension))
                {
                    rutaArchivo = rutaArchivo.Substring(0, rutaArchivo.Length - extension.Length);
                }
                rutaArchivo += ".csv";
            }

            return rutaArchivo;
        }

        /// <summary>
        /// Valida que el directorio de destino exista, si no lo crea.
        /// </summary>
        /// <param name="rutaArchivo">Ruta del archivo</param>
        /// <exception cref="IOException">Si no se puede crear el directorio</exception>
        public void ValidarYCrearDirectorio(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("La ruta del archivo no puede estar vacía", nameof(rutaArchivo));

            var directorio = Path.GetDirectoryName(rutaArchivo);
            if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
            {
                try
                {
                    Directory.CreateDirectory(directorio);
                }
                catch (Exception ex)
                {
                    throw new IOException($"No se pudo crear el directorio: {directorio}", ex);
                }
            }
        }

        #endregion
    }
}
