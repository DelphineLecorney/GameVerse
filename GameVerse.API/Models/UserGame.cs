namespace GameVerse.API.Models
{
    public class UserGame
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public string RelationType { get; set; } = "Wishlist";
        public DateTime AddedAt { get; set; } = DateTime.Now;
        public int? Rating { get; set; }

        public User User { get; set; }
        public Game Game { get; set; }

    }
}
