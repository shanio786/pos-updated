using System;
using System.Security.Cryptography;

namespace supershop
{
    /// <summary>
    /// PBKDF2-SHA1 password hashing (built into .NET, no extra package).
    /// Stored format:  PBKDF2$iterations$base64(salt)$base64(hash)
    /// Legacy plain-text passwords still verify and are upgraded on first login.
    /// </summary>
    public static class PasswordHasher
    {
        const string Prefix = "PBKDF2$";
        const int SaltSize = 16;
        const int HashSize = 32;
        const int Iterations = 20000;

        public static string Hash(string password)
        {
            if (password == null) password = "";
            byte[] salt = new byte[SaltSize];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);
            byte[] hash;
            using (Rfc2898DeriveBytes kdf = new Rfc2898DeriveBytes(password, salt, Iterations))
                hash = kdf.GetBytes(HashSize);
            return Prefix + Iterations + "$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(hash);
        }

        public static bool IsHashed(string stored)
        {
            return stored != null && stored.StartsWith(Prefix, StringComparison.Ordinal);
        }

        /// <summary>True when the password matches the stored value (hashed or legacy plain text).</summary>
        public static bool Verify(string password, string stored)
        {
            if (stored == null) return false;
            if (password == null) password = "";
            if (!IsHashed(stored))
                return FixedTimeEquals(System.Text.Encoding.UTF8.GetBytes(stored), System.Text.Encoding.UTF8.GetBytes(password));

            string[] parts = stored.Split('$');
            if (parts.Length != 4) return false;
            int iterations;
            if (!int.TryParse(parts[1], out iterations)) return false;
            byte[] salt, expected;
            try
            {
                salt = Convert.FromBase64String(parts[2]);
                expected = Convert.FromBase64String(parts[3]);
            }
            catch (FormatException) { return false; }

            byte[] actual;
            using (Rfc2898DeriveBytes kdf = new Rfc2898DeriveBytes(password, salt, iterations))
                actual = kdf.GetBytes(expected.Length);
            return FixedTimeEquals(actual, expected);
        }

        static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++) diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
