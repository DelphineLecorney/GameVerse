using GameVerse.SHARED.DTOs.Games;
using GameVerse.WEB.Services;
using GameVerse.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GameVerse.WEB.Pages
{
    public partial class Library
    {
        [Inject] public IGameService GameService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

        private List<GameDto> Games = new();
        private bool HasError;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (authState.User.Identity?.IsAuthenticated == true)
            {
                try
                {
                    Games = await GameService.GetUserLibraryAsync();
                }
                catch (Exception)
                {
                    HasError = true;
                }
            }
            else
            {
                Navigation.NavigateTo("/login");
            }
        }

        private async Task RemoveFromLibrary(int gameId)
        {
            try
            {
                await GameService.RemoveFromLibraryAsync(gameId);
                Games = await GameService.GetUserLibraryAsync();
            }
            catch (Exception)
            {
                HasError = true;
            }
        }

        private void GoToDetails(int gameId)
        {
            Navigation.NavigateTo($"/game/{gameId}");
        }
    }
}