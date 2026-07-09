using GameVerse.API.Models;
using BCrypt.Net;

namespace GameVerse.API.Data
{
    public static class DataSeeder
    {
        public static void Seed(GameVerseContext context)
        {
            // -------------------------
            // 1. Jeux
            // -------------------------
            if (!context.Games.Any())
            {
                context.Games.AddRange(
                    new Game
                    {
                        Title = "Cyberpunk 2077",
                        Description = "RPG futuriste dans Night City.",
                        Genre = "RPG",
                        ReleaseDate = new DateTime(2020, 12, 10),
                        CoverUrl = "images/Cyberpunk.png",
                        Developer = "CD Projekt Red",
                        Publisher = "CD Projekt"
                    },
                    new Game
                    {
                        Title = "Elden Ring",
                        Description = "Action RPG dans un monde ouvert sombre.",
                        Genre = "Action RPG",
                        ReleaseDate = new DateTime(2022, 2, 25),
                        CoverUrl = "images/EldenRing.png",
                        Developer = "FromSoftware",
                        Publisher = "Bandai Namco"
                    },
                    new Game
                    {
                        Title = "Minecraft",
                        Description = "Sandbox créatif et survie.",
                        Genre = "Sandbox",
                        ReleaseDate = new DateTime(2011, 11, 18),
                        CoverUrl = "images/Minecraft.png",
                        Developer = "Mojang",
                        Publisher = "Mojang"
                    },
                    new Game
                    {
                        Title = "The Witcher 3",
                        Description = "RPG narratif dans un monde ouvert.",
                        Genre = "RPG",
                        ReleaseDate = new DateTime(2015, 5, 19),
                        CoverUrl = "images/TheWitcher3.png",
                        Developer = "CD Projekt Red",
                        Publisher = "CD Projekt"
                    },
                    new Game
                    {
                        Title = "Hades",
                        Description = "Rogue-like dynamique dans l'univers grec.",
                        Genre = "Rogue-like",
                        ReleaseDate = new DateTime(2020, 9, 17),
                        CoverUrl = "images/Hades.png",
                        Developer = "Supergiant Games",
                        Publisher = "Supergiant Games"
                    },
                    new Game
                    {
                        Title = "Stardew Valley",
                        Description = "Simulation agricole relaxante.",
                        Genre = "Simulation",
                        ReleaseDate = new DateTime(2016, 2, 26),
                        CoverUrl = "images/Stardew.png",
                        Developer = "ConcernedApe",
                        Publisher = "ConcernedApe"
                    },
                    new Game
                    {
                        Title = "Valorant",
                        Description = "FPS compétitif tactique.",
                        Genre = "FPS",
                        ReleaseDate = new DateTime(2020, 6, 2),
                        CoverUrl = "images/Valorant.png",
                        Developer = "Riot Games",
                        Publisher = "Riot Games"
                    },
                    new Game
                    {
                        Title = "Baldur's Gate 3",
                        Description = "RPG tactique basé sur Donjons & Dragons.",
                        Genre = "RPG",
                        ReleaseDate = new DateTime(2023, 8, 3),
                        CoverUrl = "images/BaldurGate3.png",
                        Developer = "Larian Studios",
                        Publisher = "Larian Studios"
                    },
                    new Game
                    {
                        Title = "GTA V",
                        Description = "Action dans un monde ouvert.",
                        Genre = "Action",
                        ReleaseDate = new DateTime(2013, 9, 17),
                        CoverUrl = "images/GtaV.png",
                        Developer = "Rockstar North",
                        Publisher = "Rockstar Games"
                    },
                    new Game
                    {
                        Title = "League of Legends",
                        Description = "MOBA compétitif.",
                        Genre = "MOBA",
                        ReleaseDate = new DateTime(2009, 10, 27),
                        CoverUrl = "images/Lol.png",
                        Developer = "Riot Games",
                        Publisher = "Riot Games"
                    }
                );
            }

            // -------------------------
            // 2. Utilisateurs
            // -------------------------
            if (!context.Users.Any())
            {
                var user1 = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Username = "test",
                    Email = "test@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("test123"),
                    CreatedAt = DateTime.UtcNow
                };

                var user2 = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Username = "delphine",
                    Email = "delphine@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("azerty"),
                    CreatedAt = DateTime.UtcNow
                };

                var user3 = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Username = "admin",
                    Email = "admin@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.AddRange(user1, user2, user3);
                context.SaveChanges();

                // -------------------------
                // 3. Relations UserGame
                // -------------------------
                var games = context.Games.ToList();

                context.UserGames.AddRange(
                    new UserGame
                    {
                        UserId = user1.UserId,
                        GameId = games[0].GameId,
                        RelationType = "Wishlist"
                    },
                    new UserGame
                    {
                        UserId = user1.UserId,
                        GameId = games[1].GameId,
                        RelationType = "Library",
                        Rating = 5
                    },
                    new UserGame
                    {
                        UserId = user2.UserId,
                        GameId = games[2].GameId,
                        RelationType = "Favorites",
                        Rating = 4
                    },
                    new UserGame
                    {
                        UserId = user2.UserId,
                        GameId = games[3].GameId,
                        RelationType = "Library",
                        Rating = 5
                    },
                    new UserGame
                    {
                        UserId = user3.UserId,
                        GameId = games[4].GameId,
                        RelationType = "Wishlist"
                    }
                );

                // -------------------------
                // 4. Refresh Token de test
                // -------------------------
                context.RefreshTokens.Add(new RefreshToken
                {
                    Token = Guid.NewGuid().ToString(),
                    UserId = user1.UserId,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                });
            }

            context.SaveChanges();
        }
    }
}
