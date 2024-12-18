using Microsoft.AspNetCore.Mvc;
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class SharedFuncs : PageModel
{
    public SharedFuncs(ICheepService cheepService)
    {
        _cheepService = cheepService;
    }


    protected readonly ICheepService _cheepService;

    /// <summary>
    /// This function is called from the sendcheeps.cshtml which is the share cheeps box. 
    /// It recieves the message written in the box when a user presses share. 
    /// The function then creates a cheeps with message being from the post.
    /// The function gets author name from the logging information
    /// It then checks if the author exists in the database, if they dont, a new author is created
    /// and the cheep is created using the author, the message and a genereted timestamp.
    /// </summary>
    /// <param name="Post">The cheep message</param>
    /// <returns>A RedirectResult for /[loggedInUserName]</returns>
    public async Task<IActionResult> OnPostCheepAsync(String Post)
    {

        if (Post.Length > 160)
        {
            return BadRequest("Cheep Length too long > 160");
        }


        var loggedInUserName = User.Identity.Name;

        var AuthorLoggedIn = await _cheepService.GetCheepRepository().GetAuthorByName(loggedInUserName);
        Console.WriteLine("loggedInUserName: " + loggedInUserName);
        Console.WriteLine("Authorloggedin: " + AuthorLoggedIn);
        Cheep cheep = new Cheep();
        if (AuthorLoggedIn == default || AuthorLoggedIn == null)
        {
            Author author = new Author();
            author.AuthorId = await _cheepService.GetAuthorCount() + 1;
            author.Email = loggedInUserName + "@chirp.dk";
            author.Name = loggedInUserName;
            cheep.Author = author;
            cheep.AuthorId = author.AuthorId;
            cheep.CheepId = await _cheepService.GetTotalCheepCount() + 1;
        }
        else
        {

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

    /// <summary>
    /// Handler for deleting a cheep
    /// </summary>
    /// <param name="cheepID">Id for the cheep</param>
    /// <returns>
    /// RedirectToPageResult to the same page unless a error happens then it gets redirect /Error
    /// </returns>
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
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while attempting to delete the cheep.";
            return RedirectToPage("/Error");
        }
    }


    public async Task<Boolean> IsFollowing(String authorname)
    {
        if (await _cheepService.IsUserFollowing(User.Identity.Name, authorname))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Follow button functionality
    /// </summary>
    /// <param name="followedId">Id of followed author</param>
    /// <returns>
    /// RedirectResult to / unless a followedId is empty or equals the user, then a BadRequestObjectResult gets returned.
    /// If the user is not authenticated then a UnauthorizedResult gets returned
    /// </returns>
    public async Task<IActionResult> OnPostFollowAsync(string followedId)
    {
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

        await _cheepService.FollowAuthor(followedId, User.Identity.Name);
        return Redirect("/"); // Refresh the current page
    }
    /// <summary>
    /// unfollow functionality
    /// </summary>
    /// <param name="followedId">Id of followed author</param>
    /// <returns>
    /// RedirectResult to / unless a followedId is empty, then a BadRequestObjectResult gets returned.
    /// If the user is not authenticated then a UnauthorizedResult gets returned.
    /// </returns>
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
        await _cheepService.FollowAuthor(followedId, User.Identity.Name);
        return RedirectToPage(); // Refresh the current page

    }

}