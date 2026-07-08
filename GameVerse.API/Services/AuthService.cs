using GameVerse.API.Data;
using GameVerse.API.Models;
using GameVerse.API.Services.Interfaces;
using GameVerse.SHARED.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace GameVerse.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly GameVerseContext _context;
        private readonly IConfiguration _config;

        public AuthService(GameVerseContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> RegisterAsync(RegisterRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null!;

            var token = GenerateJwtToken(user);

            var refreshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                Username = user.Username
            };
        }


        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null || storedToken.IsRevoked)
                return null!;

            if (storedToken.ExpiresAt < DateTime.UtcNow)
                return null!;

            storedToken.IsRevoked = true;

            var newJwt = GenerateJwtToken(storedToken.User!);

            var newRefresh = new RefreshToken
            {
                UserId = storedToken.UserId,
                Token = GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(newRefresh);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Token = newJwt,
                RefreshToken = newRefresh.Token,
                Username = storedToken.User!.Username
            };
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("username", user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task LogoutAsync(string userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();
        }


    }
}
