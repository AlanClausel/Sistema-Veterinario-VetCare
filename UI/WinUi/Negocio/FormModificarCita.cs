using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class FormModificarCita : Form
    {
        private Cita _citaOriginal;
        private Cliente _cliente;
        private List<string> _tiposConsulta;
        private List<Veterinario> _veterinarios;

        public FormModificarCita(Cita cita)
        {
            InitializeComponent();
            _citaOriginal = cita;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += FormModificarCita_Load;
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;
        }

        private void FormModificarCita_Load(object sender, EventArgs e)
        {
            try
            {
                AplicarTraducciones();
                CargarTiposConsulta();
                CargarVeterinarios();
                ConfigurarDateTimePickers();
                CargarDatosCita();
                CargarMascotasCliente();
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al cargar formulario modificar cita: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_formulario")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarTraducciones()
        {
            this.Text = LanguageManager.Translate("modificar_cita");
            lblCliente.Text = $"{LanguageManager.Translate("cliente_no_modificable")}:";
            lblMascota.Text = $"{LanguageManager.Translate("mascota")}:";
            lblTipo.Text = $"{LanguageManager.Translate("tipo_consulta")}:";
            lblFecha.Text = $"{LanguageManager.Translate("fecha")}:";
            lblHora.Text = $"{LanguageManager.Translate("hora")}:";
            lblVeterinario.Text = $"{LanguageManager.Translate("veterinario")}:";
            lblObservaciones.Text = $"{LanguageManager.Translate("observaciones")}:";
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

                if (!_veterinarios.Any())
                {
                    MessageBox.Show(LanguageManager.Translate("sin_veterinarios_sistema"),
                        LanguageManager.Translate("sin_veterinarios"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

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
            dtpHora.Format = DateTimePickerFormat.Time;
            dtpHora.ShowUpDown = true;
        }

        private void CargarDatosCita()
        {
            txtCliente.Text = _citaOriginal.ClienteNombreCompleto;
            txtCliente.ReadOnly = true;

            dtpFecha.Value = _citaOriginal.FechaCita.Date;
            dtpHora.Value = _citaOriginal.FechaCita;

            // Seleccionar tipo
            int tipoIndex = _tiposConsulta.IndexOf(_citaOriginal.TipoConsulta);
            cboTipo.SelectedIndex = tipoIndex >= 0 ? tipoIndex : 0;

            // Seleccionar veterinario por IdVeterinario o por nombre (legacy)
            if (_citaOriginal.IdVeterinario.HasValue)
            {
                cboVeterinario.SelectedValue = _citaOriginal.IdVeterinario.Value;
            }
            else
            {
                // Fallback: buscar por nombre para citas antiguas sin IdVeterinario
                var vetPorNombre = _veterinarios.FirstOrDefault(v => v.Nombre == _citaOriginal.Veterinario);
                if (vetPorNombre != null)
                    cboVeterinario.SelectedValue = vetPorNombre.IdVeterinario;
            }

            txtObservaciones.Text = _citaOriginal.Observaciones;
        }

        private void CargarMascotasCliente()
        {
            try
            {
                _cliente = ClienteBLL.Current.ObtenerClienteConMascotas(_citaOriginal.IdCliente);
                var mascotasActivas = _cliente.Mascotas.Where(m => m.Activo).ToList();

                cboMascota.DataSource = mascotasActivas;
                cboMascota.DisplayMember = "Nombre";
                cboMascota.ValueMember = "IdMascota";

                // Seleccionar la mascota actual
                cboMascota.SelectedValue = _citaOriginal.IdMascota;
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al cargar mascotas: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos())
                    return;

                // Actualizar datos de la cita
                _citaOriginal.IdMascota = (Guid)cboMascota.SelectedValue;
                _citaOriginal.FechaCita = CombinarFechaHora();
                _citaOriginal.TipoConsulta = cboTipo.SelectedItem.ToString();
                _citaOriginal.IdVeterinario = (Guid)cboVeterinario.SelectedValue; // Asignar ID del veterinario
                _citaOriginal.Veterinario = cboVeterinario.Text; // Legacy: nombre del veterinario
                _citaOriginal.Observaciones = txtObservaciones.Text.Trim();

                // Guardar cambios
                bool resultado = CitaBLL.Current.ModificarCita(_citaOriginal);

                if (resultado)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    LoggerService.WriteLog($"ModificarCita retornó false. IdCita: {_citaOriginal.IdCita}, Nueva fecha: {_citaOriginal.FechaCita}", EventLevel.Warning, string.Empty);
                    MessageBox.Show(LanguageManager.Translate("error_modificar_cita_bd"),
                        LanguageManager.Translate("error_al_modificar"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (InvalidOperationException ex)
            {
                // Errores de reglas de negocio (conflictos de horario, validaciones, etc.)
                LoggerService.WriteLog($"Error de negocio al modificar cita: {ex.Message}", EventLevel.Warning, string.Empty);
                MessageBox.Show(ex.Message, LanguageManager.Translate("no_puede_modificar"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException ex)
            {
                // Errores de validación
                LoggerService.WriteLog($"Error de validación al modificar cita: {ex.Message}", EventLevel.Warning, string.Empty);
                MessageBox.Show(ex.Message, LanguageManager.Translate("datos_invalidos"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Errores inesperados
                LoggerService.WriteLog($"Error inesperado al modificar cita: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error_inesperado_modificar_cita")}:\n{ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private DateTime CombinarFechaHora()
        {
            var fecha = dtpFecha.Value.Date;
            var hora = dtpHora.Value.TimeOfDay;
            return fecha.Add(hora);
        }

        private bool ValidarCampos()
        {
            if (cboMascota.SelectedItem == null)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_mascota"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var fechaCita = CombinarFechaHora();
            if (fechaCita < DateTime.Now.AddMinutes(-5))
            {
                MessageBox.Show(LanguageManager.Translate("no_programar_cita_pasado"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
