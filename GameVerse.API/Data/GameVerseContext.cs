using Microsoft.EntityFrameworkCore;
using GameVerse.API.Models;

namespace GameVerse.API.Data
{
    public class GameVerseContext : DbContext
    {
        public GameVerseContext(DbContextOptions<GameVerseContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserGame> UserGames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Clé composite pour UserGame
            modelBuilder.Entity<UserGame>()
                .HasKey(ug => new { ug.UserId, ug.GameId, ug.RelationType });

            base.OnModelCreating(modelBuilder);
        }
    }
}
