using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class MyTimelineModel : SharedFuncs
{
    private readonly ICheepService _cheepService;
    public List<CheepViewModel> Cheeps { get; set; } = new();

    public MyTimelineModel(ICheepService service) : base(service)
    {
        _cheepService = service;
    }

    /// <summary>
    /// Fetch cheeps for a specified author
    /// </summary>
    /// <param name="author">The name of the author</param>
    /// <returns>PageResult</returns>
    public async Task<IActionResult> OnGetAsync(string author)
    {
        // Fetch cheeps for the specified author
        author = HttpContext.GetRouteValue("author")?.ToString();
        Cheeps = await _cheepService.GetOwnCheepsAsync(author);
        return Page();
    }
}
