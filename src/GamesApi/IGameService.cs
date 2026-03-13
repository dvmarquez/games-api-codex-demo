namespace GamesApi;

internal interface IGameService
{
    IReadOnlyList<Game> GetAll();
    Game? GetById(int id);
    Game Create(CreateGameRequest request);
    Game? Update(int id, UpdateGameRequest request);
    bool Delete(int id);
}
