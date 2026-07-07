using GameVerse.SHARED.DTOs.Games;
using GameVerse.API.Models;

namespace GameVerse.API.Services.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game?> GetByIdAsync(int id);
        Task<Game> CreateAsync(CreateGameDto dto);
        Task<Game?> UpdateAsync(int id, UpdateGameDto dto);
        Task<bool> DeleteAsync(int id);
    }

}
