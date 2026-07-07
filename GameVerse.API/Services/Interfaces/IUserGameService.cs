using GameVerse.SHARED.DTOs.UserGame;
using GameVerse.API.Models;

namespace GameVerse.API.Services.Interfaces
{
    public interface IUserGameService
    {
        Task<UserGame> AddAsync(AddUserGameDto dto);
        Task<IEnumerable<UserGame>> GetByUserAsync(int userId);
        Task<IEnumerable<UserGame>> GetFavoritesAsync(int userId);
        Task<UserGame?> UpdateAsync(int userId, int gameId, UpdateUserGameDto dto);
        Task<bool> RemoveAsync(int userId, int gameId);
    }


}
