using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
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
            btnCopiarDNI.Click += BtnCopiarDNI_Click;

            // Configurar estado inicial
            BloquearCampos();
            btnGuardar.Enabled = false;
        }

        private void GestionClientes_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            ConfigurarDataGridClientes();
            ConfigurarDataGridMascotas();
            CargarTodosLosClientes();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("gestion_clientes");
                groupBoxDatosCliente.Text = LanguageManager.Translate("datos_usuario");
                groupBoxAcciones.Text = LanguageManager.Translate("acciones");
                groupBoxMascotas.Text = LanguageManager.Translate("mascotas");

                // Labels
                label1.Text = $"{LanguageManager.Translate("buscar_cliente")}:";
                lblNombre.Text = $"{LanguageManager.Translate("nombre")}:";
                lblApellido.Text = $"{LanguageManager.Translate("apellido")}:";
                lblDNI.Text = $"{LanguageManager.Translate("dni")}:";
                lblTelefono.Text = $"{LanguageManager.Translate("telefono")}:";
                lblEmail.Text = $"{LanguageManager.Translate("email")}:";
                lblDireccion.Text = $"{LanguageManager.Translate("direccion")}:";
                chkActivo.Text = LanguageManager.Translate("activo");

                // Botones
                btnNuevo.Text = LanguageManager.Translate("nuevo");
                btnGuardar.Text = LanguageManager.Translate("guardar");
                btnModificar.Text = LanguageManager.Translate("modificar");
                btnEliminar.Text = LanguageManager.Translate("eliminar");
                btnVolver.Text = LanguageManager.Translate("volver");
                btnBuscar.Text = LanguageManager.Translate("buscar");
                btnAgregarMascota.Text = LanguageManager.Translate("agregar_mascota");
                btnEliminarMascota.Text = LanguageManager.Translate("eliminar_mascota");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var clientes = ClienteBLL.Current.ListarTodosLosClientes();
                dgvClientes.DataSource = null;
                dgvClientes.DataSource = clientes.OrderBy(c => c.Apellido).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_datos")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                // Asignar valores al cliente desde los controles
                _clienteSeleccionado.Nombre = txtNombre.Text.Trim();
                _clienteSeleccionado.Apellido = txtApellido.Text.Trim();
                _clienteSeleccionado.DNI = txtDNI.Text.Trim();
                _clienteSeleccionado.Telefono = txtTelefono.Text.Trim();
                _clienteSeleccionado.Email = txtEmail.Text.Trim();
                _clienteSeleccionado.Direccion = txtDireccion.Text.Trim();
                _clienteSeleccionado.Activo = chkActivo.Checked;

                if (_modoEdicion)
                {
                    // Actualizar cliente existente usando BLL
                    var clienteActualizado = ClienteBLL.Current.ModificarCliente(_clienteSeleccionado);
                    MessageBox.Show(LanguageManager.Translate("cliente_actualizado_correctamente"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Registrar nuevo cliente usando BLL
                    var clienteCreado = ClienteBLL.Current.RegistrarCliente(_clienteSeleccionado);
                    MessageBox.Show(LanguageManager.Translate("cliente_registrado_correctamente"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Recargar lista y limpiar formulario
                CargarTodosLosClientes();
                LimpiarCampos();
                BloquearCampos();
                btnGuardar.Enabled = false;
            }
            catch (ArgumentException ex)
            {
                // Errores de validación del BLL
                MessageBox.Show(ex.Message, LanguageManager.Translate("error_validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                // Errores de reglas de negocio (ej: DNI duplicado)
                MessageBox.Show(ex.Message, LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Errores inesperados
                MessageBox.Show($"{LanguageManager.Translate("error_inesperado_guardar_cliente")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (_clienteSeleccionado == null || _clienteSeleccionado.IdCliente == Guid.Empty)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_cliente"),
                    LanguageManager.Translate("advertencia"),
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
                if (_clienteSeleccionado == null || _clienteSeleccionado.IdCliente == Guid.Empty)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_cliente"),
                        LanguageManager.Translate("advertencia"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var mensaje = string.Format(LanguageManager.Translate("confirmar_eliminar_cliente_completo"),
                    _clienteSeleccionado.NombreCompleto);

                var resultado = MessageBox.Show(mensaje,
                    LanguageManager.Translate("confirmar"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Eliminar cliente usando BLL
                    ClienteBLL.Current.EliminarCliente(_clienteSeleccionado.IdCliente);

                    MessageBox.Show(LanguageManager.Translate("cliente_eliminado_correctamente"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarTodosLosClientes();
                    LimpiarCampos();
                    _clienteSeleccionado = null;
                }
            }
            catch (InvalidOperationException ex)
            {
                // Error de reglas de negocio
                MessageBox.Show(ex.Message, LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var textoBusqueda = txtBuscar.Text.Trim();

                if (string.IsNullOrEmpty(textoBusqueda))
                {
                    CargarTodosLosClientes();
                    return;
                }

                // Buscar usando BLL
                var clientesFiltrados = ClienteBLL.Current.BuscarClientes(textoBusqueda);

                dgvClientes.DataSource = null;
                dgvClientes.DataSource = clientesFiltrados.OrderBy(c => c.Apellido).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_buscar")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                if (_clienteSeleccionado != null && _clienteSeleccionado.IdCliente != Guid.Empty)
                {
                    // Cargar cliente con sus mascotas usando BLL
                    var clienteConMascotas = ClienteBLL.Current.ObtenerClienteConMascotas(_clienteSeleccionado.IdCliente);

                    if (clienteConMascotas != null && clienteConMascotas.Mascotas != null)
                    {
                        dgvMascotas.DataSource = null;
                        dgvMascotas.DataSource = clienteConMascotas.Mascotas.Where(m => m.Activo).ToList();
                    }
                    else
                    {
                        dgvMascotas.DataSource = null;
                    }
                }
                else
                {
                    dgvMascotas.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_mascotas")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvMascotas.DataSource = null;
            }
        }

        private void BtnAgregarMascota_Click(object sender, EventArgs e)
        {
            if (_clienteSeleccionado == null || _clienteSeleccionado.IdCliente == Guid.Empty)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_cliente"),
                    LanguageManager.Translate("advertencia"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crear formulario para agregar mascota
            using (var formMascota = new FormAgregarMascota(_clienteSeleccionado))
            {
                if (formMascota.ShowDialog() == DialogResult.OK)
                {
                    // Recargar mascotas después de agregar
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
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_mascota"),
                        LanguageManager.Translate("advertencia"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var mascota = (Mascota)dgvMascotas.SelectedRows[0].DataBoundItem;

                var mensaje = string.Format(LanguageManager.Translate("confirmar_desactivar_mascota"),
                    mascota.Nombre);
                var resultado = MessageBox.Show(mensaje,
                    LanguageManager.Translate("confirmar_desactivacion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Desactivar mascota usando BLL
                    MascotaBLL.Current.DesactivarMascota(mascota.IdMascota);

                    MessageBox.Show(LanguageManager.Translate("mascota_desactivada_correctamente"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarMascotasCliente();
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_desactivar_mascota")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void BtnCopiarDNI_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtDNI.Text))
                {
                    Clipboard.SetText(txtDNI.Text.Trim());
                    var mensaje = string.Format(LanguageManager.Translate("dni_copiado_portapapeles"),
                        txtDNI.Text.Trim());
                    MessageBox.Show(mensaje,
                        LanguageManager.Translate("dni_copiado"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(LanguageManager.Translate("no_hay_dni_copiar"),
                        LanguageManager.Translate("informacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_copiar_dni")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
