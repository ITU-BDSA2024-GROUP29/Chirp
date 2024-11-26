using System.Runtime.CompilerServices;
using Chirp.Core.DomainModel;
using Chirp.Repository;


public record CheepViewModel(string Author, string Message, string Timestamp)
{
    public static implicit operator CheepViewModel(Type v)
    {
        throw new NotImplementedException();
    }
}

public record AuthorViewModel(int AuthorId, string Name, string Email, ICollection<Cheep> Cheeps, ICollection<ApplicationUser> Follows) {
    public static implicit operator AuthorViewModel(Type v)
    {
        throw new NotImplementedException();
    }
}

public interface ICheepService
{
    ICheepRepository GetCheepRepository();
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
    private List<AuthorViewModel> _authors;

    public CheepService(ICheepRepository repository) 
    {
        this.repository = repository;
        _cheeps = new List<CheepViewModel>(); 
        _authors = new List<AuthorViewModel>();
        _ = LoadDB();
    }

    public ICheepRepository GetCheepRepository(){
        return repository;
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
        List<Cheep> loader_Cheeps = await repository.ReadCheeps();
        loader_Cheeps = loader_Cheeps.OrderBy(x => x.TimeStamp).ToList();
        _cheeps = loader_Cheeps.Select(cheep =>
            new CheepViewModel(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString())
        ).Reverse().ToList();
        List<Author> loader_Authors = await repository.GetAuthors();
        _authors = loader_Authors.Select(Author => 
            new AuthorViewModel(Author.AuthorId, Author.Name, Author.Email, Author.Cheeps,Author.Follows )).ToList();
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

    //get private timeline + cheeps from followed users
    public async Task<List<CheepViewModel>> GetCheepsFromAuthorAsync(string authorname)
    {
        ICheepRepository c = GetCheepRepository();
        await Task.CompletedTask;
        
        List<CheepViewModel> result = new List<CheepViewModel>();
        result.AddRange(_cheeps.Where(cheep => cheep.Author == authorname));
        
        List<Author> authors = new List<Author>();
        authors = await c.GetFollowedByAuthor(authorname);

        foreach (Author author in authors) {
            result.AddRange(_cheeps.Where(cheep => cheep.Author == author.Name));
        }
        return result;
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
        return _cheeps.Skip(startIndex).Take(pageSize).ToList();

    }

}

