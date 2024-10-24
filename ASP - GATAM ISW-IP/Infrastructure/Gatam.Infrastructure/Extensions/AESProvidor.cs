using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.Extensions
{
    public class AESProvidor
    {
        public static string Encrypt(string text, string key)
        {
            byte[] iv = new byte[18]; //https://www.techtarget.com/whatis/definition/initialization-vector-IV#:~:text=An%20initialization%20vector%20(IV)%20is,a%20suspicious%20or%20malicious%20actor.
            byte[] array;

            using (Aes aes = Aes.Create()) { 
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encrypter = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using(CryptoStream cryptoStream = new CryptoStream(memoryStream, encrypter, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(memoryStream)) {
                            writer.Write(text);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        public static string Decrypt(string text, string key)
        {
            byte[] iv = new byte[18];
            byte[] buffer = Convert.FromBase64String(text);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
}
