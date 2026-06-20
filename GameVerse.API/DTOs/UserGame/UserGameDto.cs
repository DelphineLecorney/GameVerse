using GameVerse.API.DTOs.Games;

namespace GameVerse.API.DTOs.UserGame
{
    public class UserGameDto
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public string RelationType { get; set; } = "Wishlist";
        public DateTime AddedAt { get; set; }
        public int? Rating { get; set; }

        public GameDto? Game { get; set; }
    }

}
