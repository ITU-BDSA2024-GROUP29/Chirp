using System.Security.Claims;
using System.Text;
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;

namespace Chirp.Razor.Pages;

public class AboutMeModel : SharedFuncs
{
    private readonly ICheepService _cheepService;
    private DateTime time = DateTime.Now;
    public List<CheepViewModel> Cheeps { get; set; } = new();
    public string Name { get; set; }
    public string Email { get; set; }
    public int TotalMessages { get; set; }
    public ICollection<Author> Follows{ get; set; }

    public AboutMeModel(ICheepService service) : base(service)
    {
        _cheepService = service;
    }

    public async Task<IActionResult> OnGetAsync(string author)
    {

        if (author != HttpContext.GetRouteValue("author")?.ToString())
        {
            Redirect("/");
        }

        Author authorObj = _cheepService.GetAuthorByName(author);
        TotalMessages = _cheepService.getAuthorCheepCount(author);
        Cheeps = await _cheepService.GetCheepsFromAuthorAsync(author);
        Email = authorObj.Email;
        Name = authorObj.Name;

        Follows = authorObj.Follows ?? new List<Author>();


        return Page();
    }

    public List<CheepViewModel> GeCheeps(String author){
        return _cheepService.GetCheepsFromAuthorAsync(author).Result;
    }

    public async Task<IActionResult> OnPostDownloadUserInfoAsync(String authorName){

        if (authorName != HttpContext.GetRouteValue("author")?.ToString())
        {
            Redirect("/");
        }

        Author authorObj = _cheepService.GetAuthorByName(authorName);
        TotalMessages = _cheepService.getAuthorCheepCount(authorName);
        Cheeps = await _cheepService.GetCheepsFromAuthorAsync(authorName);
        Email = authorObj.Email ?? "No Email is Associated with the Account";
        Name = authorObj.Name;
        Follows = authorObj.Follows ?? new List<Author>();

        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("----------------------------------------------------");
        stringBuilder.AppendLine("This Infomation is the infomation about the User: " + Name + "  Collected on the Cirp Application");
        stringBuilder.AppendLine("The Information is collected and Downloaded at " + time.ToLocalTime());
        stringBuilder.AppendLine("----------------------------------------------------");

        
        stringBuilder.AppendLine("Name: " + Name);
        
        stringBuilder.AppendLine("Email: " + Email);
        
        stringBuilder.AppendLine("----------------------------------------------------");
        stringBuilder.AppendLine("------- User Messages on the Chrip Application -----");
        stringBuilder.AppendLine("----------------------------------------------------");

        stringBuilder.AppendLine( "Amount of messages byt user: "+ Name + " is " + _cheepService.getAuthorCheepCount(Name));

        var list = await _cheepService.GetCheepsFromAuthorAsync(HttpContext.GetRouteValue("author")?.ToString());

        foreach (CheepViewModel cheep in list)
        {
            stringBuilder.AppendLine(cheep.Timestamp + "  -   " +  cheep.Message);
        }

        stringBuilder.AppendLine("----------------------------------------------------");
        stringBuilder.AppendLine("------- User Follows on the Chrip Application ------");
        stringBuilder.AppendLine("----------------------------------------------------");

        if(Follows != null){
            if (Follows?.Any() == true)
            {
                foreach (var user in Follows)
                {
                    stringBuilder.AppendLine("-" + user.Name);
                }
            }
            else
            {
                stringBuilder.AppendLine(Name + " Does not currently Follow any other Authors");
            }

        
        }
        // Convert content to bytes and return file
        byte[] fileBytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
        string fileName = $"{Name}_Chirp_UserData.txt";
        return File(fileBytes, "text/plain", fileName);
    }
}
