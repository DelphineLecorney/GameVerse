using Microsoft.JSInterop;

namespace GameVerse.WEB.Services
{
    public class AuthState
    {
        private readonly IJSRuntime _js;

        public AuthState(IJSRuntime js)
        {
            _js = js;
        }

        public string? Token { get; private set; }
        public string? RefreshToken { get; private set; }
        public string? Username { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
        public bool IsInitialized { get; private set; }

        public event Action? OnChange;

        public async Task InitializeAsync()
        {
            if (IsInitialized) return;

            Token = await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");
            RefreshToken = await _js.InvokeAsync<string?>("localStorage.getItem", "refreshToken");
            Username = await _js.InvokeAsync<string?>("localStorage.getItem", "username");

            IsInitialized = true;
            NotifyStateChanged();
        }

        public async Task SetAuthAsync(string token, string refreshToken, string username)
        {
            Token = token;
            RefreshToken = refreshToken;
            Username = username;
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
            await _js.InvokeVoidAsync("localStorage.setItem", "refreshToken", refreshToken);
            await _js.InvokeVoidAsync("localStorage.setItem", "username", username);
            NotifyStateChanged();
        }

        public async Task LogoutAsync()
        {
            Token = null;
            RefreshToken = null;
            Username = null;
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _js.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
            await _js.InvokeVoidAsync("localStorage.removeItem", "username");
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}