using GameVerse.API.Data;
using GameVerse.API.Models;
using GameVerse.API.Services;
using GameVerse.SHARED.DTOs.Games;
using Microsoft.EntityFrameworkCore;

namespace GameVerse.TESTS.Services
{
    public class GameServiceTests
    {
        // Crée un contexte EF Core en mémoire pour exécuter les tests sans base de données réelle.
        private static GameVerseContext BuildInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<GameVerseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new GameVerseContext(options);
        }

        // Génère un jeu d'exemple pour simplifier l'initialisation des données de test.
        private static Game SampleGame(string title = "Minecraft") => new()
        {
            Title = title,
            Description = "Sandbox créatif et survie.",
            Genre = "Sandbox",
            ReleaseDate = new DateTime(2011, 11, 18),
            CoverUrl = "images/Minecraft.png",
            Developer = "Mojang",
            Publisher = "Mojang"
        };

        // Vérifie que tous les jeux présents en base sont bien retournés.
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllGames()
        {
            using var context = BuildInMemoryContext();
            context.Games.AddRange(SampleGame("Minecraft"), SampleGame("Hades"));
            await context.SaveChangesAsync();

            var service = new GameService(context);

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        // Vérifie qu'un jeu existant est correctement récupéré par son ID.
        [Fact]
        public async Task GetByIdAsync_ExistingGame_ShouldReturnGame()
        {
            using var context = BuildInMemoryContext();
            var game = SampleGame();
            context.Games.Add(game);
            await context.SaveChangesAsync();

            var service = new GameService(context);

            var result = await service.GetByIdAsync(game.GameId);

            Assert.NotNull(result);
            Assert.Equal("Minecraft", result.Title);
        }

        // Vérifie que la récupération d'un jeu inexistant retourne null.
        [Fact]
        public async Task GetByIdAsync_UnknownGame_ShouldReturnNull()
        {
            using var context = BuildInMemoryContext();
            var service = new GameService(context);

            var result = await service.GetByIdAsync(999);

            Assert.Null(result);
        }

        // Vérifie qu'un jeu est correctement créé et persisté en base.
        [Fact]
        public async Task CreateAsync_ShouldPersistGame()
        {
            using var context = BuildInMemoryContext();
            var service = new GameService(context);

            var dto = new CreateGameDto
            {
                Title = "Hades",
                Description = "Rogue-like dynamique.",
                Genre = "Rogue-like",
                ReleaseDate = new DateTime(2020, 9, 17),
                CoverUrl = "images/Hades.png",
                Developer = "Supergiant Games",
                Publisher = "Supergiant Games"
            };

            var created = await service.CreateAsync(dto);

            Assert.True(created.GameId > 0);

            var inDb = await context.Games.FindAsync(created.GameId);
            Assert.NotNull(inDb);
            Assert.Equal("Hades", inDb.Title);
        }


        // Vérifie qu'un jeu existant est correctement mis à jour avec les nouvelles valeurs.
        [Fact]
        public async Task UpdateAsync_ExistingGame_ShouldUpdateFields()
        {
            using var context = BuildInMemoryContext();
            var game = SampleGame();
            context.Games.Add(game);
            await context.SaveChangesAsync();

            var service = new GameService(context);

            var dto = new UpdateGameDto
            {
                Title = "Minecraft: Updated",
                Description = "Nouvelle description.",
                Genre = "Sandbox",
                ReleaseDate = game.ReleaseDate,
                CoverUrl = game.CoverUrl,
                Developer = game.Developer,
                Publisher = game.Publisher
            };

            var updated = await service.UpdateAsync(game.GameId, dto);

            Assert.NotNull(updated);
            Assert.Equal("Minecraft: Updated", updated.Title);
            Assert.Equal("Nouvelle description.", updated.Description);
        }


        // Vérifie que la mise à jour d'un jeu inexistant retourne null.
        [Fact]
        public async Task UpdateAsync_UnknownGame_ShouldReturnNull()
        {
            using var context = BuildInMemoryContext();
            var service = new GameService(context);

            var dto = new UpdateGameDto
            {
                Title = "Peu importe",
                Genre = "RPG",
                ReleaseDate = DateTime.UtcNow
            };

            var result = await service.UpdateAsync(999, dto);

            Assert.Null(result);
        }

        // Vérifie qu'un jeu présent dans la bibliothèque de l'utilisateur est bien supprimé.
        [Fact]
        public async Task DeleteAsync_ExistingGame_ShouldRemoveIt()
        {
            using var context = BuildInMemoryContext();
            var game = SampleGame();
            context.Games.Add(game);
            await context.SaveChangesAsync();

            var service = new GameService(context);

            var result = await service.DeleteAsync(game.GameId);

            Assert.True(result);
            Assert.Null(await context.Games.FindAsync(game.GameId));
        }

        // Vérifie que la suppression d'un jeu inexistant retourne false.
        [Fact]
        public async Task DeleteAsync_UnknownGame_ShouldReturnFalse()
        {
            using var context = BuildInMemoryContext();
            var service = new GameService(context);

            var result = await service.DeleteAsync(999);

            Assert.False(result);
        }

        // Vérifie que la bibliothèque de l'utilisateur ne retourne que les jeux en relation "Library".
        [Fact]
        public async Task GetUserLibraryAsync_ShouldOnlyReturnLibraryGames_NotWishlist()
        {
            using var context = BuildInMemoryContext();
            var minecraft = SampleGame("Minecraft");
            var hades = SampleGame("Hades");
            context.Games.AddRange(minecraft, hades);
            await context.SaveChangesAsync();

            context.UserGames.AddRange(
                new UserGame { UserId = "user-1", GameId = minecraft.GameId, RelationType = "Library" },
                new UserGame { UserId = "user-1", GameId = hades.GameId, RelationType = "Wishlist" }
            );
            await context.SaveChangesAsync();

            var service = new GameService(context);

            var result = await service.GetUserLibraryAsync("user-1");

            Assert.Single(result);
            Assert.Equal("Minecraft", result.First().Title);
        }

        // Vérifie qu'un jeu en wishlist ne peut pas être retiré de la bibliothèque.
        [Fact]
        public async Task RemoveFromLibraryAsync_WishlistGame_ShouldReturnFalse()
        {
            using var context = BuildInMemoryContext();
            var game = SampleGame();
            context.Games.Add(game);
            await context.SaveChangesAsync();

            context.UserGames.Add(new UserGame
            {
                UserId = "user-1",
                GameId = game.GameId,
                RelationType = "Wishlist"
            });
            await context.SaveChangesAsync();

            var service = new GameService(context);

            var result = await service.RemoveFromLibraryAsync("user-1", game.GameId);

            Assert.False(result);
        }

        // Vérifie qu'un jeu en bibliothèque est bien retiré de celle-ci.
        [Fact]
        public async Task RemoveFromLibraryAsync_LibraryGame_ShouldRemoveIt()
        {
            using var context = BuildInMemoryContext();
            var game = SampleGame();
            context.Games.Add(game);
            await context.SaveChangesAsync();

            context.UserGames.Add(new UserGame
            {
                UserId = "user-1",
                GameId = game.GameId,
                RelationType = "Library"
            });
            await context.SaveChangesAsync();

            var service = new GameService(context);

            var result = await service.RemoveFromLibraryAsync("user-1", game.GameId);

            Assert.True(result);
            Assert.Empty(context.UserGames);
        }
    }
}