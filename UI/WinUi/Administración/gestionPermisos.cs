using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using ServicesSecurity.BLL;

namespace UI.WinUi.Administrador
{
    public partial class gestionPermisos : Form
    {
        private Usuario _usuarioLogueado;
        private Familia _familiaSeleccionada;
        private Usuario _usuarioSeleccionado;

        public gestionPermisos()
        {
            InitializeComponent();
        }

        public gestionPermisos(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += GestionPermisos_Load;

            // Eventos de tabs
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // Eventos de gestión de roles
            cboRoles.SelectedIndexChanged += CboRoles_SelectedIndexChanged;
            btnGuardarRol.Click += BtnGuardarRol_Click;

            // Eventos de gestión de usuarios
            cboUsuarios.SelectedIndexChanged += CboUsuarios_SelectedIndexChanged;
            btnAsignarRolUsuario.Click += BtnAsignarRolUsuario_Click;
            btnGuardarPermisosUsuario.Click += BtnGuardarPermisosUsuario_Click;
        }

        private void GestionPermisos_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarRoles();
            CargarUsuarios();
            CargarPatentesDisponibles();
        }

        private void AplicarTraducciones()
        {
            this.Text = LanguageManager.Translate("gestion_permisos");

            // Tabs
            tabGestionRoles.Text = LanguageManager.Translate("gestion_roles");
            tabGestionUsuarios.Text = LanguageManager.Translate("gestion_usuarios_permisos");

            // Tab Roles
            groupBoxRol.Text = LanguageManager.Translate("seleccionar_rol");
            lblRol.Text = LanguageManager.Translate("rol") + ":";
            groupBoxPatentesRol.Text = LanguageManager.Translate("permisos_rol");
            btnGuardarRol.Text = LanguageManager.Translate("guardar_cambios");

            // Tab Usuarios
            groupBoxUsuario.Text = LanguageManager.Translate("seleccionar_usuario");
            lblUsuario.Text = LanguageManager.Translate("usuario") + ":";
            groupBoxRolUsuario.Text = LanguageManager.Translate("rol_asignado");
            lblRolActual.Text = LanguageManager.Translate("rol_actual") + ":";
            lblNuevoRol.Text = LanguageManager.Translate("nuevo_rol") + ":";
            btnAsignarRolUsuario.Text = LanguageManager.Translate("asignar_rol");
            groupBoxPatentesUsuario.Text = LanguageManager.Translate("permisos_adicionales");
            btnGuardarPermisosUsuario.Text = LanguageManager.Translate("guardar_cambios");
        }

        #region Carga de Datos

        private void CargarRoles()
        {
            try
            {
                var roles = UsuarioBLL.ObtenerRolesDisponibles();

                cboRoles.DataSource = null;
                cboRoles.DataSource = roles.ToList();
                cboRoles.DisplayMember = "NombreRol";
                cboRoles.ValueMember = "IdComponent";
                cboRoles.SelectedIndex = -1;

                cboNuevoRol.DataSource = null;
                cboNuevoRol.DataSource = roles.ToList();
                cboNuevoRol.DisplayMember = "NombreRol";
                cboNuevoRol.ValueMember = "IdComponent";
                cboNuevoRol.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar roles: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                var usuarios = UsuarioBLL.ObtenerTodosLosUsuarios();

                cboUsuarios.DataSource = null;
                cboUsuarios.DataSource = usuarios.ToList();
                cboUsuarios.DisplayMember = "Nombre";
                cboUsuarios.ValueMember = "IdUsuario";
                cboUsuarios.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPatentesDisponibles()
        {
            try
            {
                var patentes = UsuarioBLL.ObtenerTodasLasPatentes();

                // Filtrar solo las patentes del menú principal (FormName = "menu")
                var patentesMenu = patentes.Where(p => p.FormName == "menu").OrderBy(p => p.MenuItemName).ToList();

                // Cargar en CheckedListBox de roles
                checkedListPatentesRol.Items.Clear();
                foreach (var patente in patentesMenu)
                {
                    // Crear un texto descriptivo: "MenuItemName - Descripcion"
                    string textoMostrar = $"{patente.MenuItemName} - {patente.Descripcion}";
                    checkedListPatentesRol.Items.Add(new PatenteDisplay { Patente = patente, TextoMostrar = textoMostrar }, false);
                }

                // Cargar en CheckedListBox de usuarios
                checkedListPatentesUsuario.Items.Clear();
                foreach (var patente in patentesMenu)
                {
                    string textoMostrar = $"{patente.MenuItemName} - {patente.Descripcion}";
                    checkedListPatentesUsuario.Items.Add(new PatenteDisplay { Patente = patente, TextoMostrar = textoMostrar }, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar patentes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clase auxiliar para mostrar las patentes con formato personalizado
        private class PatenteDisplay
        {
            public Patente Patente { get; set; }
            public string TextoMostrar { get; set; }

            public override string ToString()
            {
                return TextoMostrar;
            }
        }

        #endregion

        #region Gestión de Roles

        private void CboRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRoles.SelectedItem == null) return;

            _familiaSeleccionada = cboRoles.SelectedItem as Familia;
            if (_familiaSeleccionada == null) return;

            CargarPatentesDelRol(_familiaSeleccionada);
        }

        private void CargarPatentesDelRol(Familia rol)
        {
            try
            {
                // Obtener patentes directas del rol desde la BLL (no recursivas)
                var patentesDelRol = FamiliaBLL.ObtenerPatentesDirectasDeFamilia(rol.IdComponent);

                // Desmarcar todas
                for (int i = 0; i < checkedListPatentesRol.Items.Count; i++)
                {
                    checkedListPatentesRol.SetItemChecked(i, false);
                }

                // Marcar las que tiene el rol
                for (int i = 0; i < checkedListPatentesRol.Items.Count; i++)
                {
                    var displayItem = checkedListPatentesRol.Items[i] as PatenteDisplay;
                    if (displayItem != null && patentesDelRol.Any(p => p.IdComponent == displayItem.Patente.IdComponent))
                    {
                        checkedListPatentesRol.SetItemChecked(i, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar patentes del rol: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardarRol_Click(object sender, EventArgs e)
        {
            try
            {
                if (_familiaSeleccionada == null)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_rol"),
                        LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener patentes seleccionadas
                var patentesSeleccionadas = new List<Patente>();
                foreach (var item in checkedListPatentesRol.CheckedItems)
                {
                    if (item is PatenteDisplay displayItem)
                        patentesSeleccionadas.Add(displayItem.Patente);
                }

                // Actualizar permisos del rol
                FamiliaBLL.ActualizarPatentesDeRol(_familiaSeleccionada.IdComponent, patentesSeleccionadas);

                MessageBox.Show(LanguageManager.Translate("permisos_actualizados"),
                    LanguageManager.Translate("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar permisos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Gestión de Usuarios

        private void CboUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboUsuarios.SelectedItem == null) return;

            _usuarioSeleccionado = cboUsuarios.SelectedItem as Usuario;
            if (_usuarioSeleccionado == null) return;

            CargarDatosDelUsuario(_usuarioSeleccionado);
        }

        private void CargarDatosDelUsuario(Usuario usuario)
        {
            try
            {
                // Mostrar rol actual
                var rolActual = usuario.ObtenerFamiliaRol();
                lblRolActualValor.Text = rolActual != null ? rolActual.NombreRol : LanguageManager.Translate("sin_rol");

                // Obtener patentes directas del usuario (no heredadas del rol)
                var patentesDirectas = UsuarioBLL.ObtenerPatentesDelUsuario(usuario.IdUsuario);

                // Desmarcar todas
                for (int i = 0; i < checkedListPatentesUsuario.Items.Count; i++)
                {
                    checkedListPatentesUsuario.SetItemChecked(i, false);
                }

                // Marcar las patentes directas
                for (int i = 0; i < checkedListPatentesUsuario.Items.Count; i++)
                {
                    var displayItem = checkedListPatentesUsuario.Items[i] as PatenteDisplay;
                    if (displayItem != null && patentesDirectas.Any(p => p.IdComponent == displayItem.Patente.IdComponent))
                    {
                        checkedListPatentesUsuario.SetItemChecked(i, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del usuario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAsignarRolUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (_usuarioSeleccionado == null)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_usuario"),
                        LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboNuevoRol.SelectedItem == null)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_nuevo_rol"),
                        LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var nuevoRol = cboNuevoRol.SelectedItem as Familia;
                if (nuevoRol == null) return;

                // Cambiar el rol del usuario
                UsuarioBLL.CambiarRol(_usuarioSeleccionado.IdUsuario, nuevoRol.IdComponent);

                MessageBox.Show(LanguageManager.Translate("rol_actualizado"),
                    LanguageManager.Translate("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar datos
                CargarDatosDelUsuario(_usuarioSeleccionado);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al asignar rol: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardarPermisosUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (_usuarioSeleccionado == null)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_usuario"),
                        LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener patentes seleccionadas
                var patentesSeleccionadas = new List<Patente>();
                foreach (var item in checkedListPatentesUsuario.CheckedItems)
                {
                    if (item is PatenteDisplay displayItem)
                        patentesSeleccionadas.Add(displayItem.Patente);
                }

                // Obtener patentes actuales del usuario
                var patentesActuales = UsuarioBLL.ObtenerPatentesDelUsuario(_usuarioSeleccionado.IdUsuario);

                // Quitar patentes que ya no están seleccionadas
                foreach (var patenteActual in patentesActuales)
                {
                    if (!patentesSeleccionadas.Any(p => p.IdComponent == patenteActual.IdComponent))
                    {
                        UsuarioBLL.QuitarPatente(_usuarioSeleccionado.IdUsuario, patenteActual.IdComponent);
                    }
                }

                // Agregar patentes nuevas
                foreach (var patenteSeleccionada in patentesSeleccionadas)
                {
                    if (!patentesActuales.Any(p => p.IdComponent == patenteSeleccionada.IdComponent))
                    {
                        UsuarioBLL.AsignarPatente(_usuarioSeleccionado.IdUsuario, patenteSeleccionada.IdComponent);
                    }
                }

                MessageBox.Show(LanguageManager.Translate("permisos_actualizados"),
                    LanguageManager.Translate("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar permisos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Resetear selecciones al cambiar de tab
            if (tabControl.SelectedTab == tabGestionRoles)
            {
                cboRoles.SelectedIndex = -1;
            }
            else if (tabControl.SelectedTab == tabGestionUsuarios)
            {
                cboUsuarios.SelectedIndex = -1;
            }
        }
    }
}
