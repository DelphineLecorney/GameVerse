using GameVerse.SHARED.DTOs.Games;
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

        public async Task RemoveAsync(int gameId)
        {
            await _http.DeleteAsync($"api/usergames/{gameId}");
        }

        public async Task<List<GameDto>> GetFavoritesAsync()
        {
            return await _http.GetFromJsonAsync<List<GameDto>>("api/usergames/favorites")
                   ?? new List<GameDto>();
        }
    }
}
