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
                    new Game { Title = "Cyberpunk 2077", Description = "RPG futuriste dans Night City.", Genre = "RPG", ReleaseDate = new DateTime(2020, 12, 10), CoverUrl = "images/Cyberpunk.png", Developer = "CD Projekt Red", Publisher = "CD Projekt" },
                    new Game { Title = "Elden Ring", Description = "Action RPG dans un monde ouvert sombre.", Genre = "Action RPG", ReleaseDate = new DateTime(2022, 2, 25), CoverUrl = "images/EldenRing.png", Developer = "FromSoftware", Publisher = "Bandai Namco" },
                    new Game { Title = "Minecraft", Description = "Sandbox créatif et survie.", Genre = "Sandbox", ReleaseDate = new DateTime(2011, 11, 18), CoverUrl = "images/Minecraft.png", Developer = "Mojang", Publisher = "Mojang" },
                    new Game { Title = "The Witcher 3", Description = "RPG narratif dans un monde ouvert.", Genre = "RPG", ReleaseDate = new DateTime(2015, 5, 19), CoverUrl = "images/TheWitcher3.png", Developer = "CD Projekt Red", Publisher = "CD Projekt" },
                    new Game { Title = "Hades", Description = "Rogue-like dynamique dans l'univers grec.", Genre = "Rogue-like", ReleaseDate = new DateTime(2020, 9, 17), CoverUrl = "images/Hades.png", Developer = "Supergiant Games", Publisher = "Supergiant Games" },
                    new Game { Title = "Stardew Valley", Description = "Simulation agricole relaxante.", Genre = "Simulation", ReleaseDate = new DateTime(2016, 2, 26), CoverUrl = "images/Stardew.png", Developer = "ConcernedApe", Publisher = "ConcernedApe" },
                    new Game { Title = "Valorant", Description = "FPS compétitif tactique.", Genre = "FPS", ReleaseDate = new DateTime(2020, 6, 2), CoverUrl = "images/Valorant.png", Developer = "Riot Games", Publisher = "Riot Games" },
                    new Game { Title = "Baldur's Gate 3", Description = "RPG tactique basé sur Donjons & Dragons.", Genre = "RPG", ReleaseDate = new DateTime(2023, 8, 3), CoverUrl = "images/BaldurGate3.png", Developer = "Larian Studios", Publisher = "Larian Studios" },
                    new Game { Title = "GTA V", Description = "Action dans un monde ouvert.", Genre = "Action", ReleaseDate = new DateTime(2013, 9, 17), CoverUrl = "images/GtaV.png", Developer = "Rockstar North", Publisher = "Rockstar Games" },
                    new Game { Title = "League of Legends", Description = "MOBA compétitif.", Genre = "MOBA", ReleaseDate = new DateTime(2009, 10, 27), CoverUrl = "images/Lol.png", Developer = "Riot Games", Publisher = "Riot Games" },
                    new Game { Title = "Hollow Knight", Description = "Metroidvania exigeant et poétique.", Genre = "Metroidvania", ReleaseDate = new DateTime(2017, 2, 24), CoverUrl = "images/HollowKnight.png", Developer = "Team Cherry", Publisher = "Team Cherry" },
                    new Game { Title = "Animal Crossing: New Horizons", Description = "Simulation de vie sur une île déserte.", Genre = "Simulation", ReleaseDate = new DateTime(2020, 3, 20), CoverUrl = "images/AnimalCrossing.png", Developer = "Nintendo", Publisher = "Nintendo" },
                    new Game { Title = "Overwatch 2", Description = "FPS héros en équipe.", Genre = "FPS", ReleaseDate = new DateTime(2022, 10, 4), CoverUrl = "images/Overwatch2.png", Developer = "Blizzard Entertainment", Publisher = "Blizzard Entertainment" },
                    new Game { Title = "Celeste", Description = "Plateformer précis sur le dépassement de soi.", Genre = "Plateforme", ReleaseDate = new DateTime(2018, 1, 25), CoverUrl = "images/Celeste.png", Developer = "Maddy Makes Games", Publisher = "Maddy Makes Games" },
                    new Game { Title = "Dark Souls III", Description = "Action RPG exigeant et atmosphérique.", Genre = "Action RPG", ReleaseDate = new DateTime(2016, 4, 12), CoverUrl = "images/DarkSouls3.png", Developer = "FromSoftware", Publisher = "Bandai Namco" },
                    new Game { Title = "God of War Ragnarök", Description = "Action-aventure mythologique nordique.", Genre = "Action", ReleaseDate = new DateTime(2022, 11, 9), CoverUrl = "images/GodOfWarRagnarok.png", Developer = "Santa Monica Studio", Publisher = "Sony Interactive Entertainment" },
                    new Game { Title = "Terraria", Description = "Sandbox 2D d'exploration et de survie.", Genre = "Sandbox", ReleaseDate = new DateTime(2011, 5, 16), CoverUrl = "images/Terraria.png", Developer = "Re-Logic", Publisher = "Re-Logic" },
                    new Game { Title = "Persona 5 Royal", Description = "RPG japonais stylisé sur la vie de lycéens justiciers.", Genre = "RPG", ReleaseDate = new DateTime(2020, 3, 31), CoverUrl = "images/Persona5Royal.png", Developer = "Atlus", Publisher = "Atlus" },
                    new Game { Title = "Rocket League", Description = "Football énergique avec des voitures.", Genre = "Sport", ReleaseDate = new DateTime(2015, 7, 7), CoverUrl = "images/RocketLeague.png", Developer = "Psyonix", Publisher = "Psyonix" },
                    new Game { Title = "It Takes Two", Description = "Aventure coopérative à deux joueurs.", Genre = "Aventure", ReleaseDate = new DateTime(2021, 3, 26), CoverUrl = "images/ItTakesTwo.png", Developer = "Hazelight Studios", Publisher = "Electronic Arts" }
                    );

                context.SaveChanges();
            }

            // -------------------------
            // 2. Utilisateurs
            // -------------------------
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { UserId = Guid.NewGuid().ToString(), Username = "Test", Email = "test@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"), CreatedAt = DateTime.UtcNow },
                    new User { UserId = Guid.NewGuid().ToString(), Username = "Delphine", Email = "delphine@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Azerty123!"), CreatedAt = DateTime.UtcNow },
                    new User { UserId = Guid.NewGuid().ToString(), Username = "Admin", Email = "admin@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"), CreatedAt = DateTime.UtcNow },
                    new User { UserId = Guid.NewGuid().ToString(), Username = "Maxime", Email = "maxime@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Maxime123!"), CreatedAt = DateTime.UtcNow },
                    new User { UserId = Guid.NewGuid().ToString(), Username = "Lucas", Email = "lucas@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Lucas123!"), CreatedAt = DateTime.UtcNow }
                );

                context.SaveChanges();
            }

            // -------------------------
            // 3. Relations UserGame
            // -------------------------
            if (!context.UserGames.Any())
            {
                var games = context.Games.ToList();
                var users = context.Users.ToList();

                var userTest = users.First(u => u.Username == "Test");
                var userDelphine = users.First(u => u.Username == "Delphine");
                var userAdmin = users.First(u => u.Username == "Admin");
                var userMaxime = users.First(u => u.Username == "Maxime");
                var userLucas = users.First(u => u.Username == "Lucas");

                int GameId(string title) => games.First(g => g.Title == title).GameId;

                context.UserGames.AddRange(
                    //  Test 
                    new UserGame { UserId = userTest.UserId, GameId = GameId("Cyberpunk 2077"), RelationType = "Wishlist" },
                    new UserGame { UserId = userTest.UserId, GameId = GameId("Elden Ring"), RelationType = "Library", Rating = 5 },
                    new UserGame { UserId = userTest.UserId, GameId = GameId("Hollow Knight"), RelationType = "Library", IsFavorite = true, Rating = 5 },
                    new UserGame { UserId = userTest.UserId, GameId = GameId("Terraria"), RelationType = "Library", Rating = 4 },
                    new UserGame { UserId = userTest.UserId, GameId = GameId("Rocket League"), RelationType = "Wishlist" },

                    // Delphine 
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("Minecraft"), RelationType = "Library", IsFavorite = true, Rating = 4 },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("Baldur's Gate 3"), RelationType = "Library", IsFavorite = true, Rating = 5 },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("Persona 5 Royal"), RelationType = "Library", IsFavorite = true, Rating = 5 },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("The Witcher 3"), RelationType = "Library", Rating = 5 },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("Hades"), RelationType = "Library", Rating = 4 },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("GTA V"), RelationType = "Library", Rating = 3 },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("Celeste"), RelationType = "Library", Rating = 5 },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("It Takes Two"), RelationType = "Library", Rating = 5 },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("Stardew Valley"), RelationType = "Wishlist" },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("Valorant"), RelationType = "Wishlist" },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("Animal Crossing: New Horizons"), RelationType = "Wishlist" },
                    new UserGame { UserId = userDelphine.UserId, GameId = GameId("God of War Ragnarök"), RelationType = "Wishlist" },

                    // Admin 
                    new UserGame { UserId = userAdmin.UserId, GameId = GameId("League of Legends"), RelationType = "Wishlist" },
                    new UserGame { UserId = userAdmin.UserId, GameId = GameId("Dark Souls III"), RelationType = "Library", Rating = 4 },
                    new UserGame { UserId = userAdmin.UserId, GameId = GameId("Overwatch 2"), RelationType = "Library", IsFavorite = true, Rating = 4 },

                    //  Maxime 
                    new UserGame { UserId = userMaxime.UserId, GameId = GameId("Overwatch 2"), RelationType = "Library", Rating = 3 },
                    new UserGame { UserId = userMaxime.UserId, GameId = GameId("Elden Ring"), RelationType = "Library", IsFavorite = true, Rating = 5 },
                    new UserGame { UserId = userMaxime.UserId, GameId = GameId("Hollow Knight"), RelationType = "Wishlist" },
                    new UserGame { UserId = userMaxime.UserId, GameId = GameId("Rocket League"), RelationType = "Library", Rating = 4 },

                    //  Lucas 
                    new UserGame { UserId = userLucas.UserId, GameId = GameId("Cyberpunk 2077"), RelationType = "Library", Rating = 4 },
                    new UserGame { UserId = userLucas.UserId, GameId = GameId("God of War Ragnarök"), RelationType = "Library", IsFavorite = true, Rating = 5 },
                    new UserGame { UserId = userLucas.UserId, GameId = GameId("Terraria"), RelationType = "Wishlist" },
                    new UserGame { UserId = userLucas.UserId, GameId = GameId("Persona 5 Royal"), RelationType = "Wishlist" }
                );

                context.SaveChanges();
            }

            // -------------------------
            // 4. Refresh Token de test
            // -------------------------
            if (!context.RefreshTokens.Any())
            {
                var userTest = context.Users.First(u => u.Username == "Test");

                context.RefreshTokens.Add(new RefreshToken
                {
                    Token = Guid.NewGuid().ToString(),
                    UserId = userTest.UserId,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                });

                context.SaveChanges();
            }
        }
    }
}