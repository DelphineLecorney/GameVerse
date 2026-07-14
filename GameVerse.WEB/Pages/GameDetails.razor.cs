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

        public GameWithStatusDto? Game { get; set; }
        public bool HasError { get; set; }

        private string? ToastMessage;
        private CancellationTokenSource? _toastCts;

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
                Game = await GameService.GetByIdAsync(GameId); // recharge le statut mis à jour
                await ShowToast($"Ajouté ✓");
            }
            catch (Exception)
            {
                HasError = true;
            }
        }

        private async Task OnRatingChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var rating))
            {
                try
                {
                    await UserGameService.UpdateRatingAsync(GameId, rating);
                    Game!.Rating = rating;
                    await ShowToast("Note enregistrée ✓");
                }
                catch (Exception)
                {
                    HasError = true;
                }
            }
        }

        private async Task ToggleFavorite()
        {
            try
            {
                await UserGameService.ToggleFavoriteAsync(GameId, !Game!.IsFavorite);
                Game.IsFavorite = !Game.IsFavorite;
                await ShowToast(Game.IsFavorite ? "Ajouté aux favoris ✓" : "Retiré des favoris");
            }
            catch (Exception)
            {
                await ShowToast("Ajoute d'abord ce jeu à ta bibliothèque ou ta wishlist.");
            }
        }

        private async Task ShowToast(string message)
        {
            _toastCts?.Cancel();
            _toastCts = new CancellationTokenSource();
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
    }
}