using System;
using System.Windows.Forms;
using DomainModel;

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
                if (!ValidarCampos())
                    return;

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

                _cliente.Mascotas.Add(mascota);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar mascota: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre de la mascota es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEspecie.Text))
            {
                MessageBox.Show("La especie es obligatoria", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEspecie.Focus();
                return false;
            }

            if (cboSexo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar el sexo", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSexo.Focus();
                return false;
            }

            return true;
        }
    }
}
