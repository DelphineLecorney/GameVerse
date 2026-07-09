using GameVerse.WEB;
using GameVerse.WEB.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<AuthState>();
builder.Services.AddScoped<AuthHeaderHandler>();

builder.Services.AddHttpClient("GameVerse.API", client =>
        client.BaseAddress = new Uri("https://localhost:7046/"))
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("GameVerse.API"));

builder.Services.AddAuthorizationCore();


builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IGameService, GameService>();

await builder.Build().RunAsync();