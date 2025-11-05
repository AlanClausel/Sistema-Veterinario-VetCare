namespace UI.WinUi.Administrador
{
    partial class FormMiCuenta
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.grpInformacion = new System.Windows.Forms.GroupBox();
            this.txtRoles = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpContraseña = new System.Windows.Forms.GroupBox();
            this.chkMostrarContraseña = new System.Windows.Forms.CheckBox();
            this.btnCambiarContraseña = new System.Windows.Forms.Button();
            this.txtConfirmarContraseña = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtNuevaContraseña = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtContraseñaActual = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grpIdioma = new System.Windows.Forms.GroupBox();
            this.btnGuardarIdioma = new System.Windows.Forms.Button();
            this.cmbIdioma = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.grpInformacion.SuspendLayout();
            this.grpContraseña.SuspendLayout();
            this.grpIdioma.SuspendLayout();
            this.SuspendLayout();
            //
            // panelTop
            //
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(142)))), ((int)(((byte)(60)))));
            this.panelTop.Controls.Add(this.lblTitulo);
            this.panelTop.Controls.Add(this.btnCerrar);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(700, 60);
            this.panelTop.TabIndex = 0;
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 14);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(139, 32);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Mi Cuenta";
            //
            // btnCerrar
            //
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(600, 14);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(80, 35);
            this.btnCerrar.TabIndex = 0;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // grpInformacion
            //
            this.grpInformacion.Controls.Add(this.txtRoles);
            this.grpInformacion.Controls.Add(this.label3);
            this.grpInformacion.Controls.Add(this.txtEmail);
            this.grpInformacion.Controls.Add(this.label2);
            this.grpInformacion.Controls.Add(this.txtUsuario);
            this.grpInformacion.Controls.Add(this.label1);
            this.grpInformacion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.grpInformacion.Location = new System.Drawing.Point(30, 80);
            this.grpInformacion.Name = "grpInformacion";
            this.grpInformacion.Size = new System.Drawing.Size(640, 140);
            this.grpInformacion.TabIndex = 1;
            this.grpInformacion.TabStop = false;
            this.grpInformacion.Text = "Información del Usuario";
            //
            // txtRoles
            //
            this.txtRoles.Location = new System.Drawing.Point(100, 100);
            this.txtRoles.Name = "txtRoles";
            this.txtRoles.ReadOnly = true;
            this.txtRoles.Size = new System.Drawing.Size(520, 25);
            this.txtRoles.TabIndex = 5;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "Roles:";
            //
            // txtEmail
            //
            this.txtEmail.Location = new System.Drawing.Point(100, 70);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.ReadOnly = true;
            this.txtEmail.Size = new System.Drawing.Size(220, 25);
            this.txtEmail.TabIndex = 3;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Email:";
            //
            // txtUsuario
            //
            this.txtUsuario.Location = new System.Drawing.Point(100, 40);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(220, 25);
            this.txtUsuario.TabIndex = 1;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Usuario:";
            //
            // grpContraseña
            //
            this.grpContraseña.Controls.Add(this.chkMostrarContraseña);
            this.grpContraseña.Controls.Add(this.btnCambiarContraseña);
            this.grpContraseña.Controls.Add(this.txtConfirmarContraseña);
            this.grpContraseña.Controls.Add(this.label7);
            this.grpContraseña.Controls.Add(this.txtNuevaContraseña);
            this.grpContraseña.Controls.Add(this.label6);
            this.grpContraseña.Controls.Add(this.txtContraseñaActual);
            this.grpContraseña.Controls.Add(this.label5);
            this.grpContraseña.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.grpContraseña.Location = new System.Drawing.Point(30, 235);
            this.grpContraseña.Name = "grpContraseña";
            this.grpContraseña.Size = new System.Drawing.Size(640, 180);
            this.grpContraseña.TabIndex = 2;
            this.grpContraseña.TabStop = false;
            this.grpContraseña.Text = "Cambiar Contraseña";
            //
            // chkMostrarContraseña
            //
            this.chkMostrarContraseña.AutoSize = true;
            this.chkMostrarContraseña.Location = new System.Drawing.Point(150, 140);
            this.chkMostrarContraseña.Name = "chkMostrarContraseña";
            this.chkMostrarContraseña.Size = new System.Drawing.Size(149, 23);
            this.chkMostrarContraseña.TabIndex = 7;
            this.chkMostrarContraseña.Text = "Mostrar contraseñas";
            this.chkMostrarContraseña.UseVisualStyleBackColor = true;
            this.chkMostrarContraseña.CheckedChanged += new System.EventHandler(this.chkMostrarContraseña_CheckedChanged);
            //
            // btnCambiarContraseña
            //
            this.btnCambiarContraseña.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(142)))), ((int)(((byte)(60)))));
            this.btnCambiarContraseña.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarContraseña.ForeColor = System.Drawing.Color.White;
            this.btnCambiarContraseña.Location = new System.Drawing.Point(450, 130);
            this.btnCambiarContraseña.Name = "btnCambiarContraseña";
            this.btnCambiarContraseña.Size = new System.Drawing.Size(170, 35);
            this.btnCambiarContraseña.TabIndex = 6;
            this.btnCambiarContraseña.Text = "Cambiar Contraseña";
            this.btnCambiarContraseña.UseVisualStyleBackColor = false;
            this.btnCambiarContraseña.Click += new System.EventHandler(this.btnCambiarContraseña_Click);
            //
            // txtConfirmarContraseña
            //
            this.txtConfirmarContraseña.Location = new System.Drawing.Point(150, 100);
            this.txtConfirmarContraseña.Name = "txtConfirmarContraseña";
            this.txtConfirmarContraseña.Size = new System.Drawing.Size(250, 25);
            this.txtConfirmarContraseña.TabIndex = 5;
            this.txtConfirmarContraseña.UseSystemPasswordChar = true;
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 19);
            this.label7.TabIndex = 4;
            this.label7.Text = "Confirmar:";
            //
            // txtNuevaContraseña
            //
            this.txtNuevaContraseña.Location = new System.Drawing.Point(150, 70);
            this.txtNuevaContraseña.Name = "txtNuevaContraseña";
            this.txtNuevaContraseña.Size = new System.Drawing.Size(250, 25);
            this.txtNuevaContraseña.TabIndex = 3;
            this.txtNuevaContraseña.UseSystemPasswordChar = true;
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 19);
            this.label6.TabIndex = 2;
            this.label6.Text = "Nueva contraseña:";
            //
            // txtContraseñaActual
            //
            this.txtContraseñaActual.Location = new System.Drawing.Point(150, 40);
            this.txtContraseñaActual.Name = "txtContraseñaActual";
            this.txtContraseñaActual.Size = new System.Drawing.Size(250, 25);
            this.txtContraseñaActual.TabIndex = 1;
            this.txtContraseñaActual.UseSystemPasswordChar = true;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 19);
            this.label5.TabIndex = 0;
            this.label5.Text = "Contraseña actual:";
            //
            // grpIdioma
            //
            this.grpIdioma.Controls.Add(this.btnGuardarIdioma);
            this.grpIdioma.Controls.Add(this.cmbIdioma);
            this.grpIdioma.Controls.Add(this.label8);
            this.grpIdioma.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.grpIdioma.Location = new System.Drawing.Point(30, 430);
            this.grpIdioma.Name = "grpIdioma";
            this.grpIdioma.Size = new System.Drawing.Size(640, 100);
            this.grpIdioma.TabIndex = 3;
            this.grpIdioma.TabStop = false;
            this.grpIdioma.Text = "Preferencias de Idioma";
            //
            // btnGuardarIdioma
            //
            this.btnGuardarIdioma.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(142)))), ((int)(((byte)(60)))));
            this.btnGuardarIdioma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardarIdioma.ForeColor = System.Drawing.Color.White;
            this.btnGuardarIdioma.Location = new System.Drawing.Point(450, 38);
            this.btnGuardarIdioma.Name = "btnGuardarIdioma";
            this.btnGuardarIdioma.Size = new System.Drawing.Size(170, 35);
            this.btnGuardarIdioma.TabIndex = 2;
            this.btnGuardarIdioma.Text = "Guardar Idioma";
            this.btnGuardarIdioma.UseVisualStyleBackColor = false;
            this.btnGuardarIdioma.Click += new System.EventHandler(this.btnGuardarIdioma_Click);
            //
            // cmbIdioma
            //
            this.cmbIdioma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIdioma.FormattingEnabled = true;
            this.cmbIdioma.Location = new System.Drawing.Point(150, 42);
            this.cmbIdioma.Name = "cmbIdioma";
            this.cmbIdioma.Size = new System.Drawing.Size(250, 25);
            this.cmbIdioma.TabIndex = 1;
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 19);
            this.label8.TabIndex = 0;
            this.label8.Text = "Idioma:";
            //
            // FormMiCuenta
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 550);
            this.Controls.Add(this.grpIdioma);
            this.Controls.Add(this.grpContraseña);
            this.Controls.Add(this.grpInformacion);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormMiCuenta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mi Cuenta - Sistema VetCare";
            this.Load += new System.EventHandler(this.FormMiCuenta_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.grpInformacion.ResumeLayout(false);
            this.grpInformacion.PerformLayout();
            this.grpContraseña.ResumeLayout(false);
            this.grpContraseña.PerformLayout();
            this.grpIdioma.ResumeLayout(false);
            this.grpIdioma.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.GroupBox grpInformacion;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRoles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpContraseña;
        private System.Windows.Forms.Button btnCambiarContraseña;
        private System.Windows.Forms.TextBox txtConfirmarContraseña;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNuevaContraseña;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtContraseñaActual;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox grpIdioma;
        private System.Windows.Forms.Button btnGuardarIdioma;
        private System.Windows.Forms.ComboBox cmbIdioma;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkMostrarContraseña;
    }
}
