using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class EncryptionHelper
    {
        private static readonly string encryptionKey = "SeVieneLaSexta";

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return null;

            using (Aes aes = Aes.Create())
            {
                var key = Encoding.UTF8.GetBytes(encryptionKey.PadRight(32)); // 32 bytes key for AES-256
                aes.Key = key;
                aes.IV = new byte[16]; // 16 bytes IV for AES

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return null;

            using (Aes aes = Aes.Create())
            {
                var key = Encoding.UTF8.GetBytes(encryptionKey.PadRight(32));
                aes.Key = key;
                aes.IV = new byte[16];

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
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
