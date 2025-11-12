
namespace BlockchainBlazorWithE2EDemo.Models
{
    public record Block<T>
    {
        private static readonly byte[] EmptyHash = new byte[64];

        public byte[] PreviousHash { get; set; }
        public byte[] Hash { get; set; }
        public int Nonce { get; set; }
        public T Data { get; set; }

        public string PreviousHashHexString => Convert.ToHexString(PreviousHash).ToLower();
        public string HashHexString => Convert.ToHexString(Hash).ToLower();

        public Block(T data, byte[] hash, byte[]? previousHash = default, int nonce = default)
        {
            Hash = hash;
            Data = data;
            PreviousHash = previousHash ?? EmptyHash;
            Nonce = nonce;
        }
    }
}
