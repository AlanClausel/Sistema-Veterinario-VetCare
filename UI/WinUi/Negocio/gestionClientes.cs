using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DomainModel;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class gestionClientes : Form
    {
        private Usuario _usuarioLogueado;
        private Cliente _clienteSeleccionado;
        private bool _modoEdicion = false;

        // Lista temporal de clientes en memoria (sin BD)
        private static List<Cliente> _clientesEnMemoria = new List<Cliente>();

        public gestionClientes()
        {
            InitializeComponent();
        }

        public gestionClientes(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Configurar eventos
            this.Load += GestionClientes_Load;
            btnNuevo.Click += BtnNuevo_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnVolver.Click += BtnVolver_Click;
            btnBuscar.Click += BtnBuscar_Click;
            dgvClientes.SelectionChanged += DgvClientes_SelectionChanged;
            btnAgregarMascota.Click += BtnAgregarMascota_Click;
            btnEliminarMascota.Click += BtnEliminarMascota_Click;

            // Configurar estado inicial
            BloquearCampos();
            btnGuardar.Enabled = false;
        }

        private void GestionClientes_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarTodosLosClientes();
            ConfigurarDataGridClientes();
            ConfigurarDataGridMascotas();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = "Gestión de Clientes";
                groupBoxDatosCliente.Text = "Datos del Cliente";
                groupBoxAcciones.Text = "Acciones";
                groupBoxMascotas.Text = "Mascotas del Cliente";

                // Labels
                label1.Text = "Buscar Cliente:";
                lblNombre.Text = "Nombre:";
                lblApellido.Text = "Apellido:";
                lblDNI.Text = "DNI:";
                lblTelefono.Text = "Teléfono:";
                lblEmail.Text = "Email:";
                lblDireccion.Text = "Dirección:";
                chkActivo.Text = "Activo";

                // Botones
                btnNuevo.Text = "Nuevo";
                btnGuardar.Text = "Guardar";
                btnModificar.Text = "Modificar";
                btnEliminar.Text = "Eliminar";
                btnVolver.Text = "Volver";
                btnBuscar.Text = "Buscar";
                btnAgregarMascota.Text = "Agregar Mascota";
                btnEliminarMascota.Text = "Eliminar Mascota";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aplicar traducciones: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridClientes()
        {
            dgvClientes.AutoGenerateColumns = false;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.MultiSelect = false;
            dgvClientes.ReadOnly = true;
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;

            dgvClientes.Columns.Clear();
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Apellido",
                HeaderText = "Apellido",
                Width = 120
            });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 120
            });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DNI",
                HeaderText = "DNI",
                Width = 100
            });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Telefono",
                HeaderText = "Teléfono",
                Width = 100
            });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email",
                Width = 150
            });
            dgvClientes.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 60
            });
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
                Width = 100
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Especie",
                HeaderText = "Especie",
                Width = 80
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Raza",
                HeaderText = "Raza",
                Width = 100
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
                Width = 60
            });
        }

        private void CargarTodosLosClientes()
        {
            try
            {
                dgvClientes.DataSource = null;
                dgvClientes.DataSource = _clientesEnMemoria.OrderBy(c => c.Apellido).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            _modoEdicion = false;
            _clienteSeleccionado = new Cliente();
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

                // Asignar valores al cliente
                _clienteSeleccionado.Nombre = txtNombre.Text.Trim();
                _clienteSeleccionado.Apellido = txtApellido.Text.Trim();
                _clienteSeleccionado.DNI = txtDNI.Text.Trim();
                _clienteSeleccionado.Telefono = txtTelefono.Text.Trim();
                _clienteSeleccionado.Email = txtEmail.Text.Trim();
                _clienteSeleccionado.Direccion = txtDireccion.Text.Trim();
                _clienteSeleccionado.Activo = chkActivo.Checked;

                if (_modoEdicion)
                {
                    // Actualizar cliente existente
                    var clienteExistente = _clientesEnMemoria.FirstOrDefault(c => c.IdCliente == _clienteSeleccionado.IdCliente);
                    if (clienteExistente != null)
                    {
                        var index = _clientesEnMemoria.IndexOf(clienteExistente);
                        _clientesEnMemoria[index] = _clienteSeleccionado;
                    }
                    MessageBox.Show("Cliente actualizado correctamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Agregar nuevo cliente
                    _clientesEnMemoria.Add(_clienteSeleccionado);
                    MessageBox.Show("Cliente creado correctamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarTodosLosClientes();
                LimpiarCampos();
                BloquearCampos();
                btnGuardar.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar cliente: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (_clienteSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un cliente", "Advertencia",
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
                if (_clienteSeleccionado == null)
                {
                    MessageBox.Show("Debe seleccionar un cliente", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var resultado = MessageBox.Show(
                    $"¿Está seguro que desea eliminar al cliente {_clienteSeleccionado.NombreCompleto}?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    _clientesEnMemoria.Remove(_clienteSeleccionado);
                    MessageBox.Show("Cliente eliminado correctamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarTodosLosClientes();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar cliente: {ex.Message}",
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
                    CargarTodosLosClientes();
                    return;
                }

                var clientesFiltrados = _clientesEnMemoria
                    .Where(c =>
                        c.Nombre.ToLower().Contains(textoBusqueda) ||
                        c.Apellido.ToLower().Contains(textoBusqueda) ||
                        c.DNI.Contains(textoBusqueda) ||
                        (c.Email != null && c.Email.ToLower().Contains(textoBusqueda)))
                    .OrderBy(c => c.Apellido)
                    .ToList();

                dgvClientes.DataSource = null;
                dgvClientes.DataSource = clientesFiltrados;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                _clienteSeleccionado = (Cliente)dgvClientes.SelectedRows[0].DataBoundItem;
                CargarDatosCliente();
                CargarMascotasCliente();
            }
        }

        private void CargarDatosCliente()
        {
            if (_clienteSeleccionado != null)
            {
                txtNombre.Text = _clienteSeleccionado.Nombre;
                txtApellido.Text = _clienteSeleccionado.Apellido;
                txtDNI.Text = _clienteSeleccionado.DNI;
                txtTelefono.Text = _clienteSeleccionado.Telefono;
                txtEmail.Text = _clienteSeleccionado.Email;
                txtDireccion.Text = _clienteSeleccionado.Direccion;
                chkActivo.Checked = _clienteSeleccionado.Activo;
            }
        }

        private void CargarMascotasCliente()
        {
            if (_clienteSeleccionado != null)
            {
                dgvMascotas.DataSource = null;
                dgvMascotas.DataSource = _clienteSeleccionado.Mascotas.Where(m => m.Activo).ToList();
            }
            else
            {
                dgvMascotas.DataSource = null;
            }
        }

        private void BtnAgregarMascota_Click(object sender, EventArgs e)
        {
            if (_clienteSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un cliente", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crear formulario para agregar mascota
            using (var formMascota = new FormAgregarMascota(_clienteSeleccionado))
            {
                if (formMascota.ShowDialog() == DialogResult.OK)
                {
                    CargarMascotasCliente();
                }
            }
        }

        private void BtnEliminarMascota_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMascotas.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar una mascota", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var mascota = (Mascota)dgvMascotas.SelectedRows[0].DataBoundItem;

                var resultado = MessageBox.Show(
                    $"¿Está seguro que desea eliminar a {mascota.Nombre}?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    mascota.Activo = false;
                    CargarMascotasCliente();
                    MessageBox.Show("Mascota eliminada correctamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar mascota: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show("El DNI es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDNI.Focus();
                return false;
            }

            // Validar que el DNI no esté duplicado
            var dniDuplicado = _clientesEnMemoria.Any(c =>
                c.DNI == txtDNI.Text.Trim() &&
                c.IdCliente != _clienteSeleccionado.IdCliente);

            if (dniDuplicado)
            {
                MessageBox.Show("Ya existe un cliente con ese DNI", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDNI.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDNI.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            chkActivo.Checked = true;
        }

        private void BloquearCampos()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtDNI.Enabled = false;
            txtTelefono.Enabled = false;
            txtEmail.Enabled = false;
            txtDireccion.Enabled = false;
            chkActivo.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtDNI.Enabled = true;
            txtTelefono.Enabled = true;
            txtEmail.Enabled = true;
            txtDireccion.Enabled = true;
            chkActivo.Enabled = true;
        }
    }
}
