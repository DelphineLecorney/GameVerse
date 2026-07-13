using GameVerse.SHARED.DTOs.Users;
using GameVerse.WEB.Services;
using Microsoft.AspNetCore.Components;

namespace GameVerse.WEB.Pages
{
    public partial class ProfileEdit : ComponentBase
    {
        [Inject] public AuthService AuthService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;
        [Inject] public AuthState AuthState { get; set; } = default!;

        protected string Username { get; set; } = string.Empty;
        protected string Email { get; set; } = string.Empty;
        protected string ErrorMessage { get; set; } = string.Empty;
        protected string SuccessMessage { get; set; } = string.Empty;
        protected bool IsLoading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var user = await AuthService.GetCurrentUserAsync();
                if (user != null)
                {
                    Username = user.Username;
                    Email = user.Email;
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Impossible de charger ton profil.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task Save()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Le nom d'utilisateur et l'email sont obligatoires.";
                return;
            }

            try
            {
                var dto = new UpdateUserDto { Username = Username, Email = Email };
                var success = await AuthService.UpdateProfileAsync(dto);

                if (success)
                {
                    await AuthState.UpdateUsernameAsync(Username);

                    SuccessMessage = "Profil mis à jour avec succès.";
                    await Task.Delay(1200);
                    Navigation.NavigateTo("/profile");
                }
                else
                {
                    ErrorMessage = "Impossible de mettre à jour le profil.";
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Impossible de contacter le serveur.";
            }
        }

        protected void Cancel()
        {
            Navigation.NavigateTo("/profile");
        }
    }
}