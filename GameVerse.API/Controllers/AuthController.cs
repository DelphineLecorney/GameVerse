using GameVerse.API.Data;
using GameVerse.API.Models;
using GameVerse.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthLoginRequest = GameVerse.API.DTOs.Auth.LoginRequest;
using AuthRegisterRequest = GameVerse.API.DTOs.Auth.RegisterRequest;


namespace GameVerse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly GameVerseContext _context;
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;


        public AuthController(GameVerseContext context, IConfiguration config, IAuthService authService)
        {
            _context = context;
            _config = config;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthRegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email déjà utilisé");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Utilisateur enregistré avec succès" });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userIdStr = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdStr))
            {
                return Unauthorized(new { message = "Jeton d'authentification mal formé (sub manquant)." });
            }

            if (!int.TryParse(userIdStr, out int userId))
            {
                return BadRequest(new { message = "Le format de l'ID utilisateur est incorrect." });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound(new { message = $"L'utilisateur avec l'ID {userId} n'existe pas en base." });
            }

            return Ok(new
            {
                user.UserId,
                user.Username,
                user.Email,
                user.CreatedAt,
                user.LastLogin
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthLoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result == null)
                return Unauthorized(new { message = "Email ou mot de passe invalide" });

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken);

            if (result == null)
                return Unauthorized(new { message = "Jeton d'actualisation non valide ou périmé" });

            return Ok(result);
        }

    }
}
