using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class AES
    {
        // Clave y IV deben ser de longitud adecuada (16 bytes para AES-128, 32 bytes para AES-256, etc.)
        private static readonly string key = "vamoPeñarolQuer"; // 16 caracteres para AES-128 (128 bits)
        private static readonly string iv = "tAmoLeoFernandez";  // 16 caracteres para AES-128 (128 bits)

        // Función de encriptación
        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

                Console.WriteLine($"Key Length: {keyBytes.Length}");
                Console.WriteLine($"IV Length: {ivBytes.Length}");

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
    }
}
