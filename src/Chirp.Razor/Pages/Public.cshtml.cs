using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : SharedFuncs
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service) : base(service)
    {
        _service = service;
        service.GetCheepsAsync();
    }

    /// <summary>
    /// Handles GET requests to the Public page.
    /// </summary>
    /// <returns></returns>
    public IActionResult OnGet()
    {
        return Redirect("/cheeps/page/1");
    }
}
