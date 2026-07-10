using GameVerse.SHARED.DTOs.Games;
using GameVerse.WEB.Services;
using Microsoft.AspNetCore.Components;

namespace GameVerse.WEB.Pages
{
    public partial class GameDetails : ComponentBase
    {
        [Parameter] public int GameId { get; set; }

        [Inject] public IGameService GameService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;

        public GameDto? Game { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Game = await GameService.GetByIdAsync(GameId);
        }

        public void GoBack()
        {
            Navigation.NavigateTo("/library");
        }
    }
}
