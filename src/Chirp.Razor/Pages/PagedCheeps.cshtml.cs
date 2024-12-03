
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chirp.Razor.Pages
{
    public class CheepsModel : PageModel
    {private ICheepService _cheepService;

        public CheepsModel(ICheepService cheepService)
        {
            _cheepService = cheepService;
        }

        public Boolean CompareUserName(String username1, String username2) {
            if (username1.Equals(username2)) {
                return true;
            }
            return false;
        }

        public async Task<Boolean> IsFollowing(String authorname) {
            if (await _cheepService.IsUserFollowing(User.Identity.Name,authorname))
            {
                return true;
            }
            return false;
        }
        
        public async Task<IActionResult> OnPostFollowAsync(string followedId) {
            await Task.CompletedTask;
            if (string.IsNullOrEmpty(followedId))
            {
                return BadRequest("Followed ID is required.");
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

    }
}

