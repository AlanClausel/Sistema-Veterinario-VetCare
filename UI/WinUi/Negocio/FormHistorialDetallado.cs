using System;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class FormHistorialDetallado : Form
    {
        private Mascota _mascota;
        private Cliente _cliente;

        public FormHistorialDetallado(Mascota mascota, Cliente cliente)
        {
            InitializeComponent();
            _mascota = mascota;
            _cliente = cliente;
        }

        private void FormHistorialDetallado_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarDataGridView();
                MostrarDatosMascota();
                CargarHistorialConsultas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_formulario")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvConsultas.AutoGenerateColumns = false;
            dgvConsultas.Columns.Clear();

            dgvConsultas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdConsulta",
                DataPropertyName = "IdConsulta",
                Visible = false
            });

            dgvConsultas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Fecha",
                HeaderText = LanguageManager.Translate("fecha"),
                DataPropertyName = "FechaConsulta",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dgvConsultas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Diagnostico",
                HeaderText = LanguageManager.Translate("diagnostico"),
                DataPropertyName = "Diagnostico",
                Width = 250
            });

            dgvConsultas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Veterinario",
                HeaderText = LanguageManager.Translate("veterinario"),
                Width = 150
            });
        }

        private void MostrarDatosMascota()
        {
            txtDatosMascota.Text = $"{LanguageManager.Translate("mascota")}: {_mascota.Nombre} | {LanguageManager.Translate("especie")}: {_mascota.Especie} | {LanguageManager.Translate("edad")}: {_mascota.EdadEnAnios} {LanguageManager.Translate("anios")} | {LanguageManager.Translate("peso")}: {_mascota.Peso:F2} {LanguageManager.Translate("kg")}\r\n" +
                                   $"{LanguageManager.Translate("cliente")}: {_cliente.Nombre} {_cliente.Apellido} | {LanguageManager.Translate("tel_label")}: {_cliente.Telefono ?? LanguageManager.Translate("n_a")}";
        }

        private void CargarHistorialConsultas()
        {
            try
            {
                var consultas = ConsultaMedicaBLL.Current.ObtenerHistorialMascota(_mascota.IdMascota).ToList();

                dgvConsultas.DataSource = consultas;

                // Llenar columna de Veterinario
                foreach (DataGridViewRow row in dgvConsultas.Rows)
                {
                    if (row.DataBoundItem is ConsultaMedica consulta)
                    {
                        var veterinario = VeterinarioBLL.Current.ObtenerVeterinarioPorId(consulta.IdVeterinario);
                        if (veterinario != null)
                        {
                            row.Cells["Veterinario"].Value = string.Format(LanguageManager.Translate("dr_prefijo"), veterinario.Nombre);
                        }
                        else
                        {
                            row.Cells["Veterinario"].Value = LanguageManager.Translate("n_a");
                        }
                    }
                }

                if (consultas.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("mascota_no_tiene_consultas"),
                        LanguageManager.Translate("sin_historial"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ActualizarEstadoBoton();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_historial")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvConsultas_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarEstadoBoton();
        }

        private void ActualizarEstadoBoton()
        {
            btnVerDetalle.Enabled = dgvConsultas.SelectedRows.Count > 0;
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvConsultas.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_consulta"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var consultaSeleccionada = dgvConsultas.SelectedRows[0].DataBoundItem as ConsultaMedica;
                if (consultaSeleccionada == null)
                    return;

                // Abrir formulario de detalle de consulta
                var formDetalle = new FormDetalleConsulta(consultaSeleccionada);
                formDetalle.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_abrir_detalle")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvConsultas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnVerDetalle_Click(sender, e);
            }
        }
    }
}
