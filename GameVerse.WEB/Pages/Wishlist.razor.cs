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

        private async Task RemoveFromWishlist(int id)
        {
            try
            {
                await UserGameService.RemoveAsync(id);
                Games = await UserGameService.GetWishlistAsync();
            }
            catch (Exception)
            {
                HasError = true;
            }
        }
    }
}