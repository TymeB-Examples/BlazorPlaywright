using BlockchainBlazorWithE2EDemo.Factories;
using BlockchainBlazorWithE2EDemo.Models;
using BlockchainBlazorWithE2EDemo.Services;
using Microsoft.AspNetCore.Components;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainBlazorWithE2EDemo.Components.Pages
{
    public partial class BlockPage
    {
        private Block<string>? _block;
        private bool _isValidBlock = true;

        [Inject]
        protected IBlockFactory<string> BlockFactory { get; set; } = null!;

        [Inject]
        protected IProofOfWorkPolicy ProofOfWorkPolicy { get; set; } = null!;

        [Inject]
        protected IBlockMiningService<string> BlockMiningService { get; set; } = null!;

        protected override void OnInitialized()
        {
            _block = BlockFactory.CreateBlock(string.Empty, null);
            _isValidBlock = ProofOfWorkPolicy.IsValidHash(_block.Hash);
            base.OnInitialized();
        }

        protected void Mine()
        {
            _block = BlockMiningService.MineBlock(_block!);
        }

       
    }
}