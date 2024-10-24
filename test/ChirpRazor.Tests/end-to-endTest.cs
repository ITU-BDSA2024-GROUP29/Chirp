using Microsoft.AspNetCore.Mvc.Testing;


namespace Chirp.Razor.test;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class TestAPI : IClassFixture<WebApplicationFactory<Program>>
{
    /*
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;
    public TestAPI(WebApplicationFactory<Program> fixture) //changed from public to private
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    [Fact]
    public async void CanSeePublicTimeline()
    {
        var response = await _client.GetAsync("/public");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains("Public Timeline", content);
    }

    [Theory]
    [InlineData("Helge")]
    [InlineData("Adrian")]
    public async void CanSeePrivateTimeline(string author)
    {
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains($"{author}'s Timeline", content);
    }
    */

}
