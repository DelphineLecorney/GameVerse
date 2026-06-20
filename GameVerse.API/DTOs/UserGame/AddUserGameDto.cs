namespace GameVerse.API.DTOs.UserGame
{
    public class AddUserGameDto
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public string RelationType { get; set; } = "Wishlist";
        public int? Rating { get; set; }
    }
}
