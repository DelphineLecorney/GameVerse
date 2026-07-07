namespace GameVerse.SHARED.DTOs.Games
{
    public class GameDto
    {
        public int GameId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
    }

}
