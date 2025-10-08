using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DomainModel;
using ServicesSecurity.DomainModel.Security.Composite;

namespace UI.WinUi.Negocio
{
    public partial class gestionMascotas : Form
    {
        private Usuario _usuarioLogueado;
        private Mascota _mascotaSeleccionada;
        private bool _modoEdicion = false;

        // Lista temporal de mascotas en memoria (sin BD)
        private static List<Mascota> _mascotasEnMemoria = new List<Mascota>();

        // Lista temporal de clientes/dueños (sin BD)
        private static List<Cliente> _clientesEnMemoria = new List<Cliente>();

        public gestionMascotas()
        {
            InitializeComponent();
        }

        public gestionMascotas(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Configurar eventos
            this.Load += GestionMascotas_Load;
            btnNuevo.Click += BtnNuevo_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnVolver.Click += BtnVolver_Click;
            btnBuscar.Click += BtnBuscar_Click;
            dgvMascotas.SelectionChanged += DgvMascotas_SelectionChanged;

            // Configurar estado inicial
            BloquearCampos();
            btnGuardar.Enabled = false;
        }

        private void GestionMascotas_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarDuenos();
            CargarTodasLasMascotas();
            ConfigurarDataGridMascotas();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = "Gestión de Mascotas";
                groupBoxDatosMascota.Text = "Datos de la Mascota";
                groupBoxAcciones.Text = "Acciones";

                // Labels
                label1.Text = "Buscar Mascota:";
                lblNombre.Text = "Nombre:";
                lblEspecie.Text = "Especie:";
                lblRaza.Text = "Raza:";
                lblFechaNacimiento.Text = "Fecha Nacimiento:";
                lblSexo.Text = "Sexo:";
                lblPeso.Text = "Peso (kg):";
                lblColor.Text = "Color:";
                lblDueno.Text = "Dueño:";
                lblObservaciones.Text = "Observaciones:";
                chkActivo.Text = "Activo";

                // Botones
                btnNuevo.Text = "Nuevo";
                btnGuardar.Text = "Guardar";
                btnModificar.Text = "Modificar";
                btnEliminar.Text = "Eliminar";
                btnVolver.Text = "Volver";
                btnBuscar.Text = "Buscar";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aplicar traducciones: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridMascotas()
        {
            dgvMascotas.AutoGenerateColumns = false;
            dgvMascotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMascotas.MultiSelect = false;
            dgvMascotas.ReadOnly = true;
            dgvMascotas.AllowUserToAddRows = false;
            dgvMascotas.AllowUserToDeleteRows = false;

            dgvMascotas.Columns.Clear();
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 120
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Especie",
                HeaderText = "Especie",
                Width = 100
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Raza",
                HeaderText = "Raza",
                Width = 120
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EdadEnAnios",
                HeaderText = "Edad (años)",
                Width = 80
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Sexo",
                HeaderText = "Sexo",
                Width = 80
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Peso",
                HeaderText = "Peso (kg)",
                Width = 80
            });
            dgvMascotas.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 60
            });
        }

        private void CargarDuenos()
        {
            try
            {
                cmbDueno.DataSource = null;
                cmbDueno.DisplayMember = "NombreCompleto";
                cmbDueno.ValueMember = "IdCliente";
                cmbDueno.DataSource = _clientesEnMemoria.Where(c => c.Activo).OrderBy(c => c.Apellido).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar dueños: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarTodasLasMascotas()
        {
            try
            {
                dgvMascotas.DataSource = null;
                dgvMascotas.DataSource = _mascotasEnMemoria.OrderBy(m => m.Nombre).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar mascotas: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            if (_clientesEnMemoria.Count == 0)
            {
                MessageBox.Show("Debe crear al menos un cliente antes de agregar mascotas", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _modoEdicion = false;
            _mascotaSeleccionada = new Mascota();
            LimpiarCampos();
            DesbloquearCampos();
            btnGuardar.Enabled = true;
            txtNombre.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos
                if (!ValidarCampos())
                    return;

                // Asignar valores a la mascota
                _mascotaSeleccionada.Nombre = txtNombre.Text.Trim();
                _mascotaSeleccionada.Especie = txtEspecie.Text.Trim();
                _mascotaSeleccionada.Raza = txtRaza.Text.Trim();
                _mascotaSeleccionada.FechaNacimiento = dtpFechaNacimiento.Value;
                _mascotaSeleccionada.Sexo = cmbSexo.SelectedItem?.ToString() ?? "";
                _mascotaSeleccionada.Peso = numPeso.Value;
                _mascotaSeleccionada.Color = txtColor.Text.Trim();
                _mascotaSeleccionada.Observaciones = txtObservaciones.Text.Trim();
                _mascotaSeleccionada.Activo = chkActivo.Checked;
                _mascotaSeleccionada.IdCliente = (Guid)cmbDueno.SelectedValue;

                if (_modoEdicion)
                {
                    // Actualizar mascota existente
                    var mascotaExistente = _mascotasEnMemoria.FirstOrDefault(m => m.IdMascota == _mascotaSeleccionada.IdMascota);
                    if (mascotaExistente != null)
                    {
                        var index = _mascotasEnMemoria.IndexOf(mascotaExistente);
                        _mascotasEnMemoria[index] = _mascotaSeleccionada;
                    }
                    MessageBox.Show("Mascota actualizada correctamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Agregar nueva mascota
                    _mascotasEnMemoria.Add(_mascotaSeleccionada);
                    MessageBox.Show("Mascota creada correctamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarTodasLasMascotas();
                LimpiarCampos();
                BloquearCampos();
                btnGuardar.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar mascota: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (_mascotaSeleccionada == null)
            {
                MessageBox.Show("Debe seleccionar una mascota", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _modoEdicion = true;
            DesbloquearCampos();
            btnGuardar.Enabled = true;
            txtNombre.Focus();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mascotaSeleccionada == null)
                {
                    MessageBox.Show("Debe seleccionar una mascota", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var resultado = MessageBox.Show(
                    $"¿Está seguro que desea eliminar a {_mascotaSeleccionada.Nombre}?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    _mascotasEnMemoria.Remove(_mascotaSeleccionada);
                    MessageBox.Show("Mascota eliminada correctamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarTodasLasMascotas();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar mascota: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var textoBusqueda = txtBuscar.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(textoBusqueda))
                {
                    CargarTodasLasMascotas();
                    return;
                }

                var mascotasFiltradas = _mascotasEnMemoria
                    .Where(m =>
                        m.Nombre.ToLower().Contains(textoBusqueda) ||
                        m.Especie.ToLower().Contains(textoBusqueda) ||
                        (m.Raza != null && m.Raza.ToLower().Contains(textoBusqueda)))
                    .OrderBy(m => m.Nombre)
                    .ToList();

                dgvMascotas.DataSource = null;
                dgvMascotas.DataSource = mascotasFiltradas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMascotas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMascotas.SelectedRows.Count > 0)
            {
                _mascotaSeleccionada = (Mascota)dgvMascotas.SelectedRows[0].DataBoundItem;
                CargarDatosMascota();
            }
        }

        private void CargarDatosMascota()
        {
            if (_mascotaSeleccionada != null)
            {
                txtNombre.Text = _mascotaSeleccionada.Nombre;
                txtEspecie.Text = _mascotaSeleccionada.Especie;
                txtRaza.Text = _mascotaSeleccionada.Raza;
                dtpFechaNacimiento.Value = _mascotaSeleccionada.FechaNacimiento;
                cmbSexo.SelectedItem = _mascotaSeleccionada.Sexo;
                numPeso.Value = _mascotaSeleccionada.Peso;
                txtColor.Text = _mascotaSeleccionada.Color;
                txtObservaciones.Text = _mascotaSeleccionada.Observaciones;
                chkActivo.Checked = _mascotaSeleccionada.Activo;
                cmbDueno.SelectedValue = _mascotaSeleccionada.IdCliente;
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEspecie.Text))
            {
                MessageBox.Show("La especie es obligatoria", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEspecie.Focus();
                return false;
            }

            if (cmbDueno.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un dueño", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDueno.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtEspecie.Clear();
            txtRaza.Clear();
            dtpFechaNacimiento.Value = DateTime.Now;
            cmbSexo.SelectedIndex = -1;
            numPeso.Value = 0;
            txtColor.Clear();
            txtObservaciones.Clear();
            chkActivo.Checked = true;
            if (cmbDueno.Items.Count > 0)
                cmbDueno.SelectedIndex = 0;
        }

        private void BloquearCampos()
        {
            txtNombre.Enabled = false;
            txtEspecie.Enabled = false;
            txtRaza.Enabled = false;
            dtpFechaNacimiento.Enabled = false;
            cmbSexo.Enabled = false;
            numPeso.Enabled = false;
            txtColor.Enabled = false;
            txtObservaciones.Enabled = false;
            chkActivo.Enabled = false;
            cmbDueno.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            txtNombre.Enabled = true;
            txtEspecie.Enabled = true;
            txtRaza.Enabled = true;
            dtpFechaNacimiento.Enabled = true;
            cmbSexo.Enabled = true;
            numPeso.Enabled = true;
            txtColor.Enabled = true;
            txtObservaciones.Enabled = true;
            chkActivo.Enabled = true;
            cmbDueno.Enabled = true;
        }
    }
}
