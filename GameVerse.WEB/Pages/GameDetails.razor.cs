using GameVerse.SHARED.DTOs.Games;
using GameVerse.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace GameVerse.WEB.Pages
{
    public partial class GameDetails : ComponentBase
    {
        [Parameter] public int GameId { get; set; }

        [Inject] public IGameService GameService { get; set; } = default!;
        [Inject] public IUserGameService UserGameService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;

        public GameDto? Game { get; set; }
        public bool HasError { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Game = await GameService.GetByIdAsync(GameId);
            }
            catch (Exception)
            {
                HasError = true;
            }
        }

        public void GoBack()
        {
            Navigation.NavigateTo("/library");
        }

        private async Task AddTo(string relationType)
        {
            try
            {
                await UserGameService.AddToRelationAsync(GameId, relationType);
            }
            catch (Exception)
            {
                HasError = true;
            }
        }
    }
}