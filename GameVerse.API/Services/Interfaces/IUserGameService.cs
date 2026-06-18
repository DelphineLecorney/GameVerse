using GameVerse.API.Models;

namespace GameVerse.API.Services.Interfaces
{
    public interface IUserGameService
    {
        Task<UserGame> AddAsync(UserGame userGame);
        Task<IEnumerable<UserGame>> GetByUserAsync(int userId);
        Task<IEnumerable<UserGame>> GetFavoritesAsync(int userId);
        Task<UserGame?> UpdateAsync(UserGame updated);
        Task<bool> RemoveAsync(int userId, int gameId);
    }

}
