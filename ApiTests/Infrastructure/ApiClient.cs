using System.Net.Http;
using System.Text;
using System.Text.Json;
using ApiTests.Config;
using ApiTests.Dtos;
using Microsoft.Extensions.Logging;

namespace ApiTests.Infrastructure;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiClient> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(ApiConfig config, ILogger<ApiClient> logger)
    {
        _logger = logger;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(config.BaseUrl),
            Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds)
        };

        foreach (var header in config.DefaultHeaders)
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }
    }

    public async Task<HttpResponseMessage> GetAsync(string path)
    {
        _logger.LogInformation("GET {Path}", path);
        return await _httpClient.GetAsync(path);
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string path, T payload)
    {
        _logger.LogInformation("POST {Path} {@Payload}", path, payload);
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync(path, content);
    }

    public async Task<HttpResponseMessage> PutAsync<T>(string path, T payload)
    {
        _logger.LogInformation("PUT {Path} {@Payload}", path, payload);
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync(path, content);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string path)
    {
        _logger.LogInformation("DELETE {Path}", path);
        return await _httpClient.DeleteAsync(path);
    }

    public async Task<T?> ReadAsAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Response {Status} {Content}", response.StatusCode, content);
        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }
}
