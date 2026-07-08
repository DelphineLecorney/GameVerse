using GameVerse.API.Models;
using GameVerse.SHARED.DTOs.Games;

namespace GameVerse.API.Services.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<IEnumerable<Game>> GetUserLibraryAsync(string userId);


        Task<Game?> GetByIdAsync(int id);
        Task<Game> CreateAsync(CreateGameDto dto);
        Task<Game?> UpdateAsync(int id, UpdateGameDto dto);

        Task<bool> DeleteAsync(int id);
        Task<bool> RemoveFromLibraryAsync(string userId, int gameId);
    }

}
