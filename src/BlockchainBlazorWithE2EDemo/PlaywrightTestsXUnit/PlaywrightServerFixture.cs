using BlockchainBlazorWithE2EDemo.Components;
using BlockchainBlazorWithE2EDemo.Factories;
using BlockchainBlazorWithE2EDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTestsXUnit
{
    public class PlaywrightFixture : IAsyncLifetime
    {
        public IPlaywright Playwright { get; private set; }
        public IBrowser Browser { get; private set; }
        public IPage Page { get; private set; }

        private IHost _host;
        internal string BaseUrl { get; private set; }

        public async Task InitializeAsync()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ContentRootPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../BlockchainBlazorWithE2EDemo")
            });

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddSingleton(typeof(IBlockFactory<>), typeof(BlockFactory<>));
            builder.Services.AddSingleton(typeof(ISha256HashService<>), typeof(Sha256HashService<>));
            builder.Services.AddSingleton(typeof(IBlockMiningService<>), typeof(BlockMiningService<>));
            builder.Services.AddSingleton<IProofOfWorkPolicy>(
                new LeadingZeroBitsPolicy(difficulty: 16)
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Use dynamic HTTPS port
            app.Urls.Add("http://127.0.0.1:0");
            app.Urls.Add("http://[::1]:0"); // optional, for IPv6

            await app.StartAsync().ConfigureAwait(false);

            _host = app;

            // Get the actual server URL
            BaseUrl = app.Urls.First();

            // Wait until server is ready
            await WaitForServerAsync(BaseUrl).ConfigureAwait(false);

            // Start Playwright
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync().ConfigureAwait(false);
            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            }).ConfigureAwait(false);

            var context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true
            }).ConfigureAwait(false);

            Page = await context.NewPageAsync().ConfigureAwait(false);

            await Page.GotoAsync(BaseUrl).ConfigureAwait(false);
        }

        private async Task WaitForServerAsync(string url, int timeoutMs = 15000, int delayMs = 500)
        {
            using var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            using var client = new HttpClient(handler);

            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < timeoutMs)
            {
                try
                {
                    var response = await client.GetAsync(url).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                        return;
                }
                catch
                {
                    // server not ready yet
                }

                await Task.Delay(delayMs).ConfigureAwait(false);
            }

            throw new TimeoutException($"Server at {url} did not respond within {timeoutMs}ms.");
        }

        public async Task DisposeAsync()
        {
            if (Browser != null) await Browser.CloseAsync().ConfigureAwait(false);
            Playwright?.Dispose();
            if (_host != null) await _host.StopAsync().ConfigureAwait(false);
            _host?.Dispose();
        }
    }

}
