using System;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class FormAgregarMascota : Form
    {
        private Cliente _cliente;

        public FormAgregarMascota(Cliente cliente)
        {
            InitializeComponent();
            _cliente = cliente;
            dtpFechaNacimiento.Value = DateTime.Now.AddYears(-1);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación mínima de UI - solo verificar selección de sexo
                if (cboSexo.SelectedIndex == -1)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_sexo_mascota"),
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboSexo.Focus();
                    return;
                }

                // Crear objeto mascota desde los controles
                var mascota = new Mascota
                {
                    IdCliente = _cliente.IdCliente,
                    Nombre = txtNombre.Text.Trim(),
                    Especie = txtEspecie.Text.Trim(),
                    Raza = txtRaza.Text.Trim(),
                    FechaNacimiento = dtpFechaNacimiento.Value,
                    Sexo = cboSexo.SelectedItem.ToString(),
                    Peso = nudPeso.Value,
                    Color = txtColor.Text.Trim(),
                    Observaciones = txtObservaciones.Text.Trim(),
                    Activo = true
                };

                // Registrar mascota usando BLL (valida y persiste en BD)
                var mascotaCreada = MascotaBLL.Current.RegistrarMascota(mascota);

                var mensaje = string.Format(LanguageManager.Translate("mascota_registrada_detalle"),
                    mascotaCreada.Nombre, mascotaCreada.Especie, _cliente.NombreCompleto);
                MessageBox.Show(mensaje, LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ArgumentException ex)
            {
                // Errores de validación del BLL
                MessageBox.Show(ex.Message, LanguageManager.Translate("error_validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                // Errores de reglas de negocio (ej: cliente inactivo)
                MessageBox.Show(ex.Message, LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Errores inesperados
                MessageBox.Show($"{LanguageManager.Translate("error_guardar_mascota")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
