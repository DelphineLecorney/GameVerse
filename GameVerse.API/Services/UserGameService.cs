using GameVerse.API.Data;
using GameVerse.SHARED.DTOs.UserGame;
using GameVerse.API.Models;
using GameVerse.API.Services.Interfaces;
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

        public async Task<UserGame> AddAsync(AddUserGameDto dto)
        {
            var userGame = new UserGame
            {
                UserId = dto.UserId,
                GameId = dto.GameId,
                RelationType = dto.RelationType,
                Rating = dto.Rating,
                AddedAt = DateTime.UtcNow
            };

            _context.Add(userGame);
            await _context.SaveChangesAsync();

            return userGame;

        }

        public async Task<IEnumerable<UserGame>> GetByUserAsync(int userId)
        {
            return await _context.UserGames
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserGame>> GetFavoritesAsync(int userId)
        {
            return await _context.UserGames
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId && ug.RelationType == "favorite")
                .ToListAsync();
        }

        public async Task<bool> RemoveAsync(int userId, int gameId)
        {
            var userGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);

            if (userGame == null)
            {
                return false;
            }

            _context.UserGames.Remove(userGame);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserGame?> UpdateAsync(int userId, int gameId, UpdateUserGameDto dto)
        {
            var userGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);

            if (userGame == null)
            {
                return null;
            }

            userGame.RelationType = dto.RelationType;
            userGame.Rating = dto.Rating;

            await _context.SaveChangesAsync();
            return userGame;
        }
    }
}
