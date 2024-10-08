using Microsoft.EntityFrameworkCore;
namespace  Chirp.Razor.DomainModel ;

public class CheepDBContext : DbContext {
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<User> Users { get; set; }

    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options) {
        Cheeps = Set<Cheep>();
        Users = Set<User>();
    }

}

