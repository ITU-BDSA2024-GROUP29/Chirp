using Chirp.Razor.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.CheepRepository;

public class CheepRepository : ICheepRepository {
    private readonly ChirpDBContext _dbContext;
    public CheepRepository(ChirpDBContext dbContext) {
        DbInitializer.SeedDatabase(dbContext);
        Console.WriteLine("HER!");
        _dbContext = dbContext;
    }

    public async Task CreateCheepAsync(Cheep newCheep) {
        var queryResult = await _dbContext.Cheeps.AddAsync(newCheep); // does not write to the database!
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
    }

    public async Task<List<Cheep>> ReadCheeps(String userName) {
        // Formulate the query - will be translated to SQL by EF Core
        //.Include(blog => blog.Posts)
        var query = _dbContext.Cheeps.Include(cheep => cheep.Author);
        
        // Execute the query
        var result = await query.ToListAsync();
        return result;
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
}