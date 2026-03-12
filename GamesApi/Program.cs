using FluentValidation;
using GamesApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IGameService, InMemoryGameService>();
builder.Services.AddScoped<IValidator<CreateGameRequest>, CreateGameRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/games", (IGameService gameService) => Results.Ok(gameService.GetAll()));

app.MapGet("/games/{id:int}", (int id, IGameService gameService) =>
{
    var game = gameService.GetById(id);
    return game is null ? Results.NotFound() : Results.Ok(game);
});

app.MapPost("/games", async (
    CreateGameRequest request,
    IValidator<CreateGameRequest> validator,
    IGameService gameService) =>
{
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
        var errors = validationResult.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());

        return Results.ValidationProblem(errors);
    }

    var createdGame = gameService.Create(request);
    return Results.Created($"/games/{createdGame.Id}", createdGame);
});

app.Run();

public partial class Program
{
}
