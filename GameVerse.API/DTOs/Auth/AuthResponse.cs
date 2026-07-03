namespace GameVerse.API.DTOs.Auth
{
    public class AuthResponse
    {
        public string Token { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;

        public string? RefreshToken { get; init; }
    }

}
