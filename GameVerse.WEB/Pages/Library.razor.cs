using GameVerse.SHARED.DTOs.Games;
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

        private bool ShowConfirmModal;
        private string ConfirmMessage = "";
        private int _gameIdToRemove;

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

        private void AskRemoveConfirmation(int gameId, string title)
        {
            _gameIdToRemove = gameId;
            ConfirmMessage = $"Es-tu sûre de vouloir retirer \"{title}\" de ta bibliothèque ?";
            ShowConfirmModal = true;
        }

        private async Task ConfirmRemove()
        {
            ShowConfirmModal = false;

            try
            {
                await GameService.RemoveFromLibraryAsync(_gameIdToRemove);
                Games = await GameService.GetUserLibraryAsync();
            }
            catch (Exception)
            {
                HasError = true;
            }
        }

        private void CancelRemove()
        {
            ShowConfirmModal = false;
        }

        private void GoToDetails(int gameId)
        {
            Navigation.NavigateTo($"/game/{gameId}");
        }
    }
}