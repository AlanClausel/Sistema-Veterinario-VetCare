using ServicesSecurity.DomainModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesSecurity.DAL.Implementations
{

    internal sealed class LanguageRepository
    {
        #region Singleton
        private readonly static LanguageRepository _instance = new LanguageRepository();

        public static LanguageRepository Current
        {
            get
            {
                return _instance;
            }
        }

        private LanguageRepository()
        {
            //Implement here the initialization code
        }
        #endregion

        private string basePath = ConfigurationManager.AppSettings["LanguagePath"];

        /// <summary>
        /// Obtiene la ruta del archivo de idioma con fallback
        /// </summary>
        private string GetLanguageFilePath()
        {
            string culturaActual = Thread.CurrentThread.CurrentCulture.Name;
            string rutaCompleta = basePath + "." + culturaActual;

            // Si el archivo existe, usarlo
            if (File.Exists(rutaCompleta))
                return rutaCompleta;

            // Intentar con la cultura padre (ej: es-419 -> es)
            string culturaPadre = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            string rutaCulturaPadre = basePath + "." + culturaPadre;

            if (File.Exists(rutaCulturaPadre))
                return rutaCulturaPadre;

            // Buscar cualquier archivo que empiece con el código de idioma (ej: es-*)
            string directorio = Path.GetDirectoryName(basePath);
            string nombreBase = Path.GetFileName(basePath);

            if (Directory.Exists(directorio))
            {
                var archivos = Directory.GetFiles(directorio, nombreBase + "." + culturaPadre + "*");
                if (archivos.Length > 0)
                    return archivos[0];
            }

            // Fallback: Buscar el primer archivo disponible
            if (Directory.Exists(directorio))
            {
                var todosArchivos = Directory.GetFiles(directorio, nombreBase + ".*");
                if (todosArchivos.Length > 0)
                    return todosArchivos[0];
            }

            // Si no hay ningún archivo, usar el nombre original (lanzará excepción)
            return rutaCompleta;
        }

        public string Find(string word)
        {
            string rutaArchivo = GetLanguageFilePath();

            using (var sr = new StreamReader(rutaArchivo))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split('=');

                    if (line[0] == word)
                    //Encontré la clave buscada...
                    {
                        if (String.IsNullOrEmpty(line[1]))
                            //Aplicar una bitácora...
                            return line[0];

                        return line[1];//Retorno la traducción...
                    }
                }
            }

            throw new WordNotFoundException();
        }

        public void WriteNewWord(string word, string value)
        {
            try
            {
                string filePath = GetLanguageFilePath();

                // Verificar si la palabra ya existe
                bool wordExists = false;
                try
                {
                    Find(word);
                    wordExists = true;
                }
                catch (WordNotFoundException)
                {
                    // La palabra no existe, continuar para agregarla
                }

                if (!wordExists)
                {
                    // Agregar la nueva palabra al final del archivo
                    using (var sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine($"{word}={value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Services.Bitacora.Current.LogException(ex);
            }
        }

        public Dictionary<string, string> FindAll()
        {
            Dictionary<string, string> translations = new Dictionary<string, string>();

            try
            {
                string filePath = GetLanguageFilePath();

                if (!File.Exists(filePath))
                    return translations;

                using (var sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split('=');
                        if (line.Length >= 2)
                        {
                            translations[line[0]] = line[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Services.Bitacora.Current.LogException(ex);
            }

            return translations;
        }

        /// <summary>
        /// Generar una implementación que lea las extensiones de todos mis archivos dentro de I18n
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrentCultures()
        {
            List<string> cultures = new List<string>();

            try
            {
                // Obtener el directorio base de los archivos de idioma
                string directoryPath = Path.GetDirectoryName(basePath);
                string fileName = Path.GetFileName(basePath);

                if (Directory.Exists(directoryPath))
                {
                    // Buscar todos los archivos que coincidan con el patrón
                    string[] files = Directory.GetFiles(directoryPath, fileName + ".*");

                    foreach (string file in files)
                    {
                        // Extraer la cultura del nombre del archivo
                        string extension = Path.GetExtension(file).TrimStart('.');
                        if (!string.IsNullOrEmpty(extension))
                        {
                            cultures.Add(extension);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Services.Bitacora.Current.LogException(ex);
            }

            return cultures;
        }
    }

}
