using System.Net;
using ApiTests.Dtos;
using ApiTests.Infrastructure;
using Xunit;

namespace ApiTests.Tests;

[Collection("Api collection")]
public class GetTests
{
    private readonly ApiClient _client;

    public GetTests(ApiTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task Get_AllPosts_ShouldReturn200AndNonEmptyList()
    {
        var response = await _client.GetAsync("/posts");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var posts = await _client.ReadAsAsync<List<PostDto>>(response);

        Assert.NotNull(posts);
        Assert.NotEmpty(posts!);
    }

    [Fact]
    public async Task Get_PostById_ShouldReturnCorrectPost()
    {
        var response = await _client.GetAsync("/posts/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var post = await _client.ReadAsAsync<PostDto>(response);

        Assert.NotNull(post);
        Assert.Equal(1, post!.Id);
        Assert.True(post.UserId > 0);
    }

    [Fact]
    public async Task Get_FilteredPostsByUserId_ShouldReturnOnlyThatUser()
    {
        var response = await _client.GetAsync("/posts?userId=1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var posts = await _client.ReadAsAsync<List<PostDto>>(response);

        Assert.NotNull(posts);
        Assert.NotEmpty(posts!);

        var distinctUsers = posts!.Select(p => p.UserId).Distinct();

        Assert.Equal(new[] { 1 }, distinctUsers);
    }
}
