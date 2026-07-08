using GameVerse.API.Models;
using GameVerse.SHARED.DTOs.UserGame;

namespace GameVerse.API.Services.Interfaces
{
    public interface IUserGameService
    {
        Task<UserGame> AddAsync(AddUserGameDto dto);
        Task<IEnumerable<UserGame>> GetByUserAsync(string userId);
        Task<IEnumerable<UserGame>> GetFavoritesAsync(string userId);
        Task<UserGame?> UpdateAsync(string userId, int gameId, UpdateUserGameDto dto);
        Task<bool> RemoveAsync(string userId, int gameId);
    }
}
