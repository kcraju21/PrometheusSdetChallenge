using System;
using Microsoft.Extensions.Configuration;

namespace ApiTests.Config;

public static class TestConfig
{
    private static readonly Lazy<IConfigurationRoot> _config = new(() =>
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        return builder.Build();
    });

    public static IConfigurationRoot Configuration => _config.Value;

    public static ApiConfig GetApiConfig()
    {
        var apiConfig = new ApiConfig();
        Configuration.GetSection("Api").Bind(apiConfig);
        return apiConfig;
    }
}
