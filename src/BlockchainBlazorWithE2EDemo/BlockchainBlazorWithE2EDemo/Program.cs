using BlockchainBlazorWithE2EDemo.Components;
using BlockchainBlazorWithE2EDemo.Factories;
using BlockchainBlazorWithE2EDemo.Services;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();

public partial class Program { }
