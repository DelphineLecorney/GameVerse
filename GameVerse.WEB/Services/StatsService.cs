using GameVerse.SHARED.DTOs.Stats;
using GameVerse.WEB.Services.Interfaces;
using System.Net.Http.Json;

namespace GameVerse.WEB.Services
{
    public class StatsService : IStatsService
    {
        private readonly HttpClient _http;

        public StatsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<UserStatsDto?> GetMyStatsAsync()
        {
            var response = await _http.GetAsync("api/stats/me");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<UserStatsDto>();
        }
    }
}