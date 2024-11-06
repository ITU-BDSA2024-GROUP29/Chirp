using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace  Chirp.Core.DomainModel;

public class ChirpDBContext : IdentityDbContext<ApplicationUser> {
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) {
        Cheeps = Set<Cheep>();
        Authors = Set<Author>();
    }

}

