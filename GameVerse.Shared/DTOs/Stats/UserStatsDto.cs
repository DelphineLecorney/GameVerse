namespace GameVerse.SHARED.DTOs.Stats
{
    public class UserStatsDto
    {
        public int TotalGames { get; set; }
        public int LibraryCount { get; set; }
        public int WishlistCount { get; set; }
        public int FavoritesCount { get; set; }
        public double AverageRating { get; set; }

        public Dictionary<string, int> GamesByGenre { get; set; } = new();
        public Dictionary<string, int> TopDevelopers { get; set; } = new();
    }
}