using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class SharedFuncs : PageModel {
    public SharedFuncs(ICheepService cheepService) {
        _cheepService = cheepService;
    }
    
    
    protected readonly ICheepService _cheepService;
    
}