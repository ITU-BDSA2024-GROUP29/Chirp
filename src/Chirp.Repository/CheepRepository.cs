
using System.Runtime.CompilerServices;
using Chirp.Core.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace Chirp.Repository;


public class CheepRepository : ICheepRepository {
    private readonly ChirpDBContext _dbContext;
    public CheepRepository(ChirpDBContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task CreateCheepAsync(Cheep newCheep) {
        var queryResult = await _dbContext.Cheeps.AddAsync(newCheep); // does not write to the database!
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
    }

    public async Task<List<Cheep>> ReadCheeps() {
        // Formulate the query - will be translated to SQL by EF Core
        //.Include(blog => blog.Posts)
        var query = _dbContext.Cheeps.Include(cheep => cheep.Author);
        
        // Execute the query
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<Author>> GetAuthors() {
        var authors = await _dbContext.Authors.ToListAsync();
        return authors;
    }
    
    // Get paginated cheeps for a specific page and page size
    public async Task<List<Cheep>> GetPaginatedCheeps(int pageNumber, int pageSize = 32)
    {
        return await _dbContext.Cheeps
            .OrderBy(c => c.TimeStamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(c => c.Author) // Include the Author entity to avoid loading issues
            .ToListAsync();
    }

    // Get the total number of cheeps in the database
    public async Task<int> GetTotalCheepCount()
    {
        return await _dbContext.Cheeps.CountAsync();
    }

    // Get the total number of cheeps sent from a specified author
    public async Task<List<Cheep>> GetTotalCheepsFromAuthorCount(String authorname)
    {//DbContext db = var entityTypes = db.Model.getEn
        return await _dbContext.Cheeps.Where(a => a.Author.Name == authorname).ToListAsync();
    }


    // Update an existing cheep
    public async Task UpdateCheepAsync(Cheep alteredCheep) {
        _dbContext.Cheeps.Update(alteredCheep);
        await _dbContext.SaveChangesAsync();
    }

    // Interface method: returning paginated cheeps
    async Task<List<Cheep>> ICheepRepository.GetPaginatedCheeps(int pageNumber, int pageSize)
    {
        return await GetPaginatedCheeps(pageNumber, pageSize);
    }
    
    public async Task FollowAuthor(string authorname, string loggedinauthorname) {
        if (string.IsNullOrWhiteSpace(authorname))
            throw new ArgumentNullException(nameof(authorname), "Author name cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(loggedinauthorname))
            throw new ArgumentNullException(nameof(loggedinauthorname), "Logged-in author name cannot be null or whitespace.");
        
        var a = await GetAuthorByName(authorname);
        if (a == null)
            throw new InvalidOperationException($"Author with name '{authorname}' was not found.");

        var b = await GetAuthorByName(loggedinauthorname);
        if (b == null)
            throw new InvalidOperationException($"Logged-in author with name '{loggedinauthorname}' was not found.");

        await AddFollowed(a, b);
    }

    public async Task<Author> GetAuthorByName(String authorname){
        /*
       // return await  _dbContext.Authors.Where(a => a.Name )
       if (String.IsNullOrWhiteSpace(authorname)) {
           throw new ArgumentNullException(nameof(authorname));
       } 
       return await _dbContext.Authors.Where(a => a.Name.ToLower() == authorname.ToLower()).FirstOrDefaultAsync(); //sometimes give a null reference, not valid longterm
    */
        var list = await GetAuthors();
        foreach (var name in list) {
            if (name.Name == authorname) return name;
        }
        return null;
    }

    public async Task AddFollowed(Author user, Author loggedinUser) {
        await Task.CompletedTask;
        if (loggedinUser == null) 
            throw new ArgumentNullException(nameof(loggedinUser), "The logged-in user cannot be null." + loggedinUser);
        if (user == null) 
            throw new ArgumentNullException(nameof(user), "The user to be followed cannot be null.");

        if (loggedinUser.Follows == null) loggedinUser.Follows = new List<Author>();
        if (loggedinUser.Follows.Contains(user)) {
            loggedinUser.Follows.Remove(user);
        }
        else {
            loggedinUser.Follows.Add(user);
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> GetTotalAuthorsCount()
    {
        return await _dbContext.Authors.CountAsync();
    }

    //should return authors followed by author from input (untested, TODO)
    public async Task<List<Author>> GetFollowedByAuthor(String authorname) {
        await Task.CompletedTask;
        Author author = GetAuthorByName(authorname).Result;
        return _dbContext.Authors.Where(a => author.Follows.Contains(a)).ToList();
    }
    

    public async Task<bool> DeleteCheepByID(int cheepID)
{
    var cheep = await _dbContext.Cheeps.Where(c => c.CheepId == cheepID).FirstAsync();
    if (cheep == null) 
    {
        return false; // Cheep not found
    }

    _dbContext.Cheeps.Remove(cheep);
    await _dbContext.SaveChangesAsync();
    return true;
}





}
