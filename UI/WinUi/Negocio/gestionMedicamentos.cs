using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class gestionMedicamentos : Form
    {
        private MedicamentoBLL _medicamentoBLL;
        private Medicamento _medicamentoSeleccionado;
        private bool _modoEdicion = false;
        private ServicesSecurity.DomainModel.Security.Composite.Usuario _usuarioLogueado;

        public gestionMedicamentos()
        {
            InitializeComponent();
            _medicamentoBLL = MedicamentoBLL.Current;
        }

        public gestionMedicamentos(ServicesSecurity.DomainModel.Security.Composite.Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
        }

        private void gestionMedicamentos_Load(object sender, EventArgs e)
        {
            ConfigurarDataGridView();
            CargarMedicamentos();
            EstablecerModoConsulta();
        }

        #region Configuración Inicial

        private void ConfigurarDataGridView()
        {
            dgvMedicamentos.AutoGenerateColumns = false;
            dgvMedicamentos.Columns.Clear();

            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = LanguageManager.Translate("medicamento"),
                Name = "Nombre",
                Width = 200
            });

            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Presentacion",
                HeaderText = LanguageManager.Translate("presentacion"),
                Name = "Presentacion",
                Width = 180
            });

            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Stock",
                HeaderText = LanguageManager.Translate("stock"),
                Name = "Stock",
                Width = 80
            });

            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                HeaderText = LanguageManager.Translate("precio_unitario"),
                Name = "PrecioUnitario",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Observaciones",
                HeaderText = LanguageManager.Translate("observaciones"),
                Name = "Observaciones",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Colorear filas según stock
            dgvMedicamentos.RowPrePaint += dgvMedicamentos_RowPrePaint;
        }

        private void dgvMedicamentos_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvMedicamentos.Rows.Count)
            {
                var row = dgvMedicamentos.Rows[e.RowIndex];
                var medicamento = row.DataBoundItem as Medicamento;

                if (medicamento != null)
                {
                    if (medicamento.Stock == 0)
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    }
                    else if (medicamento.Stock < 10)
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }

        #endregion

        #region Carga de Datos

        private void CargarMedicamentos()
        {
            try
            {
                var medicamentos = _medicamentoBLL.ListarTodosLosMedicamentos().ToList();
                dgvMedicamentos.DataSource = medicamentos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_medicamentos")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    CargarMedicamentos();
                }
                else
                {
                    var resultados = _medicamentoBLL.BuscarMedicamentos(txtBuscar.Text).ToList();
                    dgvMedicamentos.DataSource = resultados;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_busqueda")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Selección y Visualización

        private void dgvMedicamentos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMedicamentos.SelectedRows.Count > 0)
            {
                _medicamentoSeleccionado = dgvMedicamentos.SelectedRows[0].DataBoundItem as Medicamento;

                if (_medicamentoSeleccionado != null && !_modoEdicion)
                {
                    MostrarMedicamento(_medicamentoSeleccionado);
                    ActualizarStockActual();
                }
            }
        }

        private void MostrarMedicamento(Medicamento medicamento)
        {
            txtNombre.Text = medicamento.Nombre;
            txtPresentacion.Text = medicamento.Presentacion;
            numStock.Value = medicamento.Stock;
            numPrecio.Value = medicamento.PrecioUnitario;
            txtObservaciones.Text = medicamento.Observaciones;
        }

        private void ActualizarStockActual()
        {
            if (_medicamentoSeleccionado != null)
            {
                lblStockActual.Text = $"{LanguageManager.Translate("stock")}: {_medicamentoSeleccionado.Stock}";

                if (_medicamentoSeleccionado.Stock == 0)
                {
                    lblStockActual.ForeColor = System.Drawing.Color.Red;
                }
                else if (_medicamentoSeleccionado.Stock < 10)
                {
                    lblStockActual.ForeColor = System.Drawing.Color.Orange;
                }
                else
                {
                    lblStockActual.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        #endregion

        #region Modos de Operación

        private void EstablecerModoConsulta()
        {
            _modoEdicion = false;

            // Deshabilitar campos de edición
            txtNombre.Enabled = false;
            txtPresentacion.Enabled = false;
            numStock.Enabled = false;
            numPrecio.Enabled = false;
            txtObservaciones.Enabled = false;

            // Botones
            btnNuevo.Enabled = true;
            btnAgregar.Enabled = false;
            btnModificar.Enabled = dgvMedicamentos.SelectedRows.Count > 0;
            btnEliminar.Enabled = dgvMedicamentos.SelectedRows.Count > 0;
            btnCancelar.Enabled = false;

            // Stock
            grpStock.Enabled = dgvMedicamentos.SelectedRows.Count > 0;
        }

        private void EstablecerModoEdicion(bool esNuevo)
        {
            _modoEdicion = true;

            // Habilitar campos de edición
            txtNombre.Enabled = true;
            txtPresentacion.Enabled = true;
            numStock.Enabled = esNuevo; // Solo al crear nuevo
            numPrecio.Enabled = true;
            txtObservaciones.Enabled = true;

            // Botones
            btnNuevo.Enabled = false;
            btnAgregar.Enabled = true;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = true;

            // Stock
            grpStock.Enabled = false;

            // Focus en nombre
            txtNombre.Focus();
        }

        #endregion

        #region CRUD Operations

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            EstablecerModoEdicion(true);
            _medicamentoSeleccionado = null;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos())
                    return;

                if (_medicamentoSeleccionado == null) // Nuevo medicamento
                {
                    var nuevoMedicamento = new Medicamento
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Presentacion = txtPresentacion.Text.Trim(),
                        Stock = (int)numStock.Value,
                        PrecioUnitario = numPrecio.Value,
                        Observaciones = txtObservaciones.Text.Trim()
                    };

                    _medicamentoBLL.RegistrarMedicamento(nuevoMedicamento);

                    MessageBox.Show(LanguageManager.Translate("medicamento_registrado_exitosamente"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // Modificar existente
                {
                    _medicamentoSeleccionado.Nombre = txtNombre.Text.Trim();
                    _medicamentoSeleccionado.Presentacion = txtPresentacion.Text.Trim();
                    _medicamentoSeleccionado.PrecioUnitario = numPrecio.Value;
                    _medicamentoSeleccionado.Observaciones = txtObservaciones.Text.Trim();

                    _medicamentoBLL.ActualizarMedicamento(_medicamentoSeleccionado);

                    MessageBox.Show(LanguageManager.Translate("medicamento_actualizado_exitosamente"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarMedicamentos();
                EstablecerModoConsulta();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_guardar")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (_medicamentoSeleccionado == null)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_medicamento_modificar"),
                    LanguageManager.Translate("advertencia"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EstablecerModoEdicion(false);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_medicamentoSeleccionado == null)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_medicamento_eliminar"),
                    LanguageManager.Translate("advertencia"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var mensaje = string.Format(LanguageManager.Translate("confirmar_eliminar_medicamento_nombre"),
                _medicamentoSeleccionado.Nombre);
            var resultado = MessageBox.Show(mensaje,
                LanguageManager.Translate("confirmar_eliminacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    _medicamentoBLL.EliminarMedicamento(_medicamentoSeleccionado.IdMedicamento);

                    MessageBox.Show(LanguageManager.Translate("medicamento_eliminado_exitosamente"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarMedicamentos();
                    LimpiarCampos();
                    EstablecerModoConsulta();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{LanguageManager.Translate("error_eliminar")}: {ex.Message}",
                        LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            EstablecerModoConsulta();
            LimpiarCampos();

            if (dgvMedicamentos.SelectedRows.Count > 0)
            {
                MostrarMedicamento(_medicamentoSeleccionado);
            }
        }

        #endregion

        #region Gestión de Stock

        private void btnAumentarStock_Click(object sender, EventArgs e)
        {
            if (_medicamentoSeleccionado == null)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_medicamento"),
                    LanguageManager.Translate("advertencia"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int cantidad = (int)numCantidadStock.Value;
                var medicamentoActualizado = _medicamentoBLL.AumentarStock(
                    _medicamentoSeleccionado.IdMedicamento,
                    cantidad);

                var mensaje = string.Format(LanguageManager.Translate("stock_aumentado_detalle"),
                    cantidad, medicamentoActualizado.Stock);
                MessageBox.Show(mensaje, LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarMedicamentos();
                ActualizarStockActual();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_aumentar_stock")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReducirStock_Click(object sender, EventArgs e)
        {
            if (_medicamentoSeleccionado == null)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_medicamento"),
                    LanguageManager.Translate("advertencia"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int cantidad = (int)numCantidadStock.Value;

                if (_medicamentoSeleccionado.Stock < cantidad)
                {
                    var mensaje = string.Format(LanguageManager.Translate("stock_insuficiente_detalle"),
                        _medicamentoSeleccionado.Stock);
                    MessageBox.Show(mensaje, LanguageManager.Translate("advertencia"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var medicamentoActualizado = _medicamentoBLL.ReducirStock(
                    _medicamentoSeleccionado.IdMedicamento,
                    cantidad);

                var mensajeExito = string.Format(LanguageManager.Translate("stock_reducido_detalle"),
                    cantidad, medicamentoActualizado.Stock);
                MessageBox.Show(mensajeExito, LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarMedicamentos();
                ActualizarStockActual();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_reducir_stock")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Validación y Utilidades

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show(LanguageManager.Translate("nombre_medicamento_obligatorio"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (txtNombre.Text.Trim().Length < 3)
            {
                MessageBox.Show(LanguageManager.Translate("nombre_minimo_caracteres"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (numPrecio.Value < 0)
            {
                MessageBox.Show(LanguageManager.Translate("precio_no_negativo"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numPrecio.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtPresentacion.Clear();
            numStock.Value = 0;
            numPrecio.Value = 0;
            txtObservaciones.Clear();
            lblStockActual.Text = LanguageManager.Translate("stock_cero");
            lblStockActual.ForeColor = System.Drawing.Color.Black;
        }

        #endregion

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
