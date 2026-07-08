namespace GameVerse.SHARED.DTOs.UserGame
{
    public class AddUserGameDto
    {
        public string UserId { get; set; }
        public int GameId { get; set; }
        public string RelationType { get; set; } = "Wishlist";
        public int? Rating { get; set; }
    }
}
