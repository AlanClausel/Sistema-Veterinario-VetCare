using System;
using System.Linq;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using UI.WinUi.Negocio;

namespace UI.WinUi.Administrador
{
    public partial class menu : Form
    {
        private Usuario _usuarioLogueado;

        // Nombres de las Familias que controlan cada opción del menú
        private const string FAMILIA_USUARIOS = "Gestión de Usuarios";
        private const string FAMILIA_PERMISOS = "Gestión de Permisos";
        private const string FAMILIA_CLIENTES = "Gestión de Clientes";
        private const string FAMILIA_REPORTES = "Reportes";

        public menu()
        {
            InitializeComponent();
        }

        public menu(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ActualizarTextos();
            ConfigurarVisibilidadPorPermisos();
        }

        private void ActualizarTextos()
        {
            // Traducir textos del formulario
            this.Text = LanguageManager.Translate("menu_principal");

            // Traducir menú
            usuariosToolStripMenuItem.Text = LanguageManager.Translate("gestion_usuarios");
            permisosToolStripMenuItem.Text = LanguageManager.Translate("gestion_permisos");
            clientesToolStripMenuItem.Text = "Clientes";
            reportesToolStripMenuItem.Text = LanguageManager.Translate("reportes");
            cerrarSesionToolStripMenuItem.Text = LanguageManager.Translate("cerrar_sesion");
        }

        private void ConfigurarVisibilidadPorPermisos()
        {
            // Configurar visibilidad de cada opción del menú según permisos
            usuariosToolStripMenuItem.Visible = TienePermisoFamilia(FAMILIA_USUARIOS);
            permisosToolStripMenuItem.Visible = TienePermisoFamilia(FAMILIA_PERMISOS);
            clientesToolStripMenuItem.Visible = TienePermisoFamilia(FAMILIA_CLIENTES);
            reportesToolStripMenuItem.Visible = TienePermisoFamilia(FAMILIA_REPORTES);
        }

        private bool TienePermisoFamilia(string nombreFamilia)
        {
            if (_usuarioLogueado?.Permisos == null)
                return false;

            foreach (var componente in _usuarioLogueado.Permisos)
            {
                if (TieneFamiliaOPatenteHija(componente, nombreFamilia))
                    return true;
            }

            return false;
        }

        private bool TieneFamiliaOPatenteHija(Component componente, string nombreFamilia)
        {
            if (componente == null)
                return false;

            // Si es una Familia, verificar si coincide con el nombre buscado
            if (componente is Familia familia)
            {
                // Si la familia coincide exactamente, el usuario tiene acceso a todo ese módulo
                if (familia.Nombre != null && familia.Nombre.Equals(nombreFamilia, StringComparison.OrdinalIgnoreCase))
                    return true;

                // Si no coincide, buscar recursivamente en sus hijos
                foreach (var hijo in familia.GetChildrens())
                {
                    if (hijo != null && TieneFamiliaOPatenteHija(hijo, nombreFamilia))
                        return true;
                }
            }

            // Si es una Patente, no hay match directo (las patentes no tienen nombre de familia)
            // Pero si llegamos aquí desde una búsqueda recursiva, significa que NO hay match
            return false;
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                gestionUsuarios formGestion = new gestionUsuarios(_usuarioLogueado);
                formGestion.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de usuarios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void permisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                gestionPermisos formPermisos = new gestionPermisos(_usuarioLogueado);
                formPermisos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de permisos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                gestionClientes formClientes = new gestionClientes(_usuarioLogueado);
                formClientes.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de clientes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                LanguageManager.Translate("funcionalidad_no_implementada"),
                LanguageManager.Translate("informacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var resultado = MessageBox.Show(
                    LanguageManager.Translate("confirmar_cerrar_sesion"),
                    LanguageManager.Translate("cerrar_sesion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Cerrar este formulario
                    this.Close();

                    // Mostrar el formulario de login nuevamente
                    Login loginForm = new Login();
                    loginForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar sesión: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
