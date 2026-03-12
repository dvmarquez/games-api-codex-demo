namespace GamesApi;

internal interface IGameService
{
    IReadOnlyList<Game> GetAll();
    Game? GetById(int id);
}
