namespace GameVerse.API.DTOs.Games
{
    public class GameDto
    {
        public int GameId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
    }
}
