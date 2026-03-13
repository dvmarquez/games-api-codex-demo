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
            .Select(index => new CreateGameRequest($"Game {index}", "Action", "PC"))
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
}
