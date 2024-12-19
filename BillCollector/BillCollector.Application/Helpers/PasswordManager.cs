using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Application.Helpers
{
    public class PasswordManager
    {
        public static string HashPassword(string password)
        {
            // Generate a salt
            //byte[] salt;
            //new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            //// Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            //var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            //byte[] hash = pbkdf2.GetBytes(32);

            //// Combine the salt and password bytes for later use
            //byte[] hashBytes = new byte[48];
            //Array.Copy(salt, 0, hashBytes, 0, 16);
            //Array.Copy(hash, 0, hashBytes, 16, 32);

            //// Convert to base64
            //string hashedPassword = Convert.ToBase64String(hashBytes);

            //return hashedPassword;
            string encryptedPwd = CryptoHelper.Encrypt(password);
            return encryptedPwd;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // Compare the results
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
