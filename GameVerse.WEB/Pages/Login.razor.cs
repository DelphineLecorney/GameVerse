using Microsoft.AspNetCore.Components;
using GameVerse.WEB.Services;

namespace GameVerse.WEB.Pages;

public partial class LoginBase : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    protected string Email { get; set; } = string.Empty;
    protected string Password { get; set; } = string.Empty;
    protected string ErrorMessage { get; set; } = string.Empty;

    protected async Task LoginUser()
    {
        ErrorMessage = string.Empty;

        var token = await AuthService.LoginAsync(Email, Password);

        if (token == null)
        {
            ErrorMessage = "Email ou mot de passe incorrect.";
            return;
        }

        Nav.NavigateTo("/");
    }
}
