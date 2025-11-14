using System;
using System.Windows.Forms;
using ServicesSecurity.Services;

namespace UI.WinUi
{
    /// <summary>
    /// Clase base para formularios que necesitan actualizar su contenido
    /// cuando cambia el idioma de la aplicación.
    ///
    /// Implementa el patrón Observer suscribiéndose automáticamente a
    /// LanguageManager en el evento Load y desuscribiéndose en FormClosing.
    ///
    /// Los formularios derivados deben implementar el método abstracto
    /// ActualizarTextos() donde actualizan todos sus controles.
    /// </summary>
    public abstract class BaseObservableForm : Form, ILanguageObserver
    {
        private bool _suscrito = false;

        /// <summary>
        /// Constructor base que registra eventos para suscripción automática
        /// </summary>
        protected BaseObservableForm()
        {
            // Suscribirse cuando el formulario se carga
            this.Load += BaseObservableForm_Load;

            // Desuscribirse cuando el formulario se cierra
            this.FormClosing += BaseObservableForm_FormClosing;
        }

        /// <summary>
        /// Suscribe el formulario al LanguageManager cuando se carga
        /// </summary>
        private void BaseObservableForm_Load(object sender, EventArgs e)
        {
            if (!_suscrito)
            {
                LanguageManager.Subscribe(this);
                _suscrito = true;
            }

            // Llamar a ActualizarTextos inicial
            ActualizarTextos();
        }

        /// <summary>
        /// Desuscribe el formulario del LanguageManager cuando se cierra
        /// </summary>
        private void BaseObservableForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_suscrito)
            {
                LanguageManager.Unsubscribe(this);
                _suscrito = false;
            }
        }

        /// <summary>
        /// Implementación de ILanguageObserver.
        /// Se invoca automáticamente cuando cambia el idioma.
        /// Delega a ActualizarTextos() que debe ser implementado por las clases derivadas.
        /// </summary>
        public void ActualizarIdioma()
        {
            // Asegurarse de que se ejecuta en el thread de la UI
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ActualizarTextos()));
            }
            else
            {
                ActualizarTextos();
            }
        }

        /// <summary>
        /// Método abstracto que deben implementar las clases derivadas.
        /// Aquí se deben actualizar todos los textos de los controles del formulario.
        ///
        /// Ejemplo:
        /// <code>
        /// protected override void ActualizarTextos()
        /// {
        ///     this.Text = LanguageManager.Translate("titulo_formulario");
        ///     btnGuardar.Text = LanguageManager.Translate("guardar");
        ///     lblNombre.Text = LanguageManager.Translate("nombre");
        /// }
        /// </code>
        /// </summary>
        protected abstract void ActualizarTextos();

        /// <summary>
        /// Limpia recursos cuando el formulario se destruye
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _suscrito)
            {
                LanguageManager.Unsubscribe(this);
                _suscrito = false;
            }
            base.Dispose(disposing);
        }
    }
}
