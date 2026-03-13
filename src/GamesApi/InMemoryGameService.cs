using System.Threading;

namespace GamesApi;

internal sealed class InMemoryGameService : IGameService
{
    private readonly List<Game> games =
    [
        new Game(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", "Nintendo Switch"),
        new Game(2, "Halo Infinite", "First-person shooter", "Xbox Series X|S"),
        new Game(3, "God of War Ragnarök", "Action", "PlayStation 5"),
        new Game(4, "Hades", "Roguelike", "PC"),
        new Game(5, "Stardew Valley", "Simulation", "PC")
    ];

    private readonly Lock gamesLock = new();
    private int nextId = 5;

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
        var game = new Game(id, request.Title!, request.Genre!, request.Platform!);

        lock (gamesLock)
        {
            games.Add(game);
        }

        return game;
    }
}
