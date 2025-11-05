using System;
using System.Text;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class FormDetalleConsulta : Form
    {
        private ConsultaMedica _consulta;

        public FormDetalleConsulta(ConsultaMedica consulta)
        {
            InitializeComponent();
            _consulta = consulta;
        }

        private void FormDetalleConsulta_Load(object sender, EventArgs e)
        {
            try
            {
                MostrarDetalleConsulta();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_cargar_formulario")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDetalleConsulta()
        {
            try
            {
                // Obtener información del veterinario
                var veterinario = VeterinarioBLL.Current.ObtenerVeterinarioPorId(_consulta.IdVeterinario);
                string nombreVeterinario = veterinario != null
                    ? string.Format(LanguageManager.Translate("dr_prefijo"), veterinario.Nombre)
                    : LanguageManager.Translate("n_a");

                // Construir texto con todos los detalles
                StringBuilder detalle = new StringBuilder();

                detalle.AppendLine(string.Format(LanguageManager.Translate("fecha_hora_label"),
                    _consulta.FechaConsulta.ToString("dd/MM/yyyy"),
                    _consulta.FechaConsulta.ToString("HH:mm")));
                detalle.AppendLine(string.Format(LanguageManager.Translate("veterinario_label"), nombreVeterinario));
                detalle.AppendLine();
                detalle.AppendLine(LanguageManager.Translate("sintomas_titulo"));
                detalle.AppendLine(_consulta.Sintomas ?? LanguageManager.Translate("no_especificado"));
                detalle.AppendLine();
                detalle.AppendLine(LanguageManager.Translate("diagnostico_titulo"));
                detalle.AppendLine(_consulta.Diagnostico ?? LanguageManager.Translate("no_especificado"));
                detalle.AppendLine();
                detalle.AppendLine(LanguageManager.Translate("tratamiento_titulo"));
                detalle.AppendLine(_consulta.Tratamiento ?? LanguageManager.Translate("no_especificado"));
                detalle.AppendLine();

                // Medicamentos
                if (_consulta.Medicamentos != null && _consulta.Medicamentos.Count > 0)
                {
                    detalle.AppendLine(LanguageManager.Translate("medicamentos_recetados_titulo"));
                    foreach (var medicamento in _consulta.Medicamentos)
                    {
                        detalle.AppendLine($"• {medicamento.NombreMedicamento} ({medicamento.Presentacion})");
                        detalle.AppendLine(string.Format("  {0}", string.Format(LanguageManager.Translate("cantidad_label"), medicamento.Cantidad)));
                        if (!string.IsNullOrWhiteSpace(medicamento.Indicaciones))
                        {
                            detalle.AppendLine(string.Format("  {0}", string.Format(LanguageManager.Translate("indicaciones_label"), medicamento.Indicaciones)));
                        }
                        detalle.AppendLine();
                    }
                }
                else
                {
                    detalle.AppendLine(LanguageManager.Translate("medicamentos_titulo"));
                    detalle.AppendLine(LanguageManager.Translate("no_recetaron_medicamentos"));
                    detalle.AppendLine();
                }

                // Observaciones
                if (!string.IsNullOrWhiteSpace(_consulta.Observaciones))
                {
                    detalle.AppendLine(LanguageManager.Translate("observaciones_titulo"));
                    detalle.AppendLine(_consulta.Observaciones);
                }

                txtDetalle.Text = detalle.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_mostrar_detalle")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                // Aquí se podría implementar funcionalidad de impresión
                MessageBox.Show(LanguageManager.Translate("funcionalidad_impresion_no_implementada"),
                    LanguageManager.Translate("informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LanguageManager.Translate("error_imprimir")}: {ex.Message}",
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
