using System;
using System.IO;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;

namespace UI.WinUi.Administrador
{
    /// <summary>
    /// Formulario para realizar operaciones de Backup y Restore de bases de datos
    /// </summary>
    public partial class frmBackup : BaseObservableForm
    {
        #region Variables de Instancia
        private Usuario _usuarioLogueado;
        private FolderBrowserDialog _folderDialog;
        private OpenFileDialog _openFileDialog;
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public frmBackup()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        /// <summary>
        /// Constructor con usuario logueado
        /// </summary>
        /// <param name="usuario">Usuario actualmente logueado</param>
        public frmBackup(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
        }

        #endregion

        #region Configuración Inicial

        /// <summary>
        /// Configura los controles y eventos del formulario
        /// </summary>
        private void ConfigurarFormulario()
        {
            // Configurar diálogos
            _folderDialog = new FolderBrowserDialog
            {
                Description = "Seleccione el directorio donde guardar el backup",
                ShowNewFolderButton = true
            };

            _openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de Backup (*.bak)|*.bak|Todos los archivos (*.*)|*.*",
                Title = "Seleccionar archivo de backup",
                CheckFileExists = true
            };

            // Registrar eventos
            this.Load += FrmBackup_Load;
            btnExaminarBackup.Click += BtnExaminarBackup_Click;
            btnRealizarBackup.Click += BtnRealizarBackup_Click;
            btnExaminarRestore.Click += BtnExaminarRestore_Click;
            btnRealizarRestore.Click += BtnRealizarRestore_Click;
            btnCerrar.Click += BtnCerrar_Click;

            // Configurar combo de base de datos
            cmbBaseDatos.SelectedIndex = 0; // SecurityVet por defecto
        }

        /// <summary>
        /// Evento Load del formulario
        /// </summary>
        private void FrmBackup_Load(object sender, EventArgs e)
        {
            try
            {
                // Configurar ruta por defecto para backups
                string rutaDefault = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "VetCare_Backups");

                // Crear directorio si no existe
                if (!Directory.Exists(rutaDefault))
                {
                    Directory.CreateDirectory(rutaDefault);
                }

                txtRutaBackup.Text = rutaDefault;

                AgregarLog("Sistema de Backup/Restore iniciado correctamente");
                AgregarLog($"Usuario: {_usuarioLogueado?.Nombre ?? "Sistema"}");
                AgregarLog($"Ruta por defecto: {rutaDefault}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Implementación de BaseObservableForm

        /// <summary>
        /// Actualiza todos los textos del formulario según el idioma actual
        /// </summary>
        protected override void ActualizarTextos()
        {
            // Título del formulario
            this.Text = LanguageManager.Translate("backup_titulo");

            // Grupo Backup
            grpBackup.Text = LanguageManager.Translate("backup_realizar");
            lblRutaBackup.Text = LanguageManager.Translate("backup_directorio_destino");
            btnExaminarBackup.Text = LanguageManager.Translate("examinar");
            chkBackupSeguridad.Text = LanguageManager.Translate("backup_bd_seguridad");
            chkBackupNegocio.Text = LanguageManager.Translate("backup_bd_negocio");
            btnRealizarBackup.Text = LanguageManager.Translate("backup_realizar_backup");

            // Grupo Restore
            grpRestore.Text = LanguageManager.Translate("backup_restaurar");
            lblBaseDatos.Text = LanguageManager.Translate("backup_seleccione_bd");
            lblRutaRestore.Text = LanguageManager.Translate("backup_archivo_backup");
            btnExaminarRestore.Text = LanguageManager.Translate("examinar");
            btnRealizarRestore.Text = LanguageManager.Translate("backup_restaurar_backup");

            // Otros controles
            lblLog.Text = LanguageManager.Translate("backup_registro_eventos");
            btnCerrar.Text = LanguageManager.Translate("cerrar");

            // Items del ComboBox
            cmbBaseDatos.Items.Clear();
            cmbBaseDatos.Items.Add(LanguageManager.Translate("backup_bd_seguridad_completo"));
            cmbBaseDatos.Items.Add(LanguageManager.Translate("backup_bd_negocio_completo"));
            cmbBaseDatos.SelectedIndex = 0;
        }

        #endregion

        #region Event Handlers - Backup

        /// <summary>
        /// Evento para examinar carpeta de destino del backup
        /// </summary>
        private void BtnExaminarBackup_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtRutaBackup.Text) && Directory.Exists(txtRutaBackup.Text))
                {
                    _folderDialog.SelectedPath = txtRutaBackup.Text;
                }

                if (_folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtRutaBackup.Text = _folderDialog.SelectedPath;
                    AgregarLog($"Directorio seleccionado: {_folderDialog.SelectedPath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al seleccionar directorio: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento para realizar el backup
        /// </summary>
        private void BtnRealizarBackup_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(txtRutaBackup.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("backup_error_ruta_vacia"),
                        LanguageManager.Translate("error_validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!chkBackupSeguridad.Checked && !chkBackupNegocio.Checked)
                {
                    MessageBox.Show(LanguageManager.Translate("backup_error_seleccione_bd"),
                        LanguageManager.Translate("error_validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar directorio
                if (!BackupRestoreService.Current.ValidarDirectorioDestino(txtRutaBackup.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("backup_error_directorio_invalido"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirmar operación
                DialogResult resultado = MessageBox.Show(
                    LanguageManager.Translate("backup_confirmar_backup"),
                    LanguageManager.Translate("confirmar"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes)
                    return;

                // Deshabilitar controles durante la operación
                BloquearControlesBackup(true);
                MostrarProgreso(true);

                bool exito = true;
                int backupsRealizados = 0;

                // Realizar backup de SecurityVet
                if (chkBackupSeguridad.Checked)
                {
                    AgregarLog("=== Iniciando backup de SecurityVet ===");
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string rutaArchivo = Path.Combine(txtRutaBackup.Text, $"SecurityVet_{timestamp}.bak");

                    try
                    {
                        BackupRestoreService.Current.RealizarBackupSecurity(rutaArchivo);
                        AgregarLog($"✓ Backup de SecurityVet completado: {rutaArchivo}");
                        backupsRealizados++;
                    }
                    catch (Exception ex)
                    {
                        AgregarLog($"✗ ERROR en backup de SecurityVet: {ex.Message}");
                        exito = false;
                    }
                }

                // Realizar backup de VetCareDB
                if (chkBackupNegocio.Checked)
                {
                    AgregarLog("=== Iniciando backup de VetCareDB ===");
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string rutaArchivo = Path.Combine(txtRutaBackup.Text, $"VetCareDB_{timestamp}.bak");

                    try
                    {
                        BackupRestoreService.Current.RealizarBackupNegocio(rutaArchivo);
                        AgregarLog($"✓ Backup de VetCareDB completado: {rutaArchivo}");
                        backupsRealizados++;
                    }
                    catch (Exception ex)
                    {
                        AgregarLog($"✗ ERROR en backup de VetCareDB: {ex.Message}");
                        exito = false;
                    }
                }

                // Mostrar resultado
                if (exito && backupsRealizados > 0)
                {
                    MessageBox.Show(
                        string.Format(LanguageManager.Translate("backup_exito"), backupsRealizados),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    AgregarLog($"=== BACKUPS COMPLETADOS EXITOSAMENTE ({backupsRealizados}) ===");
                }
                else if (!exito)
                {
                    MessageBox.Show(
                        LanguageManager.Translate("backup_error_parcial"),
                        LanguageManager.Translate("advertencia"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                AgregarLog($"✗ ERROR CRÍTICO: {ex.Message}");
                MessageBox.Show($"{LanguageManager.Translate("error_inesperado")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                BloquearControlesBackup(false);
                MostrarProgreso(false);
            }
        }

        #endregion

        #region Event Handlers - Restore

        /// <summary>
        /// Evento para examinar archivo de backup a restaurar
        /// </summary>
        private void BtnExaminarRestore_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtRutaBackup.Text) && Directory.Exists(txtRutaBackup.Text))
                {
                    _openFileDialog.InitialDirectory = txtRutaBackup.Text;
                }

                if (_openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtRutaRestore.Text = _openFileDialog.FileName;
                    AgregarLog($"Archivo seleccionado: {_openFileDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al seleccionar archivo: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento para realizar el restore
        /// </summary>
        private void BtnRealizarRestore_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(txtRutaRestore.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("backup_error_archivo_vacio"),
                        LanguageManager.Translate("error_validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbBaseDatos.SelectedIndex < 0)
                {
                    MessageBox.Show(LanguageManager.Translate("backup_error_seleccione_bd_restore"),
                        LanguageManager.Translate("error_validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar archivo
                if (!BackupRestoreService.Current.ValidarArchivoBackup(txtRutaRestore.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("backup_error_archivo_invalido"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ADVERTENCIA CRÍTICA
                DialogResult advertencia = MessageBox.Show(
                    LanguageManager.Translate("backup_advertencia_restore"),
                    LanguageManager.Translate("advertencia_critica"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (advertencia != DialogResult.Yes)
                    return;

                // Confirmar con el nombre de la base de datos
                string nombreBD = cmbBaseDatos.SelectedIndex == 0 ? "SecurityVet" : "VetCareDB";
                DialogResult confirmacion = MessageBox.Show(
                    string.Format(LanguageManager.Translate("backup_confirmar_restore"), nombreBD),
                    LanguageManager.Translate("confirmar"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes)
                    return;

                // Deshabilitar controles durante la operación
                BloquearControlesRestore(true);
                MostrarProgreso(true);

                AgregarLog($"=== Iniciando RESTORE de {nombreBD} ===");
                AgregarLog($"Archivo origen: {txtRutaRestore.Text}");
                AgregarLog("ADVERTENCIA: Este proceso sobrescribirá todos los datos actuales");

                try
                {
                    if (cmbBaseDatos.SelectedIndex == 0)
                    {
                        // Restore SecurityVet
                        BackupRestoreService.Current.RealizarRestoreSecurity(txtRutaRestore.Text);
                    }
                    else
                    {
                        // Restore VetCareDB
                        BackupRestoreService.Current.RealizarRestoreNegocio(txtRutaRestore.Text);
                    }

                    AgregarLog($"✓ RESTORE de {nombreBD} completado exitosamente");

                    MessageBox.Show(
                        LanguageManager.Translate("backup_restore_exito"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    AgregarLog("=== RESTORE COMPLETADO ===");
                    AgregarLog("IMPORTANTE: Se recomienda reiniciar la aplicación");
                }
                catch (Exception ex)
                {
                    AgregarLog($"✗ ERROR en RESTORE: {ex.Message}");
                    MessageBox.Show($"{LanguageManager.Translate("error_restore")}: {ex.Message}",
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                AgregarLog($"✗ ERROR CRÍTICO: {ex.Message}");
                MessageBox.Show($"{LanguageManager.Translate("error_inesperado")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                BloquearControlesRestore(false);
                MostrarProgreso(false);
            }
        }

        #endregion

        #region Event Handlers - Otros

        /// <summary>
        /// Evento para cerrar el formulario
        /// </summary>
        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Agrega una línea al log del formulario
        /// </summary>
        /// <param name="mensaje">Mensaje a agregar</param>
        private void AgregarLog(string mensaje)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"[{timestamp}] {mensaje}\r\n");
        }

        /// <summary>
        /// Bloquea o desbloquea los controles del grupo Backup
        /// </summary>
        /// <param name="bloquear">True para bloquear, False para desbloquear</param>
        private void BloquearControlesBackup(bool bloquear)
        {
            txtRutaBackup.Enabled = !bloquear;
            btnExaminarBackup.Enabled = !bloquear;
            chkBackupSeguridad.Enabled = !bloquear;
            chkBackupNegocio.Enabled = !bloquear;
            btnRealizarBackup.Enabled = !bloquear;
        }

        /// <summary>
        /// Bloquea o desbloquea los controles del grupo Restore
        /// </summary>
        /// <param name="bloquear">True para bloquear, False para desbloquear</param>
        private void BloquearControlesRestore(bool bloquear)
        {
            cmbBaseDatos.Enabled = !bloquear;
            txtRutaRestore.Enabled = !bloquear;
            btnExaminarRestore.Enabled = !bloquear;
            btnRealizarRestore.Enabled = !bloquear;
        }

        /// <summary>
        /// Muestra u oculta el progress bar
        /// </summary>
        /// <param name="mostrar">True para mostrar, False para ocultar</param>
        private void MostrarProgreso(bool mostrar)
        {
            progressBar.Visible = mostrar;
            if (mostrar)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            }
        }

        #endregion
    }
}
