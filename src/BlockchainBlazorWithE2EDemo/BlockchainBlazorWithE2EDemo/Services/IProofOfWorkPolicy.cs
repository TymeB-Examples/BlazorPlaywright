namespace BlockchainBlazorWithE2EDemo.Services
{
    public interface IProofOfWorkPolicy
    {
        bool IsValidHash(byte[] hash);
        int Difficulty { get; }
    }
}
