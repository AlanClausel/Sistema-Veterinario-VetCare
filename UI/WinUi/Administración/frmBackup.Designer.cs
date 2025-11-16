namespace UI.WinUi.Administrador
{
    partial class frmBackup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpBackup = new System.Windows.Forms.GroupBox();
            this.btnExaminarBackup = new System.Windows.Forms.Button();
            this.txtRutaBackup = new System.Windows.Forms.TextBox();
            this.lblRutaBackup = new System.Windows.Forms.Label();
            this.chkBackupNegocio = new System.Windows.Forms.CheckBox();
            this.chkBackupSeguridad = new System.Windows.Forms.CheckBox();
            this.btnRealizarBackup = new System.Windows.Forms.Button();
            this.grpRestore = new System.Windows.Forms.GroupBox();
            this.btnExaminarRestore = new System.Windows.Forms.Button();
            this.txtRutaRestore = new System.Windows.Forms.TextBox();
            this.lblRutaRestore = new System.Windows.Forms.Label();
            this.cmbBaseDatos = new System.Windows.Forms.ComboBox();
            this.lblBaseDatos = new System.Windows.Forms.Label();
            this.btnRealizarRestore = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.grpBackup.SuspendLayout();
            this.grpRestore.SuspendLayout();
            this.SuspendLayout();
            //
            // grpBackup
            //
            this.grpBackup.Controls.Add(this.btnExaminarBackup);
            this.grpBackup.Controls.Add(this.txtRutaBackup);
            this.grpBackup.Controls.Add(this.lblRutaBackup);
            this.grpBackup.Controls.Add(this.chkBackupNegocio);
            this.grpBackup.Controls.Add(this.chkBackupSeguridad);
            this.grpBackup.Controls.Add(this.btnRealizarBackup);
            this.grpBackup.Location = new System.Drawing.Point(12, 12);
            this.grpBackup.Name = "grpBackup";
            this.grpBackup.Size = new System.Drawing.Size(560, 180);
            this.grpBackup.TabIndex = 0;
            this.grpBackup.TabStop = false;
            this.grpBackup.Text = "Realizar Backup";
            //
            // btnExaminarBackup
            //
            this.btnExaminarBackup.Location = new System.Drawing.Point(470, 45);
            this.btnExaminarBackup.Name = "btnExaminarBackup";
            this.btnExaminarBackup.Size = new System.Drawing.Size(75, 23);
            this.btnExaminarBackup.TabIndex = 5;
            this.btnExaminarBackup.Text = "Examinar...";
            this.btnExaminarBackup.UseVisualStyleBackColor = true;
            //
            // txtRutaBackup
            //
            this.txtRutaBackup.Location = new System.Drawing.Point(15, 47);
            this.txtRutaBackup.Name = "txtRutaBackup";
            this.txtRutaBackup.Size = new System.Drawing.Size(449, 20);
            this.txtRutaBackup.TabIndex = 4;
            //
            // lblRutaBackup
            //
            this.lblRutaBackup.AutoSize = true;
            this.lblRutaBackup.Location = new System.Drawing.Point(12, 31);
            this.lblRutaBackup.Name = "lblRutaBackup";
            this.lblRutaBackup.Size = new System.Drawing.Size(113, 13);
            this.lblRutaBackup.TabIndex = 3;
            this.lblRutaBackup.Text = "Directorio de Destino:";
            //
            // chkBackupNegocio
            //
            this.chkBackupNegocio.AutoSize = true;
            this.chkBackupNegocio.Checked = true;
            this.chkBackupNegocio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBackupNegocio.Location = new System.Drawing.Point(15, 100);
            this.chkBackupNegocio.Name = "chkBackupNegocio";
            this.chkBackupNegocio.Size = new System.Drawing.Size(207, 17);
            this.chkBackupNegocio.TabIndex = 2;
            this.chkBackupNegocio.Text = "Base de Datos de Negocio (VetCareDB)";
            this.chkBackupNegocio.UseVisualStyleBackColor = true;
            //
            // chkBackupSeguridad
            //
            this.chkBackupSeguridad.AutoSize = true;
            this.chkBackupSeguridad.Checked = true;
            this.chkBackupSeguridad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBackupSeguridad.Location = new System.Drawing.Point(15, 77);
            this.chkBackupSeguridad.Name = "chkBackupSeguridad";
            this.chkBackupSeguridad.Size = new System.Drawing.Size(219, 17);
            this.chkBackupSeguridad.TabIndex = 1;
            this.chkBackupSeguridad.Text = "Base de Datos de Seguridad (SecurityVet)";
            this.chkBackupSeguridad.UseVisualStyleBackColor = true;
            //
            // btnRealizarBackup
            //
            this.btnRealizarBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRealizarBackup.Location = new System.Drawing.Point(15, 133);
            this.btnRealizarBackup.Name = "btnRealizarBackup";
            this.btnRealizarBackup.Size = new System.Drawing.Size(530, 35);
            this.btnRealizarBackup.TabIndex = 0;
            this.btnRealizarBackup.Text = "REALIZAR BACKUP";
            this.btnRealizarBackup.UseVisualStyleBackColor = true;
            //
            // grpRestore
            //
            this.grpRestore.Controls.Add(this.btnExaminarRestore);
            this.grpRestore.Controls.Add(this.txtRutaRestore);
            this.grpRestore.Controls.Add(this.lblRutaRestore);
            this.grpRestore.Controls.Add(this.cmbBaseDatos);
            this.grpRestore.Controls.Add(this.lblBaseDatos);
            this.grpRestore.Controls.Add(this.btnRealizarRestore);
            this.grpRestore.Location = new System.Drawing.Point(12, 198);
            this.grpRestore.Name = "grpRestore";
            this.grpRestore.Size = new System.Drawing.Size(560, 180);
            this.grpRestore.TabIndex = 1;
            this.grpRestore.TabStop = false;
            this.grpRestore.Text = "Restaurar Backup";
            //
            // btnExaminarRestore
            //
            this.btnExaminarRestore.Location = new System.Drawing.Point(470, 72);
            this.btnExaminarRestore.Name = "btnExaminarRestore";
            this.btnExaminarRestore.Size = new System.Drawing.Size(75, 23);
            this.btnExaminarRestore.TabIndex = 5;
            this.btnExaminarRestore.Text = "Examinar...";
            this.btnExaminarRestore.UseVisualStyleBackColor = true;
            //
            // txtRutaRestore
            //
            this.txtRutaRestore.Location = new System.Drawing.Point(15, 74);
            this.txtRutaRestore.Name = "txtRutaRestore";
            this.txtRutaRestore.Size = new System.Drawing.Size(449, 20);
            this.txtRutaRestore.TabIndex = 4;
            //
            // lblRutaRestore
            //
            this.lblRutaRestore.AutoSize = true;
            this.lblRutaRestore.Location = new System.Drawing.Point(12, 58);
            this.lblRutaRestore.Name = "lblRutaRestore";
            this.lblRutaRestore.Size = new System.Drawing.Size(102, 13);
            this.lblRutaRestore.TabIndex = 3;
            this.lblRutaRestore.Text = "Archivo de Backup:";
            //
            // cmbBaseDatos
            //
            this.cmbBaseDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBaseDatos.FormattingEnabled = true;
            this.cmbBaseDatos.Items.AddRange(new object[] {
            "SecurityVet - Base de Datos de Seguridad",
            "VetCareDB - Base de Datos de Negocio"});
            this.cmbBaseDatos.Location = new System.Drawing.Point(15, 34);
            this.cmbBaseDatos.Name = "cmbBaseDatos";
            this.cmbBaseDatos.Size = new System.Drawing.Size(530, 21);
            this.cmbBaseDatos.TabIndex = 2;
            //
            // lblBaseDatos
            //
            this.lblBaseDatos.AutoSize = true;
            this.lblBaseDatos.Location = new System.Drawing.Point(12, 18);
            this.lblBaseDatos.Name = "lblBaseDatos";
            this.lblBaseDatos.Size = new System.Drawing.Size(145, 13);
            this.lblBaseDatos.TabIndex = 1;
            this.lblBaseDatos.Text = "Seleccione Base de Datos:";
            //
            // btnRealizarRestore
            //
            this.btnRealizarRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRealizarRestore.ForeColor = System.Drawing.Color.DarkRed;
            this.btnRealizarRestore.Location = new System.Drawing.Point(15, 133);
            this.btnRealizarRestore.Name = "btnRealizarRestore";
            this.btnRealizarRestore.Size = new System.Drawing.Size(530, 35);
            this.btnRealizarRestore.TabIndex = 0;
            this.btnRealizarRestore.Text = "RESTAURAR BACKUP (PRECAUCIÓN)";
            this.btnRealizarRestore.UseVisualStyleBackColor = true;
            //
            // progressBar
            //
            this.progressBar.Location = new System.Drawing.Point(12, 384);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(560, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 2;
            this.progressBar.Visible = false;
            //
            // txtLog
            //
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.Lime;
            this.txtLog.Location = new System.Drawing.Point(12, 433);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(560, 120);
            this.txtLog.TabIndex = 3;
            //
            // lblLog
            //
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(12, 417);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(111, 13);
            this.lblLog.TabIndex = 4;
            this.lblLog.Text = "Registro de Eventos:";
            //
            // btnCerrar
            //
            this.btnCerrar.Location = new System.Drawing.Point(497, 559);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(75, 30);
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            //
            // frmBackup
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 601);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.grpRestore);
            this.Controls.Add(this.grpBackup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBackup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backup y Restore de Bases de Datos";
            this.grpBackup.ResumeLayout(false);
            this.grpBackup.PerformLayout();
            this.grpRestore.ResumeLayout(false);
            this.grpRestore.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBackup;
        private System.Windows.Forms.Button btnRealizarBackup;
        private System.Windows.Forms.CheckBox chkBackupSeguridad;
        private System.Windows.Forms.CheckBox chkBackupNegocio;
        private System.Windows.Forms.Button btnExaminarBackup;
        private System.Windows.Forms.TextBox txtRutaBackup;
        private System.Windows.Forms.Label lblRutaBackup;
        private System.Windows.Forms.GroupBox grpRestore;
        private System.Windows.Forms.Button btnExaminarRestore;
        private System.Windows.Forms.TextBox txtRutaRestore;
        private System.Windows.Forms.Label lblRutaRestore;
        private System.Windows.Forms.ComboBox cmbBaseDatos;
        private System.Windows.Forms.Label lblBaseDatos;
        private System.Windows.Forms.Button btnRealizarRestore;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button btnCerrar;
    }
}
