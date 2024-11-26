
using System.Runtime.CompilerServices;
using Chirp.Core.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

    public async Task<Author> GetAuthorByEmail(String Email){
        return (Author)_dbContext.Authors.Where(a => a.Email.ToLower() == Email.ToLower());
    }

    public async Task<int> GetTotalAuthorsCount()
    {
        return await _dbContext.Authors.CountAsync();
    }

    //should return authors followed by author from input (untested, TODO)
    public async Task<List<Author>> GetFollowedByAuthor(String Email) {
        Author author = GetAuthorByEmail(Email);
        return await _dbContext.Authors.Where(a => author.Follows.Contains(a)).ToListAsync();
    }



}
