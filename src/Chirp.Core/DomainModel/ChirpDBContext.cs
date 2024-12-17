using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace  Chirp.Core.DomainModel;

public class ChirpDBContext : IdentityDbContext<ApplicationUser> {
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { 
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cheep>()
            .Property(m => m.Text).HasMaxLength(160);

        modelBuilder.Entity<Author>()
            .HasIndex(c => c.Email)
            .IsUnique();
        modelBuilder.Entity<Author>()
            .HasIndex(c => c.Name)
            .IsUnique();
    }
}
//for the other contributors of this project:
//if you ever want to make a new migration use:
//dotnet ef migrations add InitialDBSchema --project Chirp.Core   --startup-project Chirp.Razor
//from the src folder, followed by:
//dotnet ef database update --project Chirp.Core   --startup-project Chirp.Razor               

