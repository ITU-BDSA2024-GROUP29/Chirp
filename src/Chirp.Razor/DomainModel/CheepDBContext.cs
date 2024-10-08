using Microsoft.EntityFrameworkCore;
namespace  Chirp.Razor.DomainModel ;

public class CheepDBContext : DbContext {
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<User> Users { get; set; }

    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options) {
        Cheeps = Set<Cheep>();
        Users = Set<User>();
        test();
    }
    
    
    public async void test() {
        
        // Define the query - with our setup, EF Core translates this to an SQLite query in the background
        var query = from Cheep in Cheeps
            where Cheep.User.Name == "Adrian"
            select new { Cheep.Text, Cheep.User };
        // Execute the query and store the results
        var result = await query.ToListAsync();
    }

}

