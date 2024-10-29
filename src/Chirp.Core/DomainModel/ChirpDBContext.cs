using Microsoft.EntityFrameworkCore;

namespace  Chirp.Core.DomainModel ;

public class ChirpDBContext : DbContext {
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) {
        Cheeps = Set<Cheep>();
        Authors = Set<Author>();
    }

}

