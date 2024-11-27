
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chirp.Razor.Pages
{
    public class CheepsModel : PageModel
    {private readonly ICheepService _cheepService;

        public CheepsModel(ICheepService cheepService)
        {
            _cheepService = cheepService;
        }

        public List<CheepViewModel> Cheeps { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNumber)
        {
            const int PageSize = 32; // Number of cheeps per page

            // Set the current page (ensure it's at least 1)
            CurrentPage = pageNumber < 1 ? 1 : pageNumber;

            var totalCheeps = await _cheepService.GetTotalCheepCount();

            TotalPages = (int)Math.Ceiling(totalCheeps / (double)PageSize);

            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }

            // Fetch the cheeps for the current page
            Cheeps = await _cheepService.GetPaginatedCheepsAsync(CurrentPage, PageSize);

            return Page();
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
            
            await _cheepService.GetCheepRepository().CreateCheepAsync(cheep);
            Console.WriteLine("Feather");
            return Page();
            
        }

    }
}

