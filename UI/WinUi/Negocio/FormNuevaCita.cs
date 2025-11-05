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
    public partial class FormNuevaCita : Form
    {
        private Usuario _usuarioLogueado;
        private Cliente _clienteSeleccionado;
        private List<string> _tiposConsulta;
        private List<Veterinario> _veterinarios;

        public FormNuevaCita()
        {
            InitializeComponent();
        }

        public FormNuevaCita(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += FormNuevaCita_Load;
            btnBuscarCliente.Click += BtnBuscarCliente_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;
            txtDNI.KeyPress += TxtDNI_KeyPress;
        }

        private void FormNuevaCita_Load(object sender, EventArgs e)
        {
            try
            {
                AplicarTraducciones();
                CargarTiposConsulta();
                CargarVeterinarios();
                ConfigurarDateTimePickers();
                BloquearCampos();
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al cargar formulario nueva cita: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_formulario")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarTraducciones()
        {
            this.Text = LanguageManager.Translate("nueva_cita");
            groupBoxBusqueda.Text = LanguageManager.Translate("buscar_cliente_titulo");
            groupBoxDatos.Text = LanguageManager.Translate("datos_cita");

            lblDNI.Text = $"{LanguageManager.Translate("dni_cliente")}:";
            lblCliente.Text = $"{LanguageManager.Translate("cliente")}:";
            lblMascota.Text = $"{LanguageManager.Translate("mascota")}:";
            lblTipo.Text = $"{LanguageManager.Translate("tipo_consulta")}:";
            lblFecha.Text = $"{LanguageManager.Translate("fecha")}:";
            lblHora.Text = $"{LanguageManager.Translate("hora")}:";
            lblVeterinario.Text = $"{LanguageManager.Translate("veterinario")}:";
            lblObservaciones.Text = $"{LanguageManager.Translate("observaciones")}:";

            btnBuscarCliente.Text = LanguageManager.Translate("buscar");
            btnGuardar.Text = LanguageManager.Translate("guardar");
            btnCancelar.Text = LanguageManager.Translate("cancelar");
        }

        private void CargarTiposConsulta()
        {
            _tiposConsulta = new List<string>
            {
                LanguageManager.Translate("consulta_general"),
                LanguageManager.Translate("vacunacion"),
                LanguageManager.Translate("emergencia"),
                LanguageManager.Translate("otros")
            };

            cboTipo.DataSource = _tiposConsulta;
        }

        private void CargarVeterinarios()
        {
            try
            {
                _veterinarios = CitaBLL.Current.ObtenerVeterinariosActivos();

                // Si no hay veterinarios activos en el sistema
                if (!_veterinarios.Any())
                {
                    MessageBox.Show(LanguageManager.Translate("no_hay_veterinarios_activos"),
                        LanguageManager.Translate("sin_veterinarios"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    // Desactivar el formulario
                    this.Close();
                    return;
                }

                // Configurar ComboBox con DisplayMember y ValueMember
                cboVeterinario.DataSource = _veterinarios;
                cboVeterinario.DisplayMember = "Nombre";
                cboVeterinario.ValueMember = "IdVeterinario";
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al cargar veterinarios: {ex.Message}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_veterinarios")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ConfigurarDateTimePickers()
        {
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.MinDate = DateTime.Today;
            dtpFecha.Value = DateTime.Today;

            dtpHora.Format = DateTimePickerFormat.Time;
            dtpHora.ShowUpDown = true;

            // Redondear a la siguiente hora (maneja correctamente el cambio de día)
            var ahora = DateTime.Now;
            var horaRedondeada = ahora.Date.AddHours(ahora.Hour + 1);
            dtpHora.Value = horaRedondeada;
        }

        private void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                string dni = txtDNI.Text.Trim();

                if (string.IsNullOrWhiteSpace(dni))
                {
                    MessageBox.Show(LanguageManager.Translate("ingrese_dni_cliente"),
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDNI.Focus();
                    return;
                }

                // Buscar cliente por DNI con sus mascotas
                _clienteSeleccionado = ClienteBLL.Current.BuscarClientePorDNIConMascotas(dni);

                if (_clienteSeleccionado == null)
                {
                    MessageBox.Show(LanguageManager.Translate("no_encontrado_cliente_dni"),
                        LanguageManager.Translate("no_encontrado"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCliente.Clear();
                    cboMascota.DataSource = null;
                    return;
                }

                if (!_clienteSeleccionado.Activo)
                {
                    MessageBox.Show(LanguageManager.Translate("cliente_inactivo"),
                        LanguageManager.Translate("cliente_inactivo_titulo"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCliente.Clear();
                    cboMascota.DataSource = null;
                    return;
                }

                // Mostrar datos del cliente
                txtCliente.Text = _clienteSeleccionado.NombreCompleto;

                // Cargar mascotas activas del cliente
                var mascotasActivas = _clienteSeleccionado.Mascotas.Where(m => m.Activo).ToList();

                if (!mascotasActivas.Any())
                {
                    MessageBox.Show(LanguageManager.Translate("cliente_sin_mascotas"),
                        LanguageManager.Translate("sin_mascotas"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMascota.DataSource = null;
                    return;
                }

                cboMascota.DataSource = mascotasActivas;
                cboMascota.DisplayMember = "Nombre";
                cboMascota.ValueMember = "IdMascota";

                // Desbloquear campos para continuar
                DesbloquearCampos();
                cboMascota.Focus();
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al buscar cliente: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error_buscar_cliente")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos())
                    return;

                // Crear nueva cita
                var cita = new Cita
                {
                    IdMascota = (Guid)cboMascota.SelectedValue,
                    FechaCita = CombinarFechaHora(),
                    TipoConsulta = cboTipo.SelectedItem.ToString(),
                    IdVeterinario = (Guid)cboVeterinario.SelectedValue, // ✅ Asignar el ID del veterinario
                    Veterinario = cboVeterinario.Text, // Legacy: nombre del veterinario
                    Observaciones = txtObservaciones.Text.Trim()
                };

                // Agendar cita
                var citaCreada = CitaBLL.Current.AgendarCita(cita);

                var mensaje = string.Format(LanguageManager.Translate("cita_agendada_detalle"),
                    _clienteSeleccionado.NombreCompleto,
                    ((Mascota)cboMascota.SelectedItem).Nombre,
                    citaCreada.FechaCitaFormateada);
                MessageBox.Show(mensaje, LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al guardar cita: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error_guardar_cita")}:\n{ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir números
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }

            // Si presiona Enter, buscar
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                BtnBuscarCliente_Click(sender, EventArgs.Empty);
            }
        }

        private DateTime CombinarFechaHora()
        {
            var fecha = dtpFecha.Value.Date;
            var hora = dtpHora.Value.TimeOfDay;
            return fecha.Add(hora);
        }

        private bool ValidarCampos()
        {
            if (_clienteSeleccionado == null)
            {
                MessageBox.Show(LanguageManager.Translate("debe_buscar_seleccionar_cliente"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDNI.Focus();
                return false;
            }

            if (cboMascota.SelectedItem == null)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_mascota"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMascota.Focus();
                return false;
            }

            if (cboTipo.SelectedItem == null)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_tipo_consulta"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTipo.Focus();
                return false;
            }

            var fechaCita = CombinarFechaHora();
            if (fechaCita < DateTime.Now)
            {
                MessageBox.Show(LanguageManager.Translate("no_agendar_cita_pasado"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFecha.Focus();
                return false;
            }

            if (cboVeterinario.SelectedItem == null)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_veterinario"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboVeterinario.Focus();
                return false;
            }

            return true;
        }

        private void BloquearCampos()
        {
            cboMascota.Enabled = false;
            cboTipo.Enabled = false;
            dtpFecha.Enabled = false;
            dtpHora.Enabled = false;
            cboVeterinario.Enabled = false;
            txtObservaciones.Enabled = false;
            btnGuardar.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            cboMascota.Enabled = true;
            cboTipo.Enabled = true;
            dtpFecha.Enabled = true;
            dtpHora.Enabled = true;
            cboVeterinario.Enabled = true;
            txtObservaciones.Enabled = true;
            btnGuardar.Enabled = true;
        }
    }
}
