namespace UI.WinUi.Negocio
{
    partial class FormReportes
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCitasSemana = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCanceladas = new System.Windows.Forms.Label();
            this.lblCompletadas = new System.Windows.Forms.Label();
            this.lblConfirmadas = new System.Windows.Forms.Label();
            this.lblAgendadas = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnExportar = new System.Windows.Forms.Button();
            this.dgvCitasSemana = new System.Windows.Forms.DataGridView();
            this.grpFiltros = new System.Windows.Forms.GroupBox();
            this.btnMesActual = new System.Windows.Forms.Button();
            this.btnSemanaActual = new System.Windows.Forms.Button();
            this.btnCargar = new System.Windows.Forms.Button();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbVeterinario = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabCitasSemana.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCitasSemana)).BeginInit();
            this.grpFiltros.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            //
            // tabControl1
            //
            this.tabControl1.Controls.Add(this.tabCitasSemana);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl1.Location = new System.Drawing.Point(0, 60);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1200, 640);
            this.tabControl1.TabIndex = 0;
            //
            // tabCitasSemana
            //
            this.tabCitasSemana.BackColor = System.Drawing.Color.White;
            this.tabCitasSemana.Controls.Add(this.panel1);
            this.tabCitasSemana.Controls.Add(this.dgvCitasSemana);
            this.tabCitasSemana.Controls.Add(this.grpFiltros);
            this.tabCitasSemana.Location = new System.Drawing.Point(4, 26);
            this.tabCitasSemana.Name = "tabCitasSemana";
            this.tabCitasSemana.Padding = new System.Windows.Forms.Padding(3);
            this.tabCitasSemana.Size = new System.Drawing.Size(1192, 610);
            this.tabCitasSemana.TabIndex = 0;
            this.tabCitasSemana.Text = "Citas por Período";
            //
            // panel1
            //
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblCanceladas);
            this.panel1.Controls.Add(this.lblCompletadas);
            this.panel1.Controls.Add(this.lblConfirmadas);
            this.panel1.Controls.Add(this.lblAgendadas);
            this.panel1.Controls.Add(this.lblTotal);
            this.panel1.Controls.Add(this.btnExportar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 545);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1186, 62);
            this.panel1.TabIndex = 2;
            //
            // lblCanceladas
            //
            this.lblCanceladas.AutoSize = true;
            this.lblCanceladas.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCanceladas.Location = new System.Drawing.Point(600, 30);
            this.lblCanceladas.Name = "lblCanceladas";
            this.lblCanceladas.Size = new System.Drawing.Size(73, 15);
            this.lblCanceladas.TabIndex = 5;
            this.lblCanceladas.Text = "Canceladas: 0";
            //
            // lblCompletadas
            //
            this.lblCompletadas.AutoSize = true;
            this.lblCompletadas.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCompletadas.Location = new System.Drawing.Point(410, 30);
            this.lblCompletadas.Name = "lblCompletadas";
            this.lblCompletadas.Size = new System.Drawing.Size(83, 15);
            this.lblCompletadas.TabIndex = 4;
            this.lblCompletadas.Text = "Completadas: 0";
            //
            // lblConfirmadas
            //
            this.lblConfirmadas.AutoSize = true;
            this.lblConfirmadas.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblConfirmadas.Location = new System.Drawing.Point(220, 30);
            this.lblConfirmadas.Name = "lblConfirmadas";
            this.lblConfirmadas.Size = new System.Drawing.Size(82, 15);
            this.lblConfirmadas.TabIndex = 3;
            this.lblConfirmadas.Text = "Confirmadas: 0";
            //
            // lblAgendadas
            //
            this.lblAgendadas.AutoSize = true;
            this.lblAgendadas.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAgendadas.Location = new System.Drawing.Point(30, 30);
            this.lblAgendadas.Name = "lblAgendadas";
            this.lblAgendadas.Size = new System.Drawing.Size(73, 15);
            this.lblAgendadas.TabIndex = 2;
            this.lblAgendadas.Text = "Agendadas: 0";
            //
            // lblTotal
            //
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(30, 8);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(101, 17);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Total de citas: 0";
            //
            // btnExportar
            //
            this.btnExportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportar.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnExportar.ForeColor = System.Drawing.Color.White;
            this.btnExportar.Location = new System.Drawing.Point(1040, 14);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(130, 35);
            this.btnExportar.TabIndex = 0;
            this.btnExportar.Text = "Exportar a CSV";
            this.btnExportar.UseVisualStyleBackColor = false;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            //
            // dgvCitasSemana
            //
            this.dgvCitasSemana.AllowUserToAddRows = false;
            this.dgvCitasSemana.AllowUserToDeleteRows = false;
            this.dgvCitasSemana.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCitasSemana.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCitasSemana.BackgroundColor = System.Drawing.Color.White;
            this.dgvCitasSemana.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvCitasSemana.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCitasSemana.Location = new System.Drawing.Point(15, 120);
            this.dgvCitasSemana.MultiSelect = false;
            this.dgvCitasSemana.Name = "dgvCitasSemana";
            this.dgvCitasSemana.ReadOnly = true;
            this.dgvCitasSemana.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCitasSemana.Size = new System.Drawing.Size(1162, 419);
            this.dgvCitasSemana.TabIndex = 1;
            //
            // grpFiltros
            //
            this.grpFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFiltros.Controls.Add(this.label3);
            this.grpFiltros.Controls.Add(this.cmbVeterinario);
            this.grpFiltros.Controls.Add(this.btnMesActual);
            this.grpFiltros.Controls.Add(this.btnSemanaActual);
            this.grpFiltros.Controls.Add(this.btnCargar);
            this.grpFiltros.Controls.Add(this.dtpHasta);
            this.grpFiltros.Controls.Add(this.dtpDesde);
            this.grpFiltros.Controls.Add(this.label2);
            this.grpFiltros.Controls.Add(this.label1);
            this.grpFiltros.Location = new System.Drawing.Point(15, 15);
            this.grpFiltros.Name = "grpFiltros";
            this.grpFiltros.Size = new System.Drawing.Size(1162, 90);
            this.grpFiltros.TabIndex = 0;
            this.grpFiltros.TabStop = false;
            this.grpFiltros.Text = "Filtros de Búsqueda";
            //
            // btnMesActual
            //
            this.btnMesActual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnMesActual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMesActual.ForeColor = System.Drawing.Color.White;
            this.btnMesActual.Location = new System.Drawing.Point(1030, 30);
            this.btnMesActual.Name = "btnMesActual";
            this.btnMesActual.Size = new System.Drawing.Size(110, 40);
            this.btnMesActual.TabIndex = 6;
            this.btnMesActual.Text = "Mes Actual";
            this.btnMesActual.UseVisualStyleBackColor = false;
            this.btnMesActual.Click += new System.EventHandler(this.btnMesActual_Click);
            //
            // btnSemanaActual
            //
            this.btnSemanaActual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnSemanaActual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSemanaActual.ForeColor = System.Drawing.Color.White;
            this.btnSemanaActual.Location = new System.Drawing.Point(900, 30);
            this.btnSemanaActual.Name = "btnSemanaActual";
            this.btnSemanaActual.Size = new System.Drawing.Size(110, 40);
            this.btnSemanaActual.TabIndex = 5;
            this.btnSemanaActual.Text = "Semana Actual";
            this.btnSemanaActual.UseVisualStyleBackColor = false;
            this.btnSemanaActual.Click += new System.EventHandler(this.btnSemanaActual_Click);
            //
            // btnCargar
            //
            this.btnCargar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(142)))), ((int)(((byte)(60)))));
            this.btnCargar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCargar.ForeColor = System.Drawing.Color.White;
            this.btnCargar.Location = new System.Drawing.Point(770, 30);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(110, 40);
            this.btnCargar.TabIndex = 4;
            this.btnCargar.Text = "Cargar";
            this.btnCargar.UseVisualStyleBackColor = false;
            this.btnCargar.Click += new System.EventHandler(this.btnCargar_Click);
            //
            // dtpHasta
            //
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(330, 40);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(150, 25);
            this.dtpHasta.TabIndex = 3;
            //
            // dtpDesde
            //
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(100, 40);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(150, 25);
            this.dtpDesde.TabIndex = 2;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Hasta:";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Desde:";
            //
            // cmbVeterinario
            //
            this.cmbVeterinario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVeterinario.FormattingEnabled = true;
            this.cmbVeterinario.Location = new System.Drawing.Point(600, 40);
            this.cmbVeterinario.Name = "cmbVeterinario";
            this.cmbVeterinario.Size = new System.Drawing.Size(150, 25);
            this.cmbVeterinario.TabIndex = 7;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(510, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Veterinario:";
            //
            // panelTop
            //
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(142)))), ((int)(((byte)(60)))));
            this.panelTop.Controls.Add(this.lblTitulo);
            this.panelTop.Controls.Add(this.btnCerrar);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 60);
            this.panelTop.TabIndex = 1;
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 14);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(115, 32);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Reportes";
            //
            // btnCerrar
            //
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(1100, 14);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(80, 35);
            this.btnCerrar.TabIndex = 0;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // FormReportes
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormReportes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reportes - Sistema VetCare";
            this.Load += new System.EventHandler(this.FormReportes_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabCitasSemana.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCitasSemana)).EndInit();
            this.grpFiltros.ResumeLayout(false);
            this.grpFiltros.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCitasSemana;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.DataGridView dgvCitasSemana;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnCargar;
        private System.Windows.Forms.Button btnMesActual;
        private System.Windows.Forms.Button btnSemanaActual;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblAgendadas;
        private System.Windows.Forms.Label lblConfirmadas;
        private System.Windows.Forms.Label lblCompletadas;
        private System.Windows.Forms.Label lblCanceladas;
        private System.Windows.Forms.ComboBox cmbVeterinario;
        private System.Windows.Forms.Label label3;
    }
}
