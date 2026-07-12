using GameVerse.SHARED.DTOs.Games;
using GameVerse.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace GameVerse.WEB.Pages
{
    public partial class Favorites : ComponentBase
    {
        [Inject] public IUserGameService UserGameService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;

        public List<GameDto>? Games { get; set; }
        public bool HasError { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Games = await UserGameService.GetFavoritesAsync();
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

        private async Task RemoveFromFavorites(int id)
        {
            try
            {
                await UserGameService.RemoveAsync(id);
                Games = await UserGameService.GetFavoritesAsync();
            }
            catch (Exception)
            {
                HasError = true;
            }
        }
    }
}