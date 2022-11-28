using System;
using System.Security.Cryptography;
using System.Text;

namespace BPLog.API.Extensions
{
    public static class StringExtensions
    {
        public static string ToSha256(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
