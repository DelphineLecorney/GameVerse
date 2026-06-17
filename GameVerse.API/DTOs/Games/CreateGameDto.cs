namespace GameVerse.API.DTOs.Games
{
    public class CreateGameDto
    {
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
    }
}
