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
    public partial class gestionMascotas : Form
    {
        private Usuario _usuarioLogueado;
        private Mascota _mascotaSeleccionada;
        private bool _modoEdicion = false;

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
            ConfigurarDataGridMascotas();
            CargarDuenos();
            CargarTodasLasMascotas();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("gestion_mascotas");
                groupBoxDatosMascota.Text = LanguageManager.Translate("datos_mascota");
                groupBoxAcciones.Text = LanguageManager.Translate("acciones");

                // Labels
                label1.Text = $"{LanguageManager.Translate("buscar_mascota")}:";
                lblNombre.Text = $"{LanguageManager.Translate("nombre")}:";
                lblEspecie.Text = $"{LanguageManager.Translate("especie")}:";
                lblRaza.Text = $"{LanguageManager.Translate("raza")}:";
                lblFechaNacimiento.Text = $"{LanguageManager.Translate("fecha_nacimiento")}:";
                lblSexo.Text = $"{LanguageManager.Translate("sexo")}:";
                lblPeso.Text = $"{LanguageManager.Translate("peso")} ({LanguageManager.Translate("kg")}):";
                lblColor.Text = $"{LanguageManager.Translate("color")}:";
                lblDueno.Text = $"{LanguageManager.Translate("dueno")}:";
                lblObservaciones.Text = $"{LanguageManager.Translate("observaciones")}:";
                chkActivo.Text = LanguageManager.Translate("activo");

                // Botones
                btnNuevo.Text = LanguageManager.Translate("nuevo");
                btnGuardar.Text = LanguageManager.Translate("guardar");
                btnModificar.Text = LanguageManager.Translate("modificar");
                btnEliminar.Text = LanguageManager.Translate("eliminar");
                btnVolver.Text = LanguageManager.Translate("volver");
                btnBuscar.Text = LanguageManager.Translate("buscar");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_aplicar_traducciones")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                HeaderText = LanguageManager.Translate("nombre"),
                Width = 120
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Especie",
                HeaderText = LanguageManager.Translate("especie"),
                Width = 100
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Raza",
                HeaderText = LanguageManager.Translate("raza"),
                Width = 120
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EdadEnAnios",
                HeaderText = LanguageManager.Translate("edad_anios"),
                Width = 80
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Sexo",
                HeaderText = LanguageManager.Translate("sexo"),
                Width = 80
            });
            dgvMascotas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Peso",
                HeaderText = $"{LanguageManager.Translate("peso")} ({LanguageManager.Translate("kg")})",
                Width = 80
            });
            dgvMascotas.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Activo",
                HeaderText = LanguageManager.Translate("activo"),
                Width = 60
            });
        }

        private void CargarDuenos()
        {
            try
            {
                // Cargar clientes activos usando BLL
                var clientes = ClienteBLL.Current.ListarClientesActivos();

                cmbDueno.DataSource = null;
                cmbDueno.DisplayMember = "NombreCompleto";
                cmbDueno.ValueMember = "IdCliente";
                cmbDueno.DataSource = clientes.OrderBy(c => c.Apellido).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_duenos")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarTodasLasMascotas()
        {
            try
            {
                // Cargar todas las mascotas usando BLL
                var mascotas = MascotaBLL.Current.ListarTodasLasMascotas();

                dgvMascotas.DataSource = null;
                dgvMascotas.DataSource = mascotas.OrderBy(m => m.Nombre).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_mascotas")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar que haya clientes disponibles usando BLL
                var clientes = ClienteBLL.Current.ListarClientesActivos();
                if (!clientes.Any())
                {
                    MessageBox.Show(LanguageManager.Translate("debe_crear_cliente_antes_mascota"),
                        LanguageManager.Translate("advertencia"),
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
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_iniciar_nueva_mascota")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Asignar valores a la mascota desde los controles
                _mascotaSeleccionada.Nombre = txtNombre.Text.Trim();
                _mascotaSeleccionada.Especie = txtEspecie.Text.Trim();
                _mascotaSeleccionada.Raza = txtRaza.Text.Trim();
                _mascotaSeleccionada.FechaNacimiento = dtpFechaNacimiento.Value;
                _mascotaSeleccionada.Sexo = cmbSexo.SelectedItem?.ToString() ?? "";
                _mascotaSeleccionada.Peso = numPeso.Value;
                _mascotaSeleccionada.Color = txtColor.Text.Trim();
                _mascotaSeleccionada.Observaciones = txtObservaciones.Text.Trim();
                _mascotaSeleccionada.Activo = chkActivo.Checked;

                // Validar que se haya seleccionado un dueño
                if (cmbDueno.SelectedValue == null)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_dueno"),
                        LanguageManager.Translate("advertencia"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbDueno.Focus();
                    return;
                }

                _mascotaSeleccionada.IdCliente = (Guid)cmbDueno.SelectedValue;

                if (_modoEdicion)
                {
                    // Actualizar mascota existente usando BLL
                    var mascotaActualizada = MascotaBLL.Current.ModificarMascota(_mascotaSeleccionada);
                    MessageBox.Show(LanguageManager.Translate("mascota_actualizada_correctamente"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Registrar nueva mascota usando BLL
                    var mascotaCreada = MascotaBLL.Current.RegistrarMascota(_mascotaSeleccionada);
                    MessageBox.Show(LanguageManager.Translate("mascota_registrada_correctamente"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Recargar lista y limpiar formulario
                CargarTodasLasMascotas();
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
                // Errores de reglas de negocio
                MessageBox.Show(ex.Message, LanguageManager.Translate("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Errores inesperados
                MessageBox.Show($"{LanguageManager.Translate("error_guardar_mascota")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (_mascotaSeleccionada == null || _mascotaSeleccionada.IdMascota == Guid.Empty)
            {
                MessageBox.Show(LanguageManager.Translate("debe_seleccionar_mascota"),
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
                if (_mascotaSeleccionada == null || _mascotaSeleccionada.IdMascota == Guid.Empty)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_mascota"),
                        LanguageManager.Translate("advertencia"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var mensaje = string.Format(LanguageManager.Translate("confirmar_eliminar_mascota_nombre"),
                    _mascotaSeleccionada.Nombre);
                var resultado = MessageBox.Show(mensaje,
                    LanguageManager.Translate("confirmar_eliminacion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Eliminar mascota usando BLL
                    MascotaBLL.Current.EliminarMascota(_mascotaSeleccionada.IdMascota);

                    MessageBox.Show(LanguageManager.Translate("mascota_eliminada_correctamente"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarTodasLasMascotas();
                    LimpiarCampos();
                    _mascotaSeleccionada = null;
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
                MessageBox.Show($"{LanguageManager.Translate("error_eliminar_mascota")}: {ex.Message}",
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
                    CargarTodasLasMascotas();
                    return;
                }

                // Buscar usando BLL
                var mascotasFiltradas = MascotaBLL.Current.BuscarMascotas(textoBusqueda);

                dgvMascotas.DataSource = null;
                dgvMascotas.DataSource = mascotasFiltradas.OrderBy(m => m.Nombre).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_buscar")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
