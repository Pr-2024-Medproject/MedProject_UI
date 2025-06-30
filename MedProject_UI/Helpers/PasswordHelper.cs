using System.Security.Cryptography;

namespace MedProject_UI.Helpers;

internal static class PasswordHelper
{
    private const int SaltSize = 16; // 128 bit
    private const int KeySize = 32; // 256 bit
    private const int Iterations = 10000;

    public static string HashPassword(string password)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var key = pbkdf2.GetBytes(KeySize);
                var hashParts = new byte[SaltSize + KeySize];
                Buffer.BlockCopy(salt, 0, hashParts, 0, SaltSize);
                Buffer.BlockCopy(key, 0, hashParts, SaltSize, KeySize);
                return Convert.ToBase64String(hashParts);
            }
        }
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);

        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
        {
            var key = pbkdf2.GetBytes(KeySize);

            for (var i = 0; i < KeySize; i++)
                if (hashBytes[SaltSize + i] != key[i])
                    return false;
            return true;
        }
    }
}