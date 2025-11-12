using BlockchainBlazorWithE2EDemo.Factories;
using BlockchainBlazorWithE2EDemo.Models;
using BlockchainBlazorWithE2EDemo.Services;
using Microsoft.AspNetCore.Components;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainBlazorWithE2EDemo.Components.Pages
{
    public class BlockBase : ComponentBase
    {
        private Block<string>? _block;

        [Inject]
        protected IBlockFactory<string> BlockFactory { get; set; } = null!;

        [Inject]
        protected IProofOfWorkPolicy ProofOfWorkPolicy { get; set; } = null!;

        protected override void OnInitialized()
        {
            _block = BlockFactory.CreateBlock(string.Empty, null);
            base.OnInitialized();
        }

        protected async Task MineAsync()
        {
            
        }

       
    }
}