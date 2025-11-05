using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class FormReportes : Form
    {
        private ServicesSecurity.DomainModel.Security.Composite.Usuario _usuarioLogueado;

        public FormReportes()
        {
            InitializeComponent();
        }

        public FormReportes(ServicesSecurity.DomainModel.Security.Composite.Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
        }

        private void FormReportes_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarControles();
                CargarReporteSemana();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_formulario")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarControles()
        {
            // Cargar veterinarios en el ComboBox
            CargarVeterinarios();

            // Configurar DataGridView de Citas de la Semana
            dgvCitasSemana.AutoGenerateColumns = false;
            dgvCitasSemana.Columns.Clear();

            dgvCitasSemana.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Fecha",
                HeaderText = LanguageManager.Translate("fecha"),
                Width = 100,
                DataPropertyName = "FechaCita"
            });

            dgvCitasSemana.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Hora",
                HeaderText = LanguageManager.Translate("hora"),
                Width = 70
            });

            dgvCitasSemana.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cliente",
                HeaderText = LanguageManager.Translate("cliente"),
                Width = 150
            });

            dgvCitasSemana.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Mascota",
                HeaderText = LanguageManager.Translate("mascota"),
                DataPropertyName = "MascotaNombre",
                Width = 120
            });

            dgvCitasSemana.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Veterinario",
                HeaderText = LanguageManager.Translate("veterinario"),
                DataPropertyName = "Veterinario",
                Width = 130
            });

            dgvCitasSemana.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TipoConsulta",
                HeaderText = LanguageManager.Translate("tipo_consulta_header"),
                DataPropertyName = "TipoConsulta",
                Width = 120
            });

            dgvCitasSemana.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Estado",
                HeaderText = LanguageManager.Translate("estado"),
                DataPropertyName = "EstadoDescripcion",
                Width = 100
            });

            // Configurar DateTimePickers con fechas de la semana actual
            var hoy = DateTime.Today;
            var inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek); // Domingo
            var finSemana = inicioSemana.AddDays(6); // Sábado

            dtpDesde.Value = inicioSemana;
            dtpHasta.Value = finSemana;
        }

        private void CargarVeterinarios()
        {
            try
            {
                // Obtener lista de veterinarios activos
                var veterinarios = VeterinarioBLL.Current.ListarVeterinariosActivos()
                                                         .OrderBy(v => v.Nombre)
                                                         .Select(v => v.Nombre)
                                                         .ToList();

                // Agregar opción "Todos"
                cmbVeterinario.Items.Clear();
                cmbVeterinario.Items.Add(LanguageManager.Translate("todos"));

                foreach (var vet in veterinarios)
                {
                    cmbVeterinario.Items.Add(vet);
                }

                // Seleccionar "Todos" por defecto
                cmbVeterinario.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_veterinarios")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarReporteSemana()
        {
            try
            {
                btnCargar.Enabled = false;
                Cursor = Cursors.WaitCursor;

                DateTime fechaDesde = dtpDesde.Value.Date;
                DateTime fechaHasta = dtpHasta.Value.Date;

                if (fechaDesde > fechaHasta)
                {
                    MessageBox.Show(LanguageManager.Translate("fecha_desde_mayor_hasta"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener veterinario seleccionado (null si es "Todos")
                string veterinarioSeleccionado = null;
                if (cmbVeterinario.SelectedIndex > 0) // Si no es "Todos"
                {
                    veterinarioSeleccionado = cmbVeterinario.SelectedItem.ToString();
                }

                // Obtener citas del rango, opcionalmente filtradas por veterinario
                var citas = CitaBLL.Current.ObtenerCitasPorRangoYVeterinario(fechaDesde, fechaHasta, veterinarioSeleccionado);

                dgvCitasSemana.DataSource = citas.OrderBy(c => c.FechaCita).ToList();

                // Rellenar columnas calculadas
                foreach (DataGridViewRow row in dgvCitasSemana.Rows)
                {
                    if (row.DataBoundItem is Cita cita)
                    {
                        row.Cells["Hora"].Value = cita.HoraCita;
                        row.Cells["Cliente"].Value = $"{cita.ClienteNombre} {cita.ClienteApellido}";

                        // Colorear según estado
                        switch (cita.Estado)
                        {
                            case EstadoCita.Agendada:
                                row.DefaultCellStyle.BackColor = Color.LightYellow;
                                break;
                            case EstadoCita.Confirmada:
                                row.DefaultCellStyle.BackColor = Color.LightGreen;
                                break;
                            case EstadoCita.Completada:
                                row.DefaultCellStyle.BackColor = Color.LightGray;
                                break;
                            case EstadoCita.Cancelada:
                                row.DefaultCellStyle.BackColor = Color.LightCoral;
                                break;
                            case EstadoCita.NoAsistio:
                                row.DefaultCellStyle.BackColor = Color.LightSalmon;
                                break;
                        }
                    }
                }

                // Actualizar estadísticas
                ActualizarEstadisticas(citas);

                lblTotal.Text = string.Format(LanguageManager.Translate("total_citas_label"), citas.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_reporte")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCargar.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void ActualizarEstadisticas(List<Cita> citas)
        {
            if (citas == null || citas.Count == 0)
            {
                lblAgendadas.Text = string.Format(LanguageManager.Translate("agendadas_label"), 0);
                lblConfirmadas.Text = string.Format(LanguageManager.Translate("confirmadas_label"), 0);
                lblCompletadas.Text = string.Format(LanguageManager.Translate("completadas_label"), 0);
                lblCanceladas.Text = string.Format(LanguageManager.Translate("canceladas_no_asistio_label"), 0, 0);
                return;
            }

            int agendadas = citas.Count(c => c.Estado == EstadoCita.Agendada);
            int confirmadas = citas.Count(c => c.Estado == EstadoCita.Confirmada);
            int completadas = citas.Count(c => c.Estado == EstadoCita.Completada);
            int canceladas = citas.Count(c => c.Estado == EstadoCita.Cancelada);
            int noAsistio = citas.Count(c => c.Estado == EstadoCita.NoAsistio);

            lblAgendadas.Text = string.Format(LanguageManager.Translate("agendadas_label"), agendadas);
            lblConfirmadas.Text = string.Format(LanguageManager.Translate("confirmadas_label"), confirmadas);
            lblCompletadas.Text = string.Format(LanguageManager.Translate("completadas_label"), completadas);
            lblCanceladas.Text = string.Format(LanguageManager.Translate("canceladas_no_asistio_label"), canceladas, noAsistio);
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            CargarReporteSemana();
        }

        private void btnSemanaActual_Click(object sender, EventArgs e)
        {
            var hoy = DateTime.Today;
            var inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek);
            var finSemana = inicioSemana.AddDays(6);

            dtpDesde.Value = inicioSemana;
            dtpHasta.Value = finSemana;

            CargarReporteSemana();
        }

        private void btnMesActual_Click(object sender, EventArgs e)
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddDays(-1);

            dtpDesde.Value = inicioMes;
            dtpHasta.Value = finMes;

            CargarReporteSemana();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCitasSemana.Rows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("no_hay_datos_exportar"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = LanguageManager.Translate("archivo_csv_filter"),
                    FileName = string.Format(LanguageManager.Translate("reporte_citas_filename"), DateTime.Now.ToString("yyyyMMdd_HHmmss"))
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportarACSV(saveDialog.FileName);
                    MessageBox.Show(LanguageManager.Translate("reporte_exportado_exitosamente"),
                        LanguageManager.Translate("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_exportar")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarACSV(string rutaArchivo)
        {
            using (var writer = new System.IO.StreamWriter(rutaArchivo, false, System.Text.Encoding.UTF8))
            {
                // Escribir encabezados
                writer.WriteLine(LanguageManager.Translate("csv_headers"));

                // Escribir filas
                foreach (DataGridViewRow row in dgvCitasSemana.Rows)
                {
                    if (row.DataBoundItem is Cita cita)
                    {
                        string fecha = cita.FechaCita.ToString("dd/MM/yyyy");
                        string hora = cita.HoraCita;
                        string cliente = $"{cita.ClienteNombre} {cita.ClienteApellido}";
                        string mascota = cita.MascotaNombre;
                        string veterinario = cita.Veterinario;
                        string tipoConsulta = cita.TipoConsulta;
                        string estado = cita.EstadoDescripcion;

                        writer.WriteLine($"{fecha},{hora},{cliente},{mascota},{veterinario},{tipoConsulta},{estado}");
                    }
                }
            }
        }
    }
}
