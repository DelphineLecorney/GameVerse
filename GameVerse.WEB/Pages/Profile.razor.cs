using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using GameVerse.WEB.Services;
using GameVerse.WEB.Models;

namespace GameVerse.WEB.Pages;

public partial class Profile
{
    [Inject] public AuthService AuthService { get; set; } = default!;
    [Inject] public AuthenticationStateProvider AuthProvider { get; set; } = default!;

    public UserDto? User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity?.IsAuthenticated ?? false)
        {
            User = null;
            return;
        }

        User = await AuthService.GetCurrentUserAsync();
    }
}
