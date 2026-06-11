using GameVerse.API.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameVerseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    options.Title = "GameVerse API";
    options.Theme = ScalarTheme.Kepler;
});

// Redirection de /openapi vers /scalar/v1 pour tests
app.MapGet("/openapi", () => Results.Redirect("/scalar/v1"));

app.MapControllers();

app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();
