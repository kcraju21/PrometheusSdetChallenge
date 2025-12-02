using System.Net;
using ApiTests.Dtos;
using ApiTests.Infrastructure;
using Xunit;

namespace ApiTests.Tests;

[Collection("Api collection")]
public class NegativeTests
{
    private readonly ApiClient _client;

    public NegativeTests(ApiTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task Get_InvalidEndpoint_ShouldReturn404()
    {
        var response = await _client.GetAsync("/invalid-endpoint");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_ToInvalidEndpoint_ShouldReturn404()
    {
        var payload = new CreatePostDto
        {
            UserId = 1,
            Title = "Invalid endpoint",
            Body = "Body"
        };

        var response = await _client.PostAsync("/postss", payload);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_WithInvalidPath_ShouldReturn404()
    {
        var payload = new PostDto
        {
            Id = 1,
            UserId = 1,
            Title = "Title",
            Body = "Body"
        };

        var response = await _client.PutAsync("/posts/foo/1", payload);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
