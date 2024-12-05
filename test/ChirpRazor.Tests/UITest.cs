using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;


namespace Chirp.Razor.test.ChirpRazor.Tests{
    
    [Parallelizable(ParallelScope.Self)]
	[TestFixture]
	public class UITest : PageTest
	{

        private Process? _serverProcess;

        [SetUp]
        public async Task Init()
        {
            _serverProcess = Chirp.Razor.test.ChirpRazor.Tests.ServerBoot.ServerStart(); // Custom utility class - not part of Playwright
            _serverProcess.Start();
            await Chirp.Razor.test.ChirpRazor.Tests.ServerBoot.WaitForServerToBoot();
        }

        [TearDown]
        public void StopServer()
        {
            if(_serverProcess != null){
                _serverProcess.Kill();
                _serverProcess.Dispose();
            }
            
        }

        [Test]
        public async Task test(){

            await Page.GotoAsync("https://playwright.dev");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));
        }





    }
}