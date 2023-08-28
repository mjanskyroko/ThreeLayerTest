namespace TestWebApp.Infrastructure.Services.Internal
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using TestWebApp.Application.Contracts.Services;

    internal sealed class PasswordService : IPasswordService
    {
        private const string B64Charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        private static readonly SHA256 hasher = SHA256.Create();
        private static readonly int HashCount = 128;
        private static readonly int DefaultSaltSize = 12;

        private byte[] GenerateSalt(int length)
        {
            byte[] salt = new byte[length];
            rng.GetBytes(salt);
            return salt;
        }

        public byte[] GenerateSalt()
        {
            return GenerateSalt(DefaultSaltSize);
        }

        public string Hash(string password, byte[] salt)
        {
            byte[] passwordUtf8 = Encoding.UTF8.GetBytes(password);
            byte[] block = new byte[salt.Length + passwordUtf8.Length];

            Buffer.BlockCopy(passwordUtf8, 0, block, 0, passwordUtf8.Length);
            Buffer.BlockCopy(salt, 0, block, passwordUtf8.Length, salt.Length);

            byte[] hash = block;
            for (int i = 0; i < HashCount; i++)
                hash = hasher.ComputeHash(hash);

            return Convert.ToBase64String(hash);
        }

        public bool Verify(string password, string guess, string salt)
        {
            if (password == null || guess == null)
                return false;
            string hashedGuess = Hash(guess, Convert.FromBase64String(salt));

            ReadOnlySpan<char> pwSpan = password.AsSpan(0, (password.Last() == '=') ? password.Length - 1 : password.Length);
            ReadOnlySpan<char> guessSpan = hashedGuess.AsSpan(0, (hashedGuess.Last() == '=') ? hashedGuess.Length - 1 : hashedGuess.Length);
            if (pwSpan.Length != guessSpan.Length)
                return false;
            return pwSpan.CompareTo(guessSpan, StringComparison.Ordinal) == 0;
        }
    }
}
