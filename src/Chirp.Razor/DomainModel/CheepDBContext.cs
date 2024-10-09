using Microsoft.EntityFrameworkCore;
namespace  Chirp.Razor.DomainModel ;

public class CheepDBContext : DbContext {
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options) {
        Cheeps = Set<Cheep>();
        Authors = Set<Author>();
    }

}

