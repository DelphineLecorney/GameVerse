using Microsoft.AspNetCore.Components;
using GameVerse.WEB.Services;

namespace GameVerse.WEB.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject] private AuthState AuthState { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    private void LogoutUser()
    {
        AuthService.Logout();
        Nav.NavigateTo("/login");
    }

    protected override void OnInitialized()
    {
        AuthState.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        AuthState.OnChange -= StateHasChanged;
    }
}
