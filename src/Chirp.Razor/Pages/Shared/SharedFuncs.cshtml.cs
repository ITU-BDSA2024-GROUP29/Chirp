using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class SharedFuncs : PageModel {
    public SharedFuncs(ICheepService cheepService) {
        _cheepService = cheepService;
    }
    protected readonly ICheepService _cheepService;
    
    public async Task<Boolean> IsFollowing(String authorname) {
        if (await _cheepService.IsUserFollowing(User.Identity.Name,authorname))
        {
            return true;
        }
        return false;
    }
    
    //follow buttons functionality
    public async Task<IActionResult> OnPostFollowAsync(string followedId) {
        await Task.CompletedTask;
        if (string.IsNullOrEmpty(followedId))
        {
            return BadRequest("Followed ID cannot be empty.");
        }

        // Ensure the user is authenticated
        if (User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }


        Console.WriteLine("Following ID is " + followedId);
        _cheepService.FollowAuthor(followedId, User.Identity.Name);
        return Page(); // Refresh the current page
    }
    //unfollow functionality
    public async Task<IActionResult> OnPostUnfollowAsync(string followedId)
    {

        if (string.IsNullOrEmpty(followedId))
        {
            return BadRequest("Followed ID is required.");
        }

        // Ensure the user is authenticated
        if (User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }
            
        //change to unfollow
        // _cheepService.FollowAuthor(followedId, User.Identity.Name);
        return RedirectToPage(); // Refresh the current page
            
    }
    
}