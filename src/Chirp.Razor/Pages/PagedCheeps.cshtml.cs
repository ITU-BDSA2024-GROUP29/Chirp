
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
        
        [HttpPost]
        public async Task<IActionResult> Follow(string followedId)
        {
            //var followerId = int.Parse(_userManager.GetUserId(User));
            //var followedUser = _cheepService.GetAuthorByName(followedId);

            /*
            if (followedUser != null && !_context.Follows.Any(f => f.FollowerId == followerId && f.FollowedId == followedUser.Id))
            {
                var follow = new Follow
                {
                    FollowerId = followerId,
                    FollowedId = followedUser.Id,
                    FollowDate = DateTime.UtcNow
                };

                _context.Follows.Add(follow);
                await _context.SaveChangesAsync();
            }*/
            _cheepService.FollowAuthor(followedId, User.Identity.Name);
            

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(string followedId)
        {
            var followerId = int.Parse(_userManager.GetUserId(User));
            var followedUser = await _userManager.FindByNameAsync(followedId);

            if (followedUser != null)
            {
                var follow = _context.Follows.FirstOrDefault(f => f.FollowerId == followerId && f.FollowedId == followedUser.Id);

                if (follow != null)
                {
                    _context.Follows.Remove(follow);
                    await _context.SaveChangesAsync();
                }
            }

            return Redirect(Request.Headers["Referer"].ToString());
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

