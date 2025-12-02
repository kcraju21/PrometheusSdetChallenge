using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace UiTests.Fixtures
{
    public class PlaywrightFixture
    {
        public IPlaywright Playwright { get; private set; } = null!;
        public IBrowser Browser { get; private set; } = null!;
        public IPage Page { get; private set; } = null!;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            // 🔥 Use Firefox — Google is much less strict with it
            Browser = await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 150
            });

            var context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true,

                // Spoof user-agent
                UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:109.0) Gecko/20100101 Firefox/119.0",

                // Remove Playwright automation fingerprints
                JavaScriptEnabled = true,
                AcceptDownloads = true,

                ViewportSize = new() { Width = 1280, Height = 800 }
            });

            Page = await context.NewPageAsync();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await Browser.CloseAsync();
            Playwright.Dispose();
        }
    }
}
