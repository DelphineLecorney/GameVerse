using GameVerse.API.Data;
using GameVerse.SHARED.DTOs.Games;
using GameVerse.API.Models;
using GameVerse.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameVerse.API.Services
{
    public class GameService : IGameService
    {
        private readonly GameVerseContext _context;

        public GameService(GameVerseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<Game> CreateAsync(CreateGameDto dto)
        {
            var game = new Game
            {
                Title = dto.Title,
                Description = dto.Description,
                Genre = dto.Genre,
                ReleaseDate = dto.ReleaseDate,
                CoverUrl = dto.CoverUrl,
                Developer = dto.Developer,
                Publisher = dto.Publisher
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return game;
        }

        public async Task<Game?> UpdateAsync(int id, UpdateGameDto dto)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return null!;

            game.Title = dto.Title;
            game.Description = dto.Description;
            game.Genre = dto.Genre;
            game.ReleaseDate = dto.ReleaseDate;
            game.CoverUrl = dto.CoverUrl;
            game.Developer = dto.Developer;
            game.Publisher = dto.Publisher;

            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
