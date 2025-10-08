using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ServicesSecurity.Services
{
    public static class CryptographyService
    {
        /// <summary>
        /// Genera un hash SHA256 de un texto
        /// IMPORTANTE: Usa Encoding.Unicode (UTF-16) para coincidir con NVARCHAR de SQL Server
        /// </summary>
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
