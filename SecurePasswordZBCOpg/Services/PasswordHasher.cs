using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecurePasswordZBCOpg
{
    public class PasswordHasher
    {
        /// <summary>
        /// Generates a random salt of 16 bytes to be used in password hashing.
        /// </summary>
        /// <returns>A byte array representing the salt.</returns>
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        /// <summary>
        /// Hashes a password using the HMACSHA512 algorithm with a given salt.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <param name="salt">The salt to use in the hashing process.</param>
        /// <returns>A byte array representing the computed hash of the password.</returns>
        public byte[] HashPassword(string password, byte[] salt)
        {
            using (HMACSHA512 hmac = new HMACSHA512(salt))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies whether a given password, when hashed with the provided salt, 
        /// matches the stored hash.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="salt">The salt used in the original hash.</param>
        /// <param name="hash">The stored hash to compare against.</param>
        /// <returns>True if the password, salt, and hash match; otherwise, false.</returns>
        public bool VerifyPassword(string password, byte[] salt, byte[] hash)
        {
            byte[] computedHash = HashPassword(password, salt);
            return computedHash.SequenceEqual(hash);
        }
    }
}
