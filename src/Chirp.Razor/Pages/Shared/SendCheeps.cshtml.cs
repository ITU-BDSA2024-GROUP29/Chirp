

using Chirp.Core.DomainModel;
using Chirp.Repository;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Chirp.Razor.Pages{
    public class SendCheepsModel : PageModel
    {
        private readonly ICheepService _service;
        public List<CheepViewModel> Cheeps { get; set; }
        public SendCheepsModel(ICheepService service)
        {
            _service = service;
        }
        public async Task<IActionResult> OnPostAsync(String Post){
            
            
            var loggedInUser = User.Identity?.Name ?? "Unknown user";
            String[] loggedInUserName = loggedInUser.Split('@');

         //   var AuthorLoggedIn = _service.GetAuthorByEmail(loggedInUser);
           // _service.GetCheepRepository.GetAuthorByEmail(loggedInUser);
/*          
            if (AuthorLoggedIn == null){
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

*/  
            Author author = new Author();
                author.AuthorId = 55;
                author.Email = "viktoremilandersen@gmail.com";
                author.Name = "Viktor";
            Cheep cheep = new Cheep();
            cheep.Author = author;
            cheep.AuthorId = author.AuthorId;
            cheep.CheepId = 999;
            
            cheep.Text = "plz virk";
            DateTime time = DateTime.Now;
            cheep.TimeStamp = time;

            Console.WriteLine("Espresso");
            
            await _service.GetCheepRepository().CreateCheepAsync(cheep);
            Console.WriteLine("Feather");
            return Page();
            
        }


    }


}
