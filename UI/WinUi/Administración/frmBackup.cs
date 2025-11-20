using System;
using System.IO;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;

namespace UI.WinUi.Administrador
{
    /// <summary>
    /// Formulario simplificado para Backup y Restore
    /// </summary>
    public partial class frmBackup : BaseObservableForm
    {
        #region Variables de Instancia
        private Usuario _usuarioLogueado;
        private string _rutaBackupsPorDefecto;
        #endregion

        #region Constructores

        public frmBackup()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        public frmBackup(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
        }

        #endregion

        #region Configuración Inicial

        private void ConfigurarFormulario()
        {
            // Configurar ruta por defecto
            _rutaBackupsPorDefecto = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "VetCare_Backups");

            // Crear directorio si no existe
            if (!Directory.Exists(_rutaBackupsPorDefecto))
            {
                Directory.CreateDirectory(_rutaBackupsPorDefecto);
            }

            // Registrar eventos
            this.Load += FrmBackup_Load;
            btnBackupCompleto.Click += BtnBackupCompleto_Click;
            btnRestaurar.Click += BtnRestaurar_Click;
            btnAbrirCarpeta.Click += BtnAbrirCarpeta_Click;
        }

        private void FrmBackup_Load(object sender, EventArgs e)
        {
            try
            {
                lblRutaActual.Text = $"Los backups se guardan en:\n{_rutaBackupsPorDefecto}";
                ActualizarTextos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Implementación de BaseObservableForm

        protected override void ActualizarTextos()
        {
            this.Text = LanguageManager.Translate("backup_titulo");
            grpBackup.Text = LanguageManager.Translate("backup_realizar");
            btnBackupCompleto.Text = LanguageManager.Translate("backup_realizar_completo");
            btnAbrirCarpeta.Text = LanguageManager.Translate("abrir_carpeta");
            grpRestore.Text = LanguageManager.Translate("backup_restaurar");
            btnRestaurar.Text = LanguageManager.Translate("backup_restaurar_backup");
            lblInfo.Text = LanguageManager.Translate("backup_info");
        }

        #endregion

        #region Event Handlers

        private void BtnBackupCompleto_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "Se realizará un backup completo de ambas bases de datos.\n¿Desea continuar?",
                    "Confirmar Backup",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes)
                    return;

                btnBackupCompleto.Enabled = false;
                Cursor = Cursors.WaitCursor;

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                int exitosos = 0;

                // Backup SecurityVet
                try
                {
                    string ruta = Path.Combine(_rutaBackupsPorDefecto, $"SecurityVet_{timestamp}.bak");
                    BackupRestoreService.Current.RealizarBackupSecurity(ruta);
                    exitosos++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error en backup SecurityVet: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Backup VetCareDB
                try
                {
                    string ruta = Path.Combine(_rutaBackupsPorDefecto, $"VetCareDB_{timestamp}.bak");
                    BackupRestoreService.Current.RealizarBackupNegocio(ruta);
                    exitosos++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error en backup VetCareDB: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (exitosos == 2)
                {
                    MessageBox.Show($"Backup completado exitosamente.\n\nArchivos guardados en:\n{_rutaBackupsPorDefecto}",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnBackupCompleto.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void BtnAbrirCarpeta_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(_rutaBackupsPorDefecto))
                {
                    System.Diagnostics.Process.Start("explorer.exe", _rutaBackupsPorDefecto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir carpeta: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers - Restore

        private void BtnRestaurar_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dialog = new OpenFileDialog
                {
                    Filter = "Archivos de Backup (*.bak)|*.bak",
                    Title = "Seleccionar archivo de backup",
                    InitialDirectory = _rutaBackupsPorDefecto
                })
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;

                    // Determinar qué base de datos restaurar según el nombre del archivo
                    string nombreArchivo = Path.GetFileName(dialog.FileName);
                    bool esSecurity = nombreArchivo.StartsWith("SecurityVet", StringComparison.OrdinalIgnoreCase);
                    string nombreBD = esSecurity ? "SecurityVet" : "VetCareDB";

                    // Advertencia crítica
                    DialogResult advertencia = MessageBox.Show(
                        $"⚠ ADVERTENCIA CRÍTICA ⚠\n\n" +
                        $"Se restaurará la base de datos: {nombreBD}\n\n" +
                        $"TODOS los datos actuales serán SOBRESCRITOS.\n" +
                        $"Esta operación NO puede deshacerse.\n\n" +
                        $"¿Está SEGURO que desea continuar?",
                        "Confirmar Restauración",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (advertencia != DialogResult.Yes)
                        return;

                    btnRestaurar.Enabled = false;
                    Cursor = Cursors.WaitCursor;

                    try
                    {
                        if (esSecurity)
                        {
                            BackupRestoreService.Current.RealizarRestoreSecurity(dialog.FileName);
                        }
                        else
                        {
                            BackupRestoreService.Current.RealizarRestoreNegocio(dialog.FileName);
                        }

                        MessageBox.Show(
                            $"Restauración de {nombreBD} completada exitosamente.\n\n" +
                            $"IMPORTANTE: Se recomienda reiniciar la aplicación.",
                            "Éxito",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al restaurar: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRestaurar.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}
