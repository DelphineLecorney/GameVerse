namespace GameVerse.SHARED.DTOs.UserGame
{
    public class UpdateUserGameDto
    {
        public string RelationType { get; set; } = "Wishlist";
        public int? Rating { get; set; }
    }

}
