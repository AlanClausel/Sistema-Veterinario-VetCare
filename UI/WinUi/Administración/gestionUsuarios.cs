using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.DomainModel.Exceptions;
using ServicesSecurity.BLL;
using ServicesSecurity.Services;

namespace UI.WinUi.Administrador
{
    public partial class gestionUsuarios : Form
    {
        private Usuario _usuarioLogueado;
        private Usuario _usuarioSeleccionado;
        private bool _modoEdicion = false;

        public gestionUsuarios()
        {
            InitializeComponent();
        }

        public gestionUsuarios(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Configurar eventos
            this.Load += GestionUsuarios_Load;
            btnNuevo.Click += BtnNuevo_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnVolver.Click += BtnVolver_Click;
            btnBuscar.Click += BtnBuscar_Click;
            dgvUsuarios.SelectionChanged += DgvUsuarios_SelectionChanged;

            // Cargar roles (Familias) en ComboBox
            CargarRolesEnComboBox();

            // Configurar estado inicial
            BloquearCampos();
            btnGuardar.Enabled = false;
        }

        private void CargarRolesEnComboBox()
        {
            try
            {
                var rolesDisponibles = UsuarioBLL.ObtenerRolesDisponibles();

                comboBoxPerfil.Items.Clear();

                foreach (var rol in rolesDisponibles)
                {
                    comboBoxPerfil.Items.Add(rol);
                }

                // Configurar cómo se muestra el texto en el ComboBox
                comboBoxPerfil.DisplayMember = "NombreRol";  // Muestra nombre sin prefijo "ROL_"
                comboBoxPerfil.ValueMember = "IdComponent";  // Valor es el ID
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar roles: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void GestionUsuarios_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarTodosLosUsuarios();
        }

        private void AplicarTraducciones()
        {
            try
            {
                // Traducir título del formulario
                this.Text = LanguageManager.Translate("gestion_usuarios");

                // Traducir GroupBox
                groupBoxDatosUsuario.Text = LanguageManager.Translate("datos_usuario");
                groupBoxAcciones.Text = LanguageManager.Translate("acciones");

                // Traducir Labels
                label1.Text = LanguageManager.Translate("buscar_usuario") + ":";
                lblNombreUsuario.Text = LanguageManager.Translate("nombre_usuario");
                lblEmail.Text = LanguageManager.Translate("email");
                lblContraseña.Text = LanguageManager.Translate("contraseña");
                lblPerfil.Text = LanguageManager.Translate("perfil");

                // Traducir Botones
                btnNuevo.Text = LanguageManager.Translate("nuevo");
                btnGuardar.Text = LanguageManager.Translate("guardar");
                btnModificar.Text = LanguageManager.Translate("editar");
                btnEliminar.Text = LanguageManager.Translate("eliminar");
                btnBuscar.Text = LanguageManager.Translate("buscar");
                btnVolver.Text = LanguageManager.Translate("volver");
            }
            catch (Exception ex)
            {
                // Log error pero no interrumpir la carga del formulario
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        #region Carga de Datos

        private void CargarTodosLosUsuarios()
        {
            try
            {
                var usuarios = UsuarioBLL.ObtenerTodosLosUsuarios();

                // Configurar DataGridView
                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = usuarios.Select(u => new
                {
                    u.IdUsuario,
                    Nombre = u.Nombre,
                    Email = u.Email ?? "",
                    Rol = u.ObtenerNombreRol() ?? "Sin asignar"
                }).ToList();

                // Ocultar columna ID
                if (dgvUsuarios.Columns["IdUsuario"] != null)
                    dgvUsuarios.Columns["IdUsuario"].Visible = false;

                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        #region Búsqueda

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreBusqueda = txtBuscarPorUsuario.Text.Trim();

                if (string.IsNullOrWhiteSpace(nombreBusqueda))
                {
                    MessageBox.Show("Ingrese un nombre de usuario para buscar",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var usuario = UsuarioBLL.ObtenerUsuarioPorNombre(nombreBusqueda);

                // Cargar datos en los campos
                _usuarioSeleccionado = usuario;
                MostrarDatosUsuario(usuario);

                MessageBox.Show($"Usuario '{usuario.Nombre}' encontrado",
                    "Búsqueda exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UsuarioNoEncontradoException ex)
            {
                MessageBox.Show(ex.Message, "Usuario no encontrado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            catch (ValidacionException ex)
            {
                MessageBox.Show(ex.Message, "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar usuario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region CRUD Operations

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            _modoEdicion = false;
            _usuarioSeleccionado = null;
            LimpiarCampos();
            DesbloquearCampos();
            btnGuardar.Enabled = true;
            txtNombreUsuario.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombreUsuario.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtContraseña.Text;

                // Validar formato de email
                if (!ValidarFormatoEmail(email))
                {
                    MessageBox.Show("El formato del email no es válido. Debe contener '@' y un dominio válido (ej: usuario@dominio.com)",
                        "Validación de Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                // Obtener la Familia de rol seleccionada
                var rolSeleccionado = comboBoxPerfil.SelectedItem as Familia;
                if (rolSeleccionado == null)
                {
                    MessageBox.Show("Debe seleccionar un rol", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_modoEdicion && _usuarioSeleccionado != null)
                {
                    // Actualizar usuario existente
                    UsuarioBLL.ActualizarUsuario(
                        _usuarioSeleccionado.IdUsuario,
                        nombre,
                        email,
                        password,
                        rolSeleccionado.IdComponent  // Pasar ID de la Familia de rol
                    );

                    MessageBox.Show("Usuario actualizado correctamente",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Crear nuevo usuario (idioma por defecto: es-AR)
                    UsuarioBLL.CrearUsuario(nombre, email, password, rolSeleccionado.IdComponent, "es-AR");

                    MessageBox.Show("Usuario creado correctamente",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarTodosLosUsuarios();
                BloquearCampos();
                btnGuardar.Enabled = false;
                _modoEdicion = false;
            }
            catch (ValidacionException ex)
            {
                MessageBox.Show(ex.Message, "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                MessageBox.Show("Seleccione un usuario de la lista o busque uno para modificar",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _modoEdicion = true;
            DesbloquearCampos();
            btnGuardar.Enabled = true;
            txtNombreUsuario.Focus();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_usuarioSeleccionado == null)
                {
                    MessageBox.Show("Seleccione un usuario de la lista para eliminar",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar que no se elimine a sí mismo
                if (_usuarioSeleccionado.IdUsuario == _usuarioLogueado.IdUsuario)
                {
                    MessageBox.Show("No puede eliminar su propio usuario",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show(
                    $"¿Está seguro que desea eliminar el usuario '{_usuarioSeleccionado.Nombre}'?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    UsuarioBLL.EliminarUsuario(_usuarioSeleccionado.IdUsuario);

                    MessageBox.Show("Usuario eliminado correctamente",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarTodosLosUsuarios();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region DataGridView Events

        private void DgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.CurrentRow != null && dgvUsuarios.CurrentRow.DataBoundItem != null)
                {
                    var item = dgvUsuarios.CurrentRow.DataBoundItem;
                    var idUsuario = (Guid)item.GetType().GetProperty("IdUsuario").GetValue(item);

                    _usuarioSeleccionado = UsuarioBLL.ObtenerUsuarioPorId(idUsuario);
                    MostrarDatosUsuario(_usuarioSeleccionado);
                }
            }
            catch (Exception ex)
            {
                // Log error silenciosamente
                Console.WriteLine($"Error al seleccionar usuario: {ex.Message}");
            }
        }

        #endregion


        #region Helpers

        private bool ValidarFormatoEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Expresión regular para validar email
            // Requiere: texto@dominio.extension
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(email, patron);
        }

        private void MostrarDatosUsuario(Usuario usuario)
        {
            if (usuario != null)
            {
                txtNombreUsuario.Text = usuario.Nombre;
                txtEmail.Text = usuario.Email ?? "";
                txtContraseña.Text = ""; // No mostrar contraseña por seguridad

                // Seleccionar el rol en el ComboBox
                var familiaRol = usuario.ObtenerFamiliaRol();
                if (familiaRol != null)
                {
                    // Buscar la Familia en el ComboBox por su ID
                    foreach (Familia item in comboBoxPerfil.Items)
                    {
                        if (item.IdComponent == familiaRol.IdComponent)
                        {
                            comboBoxPerfil.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    comboBoxPerfil.SelectedIndex = -1;
                }

                // Traducir el label de estado dinámicamente
                string estadoTraducido = LanguageManager.Translate("estado");
                string estadoValor = usuario.Activo ? LanguageManager.Translate("activo") : LanguageManager.Translate("inactivo");
                lblEstado.Text = $"{estadoTraducido}: {estadoValor}";
            }
        }

        private void LimpiarCampos()
        {
            txtNombreUsuario.Clear();
            txtEmail.Clear();
            txtContraseña.Clear();
            comboBoxPerfil.SelectedIndex = -1;
            lblEstado.Text = LanguageManager.Translate("estado") + ":";
            _usuarioSeleccionado = null;
        }

        private void BloquearCampos()
        {
            txtNombreUsuario.Enabled = false;
            txtEmail.Enabled = false;
            txtContraseña.Enabled = false;
            comboBoxPerfil.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            txtNombreUsuario.Enabled = true;
            txtEmail.Enabled = true;
            txtContraseña.Enabled = true;
            comboBoxPerfil.Enabled = true;
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Evento legacy del diseñador - redirigir a BtnModificar_Click
            BtnModificar_Click(sender, e);
        }

        #endregion

        private void lblEstado_Click(object sender, EventArgs e)
        {

        }
    }
}
