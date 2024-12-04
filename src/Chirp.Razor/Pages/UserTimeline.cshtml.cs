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

    public async Task<IActionResult> OnGetAsync(string author)
    {
        // Fetch cheeps for the specified author
        author = HttpContext.GetRouteValue("author")?.ToString();
        Cheeps = await _cheepService.GetCheepsFromAuthorAsync(author);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(String Post){
            bool newAuthor = false;
            
            var loggedInUserName = User.Identity.Name;
            
            
            var AuthorLoggedIn = await _cheepService.GetCheepRepository().GetAuthorByName(loggedInUserName);
            Console.WriteLine("loggedInUserName: " + loggedInUserName);
            Console.WriteLine("Authorloggedin: " + AuthorLoggedIn);
            Cheep cheep = new Cheep();
            if (AuthorLoggedIn == default || AuthorLoggedIn == null){
                newAuthor = true;
                Author author = new Author();
                author.AuthorId = await _cheepService.GetCheepRepository().GetTotalAuthorsCount() + 1;
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

/*

            Author author = new Author();
                author.AuthorId = 55;
                author.Email = "viktoremilandersen@gmail.com";
                author.Name = "Viktor";
            Cheep cheep = new Cheep();
            cheep.Author = author;
            cheep.AuthorId = author.AuthorId;
            cheep.CheepId = 998;
            
            cheep.Text = "plz virk222";
            DateTime time = DateTime.Now;
            cheep.TimeStamp = time;

            
            */
            await _cheepService.GetCheepRepository().CreateCheepAsync(cheep);
            
            return Redirect("/"+ loggedInUserName);
        }

}
