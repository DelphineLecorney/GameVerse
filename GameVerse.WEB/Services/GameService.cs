using GameVerse.SHARED.DTOs.Games;
using GameVerse.WEB.Services.Interfaces;
using System.Net.Http.Json;

namespace GameVerse.WEB.Services
{
    public class GameService : IGameService
    {
        private readonly HttpClient _http;

        public GameService(HttpClient http)
        {
            _http = http;
        }
        public async Task<List<GameDto>> GetUserLibraryAsync()
        {
            var result = await _http.GetFromJsonAsync<List<GameDto>>("api/games/library");
            return result ?? new List<GameDto>();
        }

        public async Task RemoveFromLibraryAsync(int gameId)
        {
            var response = await _http.DeleteAsync($"api/games/library/{gameId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<GameDto?> GetByIdAsync(int id)
        {
            var response = await _http.GetAsync($"api/games/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<GameDto>();
        }


    }

}
