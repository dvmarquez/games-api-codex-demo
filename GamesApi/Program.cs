using GamesApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IGameService, InMemoryGameService>();

var app = builder.Build();

app.MapGet("/games", (IGameService gameService) => Results.Ok(gameService.GetAll()));

app.MapGet("/games/{id:int}", (int id, IGameService gameService) =>
{
    var game = gameService.GetById(id);
    return game is null ? Results.NotFound() : Results.Ok(game);
});

app.Run();
