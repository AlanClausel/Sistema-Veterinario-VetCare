using System;
using System.Windows.Forms;
using ServicesSecurity.BLL;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;

namespace UI.WinUi.Administrador
{
    public partial class FormMiCuenta : BaseObservableForm
    {
        private Usuario _usuarioLogueado;

        public FormMiCuenta()
        {
            InitializeComponent();
        }

        public FormMiCuenta(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario ?? throw new ArgumentNullException(nameof(usuario));
        }

        private void FormMiCuenta_Load(object sender, EventArgs e)
        {
            try
            {
                // ActualizarTextos() se llama automáticamente por BaseObservableForm
                CargarInformacionUsuario();
                CargarIdiomasDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_datos")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Actualiza todos los textos del formulario según el idioma actual.
        /// Implementa el patrón Observer: se invoca automáticamente cuando cambia el idioma.
        /// </summary>
        protected override void ActualizarTextos()
        {
            // Título del formulario
            this.Text = $"{LanguageManager.Translate("mi_cuenta")} - {LanguageManager.Translate("sistema_veterinario")}";
            lblTitulo.Text = LanguageManager.Translate("mi_cuenta");

            // Botón cerrar
            btnCerrar.Text = LanguageManager.Translate("cerrar");

            // GroupBox Información
            grpInformacion.Text = LanguageManager.Translate("informacion_usuario");
            label1.Text = $"{LanguageManager.Translate("usuario")}:";
            label2.Text = $"{LanguageManager.Translate("email")}:";
            label3.Text = $"{LanguageManager.Translate("roles")}:";

            // GroupBox Contraseña
            grpContraseña.Text = LanguageManager.Translate("cambiar_contraseña");
            label5.Text = $"{LanguageManager.Translate("contraseña_actual")}:";
            label6.Text = $"{LanguageManager.Translate("nueva_contraseña")}:";
            label7.Text = $"{LanguageManager.Translate("confirmar_contraseña")}:";
            chkMostrarContraseña.Text = LanguageManager.Translate("mostrar_contraseñas");
            btnCambiarContraseña.Text = LanguageManager.Translate("cambiar_contraseña");

            // GroupBox Idioma
            grpIdioma.Text = LanguageManager.Translate("preferencias_idioma");
            label8.Text = $"{LanguageManager.Translate("idioma")}:";
            btnGuardarIdioma.Text = LanguageManager.Translate("guardar_idioma");
        }

        private void CargarInformacionUsuario()
        {
            // Información del usuario (solo lectura)
            txtUsuario.Text = _usuarioLogueado.Nombre;
            txtEmail.Text = _usuarioLogueado.Email ?? LanguageManager.Translate("no_especificado");

            // Obtener rol del usuario
            var rol = _usuarioLogueado.ObtenerNombreRol();
            txtRoles.Text = string.IsNullOrEmpty(rol) ? LanguageManager.Translate("sin_roles_asignados") : rol;
        }

        private void CargarIdiomasDisponibles()
        {
            cmbIdioma.Items.Clear();
            cmbIdioma.Items.Add(new IdiomaItem { Codigo = "es-AR", Nombre = "Español (Argentina)" });
            cmbIdioma.Items.Add(new IdiomaItem { Codigo = "en-GB", Nombre = "English (United Kingdom)" });

            // Seleccionar idioma actual
            string idiomaActual = _usuarioLogueado.IdiomaPreferido ?? "es-AR";
            for (int i = 0; i < cmbIdioma.Items.Count; i++)
            {
                var item = (IdiomaItem)cmbIdioma.Items[i];
                if (item.Codigo == idiomaActual)
                {
                    cmbIdioma.SelectedIndex = i;
                    break;
                }
            }
        }

        private void btnCambiarContraseña_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(txtContraseñaActual.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("debe_ingresar_contraseña_actual"),
                        LanguageManager.Translate("atencion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContraseñaActual.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNuevaContraseña.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("debe_ingresar_nueva_contraseña"),
                        LanguageManager.Translate("atencion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNuevaContraseña.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtConfirmarContraseña.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("debe_confirmar_contraseña"),
                        LanguageManager.Translate("atencion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmarContraseña.Focus();
                    return;
                }

                // Validar que las contraseñas coincidan
                if (txtNuevaContraseña.Text != txtConfirmarContraseña.Text)
                {
                    MessageBox.Show(LanguageManager.Translate("contraseñas_no_coinciden"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtConfirmarContraseña.Focus();
                    txtConfirmarContraseña.SelectAll();
                    return;
                }

                // Validar longitud mínima
                if (txtNuevaContraseña.Text.Length < 6)
                {
                    MessageBox.Show(LanguageManager.Translate("contraseña_min_caracteres"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNuevaContraseña.Focus();
                    return;
                }

                // Cambiar contraseña
                UsuarioBLL.CambiarContraseña(
                    _usuarioLogueado.IdUsuario,
                    txtContraseñaActual.Text,
                    txtNuevaContraseña.Text);

                MessageBox.Show(LanguageManager.Translate("contraseña_cambiada_exito"),
                    LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar campos
                txtContraseñaActual.Clear();
                txtNuevaContraseña.Clear();
                txtConfirmarContraseña.Clear();
            }
            catch (ServicesSecurity.DomainModel.Exceptions.ContraseñaInvalidaException)
            {
                MessageBox.Show(LanguageManager.Translate("contraseña_actual_incorrecta"),
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContraseñaActual.Focus();
                txtContraseñaActual.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardarIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbIdioma.SelectedIndex < 0)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_idioma"),
                        LanguageManager.Translate("atencion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var idiomaSeleccionado = (IdiomaItem)cmbIdioma.SelectedItem;
                string idiomaActual = _usuarioLogueado.IdiomaPreferido ?? "es-AR";

                if (idiomaSeleccionado.Codigo == idiomaActual)
                {
                    MessageBox.Show(LanguageManager.Translate("idioma_igual_actual"),
                        LanguageManager.Translate("informacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Actualizar idioma en base de datos
                UsuarioBLL.ActualizarIdioma(_usuarioLogueado.IdUsuario, idiomaSeleccionado.Codigo);

                // Actualizar objeto en memoria
                _usuarioLogueado.IdiomaPreferido = idiomaSeleccionado.Codigo;

                // PATRÓN OBSERVER: Cambiar idioma notifica a todos los formularios abiertos
                // Los cambios se aplican inmediatamente, sin necesidad de reiniciar
                LanguageManager.CambiarIdioma(idiomaSeleccionado.Codigo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkMostrarContraseña_CheckedChanged(object sender, EventArgs e)
        {
            bool mostrar = chkMostrarContraseña.Checked;
            txtContraseñaActual.UseSystemPasswordChar = !mostrar;
            txtNuevaContraseña.UseSystemPasswordChar = !mostrar;
            txtConfirmarContraseña.UseSystemPasswordChar = !mostrar;
        }

        /// <summary>
        /// Clase auxiliar para los items del ComboBox de idiomas
        /// </summary>
        private class IdiomaItem
        {
            public string Codigo { get; set; }
            public string Nombre { get; set; }

            public override string ToString()
            {
                return Nombre;
            }
        }
    }
}
