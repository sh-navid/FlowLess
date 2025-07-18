using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System;

namespace NoFlowEngine.Helpers.Security
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords using PBKDF2-SHA256.
    /// </summary>
    public class PasswordHasher
    {
        private const int SaltSize = 128 / 8; // 128 bits, equivalent to 16 bytes
        private const int KeySize = 256 / 8; // 256 bits, equivalent to 32 bytes
        private const int IterationCount = 10000; // Number of iterations for PBKDF2

        /// <summary>
        /// Hashes a password using PBKDF2-SHA256 and generates a new salt.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The generated salt (output parameter).</param>
        /// <returns>The base64-encoded hash of the password.</returns>
        public static string HashPassword(string password, out byte[] salt)
        {
            // Generate a 128-bit salt using a cryptographically secure random number generator.
            salt = RandomNumberGenerator.GetBytes(SaltSize);

            // Hash the password using PBKDF2-SHA256 with the generated salt, iteration count, and key size.
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!, // The password to hash.  The null-forgiving operator is used because the password cannot be null.
                salt: salt, // The salt generated for this password.
                prf: KeyDerivationPrf.HMACSHA256, // The pseudorandom function to use (HMAC-SHA256).
                iterationCount: IterationCount, // The number of iterations to perform.
                numBytesRequested: KeySize)); // The desired key length in bytes.

            return hashed; // Return the base64-encoded hash.
        }

        /// <summary>
        /// Verifies a password against a stored hash and salt.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="hash">The stored hash (base64-encoded).</param>
        /// <param name="salt">The salt used to generate the stored hash.</param>
        /// <returns>True if the password matches the hash; otherwise, false.</returns>
        public static bool VerifyPassword(string password, string hash, byte[] salt)
        {
            // Compute the hash of the provided password using the stored salt and the same PBKDF2-SHA256 parameters.
            string computedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password, // The password to verify.
                salt: salt, // The salt used during the original hashing.
                prf: KeyDerivationPrf.HMACSHA256, // The pseudorandom function (HMAC-SHA256).
                iterationCount: IterationCount, // The iteration count.
                numBytesRequested: KeySize)); // The key length.

            // Compare the computed hash with the stored hash.
            return computedHash == hash; // Return true if they match, indicating the password is correct.
        }
    }
}