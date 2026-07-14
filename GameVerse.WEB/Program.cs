using GameVerse.WEB;
using GameVerse.WEB.Services;
using GameVerse.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7046/";

builder.Services.AddSingleton<AuthState>();
builder.Services.AddScoped<AuthHeaderHandler>();

// Client principal, avec le handler qui attache le token et gère le refresh
builder.Services.AddHttpClient("GameVerse.API", client =>
        client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler<AuthHeaderHandler>();

// Client "brut", sans handler, utilisé uniquement pour l'appel de refresh
// (évite la boucle infinie si le refresh renvoie lui-même un 401)
builder.Services.AddHttpClient("GameVerse.API.Raw", client =>
    client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("GameVerse.API"));

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserGameService, UserGameService>();
builder.Services.AddScoped<IStatsService, StatsService>();


await builder.Build().RunAsync();