using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Chirp.Razor;
using Microsoft.AspNetCore.Builder;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UI_Test : PageTest
{

    private  CancellationTokenSource cts;
    [SetUp]
    public async Task Init()
    {   

        cts = new CancellationTokenSource();

        var task = Task.Run(() =>{
            var args = new string[] { "--urls=http://localhost:5000" };
            var app = Chirp.Razor.Program.WebAppBuildForTest(args);

            app.Run();
        }, cts.Token);
        
        await WaitForServerToBoot();
    }


    private static async Task WaitForServerToBoot(){
          using var httpClient = new HttpClient();
        var retries = 10;
        while (retries-- > 0)
        {
            try
            {
                var response = await httpClient.GetAsync("http://localhost:5000");
                if (response.IsSuccessStatusCode) return;
            }
            catch
            {
                // Ignore connection errors and retry
            }

            await Task.Delay(1000); // Wait and retry
        }

        throw new Exception("Server did not start in time.");
    }
    

    [TearDown]
    public async Task Cleanup()
    {
       cts.Cancel();
    }

    [Test]
    public async Task HasTitle()
    {
        await Page.GotoAsync("https://playwright.dev");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));
    }

    [Test]
    public async Task GetStartedLink()
    {
        await Page.GotoAsync("https://playwright.dev");

        // Click the get started link.
        await Page.GetByRole(AriaRole.Link, new() { Name = "Get started" }).ClickAsync();

        // Expects page to have a heading with the name of Installation.
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Installation" })).ToBeVisibleAsync();
    } 
}