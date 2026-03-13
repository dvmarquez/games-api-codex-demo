using FluentValidation;
using FluentValidation.Results;
using GamesApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IGameService, InMemoryGameService>();
builder.Services.AddScoped<IValidator<CreateGameRequest>, CreateGameRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateGameRequest>, UpdateGameRequestValidator>();
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
    var validationProblem = await ValidateRequestAsync(request, validator);
    if (validationProblem is not null)
    {
        return validationProblem;
    }

    var createdGame = gameService.Create(request);
    return Results.Created($"/games/{createdGame.Id}", createdGame);
});

app.MapPut("/games/{id:int}", async (
    int id,
    UpdateGameRequest request,
    IValidator<UpdateGameRequest> validator,
    IGameService gameService) =>
{
    var validationProblem = await ValidateRequestAsync(request, validator);
    if (validationProblem is not null)
    {
        return validationProblem;
    }

    var updatedGame = gameService.Update(id, request);
    return updatedGame is null ? Results.NotFound() : Results.Ok(updatedGame);
});

app.MapDelete("/games/{id:int}", (int id, IGameService gameService) =>
{
    var deleted = gameService.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();

static async Task<IResult?> ValidateRequestAsync<TRequest>(
    TRequest request,
    IValidator<TRequest> validator)
{
    ValidationResult validationResult = await validator.ValidateAsync(request);
    if (validationResult.IsValid)
    {
        return null;
    }

    var errors = validationResult.Errors
        .GroupBy(error => error.PropertyName)
        .ToDictionary(
            group => group.Key,
            group => group.Select(error => error.ErrorMessage).ToArray());

    return Results.ValidationProblem(errors);
}

public partial class Program
{
}
