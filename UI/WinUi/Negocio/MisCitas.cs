using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class MisCitas : Form
    {
        private Guid _idVeterinarioActual;
        private ServicesSecurity.DomainModel.Security.Composite.Usuario _usuarioLogueado;

        public MisCitas()
        {
            InitializeComponent();
        }

        public MisCitas(ServicesSecurity.DomainModel.Security.Composite.Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
        }

        private void MisCitas_Load(object sender, EventArgs e)
        {
            try
            {
                // Obtener el ID del veterinario desde el usuario pasado por parámetro o el logueado
                var usuarioLogueado = _usuarioLogueado ?? LoginService.GetUsuarioLogueado();
                if (usuarioLogueado == null)
                {
                    MessageBox.Show(LanguageManager.Translate("no_hay_usuario_logueado"),
                        LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                _idVeterinarioActual = usuarioLogueado.IdUsuario;

                // Verificar que el usuario sea veterinario
                if (!VeterinarioBLL.Current.EsVeterinario(_idVeterinarioActual))
                {
                    MessageBox.Show(LanguageManager.Translate("usuario_no_veterinario"),
                        LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Configurar fecha por defecto (hoy)
                dtpFecha.Value = DateTime.Today;

                // Configurar DataGridView
                ConfigurarDataGridView();

                // Cargar citas
                CargarCitas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_formulario")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvCitas.AutoGenerateColumns = false;
            dgvCitas.Columns.Clear();

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdCita",
                DataPropertyName = "IdCita",
                Visible = false
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Hora",
                HeaderText = LanguageManager.Translate("hora"),
                DataPropertyName = "HoraCita",
                Width = 80
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Mascota",
                HeaderText = LanguageManager.Translate("mascota"),
                DataPropertyName = "MascotaNombre",
                Width = 120
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cliente",
                HeaderText = LanguageManager.Translate("cliente"),
                DataPropertyName = "ClienteNombre",
                Width = 130
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DNI",
                HeaderText = LanguageManager.Translate("dni"),
                DataPropertyName = "ClienteDNI",
                Width = 90
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tipo",
                HeaderText = LanguageManager.Translate("tipo"),
                DataPropertyName = "TipoConsulta",
                Width = 120
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Estado",
                HeaderText = LanguageManager.Translate("estado"),
                DataPropertyName = "EstadoDescripcion",
                Width = 100
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Motivo",
                HeaderText = LanguageManager.Translate("motivo"),
                DataPropertyName = "Motivo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private void CargarCitas()
        {
            try
            {
                DateTime fechaSeleccionada = dtpFecha.Value.Date;

                // Obtener citas del veterinario para la fecha seleccionada
                var todasLasCitas = CitaBLL.Current.ObtenerCitasPorFecha(fechaSeleccionada);

                // Filtrar solo las citas del veterinario actual
                var citasVeterinario = todasLasCitas
                    .Where(c => c.IdVeterinario.HasValue && c.IdVeterinario.Value == _idVeterinarioActual)
                    .OrderBy(c => c.FechaCita)
                    .ToList();

                dgvCitas.DataSource = citasVeterinario;

                // Actualizar botones
                ActualizarEstadoBotones();

                // Colorear filas según estado
                ColorearFilasSegunEstado();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_citas")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ColorearFilasSegunEstado()
        {
            foreach (DataGridViewRow row in dgvCitas.Rows)
            {
                if (row.DataBoundItem is Cita cita)
                {
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
        }

        private void ActualizarEstadoBotones()
        {
            if (dgvCitas.SelectedRows.Count == 0)
            {
                btnIniciarConsulta.Enabled = false;
                btnCompletada.Enabled = false;
                btnCancelar.Enabled = false;
                return;
            }

            var citaSeleccionada = dgvCitas.SelectedRows[0].DataBoundItem as Cita;
            if (citaSeleccionada == null)
            {
                btnIniciarConsulta.Enabled = false;
                btnCompletada.Enabled = false;
                btnCancelar.Enabled = false;
                return;
            }

            // Iniciar Consulta: solo si está Agendada o Confirmada
            btnIniciarConsulta.Enabled = citaSeleccionada.Estado == EstadoCita.Agendada ||
                                         citaSeleccionada.Estado == EstadoCita.Confirmada;

            // Completada: solo si está Agendada o Confirmada
            btnCompletada.Enabled = citaSeleccionada.Estado == EstadoCita.Agendada ||
                                    citaSeleccionada.Estado == EstadoCita.Confirmada;

            // Cancelar: solo si puede ser cancelada
            btnCancelar.Enabled = citaSeleccionada.PuedeSerCancelada();
        }

        private void dgvCitas_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarEstadoBotones();
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            CargarCitas();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarCitas();
        }

        private void btnIniciarConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCitas.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_cita"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var citaSeleccionada = dgvCitas.SelectedRows[0].DataBoundItem as Cita;
                if (citaSeleccionada == null)
                    return;

                // Verificar si ya tiene consulta médica
                var consultaExistente = ConsultaMedicaBLL.Current.ObtenerConsultaPorCita(citaSeleccionada.IdCita);

                if (consultaExistente != null)
                {
                    // Si ya existe, abrir la consulta existente
                    var formConsulta = new FormConsultaMedica(citaSeleccionada.IdCita, _idVeterinarioActual);
                    formConsulta.ShowDialog();
                }
                else
                {
                    // Si no existe, crear nueva consulta
                    var formConsulta = new FormConsultaMedica(citaSeleccionada.IdCita, _idVeterinarioActual);
                    formConsulta.ShowDialog();
                }

                // Recargar citas después de cerrar la consulta
                CargarCitas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_iniciar_consulta")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCompletada_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCitas.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_cita"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var citaSeleccionada = dgvCitas.SelectedRows[0].DataBoundItem as Cita;
                if (citaSeleccionada == null)
                    return;

                var resultado = MessageBox.Show(
                    string.Format(LanguageManager.Translate("confirmar_marcar_completada_cita"), citaSeleccionada.MascotaNombre),
                    LanguageManager.Translate("confirmar"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    CitaBLL.Current.CompletarCita(citaSeleccionada.IdCita);
                    MessageBox.Show(LanguageManager.Translate("cita_marcada_completada"),
                        LanguageManager.Translate("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarCitas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_completar_cita")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCitas.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_cita"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var citaSeleccionada = dgvCitas.SelectedRows[0].DataBoundItem as Cita;
                if (citaSeleccionada == null)
                    return;

                var resultado = MessageBox.Show(
                    string.Format(LanguageManager.Translate("confirmar_cancelar_cita_mascota"), citaSeleccionada.MascotaNombre),
                    LanguageManager.Translate("confirmar"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    CitaBLL.Current.CancelarCita(citaSeleccionada.IdCita);
                    MessageBox.Show(LanguageManager.Translate("cita_cancelada"),
                        LanguageManager.Translate("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarCitas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cancelar_cita")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopiarDNI_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCitas.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_cita"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var citaSeleccionada = dgvCitas.SelectedRows[0].DataBoundItem as Cita;
                if (citaSeleccionada == null || string.IsNullOrWhiteSpace(citaSeleccionada.ClienteDNI))
                {
                    MessageBox.Show(LanguageManager.Translate("no_hay_dni_copiar"),
                        LanguageManager.Translate("informacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Copiar DNI al portapapeles
                Clipboard.SetText(citaSeleccionada.ClienteDNI.Trim());

                // Mostrar confirmación
                var mensaje = string.Format(LanguageManager.Translate("dni_copiado_portapapeles"),
                    citaSeleccionada.ClienteDNI.Trim());
                MessageBox.Show(mensaje,
                    LanguageManager.Translate("dni_copiado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_copiar_dni")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
