using GameVerse.SHARED.DTOs.Games;

namespace GameVerse.WEB.Services.Interfaces
{
    public interface IGameService
    {
        Task<List<GameDto>> GetUserLibraryAsync();
        Task RemoveFromLibraryAsync(int gameId);
        Task<List<GameDto>> GetAllAsync();
        Task<List<GameWithStatusDto>> GetCatalogAsync();
        Task<GameWithStatusDto?> GetByIdAsync(int id);
    }

}
