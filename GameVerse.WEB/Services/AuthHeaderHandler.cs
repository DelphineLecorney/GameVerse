using System.Net.Http.Headers;

namespace GameVerse.WEB.Services
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly AuthState _authState;

        public AuthHeaderHandler(AuthState authState)
        {
            _authState = authState;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_authState.IsAuthenticated)
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authState.Token);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}