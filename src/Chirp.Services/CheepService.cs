using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using Chirp.Core.DomainModel;
using Chirp.Repository;


public record CheepViewModel(string Author, string Message, string Timestamp, int CheepId)
{
    // Render Markdown to basic HTML manually
    public string RenderedMessage => ParseMarkdown(Message);

    private static string ParseMarkdown(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown)) {return string.Empty;}

        // Replace bold (**text**) with <strong>text</strong>
        markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"\*\*(.+?)\*\*", "<strong>$1</strong>");

        // Replace italic (*text*) with <em>text</em>
        markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"\*(.+?)\*", "<em>$1</em>");

        // Replace [link](url) with <a href="url">link</a>
        markdown = System.Text.RegularExpressions.Regex.Replace(
            markdown,
            @"\[(.+?)\]\((https?://.+?)\)",
            "<a href=\"$2\">$1</a>"
        );

        return markdown;
    }
}


public record AuthorViewModel(int AuthorId, string Name, string Email, ICollection<Cheep> Cheeps, ICollection<Author> Follows) {
    public static implicit operator AuthorViewModel(Type v)
    {
        throw new NotImplementedException();
    }
}

public interface ICheepService
{
    /// <summary>
    /// Gets the instace of the cheep database
    /// </summary>
    /// <returns>The database connection </returns>
    ICheepRepository GetCheepRepository();
    /// <summary>
    /// Gets all Cheeps
    /// </summary>
    /// <returns>a list of cheeps </returns>
    Task<List<CheepViewModel>> GetCheepsAsync();
    /// <summary>
    /// Get cheeps from specific Author
    /// </summary>
    /// <param name="author"> to get cheeps from</param>
    /// <returns> A list of Cheeps </returns>
    Task<List<CheepViewModel>> GetCheepsFromAuthorAsync(string author);
    /// <summary>
    /// Get Cheeps in a Paginated form
    /// </summary>
    /// <param name="pageNumber">specific page</param>
    /// <param name="pageSize">amount of cheeps per page</param>
    /// <returns>a list of cheeps</returns>
    Task<List<CheepViewModel>> GetPaginatedCheepsAsync(int pageNumber, int pageSize = 10);
    /// <summary>
    /// Gets the total count of cheeps
    /// </summary>
    /// <returns>int of total cheeps amount</returns>
    Task<int> GetTotalCheepCount();
    /// <summary>
    /// Get cheeps from specific Author
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    Task<List<CheepViewModel>> GetOwnCheepsAsync(string author);
    /// <summary>
    /// Checks if author follows other author
    /// </summary>
    /// <param name="author"> author to check on</param>
    /// <param name="author2">author to chech for</param>
    /// <returns>follow true,</returns>
    Task<bool> IsUserFollowing(string author, string author2);
    /// <summary>
    /// Get Author by Name of type string
    /// </summary>
    /// <param name="authorname"></param>
    /// <returns>Author with name or null in none</returns>
    Author GetAuthorByName(String authorname);
    /// <summary>
    /// ADD Author of authorname to author of LoggedinAuthorname's followers list.
    /// </summary>
    /// <param name="authorname"></param>
    /// <param name="Loggedinauthorname"></param>
    /// <returns></returns>
    Task FollowAuthor(String authorname, String Loggedinauthorname);
    /// <summary>
    /// Delete cheep with id
    /// </summary>
    /// <param name="cheepID"></param>
    /// <returns>true if complete false if no cheep with id</returns>
    Task<bool> DeleteCheepByID(int cheepID);
    /// <summary>
    /// Gets count of Author
    /// </summary>
    /// <returns></returns> <summary>
    /// 
    /// </summary>
    /// <returns>int of amount</returns>
    Task<int>  GetAuthorCount();
    /// <summary>
    /// Gets count of specific author's cheeps
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    int getAuthorCheepCount(string author);
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
        List<Cheep> loader = await repository.ReadCheeps();
        loader = loader.OrderBy(x => x.TimeStamp).ToList();
        _cheeps = loader.Select(cheep =>
            new CheepViewModel(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString(), cheep.CheepId)
        ).Reverse().ToList();
        List<Author> loader_Authors = await repository.GetAuthors();
        _authors = loader_Authors.Select(Author => 
            new AuthorViewModel(Author.AuthorId, Author.Name, Author.Email, Author.Cheeps, Author.Follows )).ToList();
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
    public async Task<List<CheepViewModel>> GetCheepsFromAuthorAsync(string author)
    {
        await Task.CompletedTask;
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    public async Task<int> GetAuthorCount(){
        return _authors.Count();
    }

    public async Task<List<CheepViewModel>> GetOwnCheepsAsync(string authorname) {
        ICheepRepository c = GetCheepRepository();
        await Task.CompletedTask;
        
        List<CheepViewModel> result = new List<CheepViewModel>();
        
        result.AddRange(await GetCheepsFromAuthorAsync(authorname));
        var authors = await c.GetFollowedByAuthor(authorname);

        foreach (var author in authors) {
            result.AddRange(await GetCheepsFromAuthorAsync(author.Name));
        }   

        
        
        return result;
    }
    
    //Checks if an author follows another author.
    //ordering is: does author1 follow author2?
    //returns boolean Task
    public async Task<bool> IsUserFollowing(string author1, string author2) {
        ICheepRepository c = GetCheepRepository();
        await Task.CompletedTask;
        Author a = await c.GetAuthorByName(author2);
    
        return c.GetFollowedByAuthor(author1).Result.Contains(a);
    }
    
    public async Task<List<CheepViewModel>> GetPaginatedCheepsAsync(int pageNumber, int pageSize = 20)
    {
        await Task.CompletedTask;
        return GetPaginatedCheeps(pageNumber, pageSize);
    }

    //Allow user to follow another used. 'Loggedinauthorname' is the user that follow 'authorname'
    public async Task FollowAuthor(string authorname, string loggedinauthorname) {
        ICheepRepository c = GetCheepRepository();
        await c.FollowAuthor(authorname, loggedinauthorname);
    }

    public Author GetAuthorByName(String authorname) {
        ICheepRepository c = GetCheepRepository();
        return c.GetAuthorByName(authorname).Result;
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

    public async Task<bool> DeleteCheepByID(int cheepID)
    {
        return await repository.DeleteCheepByID(cheepID);
    }

    public int getAuthorCheepCount(String author)
    {
        return _cheeps.Where(cheep => cheep.Author.ToLower() == author.ToLower()).Count();
    }

}

