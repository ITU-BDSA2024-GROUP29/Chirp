using Chirp.Razor.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.CheepRepository;

public class CheepRepository : ICheepRepository {
    private readonly ChirpDBContext _dbContext;
    public CheepRepository(ChirpDBContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task CreateCheep(Cheep newCheep) {
        var queryResult = await _dbContext.Cheeps.AddAsync(newCheep); // does not write to the database!
        await _dbContext.SaveChangesAsync(); // persist the changes in the database
    }

    public async Task<List<Cheep>> ReadCheeps(String userName) {
        // Formulate the query - will be translated to SQL by EF Core
        var query = _dbContext.Cheeps.Select(message => message);
        // Execute the query
        var result = await query.ToListAsync();
        return result;
    }

    public async Task UpdateCheep(Cheep alteredcheep) {
        
    }
    
}