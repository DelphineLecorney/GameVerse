using GameVerse.API.Data;
using GameVerse.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameVerse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserGamesController : ControllerBase
    {
        private readonly GameVerseContext _context;

        public UserGamesController(GameVerseContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddUserGame(UserGame userGame)
        {
            userGame.AddedAt = DateTime.UtcNow;

            _context.UserGames.Add(userGame);
            await _context.SaveChangesAsync();

            return Ok(userGame);
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserGame>>> GetUserGames(int userId)
        {
            var list = await _context.UserGames
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId)
                .ToListAsync();

            return Ok(list);
        }

        [Authorize]
        [HttpGet("user/{userId}/favorites")]
        public async Task<ActionResult<IEnumerable<UserGame>>> GetFavorites(int userId)
        {
            var list = await _context.UserGames
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId && ug.RelationType == "favorite")
                .ToListAsync();

            return Ok(list);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRelation(UserGame updated)
        {
            var existing = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == updated.UserId && ug.GameId == updated.GameId);

            if (existing == null)
                return NotFound();

            existing.RelationType = updated.RelationType;
            existing.Rating = updated.Rating;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveUserGame(int userId, int gameId)
        {
            var entry = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);

            if (entry == null)
                return NotFound();

            _context.UserGames.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
