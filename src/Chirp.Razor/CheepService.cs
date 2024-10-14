using System.Runtime.CompilerServices;
using Chirp.Razor.CheepRepository;
using Chirp.Razor.DomainModel;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{          
    private ICheepRepository repository;
    public CheepService(ICheepRepository repository) {
        this.repository = repository;
        _cheeps = loadDB();
    }

    private  List<CheepViewModel> loadDB() {
        List<Cheep> loader =  repository.ReadCheeps("").Result;
        List<CheepViewModel> result = new List<CheepViewModel>();
        foreach (Cheep cheep in loader) {
            result.Add(new CheepViewModel(cheep.Author.Name,cheep.Text,cheep.TimeStamp.ToString()));
        }
        
        return result;
    }
    
    // These would normally be loaded from a database for example
    private readonly List<CheepViewModel> _cheeps;
        
        
        
    public void newCheep(Cheep cheep) {
        
    }

    public List<CheepViewModel> GetCheeps() {
        return _cheeps;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
