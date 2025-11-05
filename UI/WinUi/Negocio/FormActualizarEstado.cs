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
    public partial class FormActualizarEstado : Form
    {
        private Cita _cita;

        public FormActualizarEstado(Cita cita)
        {
            InitializeComponent();
            _cita = cita;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += FormActualizarEstado_Load;
            btnConfirmar.Click += BtnConfirmar_Click;
            btnCancelar.Click += BtnCancelar_Click;
        }

        private void FormActualizarEstado_Load(object sender, EventArgs e)
        {
            try
            {
                AplicarTraducciones();
                MostrarInformacionCita();
                CargarEstados();
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al cargar formulario actualizar estado: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_formulario")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarTraducciones()
        {
            this.Text = LanguageManager.Translate("actualizar_estado_cita");
            groupBoxInfo.Text = LanguageManager.Translate("informacion_cita");
            lblEstadoActual.Text = $"{LanguageManager.Translate("estado_actual")}:";
            lblNuevoEstado.Text = $"{LanguageManager.Translate("nuevo_estado")}:";
            btnConfirmar.Text = LanguageManager.Translate("confirmar");
            btnCancelar.Text = LanguageManager.Translate("cancelar");
        }

        private void MostrarInformacionCita()
        {
            var info = $"{LanguageManager.Translate("cliente_info")}: {_cita.ClienteNombreCompleto}\n" +
                      $"{LanguageManager.Translate("mascota")}: {_cita.MascotaNombre}\n" +
                      $"{LanguageManager.Translate("fecha")}: {_cita.FechaCitaFormateada}\n" +
                      $"{LanguageManager.Translate("tipo")}: {_cita.TipoConsulta}\n" +
                      $"{LanguageManager.Translate("veterinario")}: {_cita.Veterinario}";

            txtInformacion.Text = info;
            txtEstadoActual.Text = _cita.EstadoDescripcion;
        }

        private void CargarEstados()
        {
            cboNuevoEstado.Items.Clear();

            // Agregar estados según el estado actual
            switch (_cita.Estado)
            {
                case EstadoCita.Agendada:
                    cboNuevoEstado.Items.Add(LanguageManager.Translate("confirmada"));
                    cboNuevoEstado.Items.Add(LanguageManager.Translate("completada"));
                    cboNuevoEstado.Items.Add(LanguageManager.Translate("cancelada"));
                    cboNuevoEstado.Items.Add(LanguageManager.Translate("no_asistio"));
                    break;

                case EstadoCita.Confirmada:
                    cboNuevoEstado.Items.Add(LanguageManager.Translate("completada"));
                    cboNuevoEstado.Items.Add(LanguageManager.Translate("cancelada"));
                    cboNuevoEstado.Items.Add(LanguageManager.Translate("no_asistio"));
                    break;

                case EstadoCita.Cancelada:
                case EstadoCita.NoAsistio:
                    cboNuevoEstado.Items.Add(LanguageManager.Translate("agendada"));
                    break;

                case EstadoCita.Completada:
                    MessageBox.Show(LanguageManager.Translate("cita_completada_no_cambiar_estado"),
                        LanguageManager.Translate("informacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnConfirmar.Enabled = false;
                    break;
            }

            if (cboNuevoEstado.Items.Count > 0)
                cboNuevoEstado.SelectedIndex = 0;
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboNuevoEstado.SelectedItem == null)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_nuevo_estado"),
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string estadoSeleccionado = cboNuevoEstado.SelectedItem.ToString();
                EstadoCita nuevoEstado = ParsearEstado(estadoSeleccionado);

                // Actualizar estado
                bool resultado = CitaBLL.Current.ActualizarEstadoCita(_cita.IdCita, nuevoEstado);

                if (resultado)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(LanguageManager.Translate("no_actualizar_estado"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al actualizar estado: {ex.Message} - StackTrace: {ex.StackTrace}", EventLevel.Error, string.Empty);
                MessageBox.Show($"{LanguageManager.Translate("error_actualizar_estado")}:\n{ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private EstadoCita ParsearEstado(string estado)
        {
            // Comparar con traducciones
            if (estado == LanguageManager.Translate("agendada"))
                return EstadoCita.Agendada;
            if (estado == LanguageManager.Translate("confirmada"))
                return EstadoCita.Confirmada;
            if (estado == LanguageManager.Translate("completada"))
                return EstadoCita.Completada;
            if (estado == LanguageManager.Translate("cancelada"))
                return EstadoCita.Cancelada;
            if (estado == LanguageManager.Translate("no_asistio"))
                return EstadoCita.NoAsistio;

            return EstadoCita.Agendada;
        }
    }
}
