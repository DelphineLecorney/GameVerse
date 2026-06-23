using GameVerse.API.DTOs.Auth;
using GameVerse.API.Models;

namespace GameVerse.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> EmailExists(string email);
        Task<User> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);

    }
}
