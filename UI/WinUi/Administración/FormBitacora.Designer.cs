namespace UI.WinUi.Administrador
{
    partial class FormBitacora
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
            this.groupBoxFiltros = new System.Windows.Forms.GroupBox();
            this.chkFechaHasta = new System.Windows.Forms.CheckBox();
            this.chkFechaDesde = new System.Windows.Forms.CheckBox();
            this.cmbCriticidad = new System.Windows.Forms.ComboBox();
            this.lblCriticidad = new System.Windows.Forms.Label();
            this.cmbAccion = new System.Windows.Forms.ComboBox();
            this.lblAccion = new System.Windows.Forms.Label();
            this.cmbModulo = new System.Windows.Forms.ComboBox();
            this.lblModulo = new System.Windows.Forms.Label();
            this.dtpFechaHasta = new System.Windows.Forms.DateTimePicker();
            this.lblFechaHasta = new System.Windows.Forms.Label();
            this.dtpFechaDesde = new System.Windows.Forms.DateTimePicker();
            this.lblFechaDesde = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.groupBoxRegistros = new System.Windows.Forms.GroupBox();
            this.dgvBitacora = new System.Windows.Forms.DataGridView();
            this.lblTotalRegistros = new System.Windows.Forms.Label();
            this.btnExportar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.groupBoxFiltros.SuspendLayout();
            this.groupBoxRegistros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).BeginInit();
            this.SuspendLayout();
            //
            // groupBoxFiltros
            //
            this.groupBoxFiltros.Controls.Add(this.btnLimpiar);
            this.groupBoxFiltros.Controls.Add(this.btnBuscar);
            this.groupBoxFiltros.Controls.Add(this.chkFechaHasta);
            this.groupBoxFiltros.Controls.Add(this.chkFechaDesde);
            this.groupBoxFiltros.Controls.Add(this.cmbCriticidad);
            this.groupBoxFiltros.Controls.Add(this.lblCriticidad);
            this.groupBoxFiltros.Controls.Add(this.cmbAccion);
            this.groupBoxFiltros.Controls.Add(this.lblAccion);
            this.groupBoxFiltros.Controls.Add(this.cmbModulo);
            this.groupBoxFiltros.Controls.Add(this.lblModulo);
            this.groupBoxFiltros.Controls.Add(this.dtpFechaHasta);
            this.groupBoxFiltros.Controls.Add(this.lblFechaHasta);
            this.groupBoxFiltros.Controls.Add(this.dtpFechaDesde);
            this.groupBoxFiltros.Controls.Add(this.lblFechaDesde);
            this.groupBoxFiltros.Location = new System.Drawing.Point(12, 12);
            this.groupBoxFiltros.Name = "groupBoxFiltros";
            this.groupBoxFiltros.Size = new System.Drawing.Size(1060, 120);
            this.groupBoxFiltros.TabIndex = 0;
            this.groupBoxFiltros.TabStop = false;
            this.groupBoxFiltros.Text = "Filtros";
            //
            // chkFechaHasta
            //
            this.chkFechaHasta.AutoSize = true;
            this.chkFechaHasta.Checked = true;
            this.chkFechaHasta.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFechaHasta.Location = new System.Drawing.Point(288, 57);
            this.chkFechaHasta.Name = "chkFechaHasta";
            this.chkFechaHasta.Size = new System.Drawing.Size(15, 14);
            this.chkFechaHasta.TabIndex = 13;
            this.chkFechaHasta.UseVisualStyleBackColor = true;
            //
            // chkFechaDesde
            //
            this.chkFechaDesde.AutoSize = true;
            this.chkFechaDesde.Checked = true;
            this.chkFechaDesde.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFechaDesde.Location = new System.Drawing.Point(288, 27);
            this.chkFechaDesde.Name = "chkFechaDesde";
            this.chkFechaDesde.Size = new System.Drawing.Size(15, 14);
            this.chkFechaDesde.TabIndex = 12;
            this.chkFechaDesde.UseVisualStyleBackColor = true;
            //
            // cmbCriticidad
            //
            this.cmbCriticidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCriticidad.FormattingEnabled = true;
            this.cmbCriticidad.Location = new System.Drawing.Point(652, 54);
            this.cmbCriticidad.Name = "cmbCriticidad";
            this.cmbCriticidad.Size = new System.Drawing.Size(180, 21);
            this.cmbCriticidad.TabIndex = 11;
            //
            // lblCriticidad
            //
            this.lblCriticidad.AutoSize = true;
            this.lblCriticidad.Location = new System.Drawing.Point(565, 57);
            this.lblCriticidad.Name = "lblCriticidad";
            this.lblCriticidad.Size = new System.Drawing.Size(58, 13);
            this.lblCriticidad.TabIndex = 10;
            this.lblCriticidad.Text = "Criticidad:";
            //
            // cmbAccion
            //
            this.cmbAccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccion.FormattingEnabled = true;
            this.cmbAccion.Location = new System.Drawing.Point(652, 24);
            this.cmbAccion.Name = "cmbAccion";
            this.cmbAccion.Size = new System.Drawing.Size(180, 21);
            this.cmbAccion.TabIndex = 9;
            //
            // lblAccion
            //
            this.lblAccion.AutoSize = true;
            this.lblAccion.Location = new System.Drawing.Point(565, 27);
            this.lblAccion.Name = "lblAccion";
            this.lblAccion.Size = new System.Drawing.Size(46, 13);
            this.lblAccion.TabIndex = 8;
            this.lblAccion.Text = "Acción:";
            //
            // cmbModulo
            //
            this.cmbModulo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModulo.FormattingEnabled = true;
            this.cmbModulo.Location = new System.Drawing.Point(377, 54);
            this.cmbModulo.Name = "cmbModulo";
            this.cmbModulo.Size = new System.Drawing.Size(180, 21);
            this.cmbModulo.TabIndex = 7;
            //
            // lblModulo
            //
            this.lblModulo.AutoSize = true;
            this.lblModulo.Location = new System.Drawing.Point(310, 57);
            this.lblModulo.Name = "lblModulo";
            this.lblModulo.Size = new System.Drawing.Size(48, 13);
            this.lblModulo.TabIndex = 6;
            this.lblModulo.Text = "Módulo:";
            //
            // dtpFechaHasta
            //
            this.dtpFechaHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaHasta.Location = new System.Drawing.Point(103, 55);
            this.dtpFechaHasta.Name = "dtpFechaHasta";
            this.dtpFechaHasta.Size = new System.Drawing.Size(180, 20);
            this.dtpFechaHasta.TabIndex = 5;
            //
            // lblFechaHasta
            //
            this.lblFechaHasta.AutoSize = true;
            this.lblFechaHasta.Location = new System.Drawing.Point(16, 57);
            this.lblFechaHasta.Name = "lblFechaHasta";
            this.lblFechaHasta.Size = new System.Drawing.Size(71, 13);
            this.lblFechaHasta.TabIndex = 4;
            this.lblFechaHasta.Text = "Fecha Hasta:";
            //
            // dtpFechaDesde
            //
            this.dtpFechaDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaDesde.Location = new System.Drawing.Point(103, 25);
            this.dtpFechaDesde.Name = "dtpFechaDesde";
            this.dtpFechaDesde.Size = new System.Drawing.Size(180, 20);
            this.dtpFechaDesde.TabIndex = 3;
            //
            // lblFechaDesde
            //
            this.lblFechaDesde.AutoSize = true;
            this.lblFechaDesde.Location = new System.Drawing.Point(16, 27);
            this.lblFechaDesde.Name = "lblFechaDesde";
            this.lblFechaDesde.Size = new System.Drawing.Size(72, 13);
            this.lblFechaDesde.TabIndex = 2;
            this.lblFechaDesde.Text = "Fecha Desde:";
            //
            // btnBuscar
            //
            this.btnBuscar.Location = new System.Drawing.Point(860, 22);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(90, 25);
            this.btnBuscar.TabIndex = 14;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            //
            // btnLimpiar
            //
            this.btnLimpiar.Location = new System.Drawing.Point(860, 52);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(90, 25);
            this.btnLimpiar.TabIndex = 15;
            this.btnLimpiar.Text = "Limpiar Filtros";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            //
            // groupBoxRegistros
            //
            this.groupBoxRegistros.Controls.Add(this.lblTotalRegistros);
            this.groupBoxRegistros.Controls.Add(this.dgvBitacora);
            this.groupBoxRegistros.Location = new System.Drawing.Point(12, 138);
            this.groupBoxRegistros.Name = "groupBoxRegistros";
            this.groupBoxRegistros.Size = new System.Drawing.Size(1060, 400);
            this.groupBoxRegistros.TabIndex = 1;
            this.groupBoxRegistros.TabStop = false;
            this.groupBoxRegistros.Text = "Registros de Auditoría";
            //
            // dgvBitacora
            //
            this.dgvBitacora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBitacora.Location = new System.Drawing.Point(6, 19);
            this.dgvBitacora.Name = "dgvBitacora";
            this.dgvBitacora.Size = new System.Drawing.Size(1048, 345);
            this.dgvBitacora.TabIndex = 0;
            //
            // lblTotalRegistros
            //
            this.lblTotalRegistros.AutoSize = true;
            this.lblTotalRegistros.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalRegistros.Location = new System.Drawing.Point(6, 370);
            this.lblTotalRegistros.Name = "lblTotalRegistros";
            this.lblTotalRegistros.Size = new System.Drawing.Size(139, 15);
            this.lblTotalRegistros.TabIndex = 1;
            this.lblTotalRegistros.Text = "Total de registros: 0";
            //
            // btnExportar
            //
            this.btnExportar.Location = new System.Drawing.Point(12, 544);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(100, 30);
            this.btnExportar.TabIndex = 2;
            this.btnExportar.Text = "Exportar a Excel";
            this.btnExportar.UseVisualStyleBackColor = true;
            //
            // btnActualizar
            //
            this.btnActualizar.Location = new System.Drawing.Point(118, 544);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(100, 30);
            this.btnActualizar.TabIndex = 3;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            //
            // btnVolver
            //
            this.btnVolver.Location = new System.Drawing.Point(972, 544);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(100, 30);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            //
            // FormBitacora
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 586);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.groupBoxRegistros);
            this.Controls.Add(this.groupBoxFiltros);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormBitacora";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bitácora del Sistema";
            this.groupBoxFiltros.ResumeLayout(false);
            this.groupBoxFiltros.PerformLayout();
            this.groupBoxRegistros.ResumeLayout(false);
            this.groupBoxRegistros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxFiltros;
        private System.Windows.Forms.Label lblFechaDesde;
        private System.Windows.Forms.DateTimePicker dtpFechaDesde;
        private System.Windows.Forms.DateTimePicker dtpFechaHasta;
        private System.Windows.Forms.Label lblFechaHasta;
        private System.Windows.Forms.ComboBox cmbModulo;
        private System.Windows.Forms.Label lblModulo;
        private System.Windows.Forms.ComboBox cmbAccion;
        private System.Windows.Forms.Label lblAccion;
        private System.Windows.Forms.ComboBox cmbCriticidad;
        private System.Windows.Forms.Label lblCriticidad;
        private System.Windows.Forms.CheckBox chkFechaDesde;
        private System.Windows.Forms.CheckBox chkFechaHasta;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.GroupBox groupBoxRegistros;
        private System.Windows.Forms.DataGridView dgvBitacora;
        private System.Windows.Forms.Label lblTotalRegistros;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnVolver;
    }
}
