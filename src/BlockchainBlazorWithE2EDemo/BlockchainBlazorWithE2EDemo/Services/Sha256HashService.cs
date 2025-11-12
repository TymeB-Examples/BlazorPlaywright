using System.Security.Cryptography;
using System.Text;

namespace BlockchainBlazorWithE2EDemo.Services
{
    public class Sha256HashService<T> : ISha256HashService<T>
    {
        public byte[] ComputeHash(byte[] previousHash, int nonce, T data)
        {
            using var sha256 = SHA256.Create();
            var inputBytes = Combine(previousHash, BitConverter.GetBytes(nonce), Serialize(data));
            return sha256.ComputeHash(inputBytes);
        }

        private static byte[] Serialize(T data)
        {
            if (data == null) 
                return [];
            return Encoding.UTF8.GetBytes(data.ToString() ?? string.Empty);
        }

        private static byte[] Combine(params byte[][] arrays)
        {
            var total = arrays.Sum(a => a.Length);
            var result = new byte[total];
            int offset = 0;
            foreach (var arr in arrays)
            {
                Buffer.BlockCopy(arr, 0, result, offset, arr.Length);
                offset += arr.Length;
            }
            return result;
        }
    }
}
