namespace GameVerse.API.Models
{
    public class UserGame
    {
        public string UserId { get; set; } = string.Empty;
        public int GameId { get; set; }

        public string RelationType { get; set; } = "Wishlist";
        public bool IsFavorite { get; set; } = false;

        public DateTime AddedAt { get; set; } = DateTime.Now;
        public int? Rating { get; set; }

        public User? User { get; set; }
        public Game? Game { get; set; }

    }
}
