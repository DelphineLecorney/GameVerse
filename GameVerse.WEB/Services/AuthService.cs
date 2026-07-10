using GameVerse.SHARED.DTOs.Auth;
using GameVerse.SHARED.DTOs.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace GameVerse.WEB.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly AuthState _authState;

    public AuthService(HttpClient http, AuthState authState)
    {
        _http = http;
        _authState = authState;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var payload = new
        {
            Email = email,
            Password = password
        };

        try
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", payload);

            if (!response.IsSuccessStatusCode)
                return null;

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (authResponse == null || string.IsNullOrEmpty(authResponse.RefreshToken))
                return null;

            await _authState.SetAuthAsync(authResponse.Token, authResponse.RefreshToken, authResponse.Username);

            return authResponse.Token;
        }
        catch
        {
            return null;
        }
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        if (!_authState.IsAuthenticated)
            return null;

        var response = await _http.GetAsync("api/auth/me");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }


    public async Task LogoutAsync()
    {
        await _authState.LogoutAsync();
    }

}