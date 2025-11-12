namespace BlockchainBlazorWithE2EDemo.Services
{
    public interface ISha256HashService<T>
    {
        byte[] ComputeHash(byte[] previousHash, int nonce, T data);
    }
}
