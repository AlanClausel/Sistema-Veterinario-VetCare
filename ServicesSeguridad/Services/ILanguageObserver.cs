using System;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Interfaz del patrón Observer para componentes que necesitan actualizar
    /// su contenido cuando cambia el idioma de la aplicación.
    ///
    /// Cualquier formulario o componente que implemente esta interfaz será
    /// notificado automáticamente cuando se cambie el idioma.
    /// </summary>
    public interface ILanguageObserver
    {
        /// <summary>
        /// Método llamado cuando cambia el idioma de la aplicación.
        /// Los implementadores deben actualizar todos sus textos en este método.
        /// </summary>
        void ActualizarIdioma();
    }
}
