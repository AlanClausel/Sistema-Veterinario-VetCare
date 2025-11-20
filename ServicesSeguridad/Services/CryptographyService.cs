using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ServicesSecurity.Services
{
    /// <summary>
    /// Servicio estático de criptografía para operaciones de hash y seguridad.
    /// Proporciona funciones de hashing seguro usando SHA256 para contraseñas y verificación de integridad.
    /// Todas las operaciones están optimizadas para compatibilidad con SQL Server.
    /// </summary>
    /// <remarks>
    /// IMPORTANTE:
    /// - Utiliza SHA256 como algoritmo de hash (considerado seguro para uso general)
    /// - Usa Encoding.Unicode (UTF-16) para compatibilidad con campos NVARCHAR de SQL Server
    /// - Retorna hash en formato hexadecimal mayúsculas para coincidir con la función HASHBYTES de SQL Server
    /// - El hash generado es determinístico (mismo texto = mismo hash)
    /// - No utiliza salt, por lo que es vulnerable a rainbow tables (considerar mejoras futuras)
    /// </remarks>
    public static class CryptographyService
    {
        /// <summary>
        /// Genera un hash SHA256 de un texto en formato hexadecimal compatible con SQL Server.
        /// Utilizado principalmente para hashear contraseñas y calcular dígitos verificadores (DVH).
        /// </summary>
        /// <param name="textPlainPass">Texto plano a hashear (contraseña, texto de verificación, etc.)</param>
        /// <returns>
        /// Hash SHA256 en formato hexadecimal mayúsculas (64 caracteres).
        /// Ejemplo: "5E884898DA28047151D0E56F8DC6292773603D0D6AABBDD62A11EF721D1542D8"
        /// </returns>
        /// <remarks>
        /// IMPORTANTE:
        /// - Usa Encoding.Unicode (UTF-16 LE) para que coincida con NVARCHAR de SQL Server
        /// - El formato de salida (X2 mayúsculas) coincide con HASHBYTES('SHA2_256', ...) de SQL Server
        /// - Este método es compatible con la validación de contraseñas hasheadas en BD
        /// - El hash resultante siempre tiene 64 caracteres hexadecimales (256 bits)
        ///
        /// CONSIDERACIONES DE SEGURIDAD:
        /// - No utiliza salt, por lo que es vulnerable a ataques de diccionario/rainbow tables
        /// - Para producción, considerar usar algoritmos más seguros como bcrypt o Argon2
        /// - El hash es determinístico: mismo input = mismo output
        /// </remarks>
        /// <example>
        /// // Hashear una contraseña
        /// string passwordHash = CryptographyService.HashPassword("miContraseña123");
        ///
        /// // Verificar una contraseña comparando hashes
        /// string inputHash = CryptographyService.HashPassword(userInput);
        /// bool esValida = (inputHash == storedHash);
        ///
        /// // Calcular DVH (Dígito Verificador Horizontal)
        /// string dvh = CryptographyService.HashPassword(nombre + email + otrosCampos);
        /// </example>
        /// <exception cref="ArgumentNullException">Si textPlainPass es null</exception>
        /// <exception cref="CryptographicException">Si ocurre un error durante el proceso de hashing</exception>
        public static string HashPassword(string textPlainPass)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 sha256 = SHA256.Create())
            {
                // IMPORTANTE: Usar Encoding.Unicode (UTF-16) para coincidir con NVARCHAR de SQL Server
                byte[] retVal = sha256.ComputeHash(Encoding.Unicode.GetBytes(textPlainPass));

                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("X2")); // X mayúscula para coincidir con SQL Server
                }
            }

            return sb.ToString();
        }
    }
}
