using System.Net;
using System.Text.Json;
using ApiTests.Dtos;
using ApiTests.Infrastructure;
using Xunit;

namespace ApiTests.Tests;

[Collection("Api collection")]
public class PostTests
{
    private readonly ApiClient _client;

    public PostTests(ApiTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task Create_Post_ShouldReturn201AndEchoBody()
    {
        var payload = new CreatePostDto
        {
            UserId = 1,
            Title = "Simple post title",
            Body = "Simple post body"
        };

        var response = await _client.PostAsync("/posts", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await _client.ReadAsAsync<PostDto>(response);

        Assert.NotNull(created);
        Assert.Equal(payload.Title, created!.Title);
        Assert.Equal(payload.Body, created.Body);
        Assert.True(created.Id > 0);
    }

    // Load data-driven posts from external JSON file
    public static IEnumerable<object[]> PostsFromJson()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "TestData", "postsData.json");
        var json = File.ReadAllText(path);
        var posts = JsonSerializer.Deserialize<List<CreatePostDto>>(json)!;

        return posts.Select(p => new object[] { p });
    }

    [Theory]
    [MemberData(nameof(PostsFromJson))]
    public async Task Create_Posts_FromExternalJson_ShouldReturn201(CreatePostDto payload)
    {
        var response = await _client.PostAsync("/posts", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await _client.ReadAsAsync<PostDto>(response);

        Assert.NotNull(created);
        Assert.Equal(payload.Title, created!.Title);
        Assert.Equal(payload.Body, created.Body);
    }

    [Fact]
    public async Task Create_Post_WithMissingBody_ShouldStillReturn201()
    {
        var payload = new CreatePostDto
        {
            UserId = 1,
            Title = "Title only"
        };

        var response = await _client.PostAsync("/posts", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await _client.ReadAsAsync<PostDto>(response);

        Assert.NotNull(created);
        Assert.Equal(payload.Title, created!.Title);
    }
}
