namespace UI.WinUi.Negocio
{
    partial class FormHistorialClinico
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
            this.lblBuscarDNI = new System.Windows.Forms.Label();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.grpCliente = new System.Windows.Forms.GroupBox();
            this.txtClienteInfo = new System.Windows.Forms.TextBox();
            this.lblMascotas = new System.Windows.Forms.Label();
            this.dgvMascotas = new System.Windows.Forms.DataGridView();
            this.btnVerHistorial = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.grpCliente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMascotas)).BeginInit();
            this.SuspendLayout();
            //
            // lblBuscarDNI
            //
            this.lblBuscarDNI.AutoSize = true;
            this.lblBuscarDNI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblBuscarDNI.Location = new System.Drawing.Point(30, 25);
            this.lblBuscarDNI.Name = "lblBuscarDNI";
            this.lblBuscarDNI.Size = new System.Drawing.Size(101, 15);
            this.lblBuscarDNI.TabIndex = 0;
            this.lblBuscarDNI.Text = "Buscar por DNI";
            //
            // txtDNI
            //
            this.txtDNI.Location = new System.Drawing.Point(33, 50);
            this.txtDNI.MaxLength = 15;
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(200, 20);
            this.txtDNI.TabIndex = 1;
            this.txtDNI.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDNI_KeyPress);
            //
            // btnBuscar
            //
            this.btnBuscar.Location = new System.Drawing.Point(250, 48);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(100, 25);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            //
            // grpCliente
            //
            this.grpCliente.Controls.Add(this.txtClienteInfo);
            this.grpCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpCliente.Location = new System.Drawing.Point(33, 90);
            this.grpCliente.Name = "grpCliente";
            this.grpCliente.Size = new System.Drawing.Size(650, 80);
            this.grpCliente.TabIndex = 3;
            this.grpCliente.TabStop = false;
            this.grpCliente.Text = "Cliente Encontrado";
            this.grpCliente.Visible = false;
            //
            // txtClienteInfo
            //
            this.txtClienteInfo.BackColor = System.Drawing.SystemColors.Window;
            this.txtClienteInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtClienteInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtClienteInfo.Location = new System.Drawing.Point(15, 25);
            this.txtClienteInfo.Multiline = true;
            this.txtClienteInfo.Name = "txtClienteInfo";
            this.txtClienteInfo.ReadOnly = true;
            this.txtClienteInfo.Size = new System.Drawing.Size(620, 40);
            this.txtClienteInfo.TabIndex = 0;
            //
            // lblMascotas
            //
            this.lblMascotas.AutoSize = true;
            this.lblMascotas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblMascotas.Location = new System.Drawing.Point(33, 185);
            this.lblMascotas.Name = "lblMascotas";
            this.lblMascotas.Size = new System.Drawing.Size(126, 15);
            this.lblMascotas.TabIndex = 4;
            this.lblMascotas.Text = "Mascotas del Cliente";
            //
            // dgvMascotas
            //
            this.dgvMascotas.AllowUserToAddRows = false;
            this.dgvMascotas.AllowUserToDeleteRows = false;
            this.dgvMascotas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMascotas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMascotas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMascotas.Location = new System.Drawing.Point(33, 210);
            this.dgvMascotas.MultiSelect = false;
            this.dgvMascotas.Name = "dgvMascotas";
            this.dgvMascotas.ReadOnly = true;
            this.dgvMascotas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMascotas.Size = new System.Drawing.Size(650, 200);
            this.dgvMascotas.TabIndex = 5;
            this.dgvMascotas.SelectionChanged += new System.EventHandler(this.dgvMascotas_SelectionChanged);
            //
            // btnVerHistorial
            //
            this.btnVerHistorial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVerHistorial.Enabled = false;
            this.btnVerHistorial.Location = new System.Drawing.Point(33, 425);
            this.btnVerHistorial.Name = "btnVerHistorial";
            this.btnVerHistorial.Size = new System.Drawing.Size(150, 35);
            this.btnVerHistorial.TabIndex = 6;
            this.btnVerHistorial.Text = "Ver Historial";
            this.btnVerHistorial.UseVisualStyleBackColor = true;
            this.btnVerHistorial.Click += new System.EventHandler(this.btnVerHistorial_Click);
            //
            // btnVolver
            //
            this.btnVolver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVolver.Location = new System.Drawing.Point(533, 425);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(150, 35);
            this.btnVolver.TabIndex = 7;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            //
            // FormHistorialClinico
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 480);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnVerHistorial);
            this.Controls.Add(this.dgvMascotas);
            this.Controls.Add(this.lblMascotas);
            this.Controls.Add(this.grpCliente);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtDNI);
            this.Controls.Add(this.lblBuscarDNI);
            this.MinimumSize = new System.Drawing.Size(736, 519);
            this.Name = "FormHistorialClinico";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Historial Clínico";
            this.Load += new System.EventHandler(this.FormHistorialClinico_Load);
            this.grpCliente.ResumeLayout(false);
            this.grpCliente.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMascotas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblBuscarDNI;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.GroupBox grpCliente;
        private System.Windows.Forms.TextBox txtClienteInfo;
        private System.Windows.Forms.Label lblMascotas;
        private System.Windows.Forms.DataGridView dgvMascotas;
        private System.Windows.Forms.Button btnVerHistorial;
        private System.Windows.Forms.Button btnVolver;
    }
}
