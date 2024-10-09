using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.DomainModel;

public class ChatRepository 
{
    private readonly ChirpDBContext _dbContext;
    public ChatRepository(ChirpDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async void test() {
        
        // Define the query - with our setup, EF Core translates this to an SQLite query in the background
        var query = from Cheep in _dbContext.Cheeps
            where Cheep.User.Name == "Adrian"
            select new { Cheep.Text, Cheep.User };
        // Execute the query and store the results
        var result = await query.ToListAsync();
    }
    
}