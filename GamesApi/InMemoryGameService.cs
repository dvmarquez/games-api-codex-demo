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

    public IReadOnlyList<Game> GetAll() => games;

    public Game? GetById(int id) => games.FirstOrDefault(game => game.Id == id);

    public Game Create(CreateGameRequest request)
    {
        var nextId = games.Count == 0 ? 1 : games.Max(game => game.Id) + 1;
        var game = new Game(nextId, request.Title!, request.Genre!, request.Platform!);
        games.Add(game);

        return game;
    }
}
