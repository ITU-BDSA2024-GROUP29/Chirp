
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

    public async Task<Author> GetAuthorByName(String authorname){
       // return await  _dbContext.Authors.Where(a => a.Name )
        return await _dbContext.Authors.Where(a => a.Name.ToLower() == authorname.ToLower()).FirstOrDefaultAsync(); //sometimes give a null reference, not valid longterm
    }

    public void AddFollowed(Author user, Author loggedinUser) {
        loggedinUser.Follows.Add(user);
    }

    public async Task<int> GetTotalAuthorsCount()
    {
        return await _dbContext.Authors.CountAsync();
    }

    //should return authors followed by author from input (untested, TODO)
    public async Task<List<Author>> GetFollowedByAuthor(String authorname) {
        Author author = GetAuthorByName(authorname).Result;
        if (author == null) {   //remove this when GetAutherByName is completely fixed
            return new List<Author>();
        }
        return await _dbContext.Authors.Where(a => author.Follows.Contains(a)).ToListAsync();
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
