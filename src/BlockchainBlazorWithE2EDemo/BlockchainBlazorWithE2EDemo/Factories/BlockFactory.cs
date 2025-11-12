using BlockchainBlazorWithE2EDemo.Models;
using BlockchainBlazorWithE2EDemo.Services;
using System.Security.Cryptography;

namespace BlockchainBlazorWithE2EDemo.Factories
{
    public class BlockFactory<T> : IBlockFactory<T>
    {
        private readonly ISha256HashService<T> _hasher;
        private readonly IProofOfWorkPolicy? _policy;

        public BlockFactory(ISha256HashService<T> hasher, IProofOfWorkPolicy? policy = null)
        {
            _hasher = hasher;
            _policy = policy;
        }

        public Block<T> CreateBlock(T data, byte[]? previousHash, bool findDefaultNonce = true)
        {
            previousHash ??= [];

            int nonce = 0;
            byte[] hash = _hasher.ComputeHash(previousHash, nonce, data);

            if (!findDefaultNonce || _policy == null)
                return new Block<T>(data, hash, previousHash, nonce);

            // Proof-of-work mining loop
            while (!_policy.IsValidHash(hash))
            {
                nonce++;
                hash = _hasher.ComputeHash(previousHash, nonce, data);

                // Optional: avoid overflow
                if (nonce == int.MaxValue)
                    throw new InvalidOperationException("Unable to find valid nonce within range.");
            }

            return new Block<T>(data, hash, previousHash, nonce);
        }
    }
}
