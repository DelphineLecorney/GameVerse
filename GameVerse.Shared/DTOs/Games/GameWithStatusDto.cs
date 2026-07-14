namespace GameVerse.SHARED.DTOs.Games
{
    public class GameWithStatusDto : GameDto
    {
        public string? RelationType { get; set; }
        public bool IsFavorite { get; set; }
        public int? Rating { get; set; }
    }
}
