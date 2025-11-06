using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        public menu()
        {
            InitializeComponent();
        }

        public menu(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;

            CargarLogo();
            ActualizarTextos();
            ConfigurarEstiloMenu();
            CargarMenuDinamico();
        }

        private void CargarLogo()
        {
            try
            {
                string logoPath = Path.Combine(Application.StartupPath, "..", "..", "Resources", "Imagenes", "LogoVetCare.png");
                logoPath = Path.GetFullPath(logoPath);

                if (File.Exists(logoPath))
                {
                    picLogo.Image = Image.FromFile(logoPath);
                }
            }
            catch (Exception ex)
            {
                // Si falla la carga del logo, continuar sin él
                Console.WriteLine($"No se pudo cargar el logo: {ex.Message}");
            }
        }

        private void ActualizarTextos()
        {
            this.Text = LanguageManager.Translate("menu_principal");
            cerrarSesionToolStripMenuItem.Text = LanguageManager.Translate("cerrar_sesion");
        }

        /// <summary>
        /// Carga el menú dinámicamente basado en las patentes del usuario
        /// </summary>
        private void CargarMenuDinamico()
        {
            try
            {
                // Validar que el usuario tiene permisos
                if (_usuarioLogueado == null || _usuarioLogueado.Permisos == null || _usuarioLogueado.Permisos.Count == 0)
                {
                    return;
                }

                // 1. Obtener todas las patentes del usuario
                var patentes = ObtenerPatentesDelUsuario();

                // 2. Agrupar por FormName (para evitar duplicados)
                var patentesAgrupadas = patentes
                    .Where(p => !string.IsNullOrWhiteSpace(p.FormName))
                    .GroupBy(p => p.FormName)
                    .Select(g => g.OrderBy(p => p.Orden).First()) // Tomar la primera por orden
                    .OrderBy(p => p.Orden)
                    .ToList();

                // 3. Limpiar ítems dinámicos anteriores para evitar duplicados
                var itemsToRemove = menuStrip1.Items.Cast<ToolStripItem>()
                    .Where(item => item.Name != null && item.Name.StartsWith("mnuDynamic_"))
                    .ToList();

                foreach (var item in itemsToRemove)
                {
                    menuStrip1.Items.Remove(item);
                }

                // 4. Crear ToolStripMenuItem para cada patente única
                int insertIndex = 0; // Insertar antes del botón "Cerrar Sesión"

                foreach (var patente in patentesAgrupadas)
                {
                    var menuItem = new ToolStripMenuItem
                    {
                        Name = $"mnuDynamic_{patente.FormName}",
                        Text = ObtenerTextoMenu(patente),
                        ForeColor = Color.FromArgb(56, 142, 60), // Verde bosque
                        Tag = patente.FormName // Guardar el FormName en el Tag
                    };

                    // Agregar event handlers
                    menuItem.Click += MenuItem_Click_Dinamico;
                    menuItem.MouseEnter += MenuItem_MouseEnter;
                    menuItem.MouseLeave += MenuItem_MouseLeave;

                    // Insertar antes de "Cerrar Sesión"
                    menuStrip1.Items.Insert(insertIndex++, menuItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el menú: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Obtiene el texto del menú basado en FormName (genérico por formulario)
        /// </summary>
        private string ObtenerTextoMenu(Patente patente)
        {
            // NO usar MenuItemName porque es específico de la acción (Alta, Baja, etc.)
            // Usar FormName humanizado para tener un nombre genérico del módulo

            return HumanizarNombre(patente.FormName);
        }

        /// <summary>
        /// Convierte "gestionClientes" o "frmGestionUsuarios" en nombres legibles
        /// </summary>
        private string HumanizarNombre(string formName)
        {
            if (string.IsNullOrWhiteSpace(formName))
                return formName;

            // Eliminar prefijos comunes
            string nombre = formName;

            // Eliminar prefijos
            if (nombre.StartsWith("frm", StringComparison.OrdinalIgnoreCase))
                nombre = nombre.Substring(3);
            if (nombre.StartsWith("Form", StringComparison.OrdinalIgnoreCase))
                nombre = nombre.Substring(4);

            // Reemplazar "gestion" o "Gestion" por "Gestión "
            nombre = System.Text.RegularExpressions.Regex.Replace(
                nombre,
                @"[gG]estion",
                "Gestión ",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Agregar espacios antes de mayúsculas (para camelCase)
            // "VisorLogs" → "Visor Logs", "GestionUsuarios" → "Gestión Usuarios"
            nombre = System.Text.RegularExpressions.Regex.Replace(
                nombre,
                @"([a-z])([A-Z])",
                "$1 $2");

            // Capitalizar primera letra
            if (nombre.Length > 0)
                nombre = char.ToUpper(nombre[0]) + nombre.Substring(1);

            return nombre.Trim();
        }

        /// <summary>
        /// Extrae todas las patentes del usuario navegando el árbol de permisos (Composite pattern)
        /// </summary>
        private List<Patente> ObtenerPatentesDelUsuario()
        {
            var patentes = new List<Patente>();

            if (_usuarioLogueado?.Permisos == null)
                return patentes;

            foreach (var componente in _usuarioLogueado.Permisos)
            {
                ExtraerPatentesRecursivo(componente, patentes);
            }

            return patentes;
        }

        /// <summary>
        /// Navega recursivamente el árbol de Familias y Patentes
        /// </summary>
        private void ExtraerPatentesRecursivo(Component componente, List<Patente> patentes)
        {
            if (componente == null)
                return;

            // Si es una Patente, agregarla a la lista
            if (componente is Patente patente)
            {
                patentes.Add(patente);
            }

            // Si es una Familia, navegar sus hijos recursivamente
            if (componente is Familia familia)
            {
                foreach (var hijo in familia.GetChildrens())
                {
                    ExtraerPatentesRecursivo(hijo, patentes);
                }
            }
        }

        /// <summary>
        /// Event handler dinámico que abre el formulario correcto basado en el FormName
        /// </summary>
        private void MenuItem_Click_Dinamico(object sender, EventArgs e)
        {
            try
            {
                var menuItem = sender as ToolStripMenuItem;
                if (menuItem?.Tag == null)
                    return;

                string formName = menuItem.Tag.ToString();

                // Factory pattern: Crear el formulario basado en el FormName
                Form formulario = CrearFormulario(formName);

                if (formulario != null)
                {
                    formulario.ShowDialog();
                }
                else
                {
                    MessageBox.Show($"El formulario '{formName}' no está implementado.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Factory Method: Crea una instancia del formulario basado en el FormName
        /// </summary>
        private Form CrearFormulario(string formName)
        {
            switch (formName)
            {
                // Módulo Administración
                case "frmGestionUsuarios":
                    return new gestionUsuarios(_usuarioLogueado);

                case "frmGestionPermisos":
                    return new gestionPermisos(_usuarioLogueado);

                // Módulo Negocio
                case "gestionClientes":
                    return new gestionClientes(_usuarioLogueado);

                case "gestionMascotas":
                    return new gestionMascotas(_usuarioLogueado);

                case "gestionCitas":
                    return new gestionCitas(_usuarioLogueado);

                case "gestionMedicamentos":
                    return new gestionMedicamentos(_usuarioLogueado);

                // Módulo Veterinario
                case "MisCitas":
                    return new MisCitas(_usuarioLogueado);

                case "FormHistorialClinico":
                    return new FormHistorialClinico(_usuarioLogueado);

                // Módulo Reportes
                case "FormReportes":
                    return new FormReportes(_usuarioLogueado);

                // Mi Cuenta (disponible para todos los usuarios)
                case "FormMiCuenta":
                    return new FormMiCuenta(_usuarioLogueado);

                // Bitácora del Sistema (solo Administradores)
                case "FormBitacora":
                    return new FormBitacora(_usuarioLogueado);

                // Otros formularios pueden agregarse aquí según sea necesario
                // case "frmConfiguracion":
                //     return new frmConfiguracion(_usuarioLogueado);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Configura el estilo visual del menú (colores, bordes)
        /// </summary>
        private void ConfigurarEstiloMenu()
        {
            // Agregar eventos hover al botón de cerrar sesión
            cerrarSesionToolStripMenuItem.MouseEnter += MenuItem_MouseEnter;
            cerrarSesionToolStripMenuItem.MouseLeave += MenuItem_MouseLeave;

            // Configurar renderer personalizado para el menú
            menuStrip1.Renderer = new CustomMenuRenderer();
        }

        /// <summary>
        /// Efecto hover: Cambia fondo a verde claro cuando el mouse entra
        /// </summary>
        private void MenuItem_MouseEnter(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                menuItem.BackColor = Color.FromArgb(232, 245, 233); // Verde claro
            }
        }

        /// <summary>
        /// Efecto hover: Restaura fondo blanco cuando el mouse sale
        /// </summary>
        private void MenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                menuItem.BackColor = Color.Transparent;
            }
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
                    // Registrar logout en bitácora
                    if (_usuarioLogueado != null)
                    {
                        Bitacora.Current.RegistrarLogout(_usuarioLogueado.IdUsuario, _usuarioLogueado.Nombre);
                    }

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

    /// <summary>
    /// Renderer personalizado para dibujar borde verde superior en el menú
    /// </summary>
    internal class CustomMenuRenderer : ToolStripProfessionalRenderer
    {
        public CustomMenuRenderer() : base(new CustomColorTable())
        {
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            // Dibujar línea verde de 3px en la parte superior
            if (e.ToolStrip is MenuStrip)
            {
                using (Pen pen = new Pen(Color.FromArgb(76, 175, 80), 3))
                {
                    e.Graphics.DrawLine(pen, 0, 0, e.ToolStrip.Width, 0);
                }
            }
        }
    }

    /// <summary>
    /// Tabla de colores personalizada para el menú
    /// </summary>
    internal class CustomColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(232, 245, 233); } // Verde claro para hover
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.FromArgb(232, 245, 233); }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.FromArgb(232, 245, 233); }
        }

        public override Color MenuItemBorder
        {
            get { return Color.Transparent; } // Sin borde en los items
        }
    }
}
