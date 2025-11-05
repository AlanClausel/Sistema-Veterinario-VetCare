using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class FormConsultaMedica : Form
    {
        private Guid _idCita;
        private Guid _idVeterinario;
        private ConsultaMedica _consultaActual;
        private Cita _citaActual;
        private List<Medicamento> _medicamentosDisponibles;
        private List<MedicamentoSeleccionado> _medicamentosSeleccionados;

        public FormConsultaMedica(Guid idCita, Guid idVeterinario)
        {
            InitializeComponent();
            _idCita = idCita;
            _idVeterinario = idVeterinario;
            _medicamentosSeleccionados = new List<MedicamentoSeleccionado>();
        }

        private void FormConsultaMedica_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargar datos de la cita
                _citaActual = CitaBLL.Current.ObtenerCita(_idCita);
                if (_citaActual == null)
                {
                    MessageBox.Show(LanguageManager.Translate("no_encontrado_cita"),
                        LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                CargarDatosCita();

                // Verificar si ya existe una consulta para esta cita
                _consultaActual = ConsultaMedicaBLL.Current.ObtenerConsultaPorCita(_idCita);

                if (_consultaActual != null)
                {
                    // Cargar consulta existente
                    CargarConsultaExistente();
                }
                else
                {
                    // Crear nueva consulta
                    _consultaActual = ConsultaMedicaBLL.Current.IniciarConsulta(_idCita, _idVeterinario);
                }

                // Cargar medicamentos disponibles
                CargarMedicamentosDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_formulario")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void CargarDatosCita()
        {
            txtFecha.Text = _citaActual.FechaCita.ToString("dd/MM/yyyy");
            txtHora.Text = _citaActual.HoraCita;
            txtTipo.Text = _citaActual.TipoConsulta;
            txtCliente.Text = _citaActual.ClienteNombre;
            txtMascota.Text = _citaActual.MascotaNombre;
        }

        private void CargarConsultaExistente()
        {
            txtSintomas.Text = _consultaActual.Sintomas;
            txtDiagnostico.Text = _consultaActual.Diagnostico;

            // Cargar medicamentos seleccionados
            var medicamentosRecetados = ConsultaMedicaBLL.Current.ObtenerMedicamentosDeConsulta(_consultaActual.IdConsulta);
            foreach (var medicamento in medicamentosRecetados)
            {
                _medicamentosSeleccionados.Add(new MedicamentoSeleccionado
                {
                    IdMedicamento = medicamento.IdMedicamento,
                    Nombre = medicamento.NombreCompleto,
                    Cantidad = medicamento.Cantidad,
                    Indicaciones = medicamento.Indicaciones
                });
            }

            ActualizarListaSeleccionados();
        }

        private void CargarMedicamentosDisponibles()
        {
            var medicamentos = MedicamentoBLL.Current.ListarMedicamentosDisponibles();
            _medicamentosDisponibles = medicamentos.ToList();

            // Mostrar todos los medicamentos al inicio
            MostrarMedicamentos(_medicamentosDisponibles);
        }

        private void MostrarMedicamentos(IEnumerable<Medicamento> medicamentos)
        {
            lstResultados.Items.Clear();
            foreach (var medicamento in medicamentos.Take(20))
            {
                lstResultados.Items.Add(new MedicamentoListItem
                {
                    Medicamento = medicamento,
                    DisplayText = $"{medicamento.NombreCompleto} - {LanguageManager.Translate("stock_label")}: {medicamento.Stock}"
                });
            }
            lstResultados.DisplayMember = "DisplayText";
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim();

            if (string.IsNullOrWhiteSpace(criterio))
            {
                // Si el campo está vacío, mostrar TODOS los medicamentos
                MostrarMedicamentos(_medicamentosDisponibles);
                return;
            }

            // Filtrar por criterio de búsqueda
            var resultados = _medicamentosDisponibles
                .Where(m => m.Nombre.ToLower().Contains(criterio.ToLower()) ||
                           (m.Presentacion != null && m.Presentacion.ToLower().Contains(criterio.ToLower())))
                .ToList();

            MostrarMedicamentos(resultados);
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstResultados.SelectedItem == null)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_medicamento_resultados"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var item = lstResultados.SelectedItem as MedicamentoListItem;
                if (item == null)
                    return;

                // Verificar que no esté ya seleccionado
                if (_medicamentosSeleccionados.Any(m => m.IdMedicamento == item.Medicamento.IdMedicamento))
                {
                    MessageBox.Show(LanguageManager.Translate("medicamento_ya_en_lista"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Solicitar cantidad e indicaciones
                using (var formCantidad = new FormCantidadMedicamento(item.Medicamento))
                {
                    if (formCantidad.ShowDialog() == DialogResult.OK)
                    {
                        _medicamentosSeleccionados.Add(new MedicamentoSeleccionado
                        {
                            IdMedicamento = item.Medicamento.IdMedicamento,
                            Nombre = item.Medicamento.NombreCompleto,
                            Cantidad = formCantidad.Cantidad,
                            Indicaciones = formCantidad.Indicaciones
                        });

                        ActualizarListaSeleccionados();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_anadir_medicamento")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstSeleccionados.SelectedItem == null)
                {
                    MessageBox.Show(LanguageManager.Translate("debe_seleccionar_medicamento_seleccionados"),
                        LanguageManager.Translate("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var medSeleccionado = lstSeleccionados.SelectedItem as MedicamentoSeleccionado;
                if (medSeleccionado == null)
                    return;

                _medicamentosSeleccionados.Remove(medSeleccionado);
                ActualizarListaSeleccionados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_quitar_medicamento")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarListaSeleccionados()
        {
            lstSeleccionados.Items.Clear();
            foreach (var medicamento in _medicamentosSeleccionados)
            {
                lstSeleccionados.Items.Add(medicamento);
            }

            lstSeleccionados.DisplayMember = "DisplayText";
        }

        private void btnFinalizarConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar datos
                if (string.IsNullOrWhiteSpace(txtSintomas.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("debe_ingresar_sintomas"),
                        LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSintomas.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDiagnostico.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("debe_ingresar_diagnostico"),
                        LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiagnostico.Focus();
                    return;
                }

                // Actualizar datos de la consulta
                _consultaActual.Sintomas = txtSintomas.Text;
                _consultaActual.Diagnostico = txtDiagnostico.Text;

                // Guardar consulta
                ConsultaMedicaBLL.Current.GuardarConsulta(_consultaActual);

                // Limpiar medicamentos actuales de la consulta
                var medicamentosActuales = ConsultaMedicaBLL.Current.ObtenerMedicamentosDeConsulta(_consultaActual.IdConsulta);
                foreach (var med in medicamentosActuales)
                {
                    ConsultaMedicaBLL.Current.EliminarMedicamento(_consultaActual.IdConsulta, med.IdMedicamento);
                }

                // Agregar nuevos medicamentos
                foreach (var med in _medicamentosSeleccionados)
                {
                    ConsultaMedicaBLL.Current.AgregarMedicamento(
                        _consultaActual.IdConsulta,
                        med.IdMedicamento,
                        med.Cantidad,
                        med.Indicaciones);
                }

                // Finalizar consulta (actualiza estado cita y reduce stock)
                bool resultado = ConsultaMedicaBLL.Current.FinalizarConsulta(_consultaActual.IdConsulta);

                if (resultado)
                {
                    MessageBox.Show(LanguageManager.Translate("consulta_finalizada_exitosamente"),
                        LanguageManager.Translate("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(LanguageManager.Translate("error_finalizar_consulta_stock"),
                        LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_finalizar_consulta")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                LanguageManager.Translate("confirmar_cancelar_sin_guardar"),
                LanguageManager.Translate("confirmar"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        // Clase auxiliar para mostrar medicamentos en ListBox
        private class MedicamentoListItem
        {
            public Medicamento Medicamento { get; set; }
            public string DisplayText { get; set; }

            public override string ToString()
            {
                return DisplayText;
            }
        }

        // Clase auxiliar para medicamentos seleccionados
        private class MedicamentoSeleccionado
        {
            public Guid IdMedicamento { get; set; }
            public string Nombre { get; set; }
            public int Cantidad { get; set; }
            public string Indicaciones { get; set; }

            public string DisplayText => $"{Nombre} x {Cantidad} - {Indicaciones}";

            public override string ToString()
            {
                return DisplayText;
            }
        }
    }

    // Formulario auxiliar para ingresar cantidad e indicaciones
    public class FormCantidadMedicamento : Form
    {
        private NumericUpDown numCantidad;
        private TextBox txtIndicaciones;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblCantidad;
        private Label lblIndicaciones;
        private Label lblMedicamento;

        public int Cantidad { get; private set; }
        public string Indicaciones { get; private set; }

        public FormCantidadMedicamento(Medicamento medicamento)
        {
            InitializeComponent();
            lblMedicamento.Text = string.Format(LanguageManager.Translate("medicamento_stock_disponible"),
                medicamento.NombreCompleto, medicamento.Stock);
            numCantidad.Maximum = medicamento.Stock;
        }

        private void InitializeComponent()
        {
            this.lblMedicamento = new Label();
            this.lblCantidad = new Label();
            this.numCantidad = new NumericUpDown();
            this.lblIndicaciones = new Label();
            this.txtIndicaciones = new TextBox();
            this.btnAceptar = new Button();
            this.btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).BeginInit();
            this.SuspendLayout();
            //
            // lblMedicamento
            //
            this.lblMedicamento.AutoSize = true;
            this.lblMedicamento.Location = new System.Drawing.Point(20, 20);
            this.lblMedicamento.Name = "lblMedicamento";
            this.lblMedicamento.Size = new System.Drawing.Size(100, 13);
            this.lblMedicamento.TabIndex = 0;
            this.lblMedicamento.Text = $"{LanguageManager.Translate("medicamento")}:";
            //
            // lblCantidad
            //
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(20, 70);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(52, 13);
            this.lblCantidad.TabIndex = 1;
            this.lblCantidad.Text = $"{LanguageManager.Translate("cantidad")}:";
            //
            // numCantidad
            //
            this.numCantidad.Location = new System.Drawing.Point(100, 68);
            this.numCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numCantidad.Name = "numCantidad";
            this.numCantidad.Size = new System.Drawing.Size(100, 20);
            this.numCantidad.TabIndex = 0;
            this.numCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblIndicaciones
            //
            this.lblIndicaciones.AutoSize = true;
            this.lblIndicaciones.Location = new System.Drawing.Point(20, 110);
            this.lblIndicaciones.Name = "lblIndicaciones";
            this.lblIndicaciones.Size = new System.Drawing.Size(74, 13);
            this.lblIndicaciones.TabIndex = 3;
            this.lblIndicaciones.Text = $"{LanguageManager.Translate("indicaciones")}:";
            //
            // txtIndicaciones
            //
            this.txtIndicaciones.Location = new System.Drawing.Point(20, 130);
            this.txtIndicaciones.Multiline = true;
            this.txtIndicaciones.Name = "txtIndicaciones";
            this.txtIndicaciones.Size = new System.Drawing.Size(360, 60);
            this.txtIndicaciones.TabIndex = 1;
            this.txtIndicaciones.Text = LanguageManager.Translate("indicaciones_default");
            //
            // btnAceptar
            //
            this.btnAceptar.Location = new System.Drawing.Point(100, 210);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(100, 30);
            this.btnAceptar.TabIndex = 2;
            this.btnAceptar.Text = LanguageManager.Translate("aceptar");
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new EventHandler(btnAceptar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.DialogResult = DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(220, 210);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 30);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = LanguageManager.Translate("cancelar");
            this.btnCancelar.UseVisualStyleBackColor = true;
            //
            // FormCantidadMedicamento
            //
            this.AcceptButton = this.btnAceptar;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(400, 260);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.txtIndicaciones);
            this.Controls.Add(this.lblIndicaciones);
            this.Controls.Add(this.numCantidad);
            this.Controls.Add(this.lblCantidad);
            this.Controls.Add(this.lblMedicamento);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCantidadMedicamento";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = LanguageManager.Translate("cantidad_indicaciones_titulo");
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Cantidad = (int)numCantidad.Value;
            Indicaciones = txtIndicaciones.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
