using GameVerse.SHARED.DTOs.Games;
using GameVerse.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace GameVerse.WEB.Pages
{
    public partial class Wishlist : ComponentBase
    {
        [Inject] public IUserGameService UserGameService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;

        public List<GameDto>? Games { get; set; }
        public bool HasError { get; set; }

        private bool ShowConfirmModal;
        private string ConfirmMessage = "";
        private int _gameIdToRemove;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Games = await UserGameService.GetWishlistAsync();
            }
            catch (Exception)
            {
                HasError = true;
            }
        }

        private void GoToDetails(int id)
        {
            Navigation.NavigateTo($"/game/{id}");
        }

        private void AskRemoveConfirmation(int gameId, string title)
        {
            _gameIdToRemove = gameId;
            ConfirmMessage = $"Es-tu sûre de vouloir retirer \"{title}\" de ta liste de souhaits ?";
            ShowConfirmModal = true;
        }

        private async Task ConfirmRemove()
        {
            ShowConfirmModal = false;

            try
            {
                await UserGameService.RemoveAsync(_gameIdToRemove);
                Games = await UserGameService.GetWishlistAsync();
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
    }
}