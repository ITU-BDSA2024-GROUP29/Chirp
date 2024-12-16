
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chirp.Razor.Pages
{
    public class CheepsModel : SharedFuncs {
        private ICheepService _cheepService;

        public CheepsModel(ICheepService cheepService) : base(cheepService)
        {
            _cheepService = cheepService;
        }

        public Boolean CompareUserName(String username1, String username2) {
            if (username1.Equals(username2)) {
                return true;
            }
            return false;
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

            await _cheepService.GetCheepRepository().CreateCheepAsync(cheep);
            
            return Redirect("/");
        }

    }
}

