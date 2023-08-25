namespace TestWebApp.Application.Internal
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    internal static class Helpers
    {
        public static readonly char[] B64Charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray();

        public static string RandomBase64String(int length)
        {
            char[] result = new char[length];

            byte[] rnd = new byte[length * 4];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(rnd);

            for (int i = 0; i < length; i++)
            {
                uint offset = BitConverter.ToUInt32(rnd, i * 4);
                int charIdx = (int)(offset % 64);
                result[i] = B64Charset[charIdx];
            }
            return new string(result);
        }

        public static string HashString(string s)
        {
            byte[] rawHash = SHA256.HashData(Encoding.UTF8.GetBytes(s));
            return Convert.ToHexString(rawHash);
        }
    }
}
