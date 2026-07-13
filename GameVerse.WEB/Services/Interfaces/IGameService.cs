using GameVerse.SHARED.DTOs.Games;

namespace GameVerse.WEB.Services.Interfaces
{
    public interface IGameService
    {
        Task<List<GameDto>> GetUserLibraryAsync();
        Task RemoveFromLibraryAsync(int gameId);
        Task<GameDto?> GetByIdAsync(int id);
        Task<List<GameDto>> GetAllAsync();
        Task<List<GameWithStatusDto>> GetCatalogAsync();
    }

}
