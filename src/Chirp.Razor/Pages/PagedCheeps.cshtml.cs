
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chirp.Razor.Pages
{
    public class CheepsModel : SharedFuncs
    {
        private ICheepService _cheepService;

        public CheepsModel(ICheepService cheepService) : base(cheepService)
        {
            _cheepService = cheepService;
        }

        public List<CheepViewModel> Cheeps { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        
        /// <summary>
        /// Fetch the cheeps for a page
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns>PageResult</returns>
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

