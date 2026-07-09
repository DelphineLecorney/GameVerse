using GameVerse.SHARED.DTOs.Games;
using GameVerse.WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;

namespace GameVerse.WEB.Pages
{
    public partial class Library
    {
        [Inject] public IGameService GameService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }

        private List<GameDto> Games = new();

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (authState.User.Identity?.IsAuthenticated == true)
            {
                try
                {
                    Games = await GameService.GetUserLibraryAsync();
                }
                catch (HttpRequestException)
                {
                    Navigation.NavigateTo("/login");
                }
            }
            else
            {
                Navigation.NavigateTo("/login");
            }
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
