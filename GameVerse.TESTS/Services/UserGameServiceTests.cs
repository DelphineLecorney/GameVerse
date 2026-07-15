using GameVerse.API.Data;
using GameVerse.API.Models;
using GameVerse.API.Services;
using GameVerse.SHARED.DTOs.UserGame;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GameVerse.TESTS.Services
{
    public class UserGameServiceTests
    {
        // Construit un contexte EF Core en mémoire pour isoler les tests et éviter toute base réelle.
        private static GameVerseContext BuildInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<GameVerseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new GameVerseContext(options);
        }

        // Ajoute un jeu de test dans la base en mémoire afin de disposer de données cohérentes pour les scénarios.
        private static async Task<Game> SeedGame(GameVerseContext context, string title = "Minecraft")
        {
            var game = new Game
            {
                Title = title,
                Description = "Sandbox créatif et survie.",
                Genre = "Sandbox",
                ReleaseDate = new DateTime(2011, 11, 18),
                CoverUrl = "images/Minecraft.png",
                Developer = "Mojang",
                Publisher = "Mojang"
            };
            context.Games.Add(game);
            await context.SaveChangesAsync();
            return game;
        }

        // Vérifie qu'un nouvel UserGame est créé si aucune entrée n'existe encore.
        [Fact]
        public async Task AddOrUpdateAsync_NewEntry_ShouldCreateUserGame()
        {
            using var context = BuildInMemoryContext();
            var game = await SeedGame(context);
            var service = new UserGameService(context);

            var dto = new AddUserGameDto
            {
                UserId = "user-1",
                GameId = game.GameId,
                RelationType = "Wishlist"
            };

            var result = await service.AddOrUpdateAsync(dto);

            Assert.Equal("Wishlist", result.RelationType);
            Assert.False(result.IsFavorite);
            Assert.Single(context.UserGames);
        }

        // Vérifie qu'une entrée existante est mise à jour sans duplication.
        [Fact]
        public async Task AddOrUpdateAsync_ExistingEntry_ShouldUpdateRelationType_NotDuplicate()
        {
            using var context = BuildInMemoryContext();
            var game = await SeedGame(context);
            var service = new UserGameService(context);

            await service.AddOrUpdateAsync(new AddUserGameDto
            {
                UserId = "user-1",
                GameId = game.GameId,
                RelationType = "Wishlist"
            });

            var result = await service.AddOrUpdateAsync(new AddUserGameDto
            {
                UserId = "user-1",
                GameId = game.GameId,
                RelationType = "Library"
            });

            Assert.Equal("Library", result.RelationType);
            Assert.Single(context.UserGames);
        }

        // Vérifie que IsFavorite = false ne désactive pas un favori déjà existant.
        [Fact]
        public async Task AddOrUpdateAsync_IsFavoriteFalse_ShouldNotUnsetExistingFavorite()
        {
            using var context = BuildInMemoryContext();
            var game = await SeedGame(context);
            var service = new UserGameService(context);

            await service.AddOrUpdateAsync(new AddUserGameDto
            {
                UserId = "user-1",
                GameId = game.GameId,
                RelationType = "Library",
                IsFavorite = true
            });

            var result = await service.AddOrUpdateAsync(new AddUserGameDto
            {
                UserId = "user-1",
                GameId = game.GameId,
                RelationType = "Library",
                IsFavorite = false
            });

            Assert.True(result.IsFavorite);
        }

        // Vérifie que ToggleFavorite modifie correctement l'état favori d'une entrée existante.
        [Fact]
        public async Task ToggleFavoriteAsync_ExistingEntry_ShouldToggle()
        {
            using var context = BuildInMemoryContext();
            var game = await SeedGame(context);
            context.UserGames.Add(new UserGame { UserId = "user-1", GameId = game.GameId, RelationType = "Library" });
            await context.SaveChangesAsync();

            var service = new UserGameService(context);

            var result = await service.ToggleFavoriteAsync("user-1", game.GameId, true);

            Assert.NotNull(result);
            Assert.True(result.IsFavorite);
        }

        // Vérifie que ToggleFavorite retourne null si le jeu n'est pas présent dans les listes de l'utilisateur
        [Fact]
        public async Task ToggleFavoriteAsync_GameNotInAnyList_ShouldReturnNull()
        {
            using var context = BuildInMemoryContext();
            var game = await SeedGame(context);
            var service = new UserGameService(context);

            var result = await service.ToggleFavoriteAsync("user-1", game.GameId, true);

            Assert.Null(result);
        }

        // Vérifie qu'un jeu présent dans la bibliothèque peut recevoir une note.
        [Fact]
        public async Task UpdateRatingAsync_LibraryGame_ShouldUpdateRating()
        {
            using var context = BuildInMemoryContext();
            var game = await SeedGame(context);
            context.UserGames.Add(new UserGame { UserId = "user-1", GameId = game.GameId, RelationType = "Library" });
            await context.SaveChangesAsync();

            var service = new UserGameService(context);

            var result = await service.UpdateRatingAsync("user-1", game.GameId, 8);

            Assert.NotNull(result);
            Assert.Equal(8, result.Rating);
        }

        // Vérifie qu'un jeu en wishlist ne peut pas recevoir de note.
        [Fact]
        public async Task UpdateRatingAsync_WishlistGame_ShouldReturnNull()
        {
            using var context = BuildInMemoryContext();
            var game = await SeedGame(context);
            context.UserGames.Add(new UserGame { UserId = "user-1", GameId = game.GameId, RelationType = "Wishlist" });
            await context.SaveChangesAsync();

            var service = new UserGameService(context);

            var result = await service.UpdateRatingAsync("user-1", game.GameId, 8);

            Assert.Null(result);
        }

        // Vérifie que la mise à jour de note retourne null si l'entrée n'existe pas.
        [Fact]
        public async Task UpdateRatingAsync_UnknownEntry_ShouldReturnNull()
        {
            using var context = BuildInMemoryContext();
            var service = new UserGameService(context);

            var result = await service.UpdateRatingAsync("user-1", 999, 8);

            Assert.Null(result);
        }

        // Vérifie que GetFavoritesAsync retourne uniquement les jeux marqués comme favoris.
        [Fact]
        public async Task GetFavoritesAsync_ShouldOnlyReturnFavorites_RegardlessOfRelationType()
        {
            using var context = BuildInMemoryContext();
            var minecraft = await SeedGame(context, "Minecraft");
            var hades = await SeedGame(context, "Hades");
            var celeste = await SeedGame(context, "Celeste");

            context.UserGames.AddRange(
                new UserGame { UserId = "user-1", GameId = minecraft.GameId, RelationType = "Library", IsFavorite = true },
                new UserGame { UserId = "user-1", GameId = hades.GameId, RelationType = "Wishlist", IsFavorite = true },
                new UserGame { UserId = "user-1", GameId = celeste.GameId, RelationType = "Library", IsFavorite = false }
            );
            await context.SaveChangesAsync();

            var service = new UserGameService(context);

            var result = await service.GetFavoritesAsync("user-1");

            Assert.Equal(2, result.Count());
            Assert.Contains(result, ug => ug.Game!.Title == "Minecraft");
            Assert.Contains(result, ug => ug.Game!.Title == "Hades");
        }

        // Vérifie qu'une entrée existante est correctement supprimée.
        [Fact]
        public async Task RemoveAsync_ExistingEntry_ShouldRemoveIt()
        {
            using var context = BuildInMemoryContext();
            var game = await SeedGame(context);
            context.UserGames.Add(new UserGame { UserId = "user-1", GameId = game.GameId, RelationType = "Library" });
            await context.SaveChangesAsync();

            var service = new UserGameService(context);

            var result = await service.RemoveAsync("user-1", game.GameId);

            Assert.True(result);
            Assert.Empty(context.UserGames);
        }

        // Vérifie que RemoveAsync retourne false si l'entrée n'existe pas.
        [Fact]
        public async Task RemoveAsync_UnknownEntry_ShouldReturnFalse()
        {
            using var context = BuildInMemoryContext();
            var service = new UserGameService(context);

            var result = await service.RemoveAsync("user-1", 999);

            Assert.False(result);
        }

        // Vérifie que les statistiques retournées sont correctes : totaux, favoris, moyenne, genres.
        [Fact]
        public async Task GetStatsAsync_ShouldComputeCorrectCounts()
        {
            using var context = BuildInMemoryContext();
            var minecraft = await SeedGame(context, "Minecraft");
            var hades = await SeedGame(context, "Hades");
            hades.Genre = "Rogue-like";
            hades.Developer = "Supergiant Games";

            var celeste = await SeedGame(context, "Celeste");
            celeste.Genre = "Sandbox";
            celeste.Developer = "Maddy Makes Games";
            await context.SaveChangesAsync();

            context.UserGames.AddRange(
                new UserGame { UserId = "user-1", GameId = minecraft.GameId, RelationType = "Library", IsFavorite = true, Rating = 8 },
                new UserGame { UserId = "user-1", GameId = hades.GameId, RelationType = "Wishlist", IsFavorite = false },
                new UserGame { UserId = "user-1", GameId = celeste.GameId, RelationType = "Library", IsFavorite = false, Rating = 10 }
            );
            await context.SaveChangesAsync();

            var service = new UserGameService(context);

            var stats = await service.GetStatsAsync("user-1");

            Assert.Equal(3, stats.TotalGames);
            Assert.Equal(2, stats.LibraryCount);
            Assert.Equal(1, stats.WishlistCount);
            Assert.Equal(1, stats.FavoritesCount);
            Assert.Equal(9, stats.AverageRating);
            Assert.Equal(2, stats.GamesByGenre["Sandbox"]);
        }

        // Vérifie que la moyenne des notes vaut 0 lorsqu'aucune note n'est présente.
        [Fact]
        public async Task GetStatsAsync_NoRatings_AverageShouldBeZero()
        {
            using var context = BuildInMemoryContext();
            var game = await SeedGame(context);
            context.UserGames.Add(new UserGame { UserId = "user-1", GameId = game.GameId, RelationType = "Wishlist" });
            await context.SaveChangesAsync();

            var service = new UserGameService(context);

            var stats = await service.GetStatsAsync("user-1");

            Assert.Equal(0, stats.AverageRating);
        }
    }
}