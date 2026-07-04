using System.Net.Http.Json;

namespace GameVerse.WEB.Services;

public class AuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
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

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }
        catch
        {
            return null;
        }
    }
}
