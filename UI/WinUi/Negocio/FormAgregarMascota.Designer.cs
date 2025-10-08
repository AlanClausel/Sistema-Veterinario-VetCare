namespace UI.WinUi.Negocio
{
    partial class FormAgregarMascota
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
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblEspecie = new System.Windows.Forms.Label();
            this.txtEspecie = new System.Windows.Forms.TextBox();
            this.lblRaza = new System.Windows.Forms.Label();
            this.txtRaza = new System.Windows.Forms.TextBox();
            this.lblFechaNacimiento = new System.Windows.Forms.Label();
            this.dtpFechaNacimiento = new System.Windows.Forms.DateTimePicker();
            this.lblSexo = new System.Windows.Forms.Label();
            this.cboSexo = new System.Windows.Forms.ComboBox();
            this.lblPeso = new System.Windows.Forms.Label();
            this.nudPeso = new System.Windows.Forms.NumericUpDown();
            this.lblColor = new System.Windows.Forms.Label();
            this.txtColor = new System.Windows.Forms.TextBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudPeso)).BeginInit();
            this.SuspendLayout();
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNombre.Location = new System.Drawing.Point(20, 20);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(67, 20);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre:";
            //
            // txtNombre
            //
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNombre.Location = new System.Drawing.Point(140, 17);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(250, 27);
            this.txtNombre.TabIndex = 1;
            //
            // lblEspecie
            //
            this.lblEspecie.AutoSize = true;
            this.lblEspecie.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEspecie.Location = new System.Drawing.Point(20, 60);
            this.lblEspecie.Name = "lblEspecie";
            this.lblEspecie.Size = new System.Drawing.Size(63, 20);
            this.lblEspecie.TabIndex = 2;
            this.lblEspecie.Text = "Especie:";
            //
            // txtEspecie
            //
            this.txtEspecie.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEspecie.Location = new System.Drawing.Point(140, 57);
            this.txtEspecie.Name = "txtEspecie";
            this.txtEspecie.Size = new System.Drawing.Size(250, 27);
            this.txtEspecie.TabIndex = 3;
            //
            // lblRaza
            //
            this.lblRaza.AutoSize = true;
            this.lblRaza.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRaza.Location = new System.Drawing.Point(20, 100);
            this.lblRaza.Name = "lblRaza";
            this.lblRaza.Size = new System.Drawing.Size(44, 20);
            this.lblRaza.TabIndex = 4;
            this.lblRaza.Text = "Raza:";
            //
            // txtRaza
            //
            this.txtRaza.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRaza.Location = new System.Drawing.Point(140, 97);
            this.txtRaza.Name = "txtRaza";
            this.txtRaza.Size = new System.Drawing.Size(250, 27);
            this.txtRaza.TabIndex = 5;
            //
            // lblFechaNacimiento
            //
            this.lblFechaNacimiento.AutoSize = true;
            this.lblFechaNacimiento.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFechaNacimiento.Location = new System.Drawing.Point(20, 140);
            this.lblFechaNacimiento.Name = "lblFechaNacimiento";
            this.lblFechaNacimiento.Size = new System.Drawing.Size(106, 20);
            this.lblFechaNacimiento.TabIndex = 6;
            this.lblFechaNacimiento.Text = "Fecha Nac.:";
            //
            // dtpFechaNacimiento
            //
            this.dtpFechaNacimiento.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFechaNacimiento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaNacimiento.Location = new System.Drawing.Point(140, 137);
            this.dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            this.dtpFechaNacimiento.Size = new System.Drawing.Size(250, 27);
            this.dtpFechaNacimiento.TabIndex = 7;
            //
            // lblSexo
            //
            this.lblSexo.AutoSize = true;
            this.lblSexo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSexo.Location = new System.Drawing.Point(20, 180);
            this.lblSexo.Name = "lblSexo";
            this.lblSexo.Size = new System.Drawing.Size(45, 20);
            this.lblSexo.TabIndex = 8;
            this.lblSexo.Text = "Sexo:";
            //
            // cboSexo
            //
            this.cboSexo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSexo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboSexo.FormattingEnabled = true;
            this.cboSexo.Items.AddRange(new object[] {
            "Macho",
            "Hembra"});
            this.cboSexo.Location = new System.Drawing.Point(140, 177);
            this.cboSexo.Name = "cboSexo";
            this.cboSexo.Size = new System.Drawing.Size(250, 28);
            this.cboSexo.TabIndex = 9;
            //
            // lblPeso
            //
            this.lblPeso.AutoSize = true;
            this.lblPeso.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPeso.Location = new System.Drawing.Point(20, 220);
            this.lblPeso.Name = "lblPeso";
            this.lblPeso.Size = new System.Drawing.Size(78, 20);
            this.lblPeso.TabIndex = 10;
            this.lblPeso.Text = "Peso (Kg):";
            //
            // nudPeso
            //
            this.nudPeso.DecimalPlaces = 2;
            this.nudPeso.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudPeso.Location = new System.Drawing.Point(140, 217);
            this.nudPeso.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudPeso.Name = "nudPeso";
            this.nudPeso.Size = new System.Drawing.Size(250, 27);
            this.nudPeso.TabIndex = 11;
            //
            // lblColor
            //
            this.lblColor.AutoSize = true;
            this.lblColor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblColor.Location = new System.Drawing.Point(20, 260);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(51, 20);
            this.lblColor.TabIndex = 12;
            this.lblColor.Text = "Color:";
            //
            // txtColor
            //
            this.txtColor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtColor.Location = new System.Drawing.Point(140, 257);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(250, 27);
            this.txtColor.TabIndex = 13;
            //
            // lblObservaciones
            //
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblObservaciones.Location = new System.Drawing.Point(20, 300);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(113, 20);
            this.lblObservaciones.TabIndex = 14;
            this.lblObservaciones.Text = "Observaciones:";
            //
            // txtObservaciones
            //
            this.txtObservaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtObservaciones.Location = new System.Drawing.Point(140, 297);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(250, 60);
            this.txtObservaciones.TabIndex = 15;
            //
            // btnGuardar
            //
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(166)))), ((int)(((byte)(154)))));
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(80, 380);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(120, 40);
            this.btnGuardar.TabIndex = 16;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.Location = new System.Drawing.Point(220, 380);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 40);
            this.btnCancelar.TabIndex = 17;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            //
            // FormAgregarMascota
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(420, 440);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtObservaciones);
            this.Controls.Add(this.lblObservaciones);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.nudPeso);
            this.Controls.Add(this.lblPeso);
            this.Controls.Add(this.cboSexo);
            this.Controls.Add(this.lblSexo);
            this.Controls.Add(this.dtpFechaNacimiento);
            this.Controls.Add(this.lblFechaNacimiento);
            this.Controls.Add(this.txtRaza);
            this.Controls.Add(this.lblRaza);
            this.Controls.Add(this.txtEspecie);
            this.Controls.Add(this.lblEspecie);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAgregarMascota";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Agregar Mascota";
            ((System.ComponentModel.ISupportInitialize)(this.nudPeso)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblEspecie;
        private System.Windows.Forms.TextBox txtEspecie;
        private System.Windows.Forms.Label lblRaza;
        private System.Windows.Forms.TextBox txtRaza;
        private System.Windows.Forms.Label lblFechaNacimiento;
        private System.Windows.Forms.DateTimePicker dtpFechaNacimiento;
        private System.Windows.Forms.Label lblSexo;
        private System.Windows.Forms.ComboBox cboSexo;
        private System.Windows.Forms.Label lblPeso;
        private System.Windows.Forms.NumericUpDown nudPeso;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.TextBox txtColor;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
