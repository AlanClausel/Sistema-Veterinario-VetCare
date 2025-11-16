using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Servicio para realizar operaciones de Backup y Restore de bases de datos
    /// Singleton pattern
    /// </summary>
    public sealed class BackupRestoreService
    {
        #region Singleton
        private static readonly BackupRestoreService _instance = new BackupRestoreService();

        public static BackupRestoreService Current
        {
            get { return _instance; }
        }

        private BackupRestoreService()
        {
        }
        #endregion

        #region Backup Operations

        /// <summary>
        /// Realiza un backup de la base de datos SecurityVet
        /// </summary>
        /// <param name="rutaDestino">Ruta completa del archivo .bak a crear</param>
        /// <returns>True si el backup fue exitoso</returns>
        public bool RealizarBackupSecurity(string rutaDestino)
        {
            return RealizarBackup("SecurityVet", rutaDestino, "ServicesConString");
        }

        /// <summary>
        /// Realiza un backup de la base de datos VetCareDB
        /// </summary>
        /// <param name="rutaDestino">Ruta completa del archivo .bak a crear</param>
        /// <returns>True si el backup fue exitoso</returns>
        public bool RealizarBackupNegocio(string rutaDestino)
        {
            return RealizarBackup("VetCareDB", rutaDestino, "VetCareConString");
        }

        /// <summary>
        /// Realiza un backup de una base de datos específica
        /// </summary>
        /// <param name="nombreBD">Nombre de la base de datos</param>
        /// <param name="rutaDestino">Ruta completa del archivo .bak</param>
        /// <param name="connectionStringName">Nombre del connection string en App.config</param>
        /// <returns>True si el backup fue exitoso</returns>
        private bool RealizarBackup(string nombreBD, string rutaDestino, string connectionStringName)
        {
            try
            {
                // Validar parámetros
                if (string.IsNullOrWhiteSpace(nombreBD))
                    throw new ArgumentException("El nombre de la base de datos es obligatorio");

                if (string.IsNullOrWhiteSpace(rutaDestino))
                    throw new ArgumentException("La ruta de destino es obligatoria");

                // Validar que la ruta de destino sea válida
                string directorio = Path.GetDirectoryName(rutaDestino);
                if (!Directory.Exists(directorio))
                {
                    throw new DirectoryNotFoundException($"El directorio no existe: {directorio}");
                }

                // Validar extensión .bak
                if (!rutaDestino.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
                {
                    rutaDestino += ".bak";
                }

                // Obtener connection string
                string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

                // Comando SQL para BACKUP
                string backupCommand = $@"
                    BACKUP DATABASE [{nombreBD}]
                    TO DISK = N'{rutaDestino}'
                    WITH FORMAT, INIT,
                    NAME = N'{nombreBD}-Full Database Backup',
                    SKIP, NOREWIND, NOUNLOAD, STATS = 10";

                // Ejecutar backup
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(backupCommand, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 600; // 10 minutos timeout

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Registrar en bitácora
                var usuario = LoginService.GetUsuarioLogueado();
                if (usuario != null)
                {
                    Bitacora.Current.RegistrarEvento(
                        usuario.IdUsuario,
                        usuario.Nombre,
                        "Sistema",
                        "Backup Realizado",
                        $"Backup de {nombreBD} creado exitosamente en: {rutaDestino}",
                        "Informativo");
                }

                Bitacora.Current.LogInfo($"Backup de {nombreBD} completado exitosamente: {rutaDestino}");

                return true;
            }
            catch (Exception ex)
            {
                // Registrar error en bitácora
                var usuario = LoginService.GetUsuarioLogueado();
                Bitacora.Current.RegistrarError(
                    $"Error al realizar backup de {nombreBD}: {ex.Message}",
                    usuario?.IdUsuario,
                    usuario?.Nombre ?? "Sistema",
                    "Sistema");

                Bitacora.Current.LogError($"Error en backup de {nombreBD}: {ex.Message}");
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        #endregion

        #region Restore Operations

        /// <summary>
        /// Restaura la base de datos SecurityVet desde un archivo de backup
        /// </summary>
        /// <param name="rutaArchivo">Ruta completa del archivo .bak</param>
        /// <returns>True si el restore fue exitoso</returns>
        public bool RealizarRestoreSecurity(string rutaArchivo)
        {
            return RealizarRestore("SecurityVet", rutaArchivo, "ServicesConString");
        }

        /// <summary>
        /// Restaura la base de datos VetCareDB desde un archivo de backup
        /// </summary>
        /// <param name="rutaArchivo">Ruta completa del archivo .bak</param>
        /// <returns>True si el restore fue exitoso</returns>
        public bool RealizarRestoreNegocio(string rutaArchivo)
        {
            return RealizarRestore("VetCareDB", rutaArchivo, "VetCareConString");
        }

        /// <summary>
        /// Restaura una base de datos desde un archivo de backup
        /// </summary>
        /// <param name="nombreBD">Nombre de la base de datos</param>
        /// <param name="rutaArchivo">Ruta completa del archivo .bak</param>
        /// <param name="connectionStringName">Nombre del connection string en App.config</param>
        /// <returns>True si el restore fue exitoso</returns>
        private bool RealizarRestore(string nombreBD, string rutaArchivo, string connectionStringName)
        {
            try
            {
                // Validar parámetros
                if (string.IsNullOrWhiteSpace(nombreBD))
                    throw new ArgumentException("El nombre de la base de datos es obligatorio");

                if (string.IsNullOrWhiteSpace(rutaArchivo))
                    throw new ArgumentException("La ruta del archivo es obligatoria");

                // Validar que el archivo existe
                if (!File.Exists(rutaArchivo))
                {
                    throw new FileNotFoundException($"El archivo de backup no existe: {rutaArchivo}");
                }

                // Obtener connection string y modificarlo para conectarse a master
                string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.InitialCatalog = "master"; // Conectarse a master para poder poner la BD en single user
                string masterConnectionString = builder.ConnectionString;

                using (SqlConnection conn = new SqlConnection(masterConnectionString))
                {
                    conn.Open();

                    // Paso 1: Poner la base de datos en modo SINGLE_USER para desconectar usuarios
                    string setSingleUserCommand = $@"
                        IF EXISTS (SELECT name FROM sys.databases WHERE name = N'{nombreBD}')
                        BEGIN
                            ALTER DATABASE [{nombreBD}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        END";

                    using (SqlCommand cmd = new SqlCommand(setSingleUserCommand, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                    }

                    // Paso 2: Restaurar la base de datos
                    string restoreCommand = $@"
                        RESTORE DATABASE [{nombreBD}]
                        FROM DISK = N'{rutaArchivo}'
                        WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10";

                    using (SqlCommand cmd = new SqlCommand(restoreCommand, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 600; // 10 minutos timeout
                        cmd.ExecuteNonQuery();
                    }

                    // Paso 3: Volver a poner la base de datos en modo MULTI_USER
                    string setMultiUserCommand = $@"
                        ALTER DATABASE [{nombreBD}] SET MULTI_USER";

                    using (SqlCommand cmd = new SqlCommand(setMultiUserCommand, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                    }
                }

                // Registrar en bitácora
                var usuario = LoginService.GetUsuarioLogueado();
                if (usuario != null)
                {
                    Bitacora.Current.RegistrarEvento(
                        usuario.IdUsuario,
                        usuario.Nombre,
                        "Sistema",
                        "Restore Realizado",
                        $"Restore de {nombreBD} completado exitosamente desde: {rutaArchivo}",
                        "Advertencia"); // Advertencia porque es una operación crítica
                }

                Bitacora.Current.LogWarning($"Restore de {nombreBD} completado exitosamente: {rutaArchivo}");

                return true;
            }
            catch (Exception ex)
            {
                // Registrar error en bitácora
                var usuario = LoginService.GetUsuarioLogueado();
                Bitacora.Current.RegistrarError(
                    $"Error al realizar restore de {nombreBD}: {ex.Message}",
                    usuario?.IdUsuario,
                    usuario?.Nombre ?? "Sistema",
                    "Sistema");

                Bitacora.Current.LogError($"Error en restore de {nombreBD}: {ex.Message}");
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Verifica si un archivo de backup es válido
        /// </summary>
        /// <param name="rutaArchivo">Ruta del archivo .bak</param>
        /// <returns>True si el archivo existe y es válido</returns>
        public bool ValidarArchivoBackup(string rutaArchivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rutaArchivo))
                    return false;

                if (!File.Exists(rutaArchivo))
                    return false;

                // Validar extensión
                if (!rutaArchivo.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
                    return false;

                // Validar que el archivo no esté vacío
                FileInfo fileInfo = new FileInfo(rutaArchivo);
                if (fileInfo.Length == 0)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogWarning($"Error al validar archivo de backup: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verifica si un directorio es válido para guardar backups
        /// </summary>
        /// <param name="rutaDirectorio">Ruta del directorio</param>
        /// <returns>True si el directorio existe y tiene permisos de escritura</returns>
        public bool ValidarDirectorioDestino(string rutaDirectorio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rutaDirectorio))
                    return false;

                if (!Directory.Exists(rutaDirectorio))
                    return false;

                // Intentar crear un archivo temporal para verificar permisos
                string archivoTemporal = Path.Combine(rutaDirectorio, $"test_{Guid.NewGuid()}.tmp");
                File.WriteAllText(archivoTemporal, "test");
                File.Delete(archivoTemporal);

                return true;
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogWarning($"Error al validar directorio de destino: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
