using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : SharedFuncs
{
    private readonly ICheepService _cheepService;
    public List<CheepViewModel> Cheeps { get; set; } = new();

    public UserTimelineModel(ICheepService service) : base(service)
    {
        _cheepService = service;
    }

    public async Task<IActionResult> OnGetAsync(string author)
    {
        // Fetch cheeps for the specified author
        author = HttpContext.GetRouteValue("author")?.ToString();
        Cheeps = await _cheepService.GetCheepsFromAuthorAsync(author);
        return Page();
    }
}
