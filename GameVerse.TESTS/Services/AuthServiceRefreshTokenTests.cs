using GameVerse.API.Data;
using GameVerse.API.Models;
using GameVerse.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameVerse.TESTS.Services
{
    public class AuthServiceRefreshTokenTests
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

        // Vérifie qu’un token inconnu retourne null lors d’une tentative de rafraîchissement.
        [Fact]
        public async Task RefreshTokenAsync_UnknownToken_ShouldReturnNull()
        {
            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();
            var authService = new AuthService(context, config);

            var result = await authService.RefreshTokenAsync("token-qui-nexiste-pas");

            Assert.Null(result);
        }


        // Vérifie qu’un token révoqué ne peut pas être utilisé pour obtenir de nouveaux tokens.
        [Fact]
        public async Task RefreshTokenAsync_RevokedToken_ShouldReturnNull()
        {
            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();

            var user = new User
            {
                UserId = "user-1",
                Username = "delphine",
                Email = "delphine@test.com",
                PasswordHash = "peu importe",
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(user);

            context.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.UserId,
                Token = "token-revoque",
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = true
            });
            await context.SaveChangesAsync();

            var authService = new AuthService(context, config);

            var result = await authService.RefreshTokenAsync("token-revoque");

            Assert.Null(result);
        }

        // Vérifie qu’un token expiré ne peut pas être rafraîchi.
        [Fact]
        public async Task RefreshTokenAsync_ExpiredToken_ShouldReturnNull()
        {
            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();

            var user = new User
            {
                UserId = "user-2",
                Username = "gamer",
                Email = "gamer@test.com",
                PasswordHash = "peu importe",
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(user);

            context.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.UserId,
                Token = "token-expire",
                ExpiresAt = DateTime.UtcNow.AddDays(-1),
                CreatedAt = DateTime.UtcNow.AddDays(-8),
                IsRevoked = false
            });
            await context.SaveChangesAsync();

            var authService = new AuthService(context, config);

            var result = await authService.RefreshTokenAsync("token-expire");

            Assert.Null(result);
        }

        // Vérifie qu’un token valide génère de nouveaux tokens et que l’ancien est correctement révoqué.
        [Fact]
        public async Task RefreshTokenAsync_ValidToken_ShouldReturnNewTokensAndRevokeOldOne()
        {
            using var context = BuildInMemoryContext();
            var config = BuildFakeConfig();

            var user = new User
            {
                UserId = "user-3",
                Username = "valid-user",
                Email = "valid@test.com",
                PasswordHash = "peu importe",
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(user);

            var oldToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = "token-valide",
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };
            context.RefreshTokens.Add(oldToken);
            await context.SaveChangesAsync();

            var authService = new AuthService(context, config);

            var result = await authService.RefreshTokenAsync("token-valide");

            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.False(string.IsNullOrEmpty(result.RefreshToken));
            Assert.NotEqual("token-valide", result.RefreshToken);

            var oldTokenInDb = await context.RefreshTokens.FirstAsync(t => t.Token == "token-valide");
            Assert.True(oldTokenInDb.IsRevoked);

            var newTokenInDb = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == result.RefreshToken);
            Assert.NotNull(newTokenInDb);
            Assert.False(newTokenInDb.IsRevoked);
        }
    }
}
