namespace GameVerse.SHARED.DTOs.UserGame
{
    public class UpdateUserGameDto
    {
        public string RelationType { get; set; } = "Wishlist";
        public bool IsFavorite { get; set; } = false;
        public int? Rating { get; set; }
    }

}
