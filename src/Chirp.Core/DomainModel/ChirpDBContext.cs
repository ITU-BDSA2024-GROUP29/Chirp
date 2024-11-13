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
//for the other contributors of this project:
//if you ever want to make a new migration use:
//dotnet ef migrations add InitialDBSchema --project Chirp.Core   --startup-project Chirp.Razor
//from the src folder, followed by:
//dotnet ef database update --project Chirp.Core   --startup-project Chirp.Razor               

