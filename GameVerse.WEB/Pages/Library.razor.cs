using GameVerse.SHARED.DTOs.Games;
using GameVerse.WEB.Services;
using Microsoft.AspNetCore.Components;

namespace GameVerse.WEB.Pages
{
    public partial class Library
    {
        [Inject] public IGameService GameService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;

        private List<GameDto> Games = new();

        protected override async Task OnInitializedAsync()
        {
            Games = await GameService.GetUserLibraryAsync();
        }

        private async Task RemoveFromLibrary(int gameId)
        {
            await GameService.RemoveFromLibraryAsync(gameId);
            Games = await GameService.GetUserLibraryAsync();
        }

        private void GoToDetails(int gameId)
        {
            Navigation.NavigateTo($"/game/{gameId}");
        }
    }
}
