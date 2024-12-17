using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core.DomainModel;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : SharedFuncs
{
    private readonly ICheepService _cheepService;
    public List<CheepViewModel> Cheeps { get; set; } = new();

    public UserTimelineModel(ICheepService service) : base(service)
    {
        _cheepService = service;
        _cheepService = service;
    }

    /// <summary>
    /// Fetch cheeps for the specified author
    /// </summary>
    /// <param name="author">Name of the author</param>
    /// <returns>Returns all cheeps from a author</returns>
    public async Task<IActionResult> OnGetAsync(string author)
    {
        // Fetch cheeps for the specified author
        author = HttpContext.GetRouteValue("author")?.ToString();
        Cheeps = await _cheepService.GetCheepsFromAuthorAsync(author);
        return Page();
    }
}
