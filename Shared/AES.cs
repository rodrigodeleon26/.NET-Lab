using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Shared
{
    public class AES
    {
        // Leer las claves y IV de las variables de entorno
        private static readonly string key = Environment.GetEnvironmentVariable("AES_KEY");
        private static readonly string iv = Environment.GetEnvironmentVariable("AES_IV");

        // Función de encriptación
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                Console.WriteLine("AES_KEY: " + key);
                throw new InvalidOperationException("Las claves de encriptación no están configuradas.");
            }

            using (Aes aesAlg = Aes.Create())
            {
                Console.WriteLine("AES_KEY: " + key);
                Console.WriteLine("AES_IV: " + iv);

                aesAlg.Key = Encoding.UTF8.GetBytes(key); // Convierte la clave a bytes
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);   // Convierte el IV a bytes

                // Crea un objeto para realizar el cifrado
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);  // Escribe el texto plano en el flujo
                    }
                    return Convert.ToBase64String(ms.ToArray());  // Convierte el resultado en base64 y lo devuelve
                }
            }
        }

        // Función de desencriptación
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                throw new InvalidOperationException("Las claves de desencriptación no están configuradas.");
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key); // Convierte la clave a bytes
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);   // Convierte el IV a bytes

                // Crea un objeto para realizar el descifrado
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))  // Convierte de base64 a bytes
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();  // Lee y devuelve el texto desencriptado
                }
            }
        }

        public static string TryDecrypt(string cipherText)
        {
            try
            {
                return Decrypt(cipherText);
            }
            catch
            {
                return cipherText;
            }
        }
    }
}
