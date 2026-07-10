using GameVerse.SHARED.DTOs.Games;

namespace GameVerse.WEB.Services
{
    public interface IGameService
    {
        Task<List<GameDto>> GetUserLibraryAsync();
        Task RemoveFromLibraryAsync(int gameId);
        Task<GameDto?> GetByIdAsync(int id);

    }

}
