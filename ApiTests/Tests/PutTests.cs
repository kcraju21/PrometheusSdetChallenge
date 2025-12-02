using System.Net;
using ApiTests.Dtos;
using ApiTests.Infrastructure;
using Xunit;

namespace ApiTests.Tests;

[Collection("Api collection")]
public class PutTests
{
    private readonly ApiClient _client;

    public PutTests(ApiTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task Update_Post_ShouldReturnUpdatedRepresentation()
    {
        var payload = new PostDto
        {
            Id = 1,
            UserId = 1,
            Title = "Updated title",
            Body = "Updated body"
        };

        var response = await _client.PutAsync("/posts/1", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await _client.ReadAsAsync<PostDto>(response);

        Assert.NotNull(updated);
        Assert.Equal(payload.Title, updated!.Title);
        Assert.Equal(payload.Body, updated.Body);
    }

    [Fact]
    public async Task Update_Post_WithDifferentUser_ShouldSucceed()
    {
        var payload = new PostDto
        {
            Id = 1,
            UserId = 99,
            Title = "Different user",
            Body = "Body"
        };

        var response = await _client.PutAsync("/posts/1", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await _client.ReadAsAsync<PostDto>(response);

        Assert.NotNull(updated);
        Assert.Equal(99, updated!.UserId);
    }

    [Fact]
    public async Task Update_Post_WithEmptyTitle_ShouldReturn200()
    {
        var payload = new PostDto
        {
            Id = 1,
            UserId = 1,
            Title = "",
            Body = "Has body"
        };

        var response = await _client.PutAsync("/posts/1", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
