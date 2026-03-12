using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GamesApi.Tests;

public sealed class GamesApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public GamesApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetGames_ReturnsOkAndNonEmptyList()
    {
        var response = await client.GetAsync("/games");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, document.RootElement.ValueKind);
        Assert.True(document.RootElement.GetArrayLength() > 0);
    }

    [Fact]
    public async Task GetGameById_ReturnsOk_WhenGameExists()
    {
        var response = await client.GetAsync("/games/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetGameById_ReturnsNotFound_WhenGameDoesNotExist()
    {
        var response = await client.GetAsync("/games/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostGames_ReturnsCreated_WhenRequestIsValid()
    {
        var request = new
        {
            Title = "Celeste",
            Genre = "Platformer",
            Platform = "PC"
        };

        var response = await client.PostAsJsonAsync("/games", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("Celeste", document.RootElement.GetProperty("title").GetString());
    }

    [Fact]
    public async Task PostGames_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        var request = new
        {
            Title = "",
            Genre = "Platformer",
            Platform = "PC"
        };

        var response = await client.PostAsJsonAsync("/games", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
