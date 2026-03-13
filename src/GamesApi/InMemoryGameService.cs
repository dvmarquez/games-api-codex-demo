using System.Threading;

namespace GamesApi;

internal sealed class InMemoryGameService : IGameService
{
    private readonly List<Game> games =
    [
        new Game(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", "Nintendo Switch", 59.99m),
        new Game(2, "Halo Infinite", "First-person shooter", "Xbox Series X|S", 39.99m),
        new Game(3, "God of War Ragnarök", "Action", "PlayStation 5", 69.99m),
        new Game(4, "Hades", "Roguelike", "PC", 24.99m),
        new Game(5, "Stardew Valley", "Simulation", "PC", 14.99m),
        new Game(6, "Elden Ring", "Action RPG", "PlayStation 5", 59.99m),
        new Game(7, "Forza Horizon 5", "Racing", "Xbox Series X|S", 59.99m),
        new Game(8, "Minecraft", "Sandbox", "PC", 29.99m),
        new Game(9, "Animal Crossing: New Horizons", "Simulation", "Nintendo Switch", 59.99m),
        new Game(10, "Red Dead Redemption 2", "Action-adventure", "PC", 39.99m),
        new Game(11, "The Witcher 3: Wild Hunt", "RPG", "PC", 29.99m),
        new Game(12, "Cyberpunk 2077", "Action RPG", "PlayStation 5", 49.99m),
        new Game(13, "Super Mario Odyssey", "Platformer", "Nintendo Switch", 59.99m),
        new Game(14, "Overwatch 2", "Hero shooter", "PC", 19.99m),
        new Game(15, "Apex Legends", "Battle royale", "PC", 9.99m),
        new Game(16, "Fortnite", "Battle royale", "PC", 9.99m),
        new Game(17, "Valorant", "Tactical shooter", "PC", 14.99m),
        new Game(18, "League of Legends", "MOBA", "PC", 9.99m),
        new Game(19, "Dota 2", "MOBA", "PC", 9.99m),
        new Game(20, "Counter-Strike 2", "First-person shooter", "PC", 14.99m),
        new Game(21, "Baldur's Gate 3", "RPG", "PC", 59.99m),
        new Game(22, "Hollow Knight", "Metroidvania", "PC", 14.99m),
        new Game(23, "Ori and the Will of the Wisps", "Platformer", "Xbox Series X|S", 29.99m),
        new Game(24, "Resident Evil 4", "Survival horror", "PlayStation 5", 59.99m),
        new Game(25, "Final Fantasy VII Rebirth", "RPG", "PlayStation 5", 69.99m),
        new Game(26, "Persona 5 Royal", "JRPG", "PlayStation 5", 59.99m),
        new Game(27, "Monster Hunter Rise", "Action RPG", "Nintendo Switch", 39.99m),
        new Game(28, "Diablo IV", "Action RPG", "PC", 69.99m),
        new Game(29, "Sea of Thieves", "Adventure", "Xbox Series X|S", 39.99m),
        new Game(30, "Rocket League", "Sports", "PC", 19.99m),
        new Game(31, "EA Sports FC 24", "Sports", "PlayStation 5", 69.99m),
        new Game(32, "NBA 2K24", "Sports", "Xbox Series X|S", 69.99m),
        new Game(33, "Gran Turismo 7", "Racing", "PlayStation 5", 69.99m),
        new Game(34, "Mario Kart 8 Deluxe", "Racing", "Nintendo Switch", 59.99m),
        new Game(35, "Splatoon 3", "Shooter", "Nintendo Switch", 59.99m),
        new Game(36, "Metroid Dread", "Metroidvania", "Nintendo Switch", 59.99m),
        new Game(37, "Fire Emblem Engage", "Strategy RPG", "Nintendo Switch", 59.99m),
        new Game(38, "XCOM 2", "Strategy", "PC", 29.99m),
        new Game(39, "Civilization VI", "Strategy", "PC", 29.99m),
        new Game(40, "Age of Empires IV", "RTS", "PC", 39.99m),
        new Game(41, "Terraria", "Sandbox", "PC", 9.99m),
        new Game(42, "No Man's Sky", "Survival", "PC", 59.99m),
        new Game(43, "Subnautica", "Survival", "PC", 29.99m),
        new Game(44, "Dead Cells", "Roguelike", "PC", 24.99m),
        new Game(45, "Slay the Spire", "Deck-building roguelike", "PC", 24.99m),
        new Game(46, "Among Us", "Party", "Mobile", 4.99m),
        new Game(47, "Genshin Impact", "Action RPG", "Mobile", 9.99m),
        new Game(48, "Clash Royale", "Strategy", "Mobile", 4.99m),
        new Game(49, "Call of Duty: Warzone", "Battle royale", "PC", 19.99m),
        new Game(50, "Street Fighter 6", "Fighting", "PlayStation 5", 54.99m)
    ];

    private readonly Lock gamesLock = new();
    private int nextId = 50;

    public IReadOnlyList<Game> GetAll()
    {
        lock (gamesLock)
        {
            return games.ToList();
        }
    }

    public Game? GetById(int id)
    {
        lock (gamesLock)
        {
            return games.FirstOrDefault(game => game.Id == id);
        }
    }

    public Game Create(CreateGameRequest request)
    {
        var id = Interlocked.Increment(ref nextId);
        var game = new Game(id, request.Title!, request.Genre!, request.Platform!, request.Price!.Value);

        lock (gamesLock)
        {
            games.Add(game);
        }

        return game;
    }

    public Game? Update(int id, UpdateGameRequest request)
    {
        lock (gamesLock)
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index < 0)
            {
                return null;
            }

            var updatedGame = new Game(id, request.Title!, request.Genre!, request.Platform!, request.Price!.Value);
            games[index] = updatedGame;
            return updatedGame;
        }
    }

    public bool Delete(int id)
    {
        lock (gamesLock)
        {
            var game = games.FirstOrDefault(existingGame => existingGame.Id == id);
            if (game is null)
            {
                return false;
            }

            return games.Remove(game);
        }
    }
}
