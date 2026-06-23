using GameVerse.API.Data;
using GameVerse.API.DTOs.Auth;
using GameVerse.API.Models;
using GameVerse.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email already in use");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
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
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password");

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = _authService.GenerateJwtToken(user);

            return Ok(new
            {
                message = "Login successful",
                token
            });
        }


    }
}
