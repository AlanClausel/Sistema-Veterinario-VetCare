namespace UI.WinUi.Negocio
{
    partial class gestionMascotas
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
            this.groupBoxDatosMascota = new System.Windows.Forms.GroupBox();
            this.cmbDueno = new System.Windows.Forms.ComboBox();
            this.lblDueno = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.chkActivo = new System.Windows.Forms.CheckBox();
            this.txtColor = new System.Windows.Forms.TextBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.numPeso = new System.Windows.Forms.NumericUpDown();
            this.lblPeso = new System.Windows.Forms.Label();
            this.cmbSexo = new System.Windows.Forms.ComboBox();
            this.lblSexo = new System.Windows.Forms.Label();
            this.dtpFechaNacimiento = new System.Windows.Forms.DateTimePicker();
            this.lblFechaNacimiento = new System.Windows.Forms.Label();
            this.txtRaza = new System.Windows.Forms.TextBox();
            this.lblRaza = new System.Windows.Forms.Label();
            this.txtEspecie = new System.Windows.Forms.TextBox();
            this.lblEspecie = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.groupBoxAcciones = new System.Windows.Forms.GroupBox();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.dgvMascotas = new System.Windows.Forms.DataGridView();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxDatosMascota.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPeso)).BeginInit();
            this.groupBoxAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMascotas)).BeginInit();
            this.SuspendLayout();
            //
            // groupBoxDatosMascota
            //
            this.groupBoxDatosMascota.Controls.Add(this.cmbDueno);
            this.groupBoxDatosMascota.Controls.Add(this.lblDueno);
            this.groupBoxDatosMascota.Controls.Add(this.txtObservaciones);
            this.groupBoxDatosMascota.Controls.Add(this.lblObservaciones);
            this.groupBoxDatosMascota.Controls.Add(this.chkActivo);
            this.groupBoxDatosMascota.Controls.Add(this.txtColor);
            this.groupBoxDatosMascota.Controls.Add(this.lblColor);
            this.groupBoxDatosMascota.Controls.Add(this.numPeso);
            this.groupBoxDatosMascota.Controls.Add(this.lblPeso);
            this.groupBoxDatosMascota.Controls.Add(this.cmbSexo);
            this.groupBoxDatosMascota.Controls.Add(this.lblSexo);
            this.groupBoxDatosMascota.Controls.Add(this.dtpFechaNacimiento);
            this.groupBoxDatosMascota.Controls.Add(this.lblFechaNacimiento);
            this.groupBoxDatosMascota.Controls.Add(this.txtRaza);
            this.groupBoxDatosMascota.Controls.Add(this.lblRaza);
            this.groupBoxDatosMascota.Controls.Add(this.txtEspecie);
            this.groupBoxDatosMascota.Controls.Add(this.lblEspecie);
            this.groupBoxDatosMascota.Controls.Add(this.txtNombre);
            this.groupBoxDatosMascota.Controls.Add(this.lblNombre);
            this.groupBoxDatosMascota.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxDatosMascota.Location = new System.Drawing.Point(12, 70);
            this.groupBoxDatosMascota.Name = "groupBoxDatosMascota";
            this.groupBoxDatosMascota.Size = new System.Drawing.Size(420, 480);
            this.groupBoxDatosMascota.TabIndex = 0;
            this.groupBoxDatosMascota.TabStop = false;
            this.groupBoxDatosMascota.Text = "Datos de la Mascota";
            //
            // cmbDueno
            //
            this.cmbDueno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDueno.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbDueno.FormattingEnabled = true;
            this.cmbDueno.Location = new System.Drawing.Point(140, 305);
            this.cmbDueno.Name = "cmbDueno";
            this.cmbDueno.Size = new System.Drawing.Size(260, 28);
            this.cmbDueno.TabIndex = 18;
            //
            // lblDueno
            //
            this.lblDueno.AutoSize = true;
            this.lblDueno.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDueno.Location = new System.Drawing.Point(20, 308);
            this.lblDueno.Name = "lblDueno";
            this.lblDueno.Size = new System.Drawing.Size(58, 20);
            this.lblDueno.TabIndex = 17;
            this.lblDueno.Text = "Dueño:";
            //
            // txtObservaciones
            //
            this.txtObservaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtObservaciones.Location = new System.Drawing.Point(140, 345);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(260, 80);
            this.txtObservaciones.TabIndex = 16;
            //
            // lblObservaciones
            //
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblObservaciones.Location = new System.Drawing.Point(20, 348);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(112, 20);
            this.lblObservaciones.TabIndex = 15;
            this.lblObservaciones.Text = "Observaciones:";
            //
            // chkActivo
            //
            this.chkActivo.AutoSize = true;
            this.chkActivo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkActivo.Location = new System.Drawing.Point(140, 440);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new System.Drawing.Size(65, 24);
            this.chkActivo.TabIndex = 14;
            this.chkActivo.Text = "Activo";
            this.chkActivo.UseVisualStyleBackColor = true;
            //
            // txtColor
            //
            this.txtColor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtColor.Location = new System.Drawing.Point(140, 265);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(260, 27);
            this.txtColor.TabIndex = 13;
            //
            // lblColor
            //
            this.lblColor.AutoSize = true;
            this.lblColor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblColor.Location = new System.Drawing.Point(20, 268);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(51, 20);
            this.lblColor.TabIndex = 12;
            this.lblColor.Text = "Color:";
            //
            // numPeso
            //
            this.numPeso.DecimalPlaces = 2;
            this.numPeso.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numPeso.Location = new System.Drawing.Point(140, 225);
            this.numPeso.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numPeso.Name = "numPeso";
            this.numPeso.Size = new System.Drawing.Size(120, 27);
            this.numPeso.TabIndex = 11;
            //
            // lblPeso
            //
            this.lblPeso.AutoSize = true;
            this.lblPeso.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPeso.Location = new System.Drawing.Point(20, 228);
            this.lblPeso.Name = "lblPeso";
            this.lblPeso.Size = new System.Drawing.Size(75, 20);
            this.lblPeso.TabIndex = 10;
            this.lblPeso.Text = "Peso (kg):";
            //
            // cmbSexo
            //
            this.cmbSexo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSexo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbSexo.FormattingEnabled = true;
            this.cmbSexo.Items.AddRange(new object[] {
            "Macho",
            "Hembra"});
            this.cmbSexo.Location = new System.Drawing.Point(140, 185);
            this.cmbSexo.Name = "cmbSexo";
            this.cmbSexo.Size = new System.Drawing.Size(150, 28);
            this.cmbSexo.TabIndex = 9;
            //
            // lblSexo
            //
            this.lblSexo.AutoSize = true;
            this.lblSexo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSexo.Location = new System.Drawing.Point(20, 188);
            this.lblSexo.Name = "lblSexo";
            this.lblSexo.Size = new System.Drawing.Size(45, 20);
            this.lblSexo.TabIndex = 8;
            this.lblSexo.Text = "Sexo:";
            //
            // dtpFechaNacimiento
            //
            this.dtpFechaNacimiento.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFechaNacimiento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaNacimiento.Location = new System.Drawing.Point(140, 145);
            this.dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            this.dtpFechaNacimiento.Size = new System.Drawing.Size(150, 27);
            this.dtpFechaNacimiento.TabIndex = 7;
            //
            // lblFechaNacimiento
            //
            this.lblFechaNacimiento.AutoSize = true;
            this.lblFechaNacimiento.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFechaNacimiento.Location = new System.Drawing.Point(20, 148);
            this.lblFechaNacimiento.Name = "lblFechaNacimiento";
            this.lblFechaNacimiento.Size = new System.Drawing.Size(99, 20);
            this.lblFechaNacimiento.TabIndex = 6;
            this.lblFechaNacimiento.Text = "Fecha Nac.:";
            //
            // txtRaza
            //
            this.txtRaza.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRaza.Location = new System.Drawing.Point(140, 105);
            this.txtRaza.Name = "txtRaza";
            this.txtRaza.Size = new System.Drawing.Size(260, 27);
            this.txtRaza.TabIndex = 5;
            //
            // lblRaza
            //
            this.lblRaza.AutoSize = true;
            this.lblRaza.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRaza.Location = new System.Drawing.Point(20, 108);
            this.lblRaza.Name = "lblRaza";
            this.lblRaza.Size = new System.Drawing.Size(45, 20);
            this.lblRaza.TabIndex = 4;
            this.lblRaza.Text = "Raza:";
            //
            // txtEspecie
            //
            this.txtEspecie.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEspecie.Location = new System.Drawing.Point(140, 65);
            this.txtEspecie.Name = "txtEspecie";
            this.txtEspecie.Size = new System.Drawing.Size(260, 27);
            this.txtEspecie.TabIndex = 3;
            //
            // lblEspecie
            //
            this.lblEspecie.AutoSize = true;
            this.lblEspecie.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEspecie.Location = new System.Drawing.Point(20, 68);
            this.lblEspecie.Name = "lblEspecie";
            this.lblEspecie.Size = new System.Drawing.Size(62, 20);
            this.lblEspecie.TabIndex = 2;
            this.lblEspecie.Text = "Especie:";
            //
            // txtNombre
            //
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNombre.Location = new System.Drawing.Point(140, 25);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(260, 27);
            this.txtNombre.TabIndex = 1;
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNombre.Location = new System.Drawing.Point(20, 28);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(71, 20);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre:";
            //
            // groupBoxAcciones
            //
            this.groupBoxAcciones.Controls.Add(this.btnVolver);
            this.groupBoxAcciones.Controls.Add(this.btnEliminar);
            this.groupBoxAcciones.Controls.Add(this.btnModificar);
            this.groupBoxAcciones.Controls.Add(this.btnGuardar);
            this.groupBoxAcciones.Controls.Add(this.btnNuevo);
            this.groupBoxAcciones.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxAcciones.Location = new System.Drawing.Point(12, 560);
            this.groupBoxAcciones.Name = "groupBoxAcciones";
            this.groupBoxAcciones.Size = new System.Drawing.Size(420, 100);
            this.groupBoxAcciones.TabIndex = 1;
            this.groupBoxAcciones.TabStop = false;
            this.groupBoxAcciones.Text = "Acciones";
            //
            // btnVolver
            //
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnVolver.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnVolver.Location = new System.Drawing.Point(320, 30);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(80, 50);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            //
            // btnEliminar
            //
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnEliminar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.Location = new System.Drawing.Point(245, 30);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(69, 50);
            this.btnEliminar.TabIndex = 3;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            //
            // btnModificar
            //
            this.btnModificar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.btnModificar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModificar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModificar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnModificar.ForeColor = System.Drawing.Color.White;
            this.btnModificar.Location = new System.Drawing.Point(165, 30);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(74, 50);
            this.btnModificar.TabIndex = 2;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = false;
            //
            // btnGuardar
            //
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(166)))), ((int)(((byte)(154)))));
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(85, 30);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(74, 50);
            this.btnGuardar.TabIndex = 1;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            //
            // btnNuevo
            //
            this.btnNuevo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnNuevo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.Location = new System.Drawing.Point(10, 30);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(69, 50);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = false;
            //
            // dgvMascotas
            //
            this.dgvMascotas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMascotas.Location = new System.Drawing.Point(440, 70);
            this.dgvMascotas.Name = "dgvMascotas";
            this.dgvMascotas.RowHeadersWidth = 51;
            this.dgvMascotas.RowTemplate.Height = 24;
            this.dgvMascotas.Size = new System.Drawing.Size(740, 590);
            this.dgvMascotas.TabIndex = 2;
            //
            // txtBuscar
            //
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBuscar.Location = new System.Drawing.Point(590, 25);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(470, 27);
            this.txtBuscar.TabIndex = 3;
            //
            // btnBuscar
            //
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.Location = new System.Drawing.Point(1070, 20);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(110, 35);
            this.btnBuscar.TabIndex = 4;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(440, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Buscar Mascota:";
            //
            // gestionMascotas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1195, 675);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.dgvMascotas);
            this.Controls.Add(this.groupBoxAcciones);
            this.Controls.Add(this.groupBoxDatosMascota);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "gestionMascotas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Mascotas";
            this.groupBoxDatosMascota.ResumeLayout(false);
            this.groupBoxDatosMascota.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPeso)).EndInit();
            this.groupBoxAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMascotas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDatosMascota;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtEspecie;
        private System.Windows.Forms.Label lblEspecie;
        private System.Windows.Forms.TextBox txtRaza;
        private System.Windows.Forms.Label lblRaza;
        private System.Windows.Forms.DateTimePicker dtpFechaNacimiento;
        private System.Windows.Forms.Label lblFechaNacimiento;
        private System.Windows.Forms.ComboBox cmbSexo;
        private System.Windows.Forms.Label lblSexo;
        private System.Windows.Forms.NumericUpDown numPeso;
        private System.Windows.Forms.Label lblPeso;
        private System.Windows.Forms.TextBox txtColor;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.CheckBox chkActivo;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.ComboBox cmbDueno;
        private System.Windows.Forms.Label lblDueno;
        private System.Windows.Forms.GroupBox groupBoxAcciones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.DataGridView dgvMascotas;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label label1;
    }
}
