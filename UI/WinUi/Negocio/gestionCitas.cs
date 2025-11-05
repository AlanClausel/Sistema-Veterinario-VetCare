using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class gestionCitas : Form
    {
        private Usuario _usuarioLogueado;
        private Cita _citaSeleccionada;
        private List<Cita> _citasCargadas;

        public gestionCitas()
        {
            InitializeComponent();
        }

        public gestionCitas(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Configurar eventos
            this.Load += GestionCitas_Load;
            btnNuevaCita.Click += BtnNuevaCita_Click;
            btnModificar.Click += BtnModificar_Click;
            btnCancelar.Click += BtnCancelar_Click;
            btnActualizarEstado.Click += BtnActualizarEstado_Click;
            btnBuscar.Click += BtnBuscar_Click;
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;
            dgvCitas.SelectionChanged += DgvCitas_SelectionChanged;
            dgvCitas.DoubleClick += DgvCitas_DoubleClick;
        }

        private void GestionCitas_Load(object sender, EventArgs e)
        {
            try
            {
                AplicarTraducciones();
                ConfigurarDataGrid();
                CargarComboFecha();
                CargarComboVeterinarios();
                CargarComboEstados();

                // Cargar citas del día por defecto
                cboFecha.SelectedIndex = 0; // "Hoy"
                CargarCitas();
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al cargar formulario de citas: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_datos")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("gestion_citas");
                groupBoxFiltros.Text = LanguageManager.Translate("filtros");
                groupBoxCitas.Text = LanguageManager.Translate("citas");
                groupBoxAcciones.Text = LanguageManager.Translate("acciones");

                lblFecha.Text = $"{LanguageManager.Translate("fecha")}:";
                lblVeterinario.Text = $"{LanguageManager.Translate("veterinario")}:";
                lblEstado.Text = $"{LanguageManager.Translate("estado")}:";

                btnNuevaCita.Text = LanguageManager.Translate("nueva_cita");
                btnModificar.Text = LanguageManager.Translate("modificar");
                btnCancelar.Text = LanguageManager.Translate("cancelar_cita");
                btnActualizarEstado.Text = LanguageManager.Translate("actualizar_estado");
                btnBuscar.Text = LanguageManager.Translate("buscar");
                btnLimpiarFiltros.Text = LanguageManager.Translate("limpiar");
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al aplicar traducciones: {ex.Message}", EventLevel.Warning, string.Empty);
            }
        }

        private void ConfigurarDataGrid()
        {
            dgvCitas.AutoGenerateColumns = false;
            dgvCitas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCitas.MultiSelect = false;
            dgvCitas.ReadOnly = true;
            dgvCitas.AllowUserToAddRows = false;
            dgvCitas.AllowUserToDeleteRows = false;

            dgvCitas.Columns.Clear();

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HoraCita",
                HeaderText = "Hora",
                Width = 70
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MascotaNombre",
                HeaderText = "Mascota",
                Width = 100
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ClienteNombreCompleto",
                HeaderText = "Dueño",
                Width = 150
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TipoConsulta",
                HeaderText = "Tipo",
                Width = 120
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Veterinario",
                HeaderText = "Veterinario",
                Width = 120
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EstadoDescripcion",
                HeaderText = "Estado",
                Width = 100
            });

            dgvCitas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Observaciones",
                HeaderText = "Observaciones",
                Width = 200
            });
        }

        private void CargarComboFecha()
        {
            cboFecha.Items.Clear();
            cboFecha.Items.Add("Hoy");
            cboFecha.Items.Add("Esta Semana");
            cboFecha.Items.Add("Este Mes");
            cboFecha.Items.Add("Todas");
            cboFecha.Items.Add("Fecha Específica...");
            cboFecha.SelectedIndex = 0;
        }

        private void CargarComboVeterinarios()
        {
            try
            {
                cboVeterinario.Items.Clear();
                cboVeterinario.Items.Add("Todos");

                var veterinarios = CitaBLL.Current.ObtenerVeterinariosActivos();
                foreach (var vet in veterinarios)
                {
                    cboVeterinario.Items.Add(vet.Nombre);
                }

                cboVeterinario.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al cargar veterinarios: {ex.Message}", EventLevel.Error, string.Empty);
            }
        }

        private void CargarComboEstados()
        {
            cboEstado.Items.Clear();
            cboEstado.Items.Add("Todos");
            cboEstado.Items.Add("Agendada");
            cboEstado.Items.Add("Confirmada");
            cboEstado.Items.Add("Completada");
            cboEstado.Items.Add("Cancelada");
            cboEstado.Items.Add("No Asistió");
            cboEstado.SelectedIndex = 0;
        }

        private void CargarCitas()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Obtener todas las citas
                List<Cita> todasLasCitas = CitaBLL.Current.ListarTodasLasCitas();

                // Aplicar filtro de fecha
                string seleccionFecha = cboFecha.SelectedItem?.ToString() ?? "Hoy";
                _citasCargadas = FiltrarPorFecha(todasLasCitas, seleccionFecha);

                // Aplicar filtro de veterinario
                if (cboVeterinario.SelectedIndex > 0)
                {
                    string veterinario = cboVeterinario.SelectedItem.ToString();
                    _citasCargadas = _citasCargadas.Where(c => c.Veterinario == veterinario).ToList();
                }

                // Aplicar filtro de estado
                EstadoCita? estadoFiltro = ObtenerEstadoFiltro();
                if (estadoFiltro.HasValue)
                {
                    _citasCargadas = _citasCargadas.Where(c => c.Estado == estadoFiltro.Value).ToList();
                }

                // Ordenar por fecha y hora
                _citasCargadas = _citasCargadas.OrderBy(c => c.FechaCita).ToList();

                // Mostrar en el grid
                dgvCitas.DataSource = null;
                dgvCitas.DataSource = _citasCargadas;

                // Mostrar contador
                lblTotalCitas.Text = $"Total: {_citasCargadas.Count} cita(s)";

                // Colorear filas según estado
                ColorearFilasPorEstado();
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al cargar citas: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_citas")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private List<Cita> FiltrarPorFecha(List<Cita> citas, string seleccion)
        {
            switch (seleccion)
            {
                case "Hoy":
                    return citas.Where(c => c.FechaCita.Date == DateTime.Today).ToList();

                case "Esta Semana":
                    // Semana actual: desde hoy hasta el domingo
                    DateTime finSemana = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek);
                    return citas.Where(c => c.FechaCita.Date >= DateTime.Today && c.FechaCita.Date <= finSemana).ToList();

                case "Este Mes":
                    // Mes actual completo
                    int mesActual = DateTime.Today.Month;
                    int añoActual = DateTime.Today.Year;
                    return citas.Where(c => c.FechaCita.Month == mesActual && c.FechaCita.Year == añoActual).ToList();

                case "Todas":
                    return citas;

                case "Fecha Específica...":
                    using (var formFecha = new FormSeleccionarFecha())
                    {
                        if (formFecha.ShowDialog() == DialogResult.OK)
                        {
                            DateTime fechaSeleccionada = formFecha.FechaSeleccionada.Date;
                            return citas.Where(c => c.FechaCita.Date == fechaSeleccionada).ToList();
                        }
                    }
                    // Si cancela, mostrar hoy
                    return citas.Where(c => c.FechaCita.Date == DateTime.Today).ToList();

                default:
                    return citas.Where(c => c.FechaCita.Date == DateTime.Today).ToList();
            }
        }

        private EstadoCita? ObtenerEstadoFiltro()
        {
            if (cboEstado.SelectedIndex <= 0)
                return null;

            string seleccion = cboEstado.SelectedItem.ToString();

            switch (seleccion)
            {
                case "Agendada":
                    return EstadoCita.Agendada;
                case "Confirmada":
                    return EstadoCita.Confirmada;
                case "Completada":
                    return EstadoCita.Completada;
                case "Cancelada":
                    return EstadoCita.Cancelada;
                case "No Asistió":
                    return EstadoCita.NoAsistio;
                default:
                    return null;
            }
        }

        private void ColorearFilasPorEstado()
        {
            foreach (DataGridViewRow row in dgvCitas.Rows)
            {
                var cita = row.DataBoundItem as Cita;
                if (cita != null)
                {
                    switch (cita.Estado)
                    {
                        case EstadoCita.Agendada:
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                            break;
                        case EstadoCita.Confirmada:
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        case EstadoCita.Completada:
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                            break;
                        case EstadoCita.Cancelada:
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                            break;
                        case EstadoCita.NoAsistio:
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                            break;
                    }
                }
            }
        }

        private void DgvCitas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCitas.SelectedRows.Count > 0)
            {
                _citaSeleccionada = (Cita)dgvCitas.SelectedRows[0].DataBoundItem;
                ActualizarEstadoBotones();
            }
        }

        private void DgvCitas_DoubleClick(object sender, EventArgs e)
        {
            if (_citaSeleccionada != null)
            {
                VerDetalleCita();
            }
        }

        private void ActualizarEstadoBotones()
        {
            bool citaSeleccionada = _citaSeleccionada != null;

            btnModificar.Enabled = citaSeleccionada && _citaSeleccionada.PuedeSerModificada();
            btnCancelar.Enabled = citaSeleccionada && _citaSeleccionada.PuedeSerCancelada();
            btnActualizarEstado.Enabled = citaSeleccionada;
        }

        #region Eventos de Botones

        private void BtnNuevaCita_Click(object sender, EventArgs e)
        {
            try
            {
                using (var formNueva = new FormNuevaCita(_usuarioLogueado))
                {
                    if (formNueva.ShowDialog() == DialogResult.OK)
                    {
                        CargarCitas();
                        MessageBox.Show(LanguageManager.Translate("cita_agendada_exito"),
                            LanguageManager.Translate("exito"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al abrir formulario nueva cita: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_citaSeleccionada == null)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_cita"),
                        LanguageManager.Translate("advertencia"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!_citaSeleccionada.PuedeSerModificada())
                {
                    MessageBox.Show($"{LanguageManager.Translate("no_puede_modificar_cita_estado")} {_citaSeleccionada.EstadoDescripcion}",
                        LanguageManager.Translate("advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var formModificar = new FormModificarCita(_citaSeleccionada))
                {
                    if (formModificar.ShowDialog() == DialogResult.OK)
                    {
                        CargarCitas();
                        MessageBox.Show(LanguageManager.Translate("cita_modificada_exito"),
                            LanguageManager.Translate("exito"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al modificar cita: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_citaSeleccionada == null)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_cita"),
                        LanguageManager.Translate("advertencia"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!_citaSeleccionada.PuedeSerCancelada())
                {
                    MessageBox.Show($"{LanguageManager.Translate("no_puede_cancelar_cita_estado")} {_citaSeleccionada.EstadoDescripcion}",
                        LanguageManager.Translate("advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var mensaje = $"{LanguageManager.Translate("confirmar_cancelar_cita")}\n\n" +
                             $"{LanguageManager.Translate("cliente")}: {_citaSeleccionada.ClienteNombreCompleto}\n" +
                             $"{LanguageManager.Translate("mascota")}: {_citaSeleccionada.MascotaNombre}\n" +
                             $"{LanguageManager.Translate("fecha")}: {_citaSeleccionada.FechaCitaFormateada}";

                var resultado = MessageBox.Show(mensaje, LanguageManager.Translate("confirmar_cancelacion"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    CitaBLL.Current.CancelarCita(_citaSeleccionada.IdCita);
                    CargarCitas();
                    MessageBox.Show(LanguageManager.Translate("cita_cancelada_exito"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al cancelar cita: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnActualizarEstado_Click(object sender, EventArgs e)
        {
            try
            {
                if (_citaSeleccionada == null)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_cita"),
                        LanguageManager.Translate("advertencia"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var formEstado = new FormActualizarEstado(_citaSeleccionada))
                {
                    if (formEstado.ShowDialog() == DialogResult.OK)
                    {
                        Guid idCitaSeleccionada = _citaSeleccionada.IdCita; // Guardar ID antes de recargar
                        CargarCitas();

                        // Actualizar referencia con datos frescos de la BD
                        _citaSeleccionada = _citasCargadas.FirstOrDefault(c => c.IdCita == idCitaSeleccionada);

                        MessageBox.Show(LanguageManager.Translate("estado_actualizado_exito"),
                            LanguageManager.Translate("exito"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al actualizar estado: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            CargarCitas();
        }

        private void BtnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            cboFecha.SelectedIndex = 0;
            cboVeterinario.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;
            CargarCitas();
        }

        #endregion

        private void VerDetalleCita()
        {
            if (_citaSeleccionada == null)
                return;

            string detalle = $"{LanguageManager.Translate("detalle_cita").ToUpper()}\n\n" +
                           $"{_citaSeleccionada.ResumenCita}\n\n" +
                           $"{LanguageManager.Translate("observaciones")}:\n{_citaSeleccionada.Observaciones}";

            MessageBox.Show(detalle, LanguageManager.Translate("detalle_cita"),
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    #region Formulario Auxiliar para Seleccionar Fecha

    public class FormSeleccionarFecha : Form
    {
        public DateTime FechaSeleccionada { get; private set; }

        private DateTimePicker dtpFecha;
        private Button btnAceptar;
        private Button btnCancelar;

        public FormSeleccionarFecha()
        {
            InitializeComponent();
            FechaSeleccionada = DateTime.Today;
        }

        private void InitializeComponent()
        {
            this.dtpFecha = new DateTimePicker();
            this.btnAceptar = new Button();
            this.btnCancelar = new Button();

            // dtpFecha
            this.dtpFecha.Format = DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(20, 20);
            this.dtpFecha.Size = new System.Drawing.Size(200, 20);

            // btnAceptar
            this.btnAceptar.Location = new System.Drawing.Point(30, 60);
            this.btnAceptar.Size = new System.Drawing.Size(80, 30);
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.Click += (s, e) => {
                FechaSeleccionada = dtpFecha.Value;
                DialogResult = DialogResult.OK;
                Close();
            };

            // btnCancelar
            this.btnCancelar.Location = new System.Drawing.Point(130, 60);
            this.btnCancelar.Size = new System.Drawing.Size(80, 30);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += (s, e) => {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            // Form
            this.ClientSize = new System.Drawing.Size(240, 110);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Seleccionar Fecha";
        }
    }

    #endregion
}
