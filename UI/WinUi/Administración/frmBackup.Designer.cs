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
            this.btnBackupCompleto = new System.Windows.Forms.Button();
            this.btnAbrirCarpeta = new System.Windows.Forms.Button();
            this.grpRestore = new System.Windows.Forms.GroupBox();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblRutaActual = new System.Windows.Forms.Label();
            this.grpBackup.SuspendLayout();
            this.grpRestore.SuspendLayout();
            this.SuspendLayout();
            //
            // grpBackup
            //
            this.grpBackup.Controls.Add(this.btnAbrirCarpeta);
            this.grpBackup.Controls.Add(this.btnBackupCompleto);
            this.grpBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBackup.Location = new System.Drawing.Point(12, 12);
            this.grpBackup.Name = "grpBackup";
            this.grpBackup.Size = new System.Drawing.Size(460, 120);
            this.grpBackup.TabIndex = 0;
            this.grpBackup.TabStop = false;
            this.grpBackup.Text = "Realizar Backup";
            //
            // btnBackupCompleto
            //
            this.btnBackupCompleto.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackupCompleto.Location = new System.Drawing.Point(15, 30);
            this.btnBackupCompleto.Name = "btnBackupCompleto";
            this.btnBackupCompleto.Size = new System.Drawing.Size(430, 45);
            this.btnBackupCompleto.TabIndex = 0;
            this.btnBackupCompleto.Text = "REALIZAR BACKUP COMPLETO";
            this.btnBackupCompleto.UseVisualStyleBackColor = true;
            //
            // btnAbrirCarpeta
            //
            this.btnAbrirCarpeta.Location = new System.Drawing.Point(15, 81);
            this.btnAbrirCarpeta.Name = "btnAbrirCarpeta";
            this.btnAbrirCarpeta.Size = new System.Drawing.Size(430, 30);
            this.btnAbrirCarpeta.TabIndex = 1;
            this.btnAbrirCarpeta.Text = "Abrir Carpeta de Backups";
            this.btnAbrirCarpeta.UseVisualStyleBackColor = true;
            //
            // grpRestore
            //
            this.grpRestore.Controls.Add(this.btnRestaurar);
            this.grpRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRestore.Location = new System.Drawing.Point(12, 138);
            this.grpRestore.Name = "grpRestore";
            this.grpRestore.Size = new System.Drawing.Size(460, 90);
            this.grpRestore.TabIndex = 1;
            this.grpRestore.TabStop = false;
            this.grpRestore.Text = "Restaurar Backup";
            //
            // btnRestaurar
            //
            this.btnRestaurar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestaurar.ForeColor = System.Drawing.Color.DarkRed;
            this.btnRestaurar.Location = new System.Drawing.Point(15, 30);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(430, 45);
            this.btnRestaurar.TabIndex = 0;
            this.btnRestaurar.Text = "RESTAURAR DESDE ARCHIVO...";
            this.btnRestaurar.UseVisualStyleBackColor = true;
            //
            // lblInfo
            //
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblInfo.Location = new System.Drawing.Point(12, 240);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(390, 26);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "El backup completo incluye ambas bases de datos (SecurityVet y VetCareDB).\r\nLa restauración detecta automáticamente qué base de datos restaurar.";
            //
            // lblRutaActual
            //
            this.lblRutaActual.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRutaActual.Location = new System.Drawing.Point(12, 275);
            this.lblRutaActual.Name = "lblRutaActual";
            this.lblRutaActual.Size = new System.Drawing.Size(460, 40);
            this.lblRutaActual.TabIndex = 3;
            this.lblRutaActual.Text = "Los backups se guardan en: [ruta]";
            //
            // frmBackup
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 325);
            this.Controls.Add(this.lblRutaActual);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.grpRestore);
            this.Controls.Add(this.grpBackup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBackup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backup y Restore";
            this.grpBackup.ResumeLayout(false);
            this.grpRestore.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBackup;
        private System.Windows.Forms.Button btnBackupCompleto;
        private System.Windows.Forms.Button btnAbrirCarpeta;
        private System.Windows.Forms.GroupBox grpRestore;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblRutaActual;
    }
}
