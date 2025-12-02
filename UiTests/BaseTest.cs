using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace UiTests
{
    public class BaseTest
    {
        public IPage Page { get; private set; } = default!;
        private IPlaywright _playwright = default!;
        private IBrowser _browser = default!;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();

            // 👇 Add SlowMo to visibly slow down automation
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,   // show browser
                SlowMo = 5000       // <-- SLOW DOWN EVERY ACTION BY 5000ms
            });

            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new() { Width = 1280, Height = 900 }
            });

            Page = await context.NewPageAsync();

            Console.WriteLine("🚀 Browser launched with SlowMo (500ms).");
        }

        [TearDown]
        public async Task Teardown()
        {
            Console.WriteLine("🧹 Cleaning up browser...");

            if (Page != null)
                await Page.CloseAsync();

            if (_browser != null)
                await _browser.CloseAsync();

            _playwright?.Dispose();

            Console.WriteLine("✅ Browser closed.");
        }
    }
}
