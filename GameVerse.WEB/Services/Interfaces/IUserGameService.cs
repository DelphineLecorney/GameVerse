using GameVerse.SHARED.DTOs.Games;

namespace GameVerse.WEB.Services.Interfaces
{
    public interface IUserGameService
    {
        Task<List<GameDto>> GetWishlistAsync();
        Task<List<GameDto>> GetFavoritesAsync();
        Task AddToRelationAsync(int gameId, string relationType);
        Task ToggleFavoriteAsync(int gameId, bool isFavorite);
        Task RemoveAsync(int gameId);
        Task UpdateRatingAsync(int gameId, int rating);
    }
}