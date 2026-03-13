using GamesApi;
using Xunit;

namespace GamesApi.Tests;

public sealed class InMemoryGameServiceTests
{
    [Fact]
    public async Task Create_AssignsUniqueIds_WhenCalledConcurrently()
    {
        var service = new InMemoryGameService();
        var requests = Enumerable.Range(1, 100)
            .Select(index => new CreateGameRequest($"Game {index}", "Action", "PC", 19.99m))
            .ToArray();

        var tasks = requests
            .Select(request => Task.Run(() => service.Create(request)))
            .ToArray();

        var createdGames = await Task.WhenAll(tasks);

        var distinctIdCount = createdGames
            .Select(game => game.Id)
            .Distinct()
            .Count();

        Assert.Equal(createdGames.Length, distinctIdCount);
    }
    [Fact]
    public void Update_ReturnsUpdatedGame_WhenGameExists()
    {
        var service = new InMemoryGameService();
        var request = new UpdateGameRequest("Updated", "Action", "PC", 39.99m);

        var updatedGame = service.Update(1, request);

        Assert.NotNull(updatedGame);
        Assert.Equal("Updated", updatedGame.Title);
        Assert.Equal(39.99m, updatedGame.Price);
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenGameDoesNotExist()
    {
        var service = new InMemoryGameService();

        var deleted = service.Delete(99999);

        Assert.False(deleted);
    }

}
