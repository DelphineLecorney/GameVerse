using GameVerse.SHARED.DTOs.Games;

namespace GameVerse.WEB.Services.Interfaces
{
    public interface IUserGameService
    {
        Task<List<GameDto>> GetWishlistAsync();
        Task<List<GameDto>> GetFavoritesAsync();
        Task RemoveAsync(int gameId);
    }
}