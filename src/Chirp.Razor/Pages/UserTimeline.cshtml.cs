using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _cheepService;
    public List<CheepViewModel> Cheeps { get; set; } = new();

    public UserTimelineModel(ICheepService service)
    {
        _cheepService = service;
    }

    public async Task<IActionResult> OnGetAsync(string author)
    {
        // Fetch cheeps for the specified author
        Cheeps = await _cheepService.GetCheepsFromAuthorAsync(author);
        return Page();
    }

    // Handler for creating a new cheep
    public async Task<IActionResult> OnPost(String Post){
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
    public async Task<IActionResult> OnPostDeleteCheepAsync(int cheepID)
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
}
