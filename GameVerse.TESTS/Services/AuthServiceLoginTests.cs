using GameVerse.API.Data;
using GameVerse.API.Models;
using GameVerse.API.Services;
using GameVerse.SHARED.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameVerse.TESTS.Services
{
    public class AuthServiceLoginTests
    {
        // Construit une configuration factice en mémoire pour les tests liés à l’authentification (JWT).
        private static IConfiguration BuildFakeConfig()
        {
            var settings = new Dictionary<string, string?>
            {
                { "Jwt:Key", "CleDeTestSuperSecreteEtLongueDau32Caracteres!" },
                { "Jwt:Issuer", "GameVerseTestIssuer" },
                { "Jwt:Audience", "GameVerseTestAudience" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }

        // Crée un contexte EF Core en mémoire pour isoler chaque test sans base de données réelle.
        private static GameVerseContext BuildInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<GameVerseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // nom unique = base isolée par test
                .Options;

            return new GameVerseContext(options);
        }

        // Vérifie qu'une tentative de connexion avec un email inconnu retourne null.
        [Fact]
        public async Task LoginAsync_UnknownEmail_ShouldReturnNull()
        {
            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();
            var authService = new AuthService(context, config);

            var request = new LoginRequest
            {
                Email = "inexistant@test.com",
                Password = "peu importe"
            };

            var result = await authService.LoginAsync(request);

            Assert.Null(result);
        }

        // Vérifie qu'un mot de passe incorrect entraîne un échec de connexion.
        [Fact]
        public async Task LoginAsync_WrongPassword_ShouldReturnNull()
        {
            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();

            context.Users.Add(new User
            {
                UserId = "user-1",
                Username = "delphine",
                Email = "delphine@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("BonMotDePasse123"),
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var authService = new AuthService(context, config);

            var request = new LoginRequest
            {
                Email = "delphine@test.com",
                Password = "MauvaisMotDePasse"
            };

            var result = await authService.LoginAsync(request);

            Assert.Null(result);
        }

        // Vérifie qu'une connexion avec des identifiants valides retourne une réponse d'authentification complète.
        [Fact]
        public async Task LoginAsync_ValidCredentials_ShouldReturnAuthResponse()
        {
            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();

            context.Users.Add(new User
            {
                UserId = "user-2",
                Username = "delphine",
                Email = "delphine@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("BonMotDePasse123"),
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var authService = new AuthService(context, config);

            var request = new LoginRequest
            {
                Email = "delphine@test.com",
                Password = "BonMotDePasse123"
            };

            var result = await authService.LoginAsync(request);

            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.False(string.IsNullOrEmpty(result.RefreshToken));
            Assert.Equal("delphine", result.Username);
        }
    }
}