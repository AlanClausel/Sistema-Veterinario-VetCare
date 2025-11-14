using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Gestor de traducciones para internacionalización (i18n) implementando el patrón Observer.
    /// Permite traducir palabras según el idioma/cultura actual del thread y notifica
    /// a todos los observers registrados cuando cambia el idioma.
    /// </summary>
    public static class LanguageManager
    {
        // Lista de observers suscritos (thread-safe)
        private static readonly List<ILanguageObserver> _observers = new List<ILanguageObserver>();
        private static readonly object _lock = new object();

        /// <summary>
        /// Traduce una palabra según el idioma actual (Thread.CurrentThread.CurrentCulture)
        /// Si la palabra no existe, la agrega al archivo y retorna la palabra original
        /// </summary>
        /// <param name="word">Palabra clave a traducir</param>
        /// <returns>Traducción de la palabra o la palabra original si no existe</returns>
        public static string Translate(string word)
        {
            return BLL.LanguageBLL.Translate(word);
        }

        /// <summary>
        /// Obtiene todas las traducciones del archivo de idioma actual
        /// </summary>
        /// <returns>Diccionario con todas las palabras y sus traducciones</returns>
        public static Dictionary<string, string> GetAllTranslations()
        {
            return DAL.Factory.ServiceFactory.LanguageRepository.FindAll();
        }

        /// <summary>
        /// Obtiene la lista de culturas/idiomas disponibles
        /// </summary>
        /// <returns>Lista de códigos de cultura (ej: "es-AR", "en-GB")</returns>
        public static List<string> GetAvailableLanguages()
        {
            return DAL.Factory.ServiceFactory.LanguageRepository.GetCurrentCultures();
        }

        #region Patrón Observer

        /// <summary>
        /// Suscribe un observer para recibir notificaciones cuando cambie el idioma.
        /// Thread-safe: puede llamarse desde cualquier thread.
        /// </summary>
        /// <param name="observer">Observer a suscribir</param>
        public static void Subscribe(ILanguageObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            lock (_lock)
            {
                if (!_observers.Contains(observer))
                {
                    _observers.Add(observer);
                }
            }
        }

        /// <summary>
        /// Desuscribe un observer para que deje de recibir notificaciones.
        /// Thread-safe: puede llamarse desde cualquier thread.
        /// </summary>
        /// <param name="observer">Observer a desuscribir</param>
        public static void Unsubscribe(ILanguageObserver observer)
        {
            if (observer == null)
                return;

            lock (_lock)
            {
                _observers.Remove(observer);
            }
        }

        /// <summary>
        /// Cambia el idioma de la aplicación y notifica a todos los observers.
        /// Este método actualiza Thread.CurrentThread.CurrentCulture y CurrentUICulture.
        /// </summary>
        /// <param name="codigoCultura">Código de cultura (ej: "es-AR", "en-GB")</param>
        /// <exception cref="ArgumentException">Si el código de cultura no es válido</exception>
        public static void CambiarIdioma(string codigoCultura)
        {
            if (string.IsNullOrWhiteSpace(codigoCultura))
                throw new ArgumentException("El código de cultura no puede ser vacío", nameof(codigoCultura));

            try
            {
                // Cambiar la cultura del thread actual
                CultureInfo nuevaCultura = new CultureInfo(codigoCultura);
                Thread.CurrentThread.CurrentCulture = nuevaCultura;
                Thread.CurrentThread.CurrentUICulture = nuevaCultura;

                // Notificar a todos los observers
                NotificarCambioIdioma();

                // Log del cambio
                Bitacora.Current.LogInfo($"Idioma cambiado a: {codigoCultura}");
            }
            catch (CultureNotFoundException ex)
            {
                throw new ArgumentException($"Código de cultura inválido: {codigoCultura}", nameof(codigoCultura), ex);
            }
        }

        /// <summary>
        /// Notifica a todos los observers registrados que el idioma ha cambiado.
        /// Los observers deben actualizar sus textos en el método ActualizarIdioma().
        /// </summary>
        private static void NotificarCambioIdioma()
        {
            ILanguageObserver[] observersCopy;

            // Copiar lista para evitar problemas si un observer se desuscribe durante la notificación
            lock (_lock)
            {
                observersCopy = _observers.ToArray();
            }

            // Notificar a cada observer (fuera del lock para evitar deadlocks)
            foreach (var observer in observersCopy)
            {
                try
                {
                    observer.ActualizarIdioma();
                }
                catch (Exception ex)
                {
                    // Si un observer falla, continuar notificando a los demás
                    Bitacora.Current.LogError($"Error al notificar observer: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Obtiene la cantidad de observers suscritos actualmente.
        /// Útil para debugging y testing.
        /// </summary>
        /// <returns>Número de observers suscritos</returns>
        public static int ContarObservers()
        {
            lock (_lock)
            {
                return _observers.Count;
            }
        }

        #endregion
    }
}
