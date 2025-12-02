using ApiTests.Config;
using Microsoft.Extensions.Logging;

namespace ApiTests.Infrastructure;

public class ApiTestFixture : IDisposable
{
    public ApiClient Client { get; }
    public ILogger<ApiClient> Logger { get; }

    private readonly ILoggerFactory _loggerFactory;

    public ApiTestFixture()
    {
        var config = TestConfig.GetApiConfig();

        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(LogLevel.Information)
                .AddConsole();
        });

        Logger = _loggerFactory.CreateLogger<ApiClient>();

        Client = new ApiClient(config, Logger);
    }

    public void Dispose()
    {
        _loggerFactory.Dispose();
    }
}
