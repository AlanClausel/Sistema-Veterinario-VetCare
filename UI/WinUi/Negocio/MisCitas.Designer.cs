namespace UI.WinUi.Negocio
{
    partial class MisCitas
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
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblAgenda = new System.Windows.Forms.Label();
            this.dgvCitas = new System.Windows.Forms.DataGridView();
            this.btnIniciarConsulta = new System.Windows.Forms.Button();
            this.btnCompletada = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnCopiarDNI = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCitas)).BeginInit();
            this.SuspendLayout();
            //
            // dtpFecha
            //
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(79, 25);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(150, 20);
            this.dtpFecha.TabIndex = 0;
            this.dtpFecha.ValueChanged += new System.EventHandler(this.dtpFecha_ValueChanged);
            //
            // lblFecha
            //
            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(30, 28);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(40, 13);
            this.lblFecha.TabIndex = 1;
            this.lblFecha.Text = "Fecha:";
            //
            // lblAgenda
            //
            this.lblAgenda.AutoSize = true;
            this.lblAgenda.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAgenda.Location = new System.Drawing.Point(30, 60);
            this.lblAgenda.Name = "lblAgenda";
            this.lblAgenda.Size = new System.Drawing.Size(119, 15);
            this.lblAgenda.TabIndex = 2;
            this.lblAgenda.Text = "Agenda de la fecha";
            //
            // dgvCitas
            //
            this.dgvCitas.AllowUserToAddRows = false;
            this.dgvCitas.AllowUserToDeleteRows = false;
            this.dgvCitas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCitas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCitas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCitas.Location = new System.Drawing.Point(33, 88);
            this.dgvCitas.MultiSelect = false;
            this.dgvCitas.Name = "dgvCitas";
            this.dgvCitas.ReadOnly = true;
            this.dgvCitas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCitas.Size = new System.Drawing.Size(735, 270);
            this.dgvCitas.TabIndex = 3;
            this.dgvCitas.SelectionChanged += new System.EventHandler(this.dgvCitas_SelectionChanged);
            //
            // btnIniciarConsulta
            //
            this.btnIniciarConsulta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIniciarConsulta.Location = new System.Drawing.Point(33, 377);
            this.btnIniciarConsulta.Name = "btnIniciarConsulta";
            this.btnIniciarConsulta.Size = new System.Drawing.Size(150, 35);
            this.btnIniciarConsulta.TabIndex = 4;
            this.btnIniciarConsulta.Text = "Iniciar Consulta Médica";
            this.btnIniciarConsulta.UseVisualStyleBackColor = true;
            this.btnIniciarConsulta.Click += new System.EventHandler(this.btnIniciarConsulta_Click);
            //
            // btnCompletada
            //
            this.btnCompletada.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCompletada.Location = new System.Drawing.Point(301, 377);
            this.btnCompletada.Name = "btnCompletada";
            this.btnCompletada.Size = new System.Drawing.Size(120, 35);
            this.btnCompletada.TabIndex = 5;
            this.btnCompletada.Text = "Completada";
            this.btnCompletada.UseVisualStyleBackColor = true;
            this.btnCompletada.Click += new System.EventHandler(this.btnCompletada_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelar.Location = new System.Drawing.Point(537, 377);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 35);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // btnActualizar
            //
            this.btnActualizar.Location = new System.Drawing.Point(250, 23);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(100, 23);
            this.btnActualizar.TabIndex = 7;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            //
            // btnCopiarDNI
            //
            this.btnCopiarDNI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopiarDNI.Location = new System.Drawing.Point(199, 377);
            this.btnCopiarDNI.Name = "btnCopiarDNI";
            this.btnCopiarDNI.Size = new System.Drawing.Size(90, 35);
            this.btnCopiarDNI.TabIndex = 8;
            this.btnCopiarDNI.Text = "Copiar DNI";
            this.btnCopiarDNI.UseVisualStyleBackColor = true;
            this.btnCopiarDNI.Click += new System.EventHandler(this.btnCopiarDNI_Click);
            //
            // MisCitas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCopiarDNI);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnCompletada);
            this.Controls.Add(this.btnIniciarConsulta);
            this.Controls.Add(this.dgvCitas);
            this.Controls.Add(this.lblAgenda);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.dtpFecha);
            this.Name = "MisCitas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mis Citas";
            this.Load += new System.EventHandler(this.MisCitas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCitas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblAgenda;
        private System.Windows.Forms.DataGridView dgvCitas;
        private System.Windows.Forms.Button btnIniciarConsulta;
        private System.Windows.Forms.Button btnCompletada;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnCopiarDNI;
    }
}
