using BlockchainBlazorWithE2EDemo.Models;

namespace BlockchainBlazorWithE2EDemo.Factories
{
    public interface IBlockFactory<T>
    {
        Block<T> CreateBlock(T data, byte[]? previousHash, bool findDefaultNonce = true);
    }
}
