using GameVerse.API.Data;
using GameVerse.API.Models;
using GameVerse.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameVerse.TESTS.Services
{
    public class AuthServiceEmailExistsTests
    {
        // Construit une configuration factice en mémoire pour les tests liés au JWT.
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

        // Crée un contexte EF Core en mémoire pour exécuter les tests sans base de données réelle.
        private static GameVerseContext BuildInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<GameVerseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new GameVerseContext(options);
        }

        // Vérifie qu'un email connu en base est correctement détecté comme existant.
        [Fact]
        public async Task EmailExists_KnownEmail_ShouldReturnTrue()
        {
            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();

            context.Users.Add(new User
            {
                UserId = "user-1",
                Username = "delphine",
                Email = "delphine@test.com",
                PasswordHash = "peu importe",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var authService = new AuthService(context, config);

            var result = await authService.EmailExists("delphine@test.com");

            Assert.True(result);
        }

        // Vérifie qu'un email absent de la base retourne false.
        [Fact]
        public async Task EmailExists_UnknownEmail_ShouldReturnFalse()
        {
            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();
            var authService = new AuthService(context, config);

            var result = await authService.EmailExists("inconnu@test.com");

            Assert.False(result);
        }
    }
}
