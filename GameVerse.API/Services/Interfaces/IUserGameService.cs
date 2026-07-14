using GameVerse.API.Models;
using GameVerse.SHARED.DTOs.Stats;
using GameVerse.SHARED.DTOs.UserGame;

namespace GameVerse.API.Services.Interfaces
{
    public interface IUserGameService
    {
        Task<UserGame> AddOrUpdateAsync(AddUserGameDto dto);
        Task<IEnumerable<UserGame>> GetByUserAsync(string userId);
        Task<IEnumerable<UserGame>> GetFavoritesAsync(string userId);
        Task<UserGame?> ToggleFavoriteAsync(string userId, int gameId, bool isFavorite);
        Task<UserGame?> UpdateAsync(string userId, int gameId, UpdateUserGameDto dto);
        Task<bool> RemoveAsync(string userId, int gameId);
        Task<UserStatsDto> GetStatsAsync(string userId);
    }
}
