using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class FormHistorialClinico : Form
    {
        private Cliente _clienteSeleccionado;
        private ServicesSecurity.DomainModel.Security.Composite.Usuario _usuarioLogueado;

        public FormHistorialClinico()
        {
            InitializeComponent();
        }

        public FormHistorialClinico(ServicesSecurity.DomainModel.Security.Composite.Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
        }

        private void FormHistorialClinico_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarDataGridView();
                LimpiarCliente();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_formulario")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvMascotas.AutoGenerateColumns = false;
            dgvMascotas.Columns.Clear();

            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdMascota",
                DataPropertyName = "IdMascota",
                Visible = false
            });

            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Nombre",
                HeaderText = LanguageManager.Translate("mascota"),
                DataPropertyName = "Nombre",
                Width = 120
            });

            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Especie",
                HeaderText = LanguageManager.Translate("especie"),
                DataPropertyName = "Especie",
                Width = 100
            });

            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Edad",
                HeaderText = LanguageManager.Translate("edad"),
                DataPropertyName = "EdadEnAnios",
                Width = 60
            });

            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UltimaVisita",
                HeaderText = LanguageManager.Translate("ultima_visita"),
                Width = 100
            });
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string dni = txtDNI.Text.Trim();

                if (string.IsNullOrWhiteSpace(dni))
                {
                    MessageBox.Show(LanguageManager.Translate("debe_ingresar_dni_buscar"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Buscar cliente por DNI
                var cliente = ClienteBLL.Current.BuscarClientePorDNI(dni);

                if (cliente == null)
                {
                    MessageBox.Show(string.Format(LanguageManager.Translate("no_encontrado_cliente_dni_detalle"), dni),
                        LanguageManager.Translate("cliente_no_encontrado_titulo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCliente();
                    return;
                }

                // Cargar cliente y sus mascotas
                _clienteSeleccionado = cliente;
                MostrarCliente(cliente);
                CargarMascotas(cliente.IdCliente);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_buscar_cliente")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarCliente(Cliente cliente)
        {
            txtClienteInfo.Text = $"{cliente.Nombre} {cliente.Apellido} - {LanguageManager.Translate("dni")}: {cliente.DNI}\r\n{LanguageManager.Translate("tel_label")}: {cliente.Telefono ?? LanguageManager.Translate("n_a")}";
            grpCliente.Visible = true;
        }

        private void LimpiarCliente()
        {
            _clienteSeleccionado = null;
            txtClienteInfo.Clear();
            grpCliente.Visible = false;
            dgvMascotas.DataSource = null;
            btnVerHistorial.Enabled = false;
        }

        private void CargarMascotas(Guid idCliente)
        {
            try
            {
                var mascotas = MascotaBLL.Current.ListarMascotasPorCliente(idCliente).ToList();

                dgvMascotas.DataSource = mascotas;

                // Calcular última visita para cada mascota
                foreach (DataGridViewRow row in dgvMascotas.Rows)
                {
                    if (row.DataBoundItem is Mascota mascota)
                    {
                        // Obtener historial de consultas
                        var consultas = ConsultaMedicaBLL.Current.ObtenerHistorialMascota(mascota.IdMascota);
                        var ultimaConsulta = consultas.OrderByDescending(c => c.FechaConsulta).FirstOrDefault();

                        if (ultimaConsulta != null)
                        {
                            row.Cells["UltimaVisita"].Value = ultimaConsulta.FechaConsulta.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            row.Cells["UltimaVisita"].Value = LanguageManager.Translate("sin_visitas");
                        }
                    }
                }

                ActualizarEstadoBoton();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_mascotas")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvMascotas_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarEstadoBoton();
        }

        private void ActualizarEstadoBoton()
        {
            btnVerHistorial.Enabled = dgvMascotas.SelectedRows.Count > 0;
        }

        private void btnVerHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMascotas.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_mascota_historial"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var mascotaSeleccionada = dgvMascotas.SelectedRows[0].DataBoundItem as Mascota;
                if (mascotaSeleccionada == null)
                    return;

                // Abrir formulario de historial detallado
                var formDetalle = new FormHistorialDetallado(mascotaSeleccionada, _clienteSeleccionado);
                formDetalle.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_abrir_historial")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Buscar con Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnBuscar_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
