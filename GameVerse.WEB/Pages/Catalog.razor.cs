using GameVerse.SHARED.DTOs.Games;
using GameVerse.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace GameVerse.WEB.Pages
{
    public partial class Catalog : ComponentBase
    {
        [Inject] public IGameService GameService { get; set; } = default!;
        [Inject] public IUserGameService UserGameService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;

        public List<GameWithStatusDto>? Games { get; set; }
        public bool HasError { get; set; }

        private string? ToastMessage;
        private CancellationTokenSource? _toastCts;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Games = await GameService.GetCatalogAsync();
            }
            catch (Exception)
            {
                HasError = true;
            }
        }

        private async Task AddTo(int gameId, string relationType)
        {
            try
            {
                await UserGameService.AddToRelationAsync(gameId, relationType);
                await RefreshCatalog();
                await ShowToast($"Ajouté à {TranslateRelation(relationType)} ✓");
            }
            catch (Exception)
            {
                HasError = true;
            }
        }

        private async Task ToggleFavorite(int gameId, bool currentState)
        {
            try
            {
                await UserGameService.ToggleFavoriteAsync(gameId, !currentState);
                await RefreshCatalog();
                await ShowToast(!currentState ? "Ajouté aux favoris ✓" : "Retiré des favoris");
            }
            catch (Exception)
            {
                await ShowToast("Ajoute d'abord ce jeu à ta bibliothèque ou ta wishlist.");
            }
        }

        private async Task RefreshCatalog()
        {
            Games = await GameService.GetCatalogAsync();
        }

        private async Task ShowToast(string message)
        {
            _toastCts?.Cancel();
            _toastCts = new System.Threading.CancellationTokenSource();
            var token = _toastCts.Token;

            ToastMessage = message;
            StateHasChanged();

            try
            {
                await Task.Delay(2500, token);
                ToastMessage = null;
                StateHasChanged();
            }
            catch (TaskCanceledException) { }
        }

        private static string TranslateRelation(string relationType) => relationType switch
        {
            "Library" => "ta bibliothèque",
            "Wishlist" => "ta liste de souhaits",
            _ => relationType
        };

        private void GoToDetails(int gameId)
        {
            Navigation.NavigateTo($"/game/{gameId}");
        }
    }
}