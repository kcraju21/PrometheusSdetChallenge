using Microsoft.Extensions.Configuration;

namespace UiTests.Utils
{
    public static class ConfigReader
    {
        private static readonly IConfigurationRoot config =
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        public static string GoogleUrl => config["Urls:Google"]!;
        public static string SearchTerm => config["SearchTerm"]!;
    }
}
