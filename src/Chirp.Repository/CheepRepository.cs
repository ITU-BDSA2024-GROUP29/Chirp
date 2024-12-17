using Chirp.Core.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Repository;


public class CheepRepository : ICheepRepository {
    private readonly ChirpDBContext _dbContext;
    public CheepRepository(ChirpDBContext dbContext) {
        _dbContext = dbContext;
    }


    /// <summary>
    /// Adds Cheeps to the database
    /// </summary>
    /// <param name="newCheep"></param>
    /// <returns>Task for Completion</returns>
    public async Task CreateCheepAsync(Cheep newCheep) {
        var queryResult = await _dbContext.Cheeps.AddAsync(newCheep); // does not write to the database!
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
    }

    /// <summary>
    /// Gets all of the cheeps from the database
    /// </summary>
    /// <returns>all cheeps</returns>
    public async Task<List<Cheep>> ReadCheeps() {
        var query = _dbContext.Cheeps.Include(cheep => cheep.Author);
        
        // Execute the query
        var result = await query.ToListAsync();
        return result;
    }

    /// <summary>
    /// Gets all of the Authors.
    /// </summary>
    /// <returns>List of Authors</returns>
    public async Task<List<Author>> GetAuthors() {
        var authors = await _dbContext.Authors.ToListAsync();
        return authors;
    }
    
    /// <summary>
    /// Get paginated cheeps for a specific page and page size
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<List<Cheep>> GetPaginatedCheeps(int pageNumber, int pageSize = 32)
    {
        return await _dbContext.Cheeps
            .OrderBy(c => c.TimeStamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(c => c.Author) // Include the Author entity to avoid loading issues
            .ToListAsync();
    }

    /// <summary>
    /// Get the count total number of Cheeps in the Database
    /// </summary>
    /// <returns> int number of total cheeps </returns>
    public async Task<int> GetTotalCheepCount()
    {
        return await _dbContext.Cheeps.CountAsync();
    }

    /// <summary>
    /// Get the total number of cheeps sent from a specified author
    /// </summary>
    /// <param name="authorname"></param>
    /// <returns>int number of author cheeps total</returns>
    public async Task<List<Cheep>> GetTotalCheepsFromAuthorCount(String authorname)
    {//DbContext db = var entityTypes = db.Model.getEn
        return await _dbContext.Cheeps.Where(a => a.Author.Name == authorname).ToListAsync();
    }


    /// <summary>
    ///  Update an existing cheep
    /// </summary>
    /// <param name="alteredCheep"></param>
    /// <returns></returns>
    public async Task UpdateCheepAsync(Cheep alteredCheep) {
        _dbContext.Cheeps.Update(alteredCheep);
        await _dbContext.SaveChangesAsync();
    }
    async Task<List<Cheep>> ICheepRepository.GetPaginatedCheeps(int pageNumber, int pageSize)
    {
        return await GetPaginatedCheeps(pageNumber, pageSize);
    }
    
    /// <summary>
    /// Makes loggedinauthor follow authorname
    /// </summary>
    /// <param name="authorname"> To follow </param>
    /// <param name="loggedinauthorname">That Follows</param>
    /// <returns> Task when done</returns>
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

    /// <summary>
    /// Get the author associated by Name string
    /// </summary>
    /// <param name="authorname"></param>
    /// <returns>The author</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<Author> GetAuthorByName(String authorname){
    if (string.IsNullOrWhiteSpace(authorname)) {
        throw new ArgumentNullException(nameof(authorname));
    }
    // Use case-insensitive search and handle missing author gracefully
    return await _dbContext.Authors
        .Where(a => a.Name.ToLower() == authorname.ToLower())
        .FirstOrDefaultAsync(); // Return null if not found
    }

    /// <summary>
    /// Adds Author to the loggedinuser's author follower list
    /// </summary>
    /// <param name="user"></param>
    /// <param name="loggedinUser"></param>
    /// <returns>Task when done</returns>
    /// <exception cref="ArgumentNullException"></exception>
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
    
    /// <summary>
    /// Get Total count of Authors in the database
    /// </summary>
    /// <returns>int of total authors</returns>
    public async Task<int> GetTotalAuthorsCount()
    {
        return await _dbContext.Authors.CountAsync();
    }

    /// <summary>
    /// Get all following of author
    /// </summary>
    /// <param name="authorname"></param>
    /// <returns> a list of followed authors</returns>
    /// <exception cref="ArgumentNullException"></exception> <summary>
    public async Task<List<Author>> GetFollowedByAuthor(String authorname) {
    if (string.IsNullOrWhiteSpace(authorname))
        throw new ArgumentNullException(nameof(authorname), "Author name cannot be null or whitespace.");

    var author = await GetAuthorByName(authorname);

    if (author == null)
        return new List<Author>(); // Return an empty list if the author is not found

    // Initialize the Follows collection if it is null
    if (author.Follows == null)
        author.Follows = new List<Author>();

    // Return the authors followed by the given author
    return await _dbContext.Authors
        .Where(a => author.Follows.Contains(a)) // Check if the author follows the other authors
        .ToListAsync(); // Make the query asynchronous
}

    
    /// <summary>
    /// Delete Cheep with matching cheepID
    /// </summary>
    /// <param name="cheepID"> id of cheep to delete</param>
    /// <returns>returns true if deleted false if not found</returns>
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
