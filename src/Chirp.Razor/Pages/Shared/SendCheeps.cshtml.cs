

using Chirp.Core.DomainModel;
using Chirp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Chirp.Razor.Pages
{
    public class SendCheepsModel : PageModel
    {
        private readonly ICheepRepository _cheepRepositoryService;
        public List<CheepViewModel> Cheeps { get; set; }
        public SendCheepsModel(ICheepRepository cheepRepository)
            {
                _cheepRepositoryService = cheepRepository;
            }
        [HttpPost]
        public async Task<ActionResult> OnInputAsync(String message)
        {
            var loggedInUser = User.Identity?.Name ?? "Unknown user";
            String[] loggedInUserName = loggedInUser.Split('@');

            var AuthorLoggedIn = _cheepRepositoryService.GetAuthorByName(loggedInUser);
            if (AuthorLoggedIn == null)
            {
                Author author = new Author();
                author.AuthorId = await _cheepRepositoryService.GetTotalAuthorsCount() + 1;
                author.Email = loggedInUser;
                author.Name = loggedInUserName[0];
            }

            Cheep cheep = new Cheep();
            cheep.Author = AuthorLoggedIn;
            cheep.AuthorId = AuthorLoggedIn.Id;
            cheep.CheepId = await _cheepRepositoryService.GetTotalCheepCount() + 1;

            cheep.Text = "message";
            DateTime time = DateTime.Now;
            cheep.TimeStamp = time;



            _cheepRepositoryService.CreateCheepAsync(cheep);
            return Page();

        }


    }


}
