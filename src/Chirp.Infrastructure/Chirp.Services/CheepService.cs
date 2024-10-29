using System.Runtime.CompilerServices;
using Chirp.Razor.CheepRepository;
using Chirp.Razor.DomainModel;
using Chirp.Razor.Pages;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    Task<List<CheepViewModel>> GetCheepsAsync();
    Task<List<CheepViewModel>> GetCheepsFromAuthorAsync(string author);
    Task<List<CheepViewModel>> GetPaginatedCheepsAsync(int pageNumber, int pageSize = 10);
    Task<int> GetTotalCheepCount();
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository repository;
    private static CheepService _instance;
    private List<CheepViewModel> _cheeps;

    public CheepService(ICheepRepository repository) 
    {
        this.repository = repository;
        _cheeps = new List<CheepViewModel>(); 
        _ = LoadDB();
    }

    public static CheepService GetInstance(ICheepRepository repository)
    {
        if (_instance == null)
        {
            _instance = new CheepService(repository);
        }
        return _instance;
    }

    private async Task LoadDB()
    {
        List<Cheep> loader = await repository.ReadCheeps();
        loader = loader.OrderBy(x => x.TimeStamp).ToList();
        _cheeps = loader.Select(cheep =>
            new CheepViewModel(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString())
        ).ToList();
    }

    public Task<int> GetTotalCheepCount()
    {
        return Task.FromResult(_cheeps.Count);
    }

    public async Task<List<CheepViewModel>> GetCheepsAsync() 
    {
        await Task.CompletedTask;
        return _cheeps;
    }

    public async Task<List<CheepViewModel>> GetCheepsFromAuthorAsync(string author)
    {
        await Task.CompletedTask;
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    public async Task<List<CheepViewModel>> GetPaginatedCheepsAsync(int pageNumber, int pageSize = 20)
    {
        await Task.CompletedTask;
        return GetPaginatedCheeps(pageNumber, pageSize);
    }

    public List<CheepViewModel> GetPaginatedCheeps(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1; // Default to page 1 if the input is less than 1
        }

        // Calculate the start index
        int startIndex = (pageNumber - 1) * pageSize;

        if (startIndex >= _cheeps.Count)
        {
            return new List<CheepViewModel>();
        }

        
        // Use LINQ to skip and reverse take for pagination
        return _cheeps.Skip(startIndex).Take(pageSize).Reverse().ToList();

    }
}

