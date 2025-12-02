using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UiTests.Pages
{
    public class GooglePage
    {
        private readonly IPage _page;

        public GooglePage(IPage page)
        {
            _page = page;
        }

        // Updated selector: Google uses <textarea> for bots and <input> for humans
        private ILocator SearchBox => _page.Locator("textarea[name='q'], input[name='q']");

        public async Task NavigateAsync(string url)
        {
            Console.WriteLine("Navigating to Google...");
            Console.WriteLine("Page URL before navigation: " + _page.Url);

            await _page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });

            Console.WriteLine("Page URL after navigation: " + _page.Url);

            // ===================================================
            // 1. HANDLE CONSENT POPUP IN IFRAMES
            // ===================================================
            foreach (var frame in _page.Frames)
            {
                if (Regex.IsMatch(frame.Url, "consent", RegexOptions.IgnoreCase))
                {
                    Console.WriteLine("Consent iframe found!");

                    var agree = frame.GetByRole(AriaRole.Button, new() { Name = "I agree" });
                    if (await agree.IsVisibleAsync()) await agree.ClickAsync();

                    var acceptAll = frame.GetByRole(AriaRole.Button, new() { Name = "Accept all" });
                    if (await acceptAll.IsVisibleAsync()) await acceptAll.ClickAsync();

                    var allowAll = frame.GetByRole(AriaRole.Button, new() { Name = "Allow all" });
                    if (await allowAll.IsVisibleAsync()) await allowAll.ClickAsync();
                }
            }

            // ===================================================
            // 2. HANDLE CONSENT POPUPS DIRECTLY ON PAGE
            // ===================================================
            string[] buttonLabels =
            {
                "I agree",
                "Accept all",
                "Allow all",
                "No thanks",
                "Reject all",
                "Got it",
                "OK"
            };

            foreach (var btnLabel in buttonLabels)
            {
                var btn = _page.GetByRole(AriaRole.Button, new() { Name = btnLabel });
                if (await btn.IsVisibleAsync())
                {
                    Console.WriteLine($"Clicking consent button: {btnLabel}");
                    await btn.ClickAsync();
                }
            }

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // ===================================================
            // 3. TAKE DEBUG SCREENSHOT
            // ===================================================
            Console.WriteLine("Taking debug screenshot: google_debug.png");
            await _page.ScreenshotAsync(new()
            {
                Path = "google_debug.png",
                FullPage = true
            });

            // ===================================================
            // 4. WAIT FOR SEARCH BOX USING MULTI-SELECTOR
            // ===================================================
            Console.WriteLine("Waiting for search box...");

            await _page.WaitForSelectorAsync("textarea[name='q'], input[name='q']", new()
            {
                Timeout = 15000
            });

            await Assertions.Expect(SearchBox).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
            {
                Timeout = 15000
            });

            Console.WriteLine("Search box is visible!");
        }

        public async Task SearchAsync(string term)
        {
            Console.WriteLine($"Searching for: {term}");

            await SearchBox.FillAsync(term);
            await _page.Keyboard.PressAsync("Enter");
        }
    }
}
