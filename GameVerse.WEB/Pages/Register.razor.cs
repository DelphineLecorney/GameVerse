using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace GameVerse.WEB.Pages;

public partial class RegisterBase : ComponentBase
{
    [Inject] private HttpClient Http { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    protected string Email { get; set; } = string.Empty;
    protected string Username { get; set; } = string.Empty;
    protected string Password { get; set; } = string.Empty;
    protected string ConfirmPassword { get; set; } = string.Empty;
    protected string ErrorMessage { get; set; } = string.Empty;

    protected async Task RegisterUser()
    {
        ErrorMessage = string.Empty;

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Les mots de passe ne correspondent pas.";
            return;
        }

        var payload = new
        {
            Email,
            Username,
            Password
        };

        var response = await Http.PostAsJsonAsync("api/auth/register", payload);

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = "Impossible de créer le compte. Vérifiez les informations.";
            return;
        }

        Nav.NavigateTo("/login");
    }
}
