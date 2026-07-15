using GameVerse.API.Data;
using GameVerse.API.Services;
using GameVerse.SHARED.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameVerse.TESTS.Services
{
    public class AuthServiceRegisterTests
    {
        // Construit une configuration factice en mémoire pour tester la logique JWT sans dépendances externes.
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

        // Crée un contexte EF Core en mémoire pour isoler chaque test et éviter toute base réelle.
        private static GameVerseContext BuildInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<GameVerseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new GameVerseContext(options);
        }

        // Vérifie que l'inscription crée un utilisateur avec un mot de passe correctement hashé.
        [Fact]
        public async Task RegisterAsync_ShouldCreateUserWithHashedPassword()
        {

            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();
            var authService = new AuthService(context, config);

            var request = new RegisterRequest
            {
                Username = "delphine",
                Email = "delphine@test.com",
                Password = "MonMotDePasse123"
            };

            var createdUser = await authService.RegisterAsync(request);

            Assert.NotNull(createdUser);
            Assert.Equal("delphine", createdUser.Username);
            Assert.Equal("delphine@test.com", createdUser.Email);

            Assert.NotEqual("MonMotDePasse123", createdUser.PasswordHash);

            Assert.True(BCrypt.Net.BCrypt.Verify("MonMotDePasse123", createdUser.PasswordHash));
        }

        // Vérifie que l'utilisateur nouvellement inscrit est bien enregistré en base de données.
        [Fact]
        public async Task RegisterAsync_ShouldPersistUserInDatabase()
        {
            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();
            var authService = new AuthService(context, config);

            var request = new RegisterRequest
            {
                Username = "gamer42",
                Email = "gamer42@test.com",
                Password = "AutreMotDePasse456"
            };

            await authService.RegisterAsync(request);

            var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Email == "gamer42@test.com");

            Assert.NotNull(userInDb);
            Assert.Equal("gamer42", userInDb.Username);
        }
    }
}