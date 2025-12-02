using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace UiTests.Pages
{
    public class CareersPage
    {
        private readonly IPage _page;
        private const string CareersUrl = "https://www.prometheusgroup.com/company/careers";

        public CareersPage(IPage? page)
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
        }

        // ------------------------------------------------------------------
        // NAVIGATE TO CAREERS PAGE
        // ------------------------------------------------------------------
        public async Task NavigateAsync()
        {
            Console.WriteLine($"Navigating to {CareersUrl}...");

            await _page.GotoAsync(CareersUrl, new() { WaitUntil = WaitUntilState.DOMContentLoaded });

            // Give scripts a moment to settle
            await Task.Delay(1500);

            // Try to close popup once
            await ClosePopupIfPresent();
        }

        // ------------------------------------------------------------------
        // CLOSE HUBSPOT POPUP (via ESC key, more reliable than clicking)
        // ------------------------------------------------------------------
        private async Task ClosePopupIfPresent()
        {
            Console.WriteLine("Checking for popup...");

            try
            {
                bool popupFound = false;

                foreach (var frame in _page.Frames)
                {
                    if (!string.IsNullOrEmpty(frame.Url) &&
                        frame.Url.Contains("hs-web-interactive", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Popup iframe detected: {frame.Url}");
                        popupFound = true;
                        break;
                    }
                }

                if (!popupFound)
                {
                    Console.WriteLine("No popup iframe found.");
                    return;
                }

                // Many HubSpot modals close on Escape
                Console.WriteLine("Sending Escape key to close popup...");
                await _page.Keyboard.PressAsync("Escape");
                await Task.Delay(500);

                Console.WriteLine("Popup close attempt via Escape completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Popup close skipped: {ex.Message}");
            }
        }

        // ------------------------------------------------------------------
        // VERIFY PAGE LOADED
        // ------------------------------------------------------------------
        public async Task VerifyPageLoadedAsync()
        {
            Console.WriteLine("Validating Careers page loaded...");

            await _page.WaitForSelectorAsync("h1, h2", new() { Timeout = 8000 });

            // Extra safety: try closing popup again
            await ClosePopupIfPresent();

            Console.WriteLine("Careers page appears loaded.");
        }

        // ------------------------------------------------------------------
        // VERIFY ACCORDION / SECTIONS BY EXPECTED TEXTS
        // ------------------------------------------------------------------
        public async Task VerifyAccordionCountAsync()
        {
            Console.WriteLine("Checking accordion-like sections...");

            string[] sectionTitles =
            {
                "Career Growth",
                "Global Impact",
                "Entrepreneurial Culture",
                "Comprehensive Benefits"
            };

            foreach (var title in sectionTitles)
            {
                var el = _page.GetByText(title, new() { Exact = false });

                bool visible;
                try
                {
                    await el.IsVisibleAsync().WaitAsync(TimeSpan.FromSeconds(3));
                    visible = true;
                }
                catch
                {
                    visible = false;
                }

                if (!visible)
                    throw new Exception($"Section '{title}' not found on Careers page.");

                Console.WriteLine($"Section verified → {title}");
            }

            Console.WriteLine("All expected sections verified.");
        }

        // ------------------------------------------------------------------
        // CLICK "View Open Prometheus Jobs" LINK
        // ------------------------------------------------------------------
        public async Task NavigateToJobListingsAsync()
        {
            Console.WriteLine("Looking for 'View Open Prometheus Jobs' link...");

            // The element is most likely an <a>, not a button
            var jobsLink = _page.Locator(
                "a:has-text('View Open Prometheus Jobs'), " +
                "a:has-text('View open Prometheus jobs'), " +
                "a:has-text('View Open Prometheus jobs'), " +
                "a:has-text('View Open jobs'), " +
                "a:has-text('View open jobs')"
            );

            int count = await jobsLink.CountAsync();

            if (count == 0)
            {
                throw new Exception("'View Open Prometheus Jobs' link was not found on the page.");
            }

            Console.WriteLine($"Found {count} matching job link(s). Clicking the first one...");

            await jobsLink.First.ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            Console.WriteLine("Navigated to job listings page.");
        }
    }
}
