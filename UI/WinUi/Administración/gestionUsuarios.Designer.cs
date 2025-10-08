namespace UI.WinUi.Administrador
{
    partial class gestionUsuarios
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtBuscarPorUsuario = new System.Windows.Forms.TextBox();
            this.txtNombreUsuario = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtContraseña = new System.Windows.Forms.TextBox();
            this.comboBoxPerfil = new System.Windows.Forms.ComboBox();
            this.lblNombreUsuario = new System.Windows.Forms.Label();
            this.lblPerfil = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblContraseña = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.groupBoxDatosUsuario = new System.Windows.Forms.GroupBox();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.groupBoxAcciones = new System.Windows.Forms.GroupBox();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.groupBoxDatosUsuario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.groupBoxAcciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Buscar por Usuario";
            // 
            // txtBuscarPorUsuario
            // 
            this.txtBuscarPorUsuario.Location = new System.Drawing.Point(248, 21);
            this.txtBuscarPorUsuario.Margin = new System.Windows.Forms.Padding(2);
            this.txtBuscarPorUsuario.Name = "txtBuscarPorUsuario";
            this.txtBuscarPorUsuario.Size = new System.Drawing.Size(130, 20);
            this.txtBuscarPorUsuario.TabIndex = 1;
            // 
            // txtNombreUsuario
            // 
            this.txtNombreUsuario.Location = new System.Drawing.Point(147, 32);
            this.txtNombreUsuario.Margin = new System.Windows.Forms.Padding(2);
            this.txtNombreUsuario.Name = "txtNombreUsuario";
            this.txtNombreUsuario.Size = new System.Drawing.Size(113, 20);
            this.txtNombreUsuario.TabIndex = 2;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(147, 67);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(2);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(113, 20);
            this.txtEmail.TabIndex = 3;
            // 
            // txtContraseña
            // 
            this.txtContraseña.Location = new System.Drawing.Point(147, 102);
            this.txtContraseña.Margin = new System.Windows.Forms.Padding(2);
            this.txtContraseña.Name = "txtContraseña";
            this.txtContraseña.PasswordChar = '*';
            this.txtContraseña.Size = new System.Drawing.Size(113, 20);
            this.txtContraseña.TabIndex = 4;
            // 
            // comboBoxPerfil
            // 
            this.comboBoxPerfil.FormattingEnabled = true;
            this.comboBoxPerfil.Location = new System.Drawing.Point(405, 29);
            this.comboBoxPerfil.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxPerfil.Name = "comboBoxPerfil";
            this.comboBoxPerfil.Size = new System.Drawing.Size(125, 21);
            this.comboBoxPerfil.TabIndex = 5;
            //
            // lblNombreUsuario
            // 
            this.lblNombreUsuario.AutoSize = true;
            this.lblNombreUsuario.Location = new System.Drawing.Point(39, 32);
            this.lblNombreUsuario.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombreUsuario.Name = "lblNombreUsuario";
            this.lblNombreUsuario.Size = new System.Drawing.Size(98, 13);
            this.lblNombreUsuario.TabIndex = 6;
            this.lblNombreUsuario.Text = "Nombre de Usuario";
            // 
            // lblPerfil
            // 
            this.lblPerfil.AutoSize = true;
            this.lblPerfil.Location = new System.Drawing.Point(341, 32);
            this.lblPerfil.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPerfil.Name = "lblPerfil";
            this.lblPerfil.Size = new System.Drawing.Size(30, 13);
            this.lblPerfil.TabIndex = 7;
            this.lblPerfil.Text = "Perfil";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(39, 70);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 8;
            this.lblEmail.Text = "Email";
            // 
            // lblContraseña
            // 
            this.lblContraseña.AutoSize = true;
            this.lblContraseña.Location = new System.Drawing.Point(39, 105);
            this.lblContraseña.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblContraseña.Name = "lblContraseña";
            this.lblContraseña.Size = new System.Drawing.Size(61, 13);
            this.lblContraseña.TabIndex = 9;
            this.lblContraseña.Text = "Contraseña";
            //
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(339, 69);
            this.lblEstado.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(40, 13);
            this.lblEstado.TabIndex = 10;
            this.lblEstado.Text = "Estado";
            this.lblEstado.Click += new System.EventHandler(this.lblEstado_Click);
            // 
            // groupBoxDatosUsuario
            // 
            this.groupBoxDatosUsuario.Controls.Add(this.dgvUsuarios);
            this.groupBoxDatosUsuario.Controls.Add(this.lblNombreUsuario);
            this.groupBoxDatosUsuario.Controls.Add(this.lblEstado);
            this.groupBoxDatosUsuario.Controls.Add(this.txtNombreUsuario);
            this.groupBoxDatosUsuario.Controls.Add(this.comboBoxPerfil);
            this.groupBoxDatosUsuario.Controls.Add(this.lblPerfil);
            this.groupBoxDatosUsuario.Controls.Add(this.lblContraseña);
            this.groupBoxDatosUsuario.Controls.Add(this.lblEmail);
            this.groupBoxDatosUsuario.Controls.Add(this.txtContraseña);
            this.groupBoxDatosUsuario.Controls.Add(this.txtEmail);
            this.groupBoxDatosUsuario.Location = new System.Drawing.Point(9, 63);
            this.groupBoxDatosUsuario.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxDatosUsuario.Name = "groupBoxDatosUsuario";
            this.groupBoxDatosUsuario.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxDatosUsuario.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBoxDatosUsuario.Size = new System.Drawing.Size(582, 326);
            this.groupBoxDatosUsuario.TabIndex = 11;
            this.groupBoxDatosUsuario.TabStop = false;
            this.groupBoxDatosUsuario.Text = "Datos del usuario";
            // 
            // dgvUsuarios
            // 
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsuarios.Location = new System.Drawing.Point(0, 153);
            this.dgvUsuarios.Margin = new System.Windows.Forms.Padding(2);
            this.dgvUsuarios.MultiSelect = false;
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.RowHeadersWidth = 51;
            this.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Size = new System.Drawing.Size(582, 157);
            this.dgvUsuarios.TabIndex = 14;
            this.dgvUsuarios.TabStop = false;
            // 
            // groupBoxAcciones
            // 
            this.groupBoxAcciones.Controls.Add(this.btnVolver);
            this.groupBoxAcciones.Controls.Add(this.btnGuardar);
            this.groupBoxAcciones.Controls.Add(this.btnModificar);
            this.groupBoxAcciones.Controls.Add(this.btnEliminar);
            this.groupBoxAcciones.Controls.Add(this.btnNuevo);
            this.groupBoxAcciones.Location = new System.Drawing.Point(9, 393);
            this.groupBoxAcciones.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxAcciones.Name = "groupBoxAcciones";
            this.groupBoxAcciones.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxAcciones.Size = new System.Drawing.Size(582, 73);
            this.groupBoxAcciones.TabIndex = 12;
            this.groupBoxAcciones.TabStop = false;
            this.groupBoxAcciones.Text = "Acciones";
            // 
            // btnVolver
            // 
            this.btnVolver.Location = new System.Drawing.Point(435, 28);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(2);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(66, 19);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(164, 28);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(66, 19);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnModificar
            // 
            this.btnModificar.Location = new System.Drawing.Point(344, 28);
            this.btnModificar.Margin = new System.Windows.Forms.Padding(2);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(66, 19);
            this.btnModificar.TabIndex = 2;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(254, 28);
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(2);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(66, 19);
            this.btnEliminar.TabIndex = 1;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnNuevo
            // 
            this.btnNuevo.Location = new System.Drawing.Point(79, 28);
            this.btnNuevo.Margin = new System.Windows.Forms.Padding(2);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(66, 19);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(390, 20);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(2);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(56, 21);
            this.btnBuscar.TabIndex = 13;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            // 
            // gestionUsuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 462);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.groupBoxAcciones);
            this.Controls.Add(this.groupBoxDatosUsuario);
            this.Controls.Add(this.txtBuscarPorUsuario);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "gestionUsuarios";
            this.Text = "Gestión de Usuarios";
            this.groupBoxDatosUsuario.ResumeLayout(false);
            this.groupBoxDatosUsuario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.groupBoxAcciones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBuscarPorUsuario;
        private System.Windows.Forms.TextBox txtNombreUsuario;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtContraseña;
        private System.Windows.Forms.ComboBox comboBoxPerfil;
        private System.Windows.Forms.Label lblNombreUsuario;
        private System.Windows.Forms.Label lblPerfil;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblContraseña;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.GroupBox groupBoxDatosUsuario;
        private System.Windows.Forms.GroupBox groupBoxAcciones;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.Button btnBuscar;
    }
}