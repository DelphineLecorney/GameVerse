using GameVerse.SHARED.DTOs.Stats;
using GameVerse.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GameVerse.WEB.Pages
{
    public partial class Stats : ComponentBase
    {
        [Inject] public IStatsService StatsService { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;

        public UserStatsDto? StatsData { get; set; }
        public bool HasError { get; set; }

        private bool _chartsRendered = false;

        private static readonly string[] ChartColors =
        {
            "#39ff88", "#6fe6a0", "#2ecc71", "#16301f", "#a8c4b3", "#234d33"
        };

        protected override async Task OnInitializedAsync()
        {
            try
            {
                StatsData = await StatsService.GetMyStatsAsync();
            }
            catch (Exception)
            {
                HasError = true;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (StatsData != null && StatsData.TotalGames > 0 && !_chartsRendered)
            {
                _chartsRendered = true;
                await RenderCharts();
            }
        }

        private async Task RenderCharts()
        {
            await Task.Delay(50);

            if (StatsData!.GamesByGenre.Any())
            {
                await JS.InvokeVoidAsync("renderPieChart", "genreChart",
                    StatsData.GamesByGenre.Keys.ToArray(),
                    StatsData.GamesByGenre.Values.ToArray(),
                    ChartColors);
            }

            if (StatsData.TopDevelopers.Any())
            {
                await JS.InvokeVoidAsync("renderBarChart", "developersChart",
                    StatsData.TopDevelopers.Keys.ToArray(),
                    StatsData.TopDevelopers.Values.ToArray(),
                    "#39ff88");
            }
        }
    }
}