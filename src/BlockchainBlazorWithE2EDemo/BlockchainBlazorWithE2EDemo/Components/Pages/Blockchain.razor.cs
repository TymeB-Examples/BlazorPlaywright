using BlockchainBlazorWithE2EDemo.Models;
using Microsoft.AspNetCore.Components;
using System.Security.Cryptography;

namespace BlockchainBlazorWithE2EDemo.Components.Pages
{
    public partial class Blockchain : ComponentBase
    {
        private readonly Blockchain<string> _chain;

        public Blockchain()
        {
            // Create a demo chain
            _chain = new Blockchain<string>();
            _chain.AddBlock("First block");
            _chain.AddBlock("Second block");
            _chain.AddBlock("Third block");
        }

        protected override void OnInitialized()
        {
        }



        protected void AddBlock()
        {
            _chain.AddBlock(string.Empty);
        }

        protected void CorruptRandom()
        {
            if (_chain.Count <= 1) return;
            var rnd = RandomNumberGenerator.GetInt32(_chain.Count);
            // create a modified block with tampered hash
            var target = _chain[rnd];
            var corruptedHash = (byte[])target.Hash.Clone();
            if (corruptedHash.Length > 0)
                corruptedHash[0] ^= 0xFF; // flip first byte

            var replaced = new Block<string>(target.PreviousHash, corruptedHash, target.Nonce, target.Data);
            _chain[rnd] = replaced;
            StateHasChanged();
        }

        protected byte[] GetPreviousHash(int index)
        {
            if (index == 0) return Array.Empty<byte>();
            return DisplayBlocks[index - 1].Hash;
        }

        protected bool IsChainValid
        {
            get
            {
                for (int i = 1; i < DisplayBlocks.Count; i++)
                {
                    var current = DisplayBlocks[i];
                    var previous = DisplayBlocks[i - 1];
                    if (!current.PreviousHash.SequenceEqual(previous.Hash))
                        return false;
                }
                return true;
            }
        }
    }
}