﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
        service.GetCheepsAsync();
    }

    public async Task<ActionResult> OnGet()
    {
        Cheeps = await _service.GetCheepsAsync();
        return Page();
    }
}
