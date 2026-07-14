using GameVerse.SHARED.DTOs.Stats;

namespace GameVerse.WEB.Services.Interfaces
{
    public interface IStatsService
    {
        Task<UserStatsDto?> GetMyStatsAsync();
    }
}
