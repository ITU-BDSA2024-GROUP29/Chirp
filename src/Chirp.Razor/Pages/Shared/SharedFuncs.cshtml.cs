using Microsoft.AspNetCore.Mvc;
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class SharedFuncs : PageModel {
    public SharedFuncs(ICheepService cheepService) {
        _cheepService = cheepService;
    }
    
    
    protected readonly ICheepService _cheepService;
    
    public async Task<IActionResult> OnPostCheepAsync(String Post){
            bool newAuthor = false;
            
            var loggedInUserName = User.Identity.Name;
            
            
            var AuthorLoggedIn = await _cheepService.GetCheepRepository().GetAuthorByName(loggedInUserName);
            Console.WriteLine("loggedInUserName: " + loggedInUserName);
            Console.WriteLine("Authorloggedin: " + AuthorLoggedIn);
            Cheep cheep = new Cheep();
            if (AuthorLoggedIn == default || AuthorLoggedIn == null){
                newAuthor = true;
                Author author = new Author();
                author.AuthorId = await _cheepService.GetAuthorCount() + 1;
                author.Email = loggedInUserName + "@chirp.dk";
                author.Name = loggedInUserName;
                cheep.Author = author;
                cheep.AuthorId = author.AuthorId;
                cheep.CheepId = await _cheepService.GetTotalCheepCount() + 1;
            }
            else{
                
                cheep.Author = AuthorLoggedIn;
                cheep.AuthorId = AuthorLoggedIn.AuthorId;
                cheep.CheepId = await _cheepService.GetTotalCheepCount() + 1;
            }

            cheep.Text = Post;
            DateTime time = DateTime.Now;
            cheep.TimeStamp = time;

            await _cheepService.GetCheepRepository().CreateCheepAsync(cheep);
            
            return Redirect("/");
        }

    // Handler for deleting a cheep
    public async Task<IActionResult> OnPostDelete(int cheepID)
    {
        try
        {
            var isDeleted = await _cheepService.DeleteCheepByID(cheepID);

            if (!isDeleted)
            {
                TempData["ErrorMessage"] = "Cheep not found or could not be deleted.";
            }
            else
            {
                TempData["SuccessMessage"] = "Cheep successfully deleted.";
            }

            return RedirectToPage();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while attempting to delete the cheep.";
            return RedirectToPage("/Error");
        }
    }
    public async Task<Boolean> IsFollowing(String authorname) {
        if (await _cheepService.IsUserFollowing(User.Identity.Name,authorname))
        {
            return true;
        }
        return false;
    }
    
    //follow button functionality
    public async Task<IActionResult> OnPostFollowAsync(string followedId) {
        await Task.CompletedTask;
        
        if (string.IsNullOrEmpty(followedId) || followedId.Equals(User.Identity.Name))
        {
            return BadRequest("Followed ID cannot be empty or your own");
        }

        // Ensure the user is authenticated
        if (User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }


        Console.WriteLine("Following ID is " + followedId);
        _cheepService.FollowAuthor(followedId, User.Identity.Name);
        return Redirect("/"); // Refresh the current page
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