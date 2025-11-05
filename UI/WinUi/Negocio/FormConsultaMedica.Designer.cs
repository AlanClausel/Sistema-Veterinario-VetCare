namespace UI.WinUi.Negocio
{
    partial class FormConsultaMedica
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
            this.grpDatosCita = new System.Windows.Forms.GroupBox();
            this.txtTipo = new System.Windows.Forms.TextBox();
            this.txtHora = new System.Windows.Forms.TextBox();
            this.txtFecha = new System.Windows.Forms.TextBox();
            this.lblTipo = new System.Windows.Forms.Label();
            this.lblHora = new System.Windows.Forms.Label();
            this.lblFecha = new System.Windows.Forms.Label();
            this.grpDatosClienteMascota = new System.Windows.Forms.GroupBox();
            this.txtMascota = new System.Windows.Forms.TextBox();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.lblMascota = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.grpDatosConsulta = new System.Windows.Forms.GroupBox();
            this.txtDiagnostico = new System.Windows.Forms.TextBox();
            this.txtSintomas = new System.Windows.Forms.TextBox();
            this.lblDiagnostico = new System.Windows.Forms.Label();
            this.lblSintomas = new System.Windows.Forms.Label();
            this.grpMedicamentos = new System.Windows.Forms.GroupBox();
            this.lstSeleccionados = new System.Windows.Forms.ListBox();
            this.lstResultados = new System.Windows.Forms.ListBox();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.lblSeleccionados = new System.Windows.Forms.Label();
            this.lblResultados = new System.Windows.Forms.Label();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.btnAñadir = new System.Windows.Forms.Button();
            this.btnQuitar = new System.Windows.Forms.Button();
            this.btnFinalizarConsulta = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.grpDatosCita.SuspendLayout();
            this.grpDatosClienteMascota.SuspendLayout();
            this.grpDatosConsulta.SuspendLayout();
            this.grpMedicamentos.SuspendLayout();
            this.SuspendLayout();
            //
            // grpDatosCita
            //
            this.grpDatosCita.Controls.Add(this.txtTipo);
            this.grpDatosCita.Controls.Add(this.txtHora);
            this.grpDatosCita.Controls.Add(this.txtFecha);
            this.grpDatosCita.Controls.Add(this.lblTipo);
            this.grpDatosCita.Controls.Add(this.lblHora);
            this.grpDatosCita.Controls.Add(this.lblFecha);
            this.grpDatosCita.Location = new System.Drawing.Point(20, 20);
            this.grpDatosCita.Name = "grpDatosCita";
            this.grpDatosCita.Size = new System.Drawing.Size(600, 80);
            this.grpDatosCita.TabIndex = 0;
            this.grpDatosCita.TabStop = false;
            this.grpDatosCita.Text = "Datos de la Cita";
            //
            // txtTipo
            //
            this.txtTipo.Location = new System.Drawing.Point(450, 35);
            this.txtTipo.Name = "txtTipo";
            this.txtTipo.ReadOnly = true;
            this.txtTipo.Size = new System.Drawing.Size(130, 20);
            this.txtTipo.TabIndex = 5;
            //
            // txtHora
            //
            this.txtHora.Location = new System.Drawing.Point(250, 35);
            this.txtHora.Name = "txtHora";
            this.txtHora.ReadOnly = true;
            this.txtHora.Size = new System.Drawing.Size(130, 20);
            this.txtHora.TabIndex = 4;
            //
            // txtFecha
            //
            this.txtFecha.Location = new System.Drawing.Point(60, 35);
            this.txtFecha.Name = "txtFecha";
            this.txtFecha.ReadOnly = true;
            this.txtFecha.Size = new System.Drawing.Size(130, 20);
            this.txtFecha.TabIndex = 3;
            //
            // lblTipo
            //
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(410, 38);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(28, 13);
            this.lblTipo.TabIndex = 2;
            this.lblTipo.Text = "Tipo";
            //
            // lblHora
            //
            this.lblHora.AutoSize = true;
            this.lblHora.Location = new System.Drawing.Point(210, 38);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(30, 13);
            this.lblHora.TabIndex = 1;
            this.lblHora.Text = "Hora";
            //
            // lblFecha
            //
            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(15, 38);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(37, 13);
            this.lblFecha.TabIndex = 0;
            this.lblFecha.Text = "Fecha";
            //
            // grpDatosClienteMascota
            //
            this.grpDatosClienteMascota.Controls.Add(this.txtMascota);
            this.grpDatosClienteMascota.Controls.Add(this.txtCliente);
            this.grpDatosClienteMascota.Controls.Add(this.lblMascota);
            this.grpDatosClienteMascota.Controls.Add(this.lblCliente);
            this.grpDatosClienteMascota.Location = new System.Drawing.Point(20, 110);
            this.grpDatosClienteMascota.Name = "grpDatosClienteMascota";
            this.grpDatosClienteMascota.Size = new System.Drawing.Size(600, 80);
            this.grpDatosClienteMascota.TabIndex = 1;
            this.grpDatosClienteMascota.TabStop = false;
            this.grpDatosClienteMascota.Text = "Datos Cliente y Mascota";
            //
            // txtMascota
            //
            this.txtMascota.Location = new System.Drawing.Point(370, 35);
            this.txtMascota.Name = "txtMascota";
            this.txtMascota.ReadOnly = true;
            this.txtMascota.Size = new System.Drawing.Size(210, 20);
            this.txtMascota.TabIndex = 3;
            //
            // txtCliente
            //
            this.txtCliente.Location = new System.Drawing.Point(80, 35);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.ReadOnly = true;
            this.txtCliente.Size = new System.Drawing.Size(210, 20);
            this.txtCliente.TabIndex = 2;
            //
            // lblMascota
            //
            this.lblMascota.AutoSize = true;
            this.lblMascota.Location = new System.Drawing.Point(310, 38);
            this.lblMascota.Name = "lblMascota";
            this.lblMascota.Size = new System.Drawing.Size(48, 13);
            this.lblMascota.TabIndex = 1;
            this.lblMascota.Text = "Mascota";
            //
            // lblCliente
            //
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(15, 38);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(39, 13);
            this.lblCliente.TabIndex = 0;
            this.lblCliente.Text = "Cliente";
            //
            // grpDatosConsulta
            //
            this.grpDatosConsulta.Controls.Add(this.txtDiagnostico);
            this.grpDatosConsulta.Controls.Add(this.txtSintomas);
            this.grpDatosConsulta.Controls.Add(this.lblDiagnostico);
            this.grpDatosConsulta.Controls.Add(this.lblSintomas);
            this.grpDatosConsulta.Location = new System.Drawing.Point(20, 200);
            this.grpDatosConsulta.Name = "grpDatosConsulta";
            this.grpDatosConsulta.Size = new System.Drawing.Size(600, 150);
            this.grpDatosConsulta.TabIndex = 2;
            this.grpDatosConsulta.TabStop = false;
            this.grpDatosConsulta.Text = "Datos de la Consulta";
            //
            // txtDiagnostico
            //
            this.txtDiagnostico.Location = new System.Drawing.Point(100, 90);
            this.txtDiagnostico.Multiline = true;
            this.txtDiagnostico.Name = "txtDiagnostico";
            this.txtDiagnostico.Size = new System.Drawing.Size(480, 45);
            this.txtDiagnostico.TabIndex = 3;
            //
            // txtSintomas
            //
            this.txtSintomas.Location = new System.Drawing.Point(100, 30);
            this.txtSintomas.Multiline = true;
            this.txtSintomas.Name = "txtSintomas";
            this.txtSintomas.Size = new System.Drawing.Size(480, 45);
            this.txtSintomas.TabIndex = 2;
            //
            // lblDiagnostico
            //
            this.lblDiagnostico.AutoSize = true;
            this.lblDiagnostico.Location = new System.Drawing.Point(15, 93);
            this.lblDiagnostico.Name = "lblDiagnostico";
            this.lblDiagnostico.Size = new System.Drawing.Size(63, 13);
            this.lblDiagnostico.TabIndex = 1;
            this.lblDiagnostico.Text = "Diagnóstico";
            //
            // lblSintomas
            //
            this.lblSintomas.AutoSize = true;
            this.lblSintomas.Location = new System.Drawing.Point(15, 33);
            this.lblSintomas.Name = "lblSintomas";
            this.lblSintomas.Size = new System.Drawing.Size(52, 13);
            this.lblSintomas.TabIndex = 0;
            this.lblSintomas.Text = "Síntomas";
            //
            // grpMedicamentos
            //
            this.grpMedicamentos.Controls.Add(this.btnQuitar);
            this.grpMedicamentos.Controls.Add(this.btnAñadir);
            this.grpMedicamentos.Controls.Add(this.lstSeleccionados);
            this.grpMedicamentos.Controls.Add(this.lstResultados);
            this.grpMedicamentos.Controls.Add(this.txtBuscar);
            this.grpMedicamentos.Controls.Add(this.lblSeleccionados);
            this.grpMedicamentos.Controls.Add(this.lblResultados);
            this.grpMedicamentos.Controls.Add(this.lblBuscar);
            this.grpMedicamentos.Location = new System.Drawing.Point(20, 360);
            this.grpMedicamentos.Name = "grpMedicamentos";
            this.grpMedicamentos.Size = new System.Drawing.Size(600, 200);
            this.grpMedicamentos.TabIndex = 3;
            this.grpMedicamentos.TabStop = false;
            this.grpMedicamentos.Text = "Medicamentos";
            //
            // lstSeleccionados
            //
            this.lstSeleccionados.FormattingEnabled = true;
            this.lstSeleccionados.Location = new System.Drawing.Point(360, 90);
            this.lstSeleccionados.Name = "lstSeleccionados";
            this.lstSeleccionados.Size = new System.Drawing.Size(220, 95);
            this.lstSeleccionados.TabIndex = 5;
            //
            // lstResultados
            //
            this.lstResultados.FormattingEnabled = true;
            this.lstResultados.Location = new System.Drawing.Point(20, 90);
            this.lstResultados.Name = "lstResultados";
            this.lstResultados.Size = new System.Drawing.Size(220, 95);
            this.lstResultados.TabIndex = 4;
            //
            // txtBuscar
            //
            this.txtBuscar.Location = new System.Drawing.Point(70, 35);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(170, 20);
            this.txtBuscar.TabIndex = 3;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            //
            // lblSeleccionados
            //
            this.lblSeleccionados.AutoSize = true;
            this.lblSeleccionados.Location = new System.Drawing.Point(357, 70);
            this.lblSeleccionados.Name = "lblSeleccionados";
            this.lblSeleccionados.Size = new System.Drawing.Size(80, 13);
            this.lblSeleccionados.TabIndex = 2;
            this.lblSeleccionados.Text = "Seleccionados";
            //
            // lblResultados
            //
            this.lblResultados.AutoSize = true;
            this.lblResultados.Location = new System.Drawing.Point(17, 70);
            this.lblResultados.Name = "lblResultados";
            this.lblResultados.Size = new System.Drawing.Size(63, 13);
            this.lblResultados.TabIndex = 1;
            this.lblResultados.Text = "Resultados";
            //
            // lblBuscar
            //
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new System.Drawing.Point(17, 38);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(40, 13);
            this.lblBuscar.TabIndex = 0;
            this.lblBuscar.Text = "Buscar";
            //
            // btnAñadir
            //
            this.btnAñadir.Location = new System.Drawing.Point(260, 110);
            this.btnAñadir.Name = "btnAñadir";
            this.btnAñadir.Size = new System.Drawing.Size(75, 23);
            this.btnAñadir.TabIndex = 6;
            this.btnAñadir.Text = "Añadir >";
            this.btnAñadir.UseVisualStyleBackColor = true;
            this.btnAñadir.Click += new System.EventHandler(this.btnAñadir_Click);
            //
            // btnQuitar
            //
            this.btnQuitar.Location = new System.Drawing.Point(260, 140);
            this.btnQuitar.Name = "btnQuitar";
            this.btnQuitar.Size = new System.Drawing.Size(75, 23);
            this.btnQuitar.TabIndex = 7;
            this.btnQuitar.Text = "< Quitar";
            this.btnQuitar.UseVisualStyleBackColor = true;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
            //
            // btnFinalizarConsulta
            //
            this.btnFinalizarConsulta.Location = new System.Drawing.Point(140, 580);
            this.btnFinalizarConsulta.Name = "btnFinalizarConsulta";
            this.btnFinalizarConsulta.Size = new System.Drawing.Size(150, 35);
            this.btnFinalizarConsulta.TabIndex = 4;
            this.btnFinalizarConsulta.Text = "Finalizar Consulta";
            this.btnFinalizarConsulta.UseVisualStyleBackColor = true;
            this.btnFinalizarConsulta.Click += new System.EventHandler(this.btnFinalizarConsulta_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(370, 580);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(150, 35);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // FormConsultaMedica
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 630);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnFinalizarConsulta);
            this.Controls.Add(this.grpMedicamentos);
            this.Controls.Add(this.grpDatosConsulta);
            this.Controls.Add(this.grpDatosClienteMascota);
            this.Controls.Add(this.grpDatosCita);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConsultaMedica";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consulta Médica";
            this.Load += new System.EventHandler(this.FormConsultaMedica_Load);
            this.grpDatosCita.ResumeLayout(false);
            this.grpDatosCita.PerformLayout();
            this.grpDatosClienteMascota.ResumeLayout(false);
            this.grpDatosClienteMascota.PerformLayout();
            this.grpDatosConsulta.ResumeLayout(false);
            this.grpDatosConsulta.PerformLayout();
            this.grpMedicamentos.ResumeLayout(false);
            this.grpMedicamentos.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpDatosCita;
        private System.Windows.Forms.TextBox txtTipo;
        private System.Windows.Forms.TextBox txtHora;
        private System.Windows.Forms.TextBox txtFecha;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.Label lblHora;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.GroupBox grpDatosClienteMascota;
        private System.Windows.Forms.TextBox txtMascota;
        private System.Windows.Forms.TextBox txtCliente;
        private System.Windows.Forms.Label lblMascota;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.GroupBox grpDatosConsulta;
        private System.Windows.Forms.TextBox txtDiagnostico;
        private System.Windows.Forms.TextBox txtSintomas;
        private System.Windows.Forms.Label lblDiagnostico;
        private System.Windows.Forms.Label lblSintomas;
        private System.Windows.Forms.GroupBox grpMedicamentos;
        private System.Windows.Forms.Button btnQuitar;
        private System.Windows.Forms.Button btnAñadir;
        private System.Windows.Forms.ListBox lstSeleccionados;
        private System.Windows.Forms.ListBox lstResultados;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label lblSeleccionados;
        private System.Windows.Forms.Label lblResultados;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.Button btnFinalizarConsulta;
        private System.Windows.Forms.Button btnCancelar;
    }
}
