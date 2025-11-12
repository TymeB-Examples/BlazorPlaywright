using BlockchainBlazorWithE2EDemo.Models;

namespace BlockchainBlazorWithE2EDemo.Services
{
    public interface IBlockMiningService<T>
    {
        Block<T> MineBlock(Block<T> block);
    }
}
