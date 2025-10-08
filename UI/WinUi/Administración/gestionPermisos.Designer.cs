namespace UI.WinUi.Administrador
{
    partial class gestionPermisos
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGestionRoles = new System.Windows.Forms.TabPage();
            this.groupBoxRol = new System.Windows.Forms.GroupBox();
            this.cboRoles = new System.Windows.Forms.ComboBox();
            this.lblRol = new System.Windows.Forms.Label();
            this.groupBoxPatentesRol = new System.Windows.Forms.GroupBox();
            this.checkedListPatentesRol = new System.Windows.Forms.CheckedListBox();
            this.btnGuardarRol = new System.Windows.Forms.Button();
            this.tabGestionUsuarios = new System.Windows.Forms.TabPage();
            this.groupBoxUsuario = new System.Windows.Forms.GroupBox();
            this.cboUsuarios = new System.Windows.Forms.ComboBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.groupBoxRolUsuario = new System.Windows.Forms.GroupBox();
            this.cboNuevoRol = new System.Windows.Forms.ComboBox();
            this.lblNuevoRol = new System.Windows.Forms.Label();
            this.btnAsignarRolUsuario = new System.Windows.Forms.Button();
            this.lblRolActualValor = new System.Windows.Forms.Label();
            this.lblRolActual = new System.Windows.Forms.Label();
            this.groupBoxPatentesUsuario = new System.Windows.Forms.GroupBox();
            this.checkedListPatentesUsuario = new System.Windows.Forms.CheckedListBox();
            this.btnGuardarPermisosUsuario = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabGestionRoles.SuspendLayout();
            this.groupBoxRol.SuspendLayout();
            this.groupBoxPatentesRol.SuspendLayout();
            this.tabGestionUsuarios.SuspendLayout();
            this.groupBoxUsuario.SuspendLayout();
            this.groupBoxRolUsuario.SuspendLayout();
            this.groupBoxPatentesUsuario.SuspendLayout();
            this.SuspendLayout();
            //
            // tabControl
            //
            this.tabControl.Controls.Add(this.tabGestionRoles);
            this.tabControl.Controls.Add(this.tabGestionUsuarios);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(784, 561);
            this.tabControl.TabIndex = 0;
            //
            // tabGestionRoles
            //
            this.tabGestionRoles.Controls.Add(this.btnGuardarRol);
            this.tabGestionRoles.Controls.Add(this.groupBoxPatentesRol);
            this.tabGestionRoles.Controls.Add(this.groupBoxRol);
            this.tabGestionRoles.Location = new System.Drawing.Point(4, 22);
            this.tabGestionRoles.Name = "tabGestionRoles";
            this.tabGestionRoles.Padding = new System.Windows.Forms.Padding(3);
            this.tabGestionRoles.Size = new System.Drawing.Size(776, 535);
            this.tabGestionRoles.TabIndex = 0;
            this.tabGestionRoles.Text = "Gestión de Roles";
            this.tabGestionRoles.UseVisualStyleBackColor = true;
            //
            // groupBoxRol
            //
            this.groupBoxRol.Controls.Add(this.cboRoles);
            this.groupBoxRol.Controls.Add(this.lblRol);
            this.groupBoxRol.Location = new System.Drawing.Point(20, 20);
            this.groupBoxRol.Name = "groupBoxRol";
            this.groupBoxRol.Size = new System.Drawing.Size(730, 80);
            this.groupBoxRol.TabIndex = 0;
            this.groupBoxRol.TabStop = false;
            this.groupBoxRol.Text = "Seleccionar Rol";
            //
            // cboRoles
            //
            this.cboRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoles.FormattingEnabled = true;
            this.cboRoles.Location = new System.Drawing.Point(90, 35);
            this.cboRoles.Name = "cboRoles";
            this.cboRoles.Size = new System.Drawing.Size(600, 21);
            this.cboRoles.TabIndex = 1;
            //
            // lblRol
            //
            this.lblRol.AutoSize = true;
            this.lblRol.Location = new System.Drawing.Point(20, 38);
            this.lblRol.Name = "lblRol";
            this.lblRol.Size = new System.Drawing.Size(26, 13);
            this.lblRol.TabIndex = 0;
            this.lblRol.Text = "Rol:";
            //
            // groupBoxPatentesRol
            //
            this.groupBoxPatentesRol.Controls.Add(this.checkedListPatentesRol);
            this.groupBoxPatentesRol.Location = new System.Drawing.Point(20, 115);
            this.groupBoxPatentesRol.Name = "groupBoxPatentesRol";
            this.groupBoxPatentesRol.Size = new System.Drawing.Size(730, 350);
            this.groupBoxPatentesRol.TabIndex = 1;
            this.groupBoxPatentesRol.TabStop = false;
            this.groupBoxPatentesRol.Text = "Permisos del Rol";
            //
            // checkedListPatentesRol
            //
            this.checkedListPatentesRol.CheckOnClick = true;
            this.checkedListPatentesRol.FormattingEnabled = true;
            this.checkedListPatentesRol.Location = new System.Drawing.Point(20, 30);
            this.checkedListPatentesRol.Name = "checkedListPatentesRol";
            this.checkedListPatentesRol.Size = new System.Drawing.Size(690, 304);
            this.checkedListPatentesRol.TabIndex = 0;
            //
            // btnGuardarRol
            //
            this.btnGuardarRol.Location = new System.Drawing.Point(320, 480);
            this.btnGuardarRol.Name = "btnGuardarRol";
            this.btnGuardarRol.Size = new System.Drawing.Size(150, 35);
            this.btnGuardarRol.TabIndex = 2;
            this.btnGuardarRol.Text = "Guardar Cambios";
            this.btnGuardarRol.UseVisualStyleBackColor = true;
            //
            // tabGestionUsuarios
            //
            this.tabGestionUsuarios.Controls.Add(this.btnGuardarPermisosUsuario);
            this.tabGestionUsuarios.Controls.Add(this.groupBoxPatentesUsuario);
            this.tabGestionUsuarios.Controls.Add(this.groupBoxRolUsuario);
            this.tabGestionUsuarios.Controls.Add(this.groupBoxUsuario);
            this.tabGestionUsuarios.Location = new System.Drawing.Point(4, 22);
            this.tabGestionUsuarios.Name = "tabGestionUsuarios";
            this.tabGestionUsuarios.Padding = new System.Windows.Forms.Padding(3);
            this.tabGestionUsuarios.Size = new System.Drawing.Size(776, 535);
            this.tabGestionUsuarios.TabIndex = 1;
            this.tabGestionUsuarios.Text = "Gestión de Usuarios";
            this.tabGestionUsuarios.UseVisualStyleBackColor = true;
            //
            // groupBoxUsuario
            //
            this.groupBoxUsuario.Controls.Add(this.cboUsuarios);
            this.groupBoxUsuario.Controls.Add(this.lblUsuario);
            this.groupBoxUsuario.Location = new System.Drawing.Point(20, 20);
            this.groupBoxUsuario.Name = "groupBoxUsuario";
            this.groupBoxUsuario.Size = new System.Drawing.Size(730, 80);
            this.groupBoxUsuario.TabIndex = 0;
            this.groupBoxUsuario.TabStop = false;
            this.groupBoxUsuario.Text = "Seleccionar Usuario";
            //
            // cboUsuarios
            //
            this.cboUsuarios.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsuarios.FormattingEnabled = true;
            this.cboUsuarios.Location = new System.Drawing.Point(90, 35);
            this.cboUsuarios.Name = "cboUsuarios";
            this.cboUsuarios.Size = new System.Drawing.Size(600, 21);
            this.cboUsuarios.TabIndex = 1;
            //
            // lblUsuario
            //
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(20, 38);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuario:";
            //
            // groupBoxRolUsuario
            //
            this.groupBoxRolUsuario.Controls.Add(this.lblRolActual);
            this.groupBoxRolUsuario.Controls.Add(this.lblRolActualValor);
            this.groupBoxRolUsuario.Controls.Add(this.btnAsignarRolUsuario);
            this.groupBoxRolUsuario.Controls.Add(this.lblNuevoRol);
            this.groupBoxRolUsuario.Controls.Add(this.cboNuevoRol);
            this.groupBoxRolUsuario.Location = new System.Drawing.Point(20, 115);
            this.groupBoxRolUsuario.Name = "groupBoxRolUsuario";
            this.groupBoxRolUsuario.Size = new System.Drawing.Size(730, 120);
            this.groupBoxRolUsuario.TabIndex = 1;
            this.groupBoxRolUsuario.TabStop = false;
            this.groupBoxRolUsuario.Text = "Rol Asignado";
            //
            // lblRolActual
            //
            this.lblRolActual.AutoSize = true;
            this.lblRolActual.Location = new System.Drawing.Point(20, 30);
            this.lblRolActual.Name = "lblRolActual";
            this.lblRolActual.Size = new System.Drawing.Size(61, 13);
            this.lblRolActual.TabIndex = 0;
            this.lblRolActual.Text = "Rol Actual:";
            //
            // lblRolActualValor
            //
            this.lblRolActualValor.AutoSize = true;
            this.lblRolActualValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRolActualValor.Location = new System.Drawing.Point(110, 30);
            this.lblRolActualValor.Name = "lblRolActualValor";
            this.lblRolActualValor.Size = new System.Drawing.Size(46, 13);
            this.lblRolActualValor.TabIndex = 1;
            this.lblRolActualValor.Text = "Sin rol";
            //
            // lblNuevoRol
            //
            this.lblNuevoRol.AutoSize = true;
            this.lblNuevoRol.Location = new System.Drawing.Point(20, 68);
            this.lblNuevoRol.Name = "lblNuevoRol";
            this.lblNuevoRol.Size = new System.Drawing.Size(64, 13);
            this.lblNuevoRol.TabIndex = 2;
            this.lblNuevoRol.Text = "Nuevo Rol:";
            //
            // cboNuevoRol
            //
            this.cboNuevoRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNuevoRol.FormattingEnabled = true;
            this.cboNuevoRol.Location = new System.Drawing.Point(110, 65);
            this.cboNuevoRol.Name = "cboNuevoRol";
            this.cboNuevoRol.Size = new System.Drawing.Size(400, 21);
            this.cboNuevoRol.TabIndex = 3;
            //
            // btnAsignarRolUsuario
            //
            this.btnAsignarRolUsuario.Location = new System.Drawing.Point(530, 60);
            this.btnAsignarRolUsuario.Name = "btnAsignarRolUsuario";
            this.btnAsignarRolUsuario.Size = new System.Drawing.Size(160, 30);
            this.btnAsignarRolUsuario.TabIndex = 4;
            this.btnAsignarRolUsuario.Text = "Asignar Rol";
            this.btnAsignarRolUsuario.UseVisualStyleBackColor = true;
            //
            // groupBoxPatentesUsuario
            //
            this.groupBoxPatentesUsuario.Controls.Add(this.checkedListPatentesUsuario);
            this.groupBoxPatentesUsuario.Location = new System.Drawing.Point(20, 250);
            this.groupBoxPatentesUsuario.Name = "groupBoxPatentesUsuario";
            this.groupBoxPatentesUsuario.Size = new System.Drawing.Size(730, 220);
            this.groupBoxPatentesUsuario.TabIndex = 2;
            this.groupBoxPatentesUsuario.TabStop = false;
            this.groupBoxPatentesUsuario.Text = "Permisos Adicionales (independientes del rol)";
            //
            // checkedListPatentesUsuario
            //
            this.checkedListPatentesUsuario.CheckOnClick = true;
            this.checkedListPatentesUsuario.FormattingEnabled = true;
            this.checkedListPatentesUsuario.Location = new System.Drawing.Point(20, 30);
            this.checkedListPatentesUsuario.Name = "checkedListPatentesUsuario";
            this.checkedListPatentesUsuario.Size = new System.Drawing.Size(690, 169);
            this.checkedListPatentesUsuario.TabIndex = 0;
            //
            // btnGuardarPermisosUsuario
            //
            this.btnGuardarPermisosUsuario.Location = new System.Drawing.Point(320, 485);
            this.btnGuardarPermisosUsuario.Name = "btnGuardarPermisosUsuario";
            this.btnGuardarPermisosUsuario.Size = new System.Drawing.Size(150, 35);
            this.btnGuardarPermisosUsuario.TabIndex = 3;
            this.btnGuardarPermisosUsuario.Text = "Guardar Cambios";
            this.btnGuardarPermisosUsuario.UseVisualStyleBackColor = true;
            //
            // gestionPermisos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tabControl);
            this.Name = "gestionPermisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Permisos";
            this.tabControl.ResumeLayout(false);
            this.tabGestionRoles.ResumeLayout(false);
            this.groupBoxRol.ResumeLayout(false);
            this.groupBoxRol.PerformLayout();
            this.groupBoxPatentesRol.ResumeLayout(false);
            this.tabGestionUsuarios.ResumeLayout(false);
            this.groupBoxUsuario.ResumeLayout(false);
            this.groupBoxUsuario.PerformLayout();
            this.groupBoxRolUsuario.ResumeLayout(false);
            this.groupBoxRolUsuario.PerformLayout();
            this.groupBoxPatentesUsuario.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGestionRoles;
        private System.Windows.Forms.TabPage tabGestionUsuarios;
        private System.Windows.Forms.GroupBox groupBoxRol;
        private System.Windows.Forms.ComboBox cboRoles;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.GroupBox groupBoxPatentesRol;
        private System.Windows.Forms.CheckedListBox checkedListPatentesRol;
        private System.Windows.Forms.Button btnGuardarRol;
        private System.Windows.Forms.GroupBox groupBoxUsuario;
        private System.Windows.Forms.ComboBox cboUsuarios;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.GroupBox groupBoxRolUsuario;
        private System.Windows.Forms.Label lblRolActual;
        private System.Windows.Forms.Label lblRolActualValor;
        private System.Windows.Forms.Label lblNuevoRol;
        private System.Windows.Forms.ComboBox cboNuevoRol;
        private System.Windows.Forms.Button btnAsignarRolUsuario;
        private System.Windows.Forms.GroupBox groupBoxPatentesUsuario;
        private System.Windows.Forms.CheckedListBox checkedListPatentesUsuario;
        private System.Windows.Forms.Button btnGuardarPermisosUsuario;
    }
}
