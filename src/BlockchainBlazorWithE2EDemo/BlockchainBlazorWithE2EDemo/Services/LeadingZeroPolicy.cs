namespace BlockchainBlazorWithE2EDemo.Services
{
    public class LeadingZeroBitsPolicy : IProofOfWorkPolicy
    {
        public int Difficulty { get; }

        public LeadingZeroBitsPolicy(int difficulty)
        {
            Difficulty = difficulty;
        }

        public bool IsValidHash(byte[] hash)
        {
            int bitsChecked = 0;

            foreach (var b in hash)
            {
                for (int bit = 7; bit >= 0; bit--)
                {
                    if ((b & (1 << bit)) != 0)
                        return bitsChecked >= Difficulty;

                    bitsChecked++;
                    if (bitsChecked >= Difficulty)
                        return true;
                }
            }

            return bitsChecked >= Difficulty;
        }
    }
}
