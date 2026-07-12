
using GameVerse.SHARED.DTOs.Games;

namespace GameVerse.SHARED.DTOs.UserGame
{
    public class UserGameDto
    {
        public string UserId { get; set; } = string.Empty;
        public int GameId { get; set; }
        public string RelationType { get; set; } = "Wishlist";
        public DateTime AddedAt { get; set; }
        public int? Rating { get; set; }

        public GameDto? Game { get; set; }
    }

}
