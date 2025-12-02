using NUnit.Framework;
using System.Threading.Tasks;
using UiTests.Pages;

namespace UiTests.Tests
{
    public class GoogleCareerTests : BaseTest
    {
        [Test]
        public async Task Validate_Prometheus_Careers_Flow()
        {
            // Go directly to careers page (skip Google due to CAPTCHA)
            var careers = new CareersPage(Page);
            await careers.NavigateAsync();

            // Validate careers page structure
            await careers.VerifyPageLoadedAsync();

            // Validate accordion sections
            await careers.VerifyAccordionCountAsync();

            // Click "View Open Prometheus Jobs"
            await careers.NavigateToJobListingsAsync();

            // Validate Senior SDET posting
            var jobs = new LinkedInJobsPage(Page);
            await jobs.VerifySeniorSDETExists();
        }
    }
}
