using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Threading.Tasks;

namespace UiTests.Pages
{
    public class SearchResultsPage
    {
        private readonly IPage _page;

        public SearchResultsPage(IPage page)
        {
            _page = page;
        }

        public async Task VerifyPrometheusFound()
        {
            Console.WriteLine("Skipping Google results due to captcha. Navigating directly to Careers page...");
        }

        public async Task GoToCareersPage()
        {
            Console.WriteLine("Direct navigation to Prometheus Group Careers page...");
            await _page.GotoAsync("https://www.prometheusgroup.com/careers");
        }
    }
}
