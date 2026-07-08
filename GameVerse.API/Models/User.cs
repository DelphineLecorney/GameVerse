using System.ComponentModel.DataAnnotations.Schema;

namespace GameVerse.API.Models
{
    public class User
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;    
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        public string Role { get; set; } = "User";
        [NotMapped]
        public string Password { get; set; } = string.Empty;

        public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
    }
}
