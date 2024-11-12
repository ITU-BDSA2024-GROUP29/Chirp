

using Chirp.Core.DomainModel;
using Chirp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public ActionResult OnInput(String message)
        {

            Author author = new Author();
            author.AuthorId = 9999;
            author.Email = "test@test.com";
            author.Name = "Test";
            Cheep cheep = new Cheep();
            cheep.Author = author;
            cheep.AuthorId = author.AuthorId;
            cheep.CheepId = 9999;
            cheep.Text = "message";
            DateTime time = DateTime.Now;
            cheep.TimeStamp = time;

            _cheepRepositoryService.CreateCheepAsync(cheep);
            return Page();

        }


    }


}
