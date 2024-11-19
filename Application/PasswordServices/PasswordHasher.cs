using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.PasswordServices
{
    public class PasswordHasher
    {
        public static string HashPassword(string password, out byte[] salt)
        {
            // Generate a cryptographic random number.
            using var rng = RandomNumberGenerator.Create();
            salt = new byte[16];
            rng.GetBytes(salt);
            // Combine the salt and the password.
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string password, string storedHash, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                string computedHash = Convert.ToBase64String(hash);
                return computedHash == storedHash;
            }
        }
    }
}