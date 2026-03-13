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
        Assert.True(document.RootElement[0].GetProperty("price").GetDecimal() > 0);
    }

    [Fact]
    public async Task GetGameById_ReturnsOkAndPrice_WhenGameExists()
    {
        var response = await client.GetAsync("/games/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(document.RootElement.GetProperty("price").GetDecimal() > 0);
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
            Platform = "PC",
            Price = 19.99m
        };

        var response = await client.PostAsJsonAsync("/games", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("Celeste", document.RootElement.GetProperty("title").GetString());
        Assert.Equal(19.99m, document.RootElement.GetProperty("price").GetDecimal());
    }

    [Fact]
    public async Task PostGames_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        var request = new
        {
            Title = "",
            Genre = "Platformer",
            Platform = "PC",
            Price = 0
        };

        var response = await client.PostAsJsonAsync("/games", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(document.RootElement.GetProperty("errors").TryGetProperty("Price", out _));
    }

    [Fact]
    public async Task PutGame_ReturnsOk_WhenRequestIsValidAndGameExists()
    {
        var request = new
        {
            Title = "The Legend of Zelda: Tears of the Kingdom",
            Genre = "Action-adventure",
            Platform = "Nintendo Switch",
            Price = 69.99m
        };

        var response = await client.PutAsJsonAsync("/games/1", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("The Legend of Zelda: Tears of the Kingdom", document.RootElement.GetProperty("title").GetString());
        Assert.Equal(69.99m, document.RootElement.GetProperty("price").GetDecimal());
    }

    [Fact]
    public async Task PutGame_ReturnsNotFound_WhenGameDoesNotExist()
    {
        var request = new
        {
            Title = "Game",
            Genre = "Action",
            Platform = "PC",
            Price = 19.99m
        };

        var response = await client.PutAsJsonAsync("/games/99999", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PutGame_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        var request = new
        {
            Title = "",
            Genre = "Action",
            Platform = "PC",
            Price = -1
        };

        var response = await client.PutAsJsonAsync("/games/1", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(document.RootElement.GetProperty("errors").TryGetProperty("Price", out _));
    }

    [Fact]
    public async Task DeleteGame_ReturnsNoContent_WhenGameExists()
    {
        var createRequest = new
        {
            Title = "Delete Me",
            Genre = "Action",
            Platform = "PC",
            Price = 24.99m
        };

        var createResponse = await client.PostAsJsonAsync("/games", createRequest);
        var createdGame = JsonDocument.Parse(await createResponse.Content.ReadAsStringAsync()).RootElement;
        var id = createdGame.GetProperty("id").GetInt32();

        var response = await client.DeleteAsync($"/games/{id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteGame_ReturnsNotFound_WhenGameDoesNotExist()
    {
        var response = await client.DeleteAsync("/games/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
