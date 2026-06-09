namespace GameVerse.API.Models
{
    public class UserGame
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public string RelationType { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
