using BlockchainBlazorWithE2EDemo.Models;

namespace BlockchainBlazorWithE2EDemo.Services
{
    public class BlockMiningService<T>(ISha256HashService<T> hasher, IProofOfWorkPolicy policy) 
        : IBlockMiningService<T>
    {
        private readonly ISha256HashService<T> _hasher = hasher;
        private readonly IProofOfWorkPolicy _policy = policy;

        public Block<T> MineBlock(Block<T> block)
        {
            int nonce = 0;
            byte[] hash = block.Hash;

            while (!_policy.IsValidHash(hash))
            {
                nonce++;
                hash = _hasher.ComputeHash(block.PreviousHash, nonce, block.Data);

                if (nonce == int.MaxValue)
                    throw new InvalidOperationException("Mining failed: nonce overflow.");
            }

            block.Hash = hash;
            block.Nonce = nonce;

            return block;
        }
    }
}
