namespace GamesApi;

internal sealed record CreateGameRequest(string? Title, string? Genre, string? Platform, decimal? Price);
