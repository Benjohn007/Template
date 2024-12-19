using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Core
{
    public static class PasswordManager
    {
        const string ENCRYPTION_KEY = "TzDjFYvMVQylHXpY";
        const string ENCRYPTION_IV = "Q7ptfAFg57cYacq5";

        public static string Encrypt(string text, string key=ENCRYPTION_KEY, string iv = ENCRYPTION_IV)
        {
            byte[] result = null;
            byte[] wordBytes = Encoding.UTF8.GetBytes(text);
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.Padding = PaddingMode.PKCS7;

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = Encoding.UTF8.GetBytes(key);
                    AES.IV = Encoding.UTF8.GetBytes(iv);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(wordBytes, 0, wordBytes.Length);
                        cs.Close();
                    }
                    byte[] encryptedBytes = ms.ToArray();
                    result = encryptedBytes;
                    return ByteArrayToString(encryptedBytes);
                }
            }
        }
        public static string Decrypt(string text, string key = ENCRYPTION_KEY, string iv = ENCRYPTION_IV)
        {
            try
            {
                byte[] wordBytes = StringToByteArray(text);
                byte[] byteBuffer = new byte[wordBytes.Length];
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.Padding = PaddingMode.PKCS7;

                        AES.KeySize = 256;
                        AES.BlockSize = 128;
                        AES.Key = Encoding.UTF8.GetBytes(key);
                        AES.IV = Encoding.UTF8.GetBytes(iv);
                        //AES.Padding = PaddingMode.Zeros;

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(wordBytes, 0, wordBytes.Length);
                            cs.Close();
                        }
                        byte[] decryptedBytes = ms.ToArray();
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b).ToString().ToLower();
            return hex.ToString();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
        }
    }
}
