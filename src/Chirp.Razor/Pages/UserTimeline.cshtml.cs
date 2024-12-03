using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : SharedFuncs
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps;

    public UserTimelineModel(ICheepService service) : base(service) {
        _service = service;
    }

    public async Task <ActionResult> OnGet(string author) {
        Cheeps = await _service.GetOwnCheepsAsync(author);
        return Page();
    }
}
