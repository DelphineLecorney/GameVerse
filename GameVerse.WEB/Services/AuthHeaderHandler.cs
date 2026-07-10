using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using GameVerse.SHARED.DTOs.Auth;
using Microsoft.AspNetCore.Components;

namespace GameVerse.WEB.Services
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly AuthState _authState;
        private readonly NavigationManager _navigation;
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly SemaphoreSlim _refreshLock = new(1, 1);

        public AuthHeaderHandler(AuthState authState, NavigationManager navigation, IHttpClientFactory httpClientFactory)
        {
            _authState = authState;
            _navigation = navigation;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_authState.IsAuthenticated)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authState.Token);

            // On garde une copie de la requête au cas où il faut la rejouer
            // (un HttpRequestMessage ne peut être envoyé qu'une seule fois)
            var requestClone = request.Content != null ? await CloneRequestAsync(request) : null;

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized && _authState.IsAuthenticated)
            {
                var refreshed = await TryRefreshTokenAsync();

                if (refreshed)
                {
                    var retryRequest = requestClone ?? new HttpRequestMessage(request.Method, request.RequestUri);
                    retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authState.Token);
                    response.Dispose();
                    response = await base.SendAsync(retryRequest, cancellationToken);
                }
                else
                {
                    await _authState.LogoutAsync();
                    _navigation.NavigateTo("/login", forceLoad: true);
                }
            }

            return response;
        }

        private async Task<bool> TryRefreshTokenAsync()
        {
            await _refreshLock.WaitAsync();
            try
            {
                var rawClient = _httpClientFactory.CreateClient("GameVerse.API.Raw");
                var response = await rawClient.PostAsJsonAsync("api/auth/refresh",
                    new { RefreshToken = _authState.RefreshToken });

                if (!response.IsSuccessStatusCode)
                    return false;

                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (result == null || string.IsNullOrEmpty(result.RefreshToken))
                    return false;

                await _authState.SetAuthAsync(result.Token, result.RefreshToken, result.Username);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage req)
        {
            var clone = new HttpRequestMessage(req.Method, req.RequestUri);

            var ms = new MemoryStream();
            await req.Content!.CopyToAsync(ms);
            ms.Position = 0;
            clone.Content = new StreamContent(ms);
            foreach (var h in req.Content.Headers)
                clone.Content.Headers.Add(h.Key, h.Value);

            foreach (var h in req.Headers)
                clone.Headers.TryAddWithoutValidation(h.Key, h.Value);

            return clone;
        }
    }
}