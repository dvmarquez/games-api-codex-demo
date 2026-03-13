namespace GamesApi;

internal sealed record UpdateGameRequest(string? Title, string? Genre, string? Platform, decimal? Price);
