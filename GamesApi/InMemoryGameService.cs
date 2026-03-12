namespace GamesApi;

internal sealed class InMemoryGameService : IGameService
{
    private readonly IReadOnlyList<Game> games =
    [
        new Game(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", "Nintendo Switch"),
        new Game(2, "Halo Infinite", "First-person shooter", "Xbox Series X|S"),
        new Game(3, "God of War Ragnarök", "Action", "PlayStation 5"),
        new Game(4, "Hades", "Roguelike", "PC"),
        new Game(5, "Stardew Valley", "Simulation", "PC")
    ];

    public IReadOnlyList<Game> GetAll() => games;

    public Game? GetById(int id) => games.FirstOrDefault(game => game.Id == id);
}
