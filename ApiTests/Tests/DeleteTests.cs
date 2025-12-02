using System.Net;
using ApiTests.Infrastructure;
using Xunit;

namespace ApiTests.Tests;

[Collection("Api collection")]
public class DeleteTests
{
    private readonly ApiClient _client;

    public DeleteTests(ApiTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task Delete_ExistingPost_ShouldReturn200Or204()
    {
        var response = await _client.DeleteAsync("/posts/1");

        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.NoContent
        );
    }

    [Fact]
    public async Task Delete_NonExistingPost_ShouldStillReturnSuccess()
    {
        var response = await _client.DeleteAsync("/posts/9999");

        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.NoContent
        );
    }

    [Fact]
    public async Task Delete_InvalidPath_ShouldReturn404()
    {
        var response = await _client.DeleteAsync("/posts/foo/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
