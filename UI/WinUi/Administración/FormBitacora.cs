using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.BLL;
using ServicesSecurity.Services;
using BitacoraEntity = ServicesSecurity.DomainModel.Security.Bitacora;
using BitacoraService = ServicesSecurity.Services.Bitacora;

namespace UI.WinUi.Administrador
{
    public partial class FormBitacora : Form
    {
        private readonly Usuario _usuarioLogueado;
        private List<BitacoraEntity> _registros;

        public FormBitacora()
        {
            InitializeComponent();
        }

        public FormBitacora(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Configurar eventos
            this.Load += FormBitacora_Load;
            btnBuscar.Click += BtnBuscar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnExportar.Click += BtnExportar_Click;
            btnVolver.Click += BtnVolver_Click;
            btnActualizar.Click += BtnActualizar_Click;

            // Configurar DataGridView
            ConfigurarDataGridView();

            // Cargar opciones en ComboBoxes
            CargarOpcionesComboBoxes();
        }

        private void ConfigurarDataGridView()
        {
            dgvBitacora.AutoGenerateColumns = false;
            dgvBitacora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBitacora.MultiSelect = false;
            dgvBitacora.ReadOnly = true;
            dgvBitacora.AllowUserToAddRows = false;
            dgvBitacora.AllowUserToDeleteRows = false;

            // Configurar columnas
            dgvBitacora.Columns.Clear();

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FechaHora",
                DataPropertyName = "FechaHora",
                HeaderText = "Fecha y Hora",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm:ss" }
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreUsuario",
                DataPropertyName = "NombreUsuario",
                HeaderText = "Usuario",
                Width = 100
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Modulo",
                DataPropertyName = "Modulo",
                HeaderText = "Módulo",
                Width = 100
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Accion",
                DataPropertyName = "Accion",
                HeaderText = "Acción",
                Width = 120
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Descripcion",
                DataPropertyName = "Descripcion",
                HeaderText = "Descripción",
                Width = 300,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Criticidad",
                DataPropertyName = "Criticidad",
                HeaderText = "Criticidad",
                Width = 90
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tabla",
                DataPropertyName = "Tabla",
                HeaderText = "Tabla",
                Width = 80
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IP",
                DataPropertyName = "IP",
                HeaderText = "IP",
                Width = 100
            });

            // Evento para colorear filas según criticidad
            dgvBitacora.CellFormatting += DgvBitacora_CellFormatting;
        }

        private void DgvBitacora_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvBitacora.Columns[e.ColumnIndex].DataPropertyName == "Criticidad" && e.RowIndex >= 0)
            {
                var criticidad = dgvBitacora.Rows[e.RowIndex].Cells["Criticidad"].Value?.ToString();

                switch (criticidad)
                {
                    case "Critico":
                        dgvBitacora.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                        dgvBitacora.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                        break;
                    case "Error":
                        dgvBitacora.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                        break;
                    case "Advertencia":
                        dgvBitacora.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                        break;
                    case "Info":
                        dgvBitacora.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                        break;
                }
            }
        }

        private void CargarOpcionesComboBoxes()
        {
            // ComboBox Criticidad
            cmbCriticidad.Items.Clear();
            cmbCriticidad.Items.Add("Todos");
            cmbCriticidad.Items.Add(CriticidadBitacora.Info);
            cmbCriticidad.Items.Add(CriticidadBitacora.Advertencia);
            cmbCriticidad.Items.Add(CriticidadBitacora.Error);
            cmbCriticidad.Items.Add(CriticidadBitacora.Critico);
            cmbCriticidad.SelectedIndex = 0;

            // ComboBox Módulo (se cargará dinámicamente basado en datos existentes)
            cmbModulo.Items.Clear();
            cmbModulo.Items.Add("Todos");
            cmbModulo.SelectedIndex = 0;

            // ComboBox Acción
            cmbAccion.Items.Clear();
            cmbAccion.Items.Add("Todos");
            cmbAccion.SelectedIndex = 0;

            // Configurar DateTimePickers
            dtpFechaDesde.Value = DateTime.Now.AddDays(-7); // Última semana por defecto
            dtpFechaHasta.Value = DateTime.Now;
            chkFechaDesde.Checked = true;
            chkFechaHasta.Checked = true;
        }

        private void FormBitacora_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarRegistros();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("bitacora_sistema");
                groupBoxFiltros.Text = LanguageManager.Translate("filtros");
                groupBoxRegistros.Text = LanguageManager.Translate("registros_auditoria");
                lblFechaDesde.Text = LanguageManager.Translate("fecha_desde") + ":";
                lblFechaHasta.Text = LanguageManager.Translate("fecha_hasta") + ":";
                lblModulo.Text = LanguageManager.Translate("modulo") + ":";
                lblAccion.Text = LanguageManager.Translate("accion") + ":";
                lblCriticidad.Text = LanguageManager.Translate("criticidad") + ":";
                btnBuscar.Text = LanguageManager.Translate("buscar");
                btnLimpiar.Text = LanguageManager.Translate("limpiar_filtros");
                btnExportar.Text = LanguageManager.Translate("exportar");
                btnActualizar.Text = LanguageManager.Translate("actualizar");
                btnVolver.Text = LanguageManager.Translate("volver");
            }
            catch (Exception ex)
            {
                // Si falla la traducción, continuar con textos por defecto
                BitacoraService.Current.LogError($"Error al aplicar traducciones en FormBitacora: {ex.Message}");
            }
        }

        private void CargarRegistros()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                DateTime? fechaDesde = chkFechaDesde.Checked ? (DateTime?)dtpFechaDesde.Value : null;
                DateTime? fechaHasta = chkFechaHasta.Checked ? (DateTime?)dtpFechaHasta.Value : null;
                string modulo = cmbModulo.SelectedIndex > 0 ? cmbModulo.SelectedItem.ToString() : null;
                string accion = cmbAccion.SelectedIndex > 0 ? cmbAccion.SelectedItem.ToString() : null;
                string criticidad = cmbCriticidad.SelectedIndex > 0 ? cmbCriticidad.SelectedItem.ToString() : null;

                _registros = BitacoraBLL.ObtenerPorFiltros(
                    fechaDesde,
                    fechaHasta,
                    null, // idUsuario
                    modulo,
                    accion,
                    criticidad,
                    1000 // límite
                ).ToList();

                dgvBitacora.DataSource = null;
                dgvBitacora.DataSource = _registros;

                lblTotalRegistros.Text = $"Total de registros: {_registros.Count}";

                // Actualizar opciones de ComboBoxes basadas en datos cargados
                ActualizarOpcionesDinamicas();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Error al cargar registros de bitácora: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BitacoraService.Current.RegistrarExcepcion(ex, _usuarioLogueado?.IdUsuario, _usuarioLogueado?.Nombre, "Bitacora");
            }
        }

        private void ActualizarOpcionesDinamicas()
        {
            if (_registros == null || _registros.Count == 0)
                return;

            // Actualizar módulos disponibles
            var modulosActuales = cmbModulo.SelectedItem?.ToString();
            cmbModulo.Items.Clear();
            cmbModulo.Items.Add("Todos");
            foreach (var modulo in _registros.Select(r => r.Modulo).Distinct().OrderBy(m => m))
            {
                cmbModulo.Items.Add(modulo);
            }
            if (modulosActuales != null && cmbModulo.Items.Contains(modulosActuales))
                cmbModulo.SelectedItem = modulosActuales;
            else
                cmbModulo.SelectedIndex = 0;

            // Actualizar acciones disponibles
            var accionesActuales = cmbAccion.SelectedItem?.ToString();
            cmbAccion.Items.Clear();
            cmbAccion.Items.Add("Todos");
            foreach (var accion in _registros.Select(r => r.Accion).Distinct().OrderBy(a => a))
            {
                cmbAccion.Items.Add(accion);
            }
            if (accionesActuales != null && cmbAccion.Items.Contains(accionesActuales))
                cmbAccion.SelectedItem = accionesActuales;
            else
                cmbAccion.SelectedIndex = 0;
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            CargarRegistros();
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            chkFechaDesde.Checked = true;
            chkFechaHasta.Checked = true;
            dtpFechaDesde.Value = DateTime.Now.AddDays(-7);
            dtpFechaHasta.Value = DateTime.Now;
            cmbModulo.SelectedIndex = 0;
            cmbAccion.SelectedIndex = 0;
            cmbCriticidad.SelectedIndex = 0;
            CargarRegistros();
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            CargarRegistros();
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_registros == null || _registros.Count == 0)
                {
                    MessageBox.Show("No hay registros para exportar", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Archivo Excel (*.xlsx)|*.xlsx",
                    FileName = $"Bitacora_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Definir columnas a exportar
                    var columnas = new Dictionary<string, Func<ServicesSecurity.DomainModel.Security.Bitacora, string>>
                    {
                        { "Fecha y Hora", r => r.FechaHora.ToString("dd/MM/yyyy HH:mm:ss") },
                        { "Usuario", r => r.NombreUsuario },
                        { "Módulo", r => r.Modulo },
                        { "Acción", r => r.Accion },
                        { "Descripción", r => r.Descripcion },
                        { "Criticidad", r => r.Criticidad },
                        { "Tabla", r => r.Tabla },
                        { "IP", r => r.IP }
                    };

                    // Exportar usando el servicio genérico
                    ExportarAExcel.Current.ExportarACSV(_registros.ToList(), saveFileDialog.FileName, columnas);

                    MessageBox.Show("Registros exportados exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                BitacoraService.Current.RegistrarExcepcion(ex, _usuarioLogueado?.IdUsuario, _usuarioLogueado?.Nombre, "Bitacora");
            }
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
