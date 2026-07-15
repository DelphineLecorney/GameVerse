using GameVerse.API.Models;
using GameVerse.API.Services;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace GameVerse.TESTS.Services
{
    public class AuthServiceTests
    {
        // Construit une configuration factice en mémoire pour tester la génération de JWT.
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

        // Vérifie que le JWT généré contient bien les claims attendus pour l'utilisateur.
        [Fact]
        public void GenerateJwtToken_ShouldContainCorrectClaims()
        {
            var config = BuildFakeConfig();
            var authService = new AuthService(context: null!, config: config);

            var user = new User
            {
                UserId = "user-123",
                Username = "delphine",
                Email = "delphine@test.com",
                Role = "User"
            };

            var token = authService.GenerateJwtToken(user);

            Assert.False(string.IsNullOrEmpty(token));

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            Assert.Equal("user-123", jwt.Claims.First(c => c.Type == "sub").Value);
            Assert.Equal("delphine@test.com", jwt.Claims.First(c => c.Type == "email").Value);
            Assert.Equal("delphine", jwt.Claims.First(c => c.Type == "username").Value);
        }

        // Vérifie que le JWT généré possède une date d’expiration valide et future.
        [Fact]
        public void GenerateJwtToken_ShouldHaveFutureExpiration()
        {
            var config = BuildFakeConfig();
            var authService = new AuthService(context: null!, config: config);

            var user = new User
            {
                UserId = "user-456",
                Username = "test",
                Email = "test@test.com",
                Role = "User"
            };

            var token = authService.GenerateJwtToken(user);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            Assert.True(jwt.ValidTo > DateTime.UtcNow);
        }
    }
}
