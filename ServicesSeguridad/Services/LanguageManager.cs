using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Gestor de traducciones para internacionalización (i18n)
    /// Permite traducir palabras según el idioma/cultura actual del thread
    /// </summary>
    public static class LanguageManager
    {
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
    }
}
