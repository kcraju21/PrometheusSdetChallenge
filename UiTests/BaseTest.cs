using Microsoft.Playwright;
using NUnit.Framework;
using System;
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

            // Decide headed/headless
            var isCi = Environment.GetEnvironmentVariable("CI") == "true";
            var headedEnv = Environment.GetEnvironmentVariable("HEADED");

            // Local default: headed (visible)
            // CI default: headless
            bool headed;
            if (isCi)
            {
                headed = false;
            }
            else if (headedEnv == "0")
            {
                headed = false;
            }
            else if (headedEnv == "1")
            {
                headed = true;
            }
            else
            {
                headed = true;
            }

            _browser = await _playwright.Chromium.LaunchAsync(new()
            {
                Headless = !headed,
                Args = new[]
                {
                    "--disable-dev-shm-usage",
                    "--no-sandbox",
                    "--disable-setuid-sandbox"
                }
            });

            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new() { Width = 1280, Height = 900 }
            });

            Page = await context.NewPageAsync();
        }

        [TearDown]
        public async Task Teardown()
        {
            if (Page is not null)
                await Page.CloseAsync();

            if (_browser is not null)
                await _browser.CloseAsync();

            _playwright?.Dispose();
        }
    }
}
