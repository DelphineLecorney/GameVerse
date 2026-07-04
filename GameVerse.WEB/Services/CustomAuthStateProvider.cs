using GameVerse.WEB.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthState _authState;

    public CustomAuthStateProvider(AuthState authState)
    {
        _authState = authState;

        _authState.OnChange += () =>
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        };
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_authState.IsAuthenticated)
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(anonymous));
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, _authState.Username ?? "User")
        }, "jwt");

        var user = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(user));
    }
}
