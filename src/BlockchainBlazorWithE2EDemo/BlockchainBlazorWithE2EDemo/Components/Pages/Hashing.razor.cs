using Microsoft.AspNetCore.Components;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainBlazorWithE2EDemo.Components.Pages
{
    public sealed partial class Hashing
    {
        private string _inputText = string.Empty;

        private void OnInputChanged(ChangeEventArgs e)
        {
            _inputText = e.Value?.ToString() ?? string.Empty;
        }

        private static string ComputeHashSHA256(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA256.HashData(bytes);
            return Convert.ToHexString(hashBytes).ToLower();
        }

        private static string ComputeHashSHA3_256(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA3_256.HashData(bytes);
            return Convert.ToHexString(hashBytes).ToLower();
        }

        private static string ComputeHashSHA512(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA512.HashData(bytes);
            return Convert.ToHexString(hashBytes).ToLower();
        }

        private static string ComputeHashSHA3_512(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA3_512.HashData(bytes);
            return Convert.ToHexString(hashBytes).ToLower();
        }

    }
}