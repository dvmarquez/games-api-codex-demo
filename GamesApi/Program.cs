var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var games = new[]
{
    new Game(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", "Nintendo Switch"),
    new Game(2, "Halo Infinite", "First-person shooter", "Xbox Series X|S"),
    new Game(3, "God of War Ragnarök", "Action", "PlayStation 5"),
    new Game(4, "Hades", "Roguelike", "PC"),
    new Game(5, "Stardew Valley", "Simulation", "PC")
};

app.MapGet("/games", () => Results.Ok(games));

app.Run();

internal sealed record Game(int Id, string Title, string Genre, string Platform);
