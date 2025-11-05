namespace UI.WinUi.Negocio
{
    partial class FormHistorialDetallado
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpDatos = new System.Windows.Forms.GroupBox();
            this.txtDatosMascota = new System.Windows.Forms.TextBox();
            this.lblHistorial = new System.Windows.Forms.Label();
            this.dgvConsultas = new System.Windows.Forms.DataGridView();
            this.btnVerDetalle = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.grpDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsultas)).BeginInit();
            this.SuspendLayout();
            //
            // grpDatos
            //
            this.grpDatos.Controls.Add(this.txtDatosMascota);
            this.grpDatos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpDatos.Location = new System.Drawing.Point(30, 20);
            this.grpDatos.Name = "grpDatos";
            this.grpDatos.Size = new System.Drawing.Size(640, 80);
            this.grpDatos.TabIndex = 0;
            this.grpDatos.TabStop = false;
            this.grpDatos.Text = "Datos de la Mascota";
            //
            // txtDatosMascota
            //
            this.txtDatosMascota.BackColor = System.Drawing.SystemColors.Window;
            this.txtDatosMascota.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDatosMascota.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtDatosMascota.Location = new System.Drawing.Point(15, 25);
            this.txtDatosMascota.Multiline = true;
            this.txtDatosMascota.Name = "txtDatosMascota";
            this.txtDatosMascota.ReadOnly = true;
            this.txtDatosMascota.Size = new System.Drawing.Size(610, 45);
            this.txtDatosMascota.TabIndex = 0;
            //
            // lblHistorial
            //
            this.lblHistorial.AutoSize = true;
            this.lblHistorial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblHistorial.Location = new System.Drawing.Point(30, 115);
            this.lblHistorial.Name = "lblHistorial";
            this.lblHistorial.Size = new System.Drawing.Size(134, 15);
            this.lblHistorial.TabIndex = 1;
            this.lblHistorial.Text = "Historial de Consultas";
            //
            // dgvConsultas
            //
            this.dgvConsultas.AllowUserToAddRows = false;
            this.dgvConsultas.AllowUserToDeleteRows = false;
            this.dgvConsultas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvConsultas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvConsultas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConsultas.Location = new System.Drawing.Point(30, 140);
            this.dgvConsultas.MultiSelect = false;
            this.dgvConsultas.Name = "dgvConsultas";
            this.dgvConsultas.ReadOnly = true;
            this.dgvConsultas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConsultas.Size = new System.Drawing.Size(640, 250);
            this.dgvConsultas.TabIndex = 2;
            this.dgvConsultas.SelectionChanged += new System.EventHandler(this.dgvConsultas_SelectionChanged);
            this.dgvConsultas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConsultas_CellDoubleClick);
            //
            // btnVerDetalle
            //
            this.btnVerDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVerDetalle.Enabled = false;
            this.btnVerDetalle.Location = new System.Drawing.Point(30, 405);
            this.btnVerDetalle.Name = "btnVerDetalle";
            this.btnVerDetalle.Size = new System.Drawing.Size(150, 35);
            this.btnVerDetalle.TabIndex = 3;
            this.btnVerDetalle.Text = "Ver Detalle";
            this.btnVerDetalle.UseVisualStyleBackColor = true;
            this.btnVerDetalle.Click += new System.EventHandler(this.btnVerDetalle_Click);
            //
            // btnVolver
            //
            this.btnVolver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVolver.Location = new System.Drawing.Point(520, 405);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(150, 35);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            //
            // FormHistorialDetallado
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 460);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnVerDetalle);
            this.Controls.Add(this.dgvConsultas);
            this.Controls.Add(this.lblHistorial);
            this.Controls.Add(this.grpDatos);
            this.MinimumSize = new System.Drawing.Size(716, 499);
            this.Name = "FormHistorialDetallado";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Historial Detallado";
            this.Load += new System.EventHandler(this.FormHistorialDetallado_Load);
            this.grpDatos.ResumeLayout(false);
            this.grpDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsultas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpDatos;
        private System.Windows.Forms.TextBox txtDatosMascota;
        private System.Windows.Forms.Label lblHistorial;
        private System.Windows.Forms.DataGridView dgvConsultas;
        private System.Windows.Forms.Button btnVerDetalle;
        private System.Windows.Forms.Button btnVolver;
    }
}
