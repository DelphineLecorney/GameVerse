using GameVerse.API.Data;
using GameVerse.API.Models;
using GameVerse.API.Services.Interfaces;
using GameVerse.SHARED.DTOs.Stats;
using GameVerse.SHARED.DTOs.UserGame;
using Microsoft.EntityFrameworkCore;

namespace GameVerse.API.Services
{
    public class UserGameService : IUserGameService
    {
        private readonly GameVerseContext _context;

        public UserGameService(GameVerseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserGame>> GetByUserAsync(string userId)
        {
            return await _context.UserGames
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> RemoveAsync(string userId, int gameId)
        {
            var userGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);

            if (userGame == null)
                return false;

            _context.UserGames.Remove(userGame);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserGame?> UpdateAsync(string userId, int gameId, UpdateUserGameDto dto)
        {
            var userGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);

            if (userGame == null)
                return null;

            userGame.RelationType = dto.RelationType;
            userGame.Rating = dto.Rating;

            await _context.SaveChangesAsync();
            return userGame;
        }

        public async Task<UserGame> AddOrUpdateAsync(AddUserGameDto dto)
        {
            var existing = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == dto.UserId && ug.GameId == dto.GameId);

            if (existing != null)
            {
                existing.RelationType = dto.RelationType;
                if (dto.IsFavorite)
                    existing.IsFavorite = true;
                existing.Rating = dto.Rating ?? existing.Rating;
                await _context.SaveChangesAsync();
                return existing;
            }

            var userGame = new UserGame
            {
                UserId = dto.UserId,
                GameId = dto.GameId,
                RelationType = dto.RelationType,
                IsFavorite = dto.IsFavorite,
                Rating = dto.Rating,
                AddedAt = DateTime.UtcNow
            };

            _context.Add(userGame);
            await _context.SaveChangesAsync();

            return userGame;
        }

        public async Task<IEnumerable<UserGame>> GetFavoritesAsync(string userId)
        {
            return await _context.UserGames
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId && ug.IsFavorite)
                .ToListAsync();
        }

        public async Task<UserGame?> ToggleFavoriteAsync(string userId, int gameId, bool isFavorite)
        {
            var userGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);

            if (userGame == null)
                return null;

            userGame.IsFavorite = isFavorite;
            await _context.SaveChangesAsync();
            return userGame;
        }

        public async Task<UserStatsDto> GetStatsAsync(string userId)
        {
            var userGames = await _context.UserGames
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId)
                .ToListAsync();

            var stats = new UserStatsDto
            {
                TotalGames = userGames.Count,
                LibraryCount = userGames.Count(ug => ug.RelationType == "Library"),
                WishlistCount = userGames.Count(ug => ug.RelationType == "Wishlist"),
                FavoritesCount = userGames.Count(ug => ug.IsFavorite),
                AverageRating = userGames.Any(ug => ug.Rating.HasValue)
                    ? Math.Round(userGames.Where(ug => ug.Rating.HasValue).Average(ug => ug.Rating!.Value), 1)
                    : 0,
                GamesByGenre = userGames
                    .Where(ug => ug.Game != null)
                    .GroupBy(ug => ug.Game!.Genre)
                    .ToDictionary(g => g.Key, g => g.Count()),
                TopDevelopers = userGames
                    .Where(ug => ug.Game != null)
                    .GroupBy(ug => ug.Game!.Developer)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return stats;
        }
    }
}
