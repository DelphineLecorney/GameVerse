using GameVerse.SHARED.DTOs.Games;
using GameVerse.SHARED.DTOs.UserGame;
using GameVerse.WEB.Services.Interfaces;
using System.Net.Http.Json;

namespace GameVerse.WEB.Services
{
    public class UserGameService : IUserGameService
    {
        private readonly HttpClient _http;

        public UserGameService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<GameDto>> GetWishlistAsync()
        {
            return await _http.GetFromJsonAsync<List<GameDto>>("api/usergames/wishlist")
                   ?? new List<GameDto>();
        }

        public async Task<List<GameDto>> GetFavoritesAsync()
        {
            return await _http.GetFromJsonAsync<List<GameDto>>("api/usergames/favorites")
                   ?? new List<GameDto>();
        }

        public async Task AddToRelationAsync(int gameId, string relationType)
        {
            var dto = new AddUserGameDto
            {
                GameId = gameId,
                RelationType = relationType
            };

            var response = await _http.PostAsJsonAsync("api/usergames/add", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task ToggleFavoriteAsync(int gameId, bool isFavorite)
        {
            var response = await _http.PutAsJsonAsync($"api/usergames/{gameId}/favorite", isFavorite);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveAsync(int gameId)
        {
            await _http.DeleteAsync($"api/usergames/{gameId}");
        }
    }
}