using Chirp.Core.DomainModel;
using Chirp.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.test.ChirpRazor.Tests
{
    public class CheepServiceTest(){
    [Fact]
    public async Task Test_CheepServiceDependencyInjection_NotNull_NotEmpty(){
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);

        ICheepRepository cheepRepository = new CheepRepository(context);
        

        ICheepService service = CheepService.GetInstance(cheepRepository);

        var list = service.GetCheepsAsync().Result;

        Assert.False(list.Count == 0);

        Assert.Same(cheepRepository, service.GetCheepRepository());

        connection.Dispose();
    }
[Fact]
    public async Task Test_CheepCount_Equal(){
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);

        CheepRepository cheepRepository = new CheepRepository(context);
        

        ICheepService service = new CheepService(cheepRepository);

        var list = service.GetCheepsAsync().Result;

        
        Assert.Equal(service.GetTotalCheepCount().Result, list.Count);

        connection.Dispose();
    }

        [Fact]
    public async Task Test_GetCheeps_IdenticalCheep(){
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);

        CheepRepository cheepRepository = new CheepRepository(context);
        

        ICheepService service = new CheepService(cheepRepository);

        var list = service.GetCheepsAsync().Result;

        var userName = "Helge";
        var Message = "Hello, BDSA students!";
        var TimeStamp = "08/01/2023 12:16:48";

        Assert.Equal(userName,list[list.Count-1].Author);
        Assert.Equal(Message,list[list.Count-1].Message);
        
        connection.Dispose();
    }

    [Fact]
    public async Task Test_getCheepsByAuthor_IdenticalCheep(){
         using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);

        CheepRepository cheepRepository = new CheepRepository(context);
        

        ICheepService service = new CheepService(cheepRepository);

        var list = service.GetCheepsFromAuthorAsync("Helge").Result;

        var userName = "Helge";
        var Message = "Hello, BDSA students!";
        var TimeStamp = "01/08/2023 12:16:48";

        Assert.Equal(userName,list[0].Author);
        Assert.Equal(Message,list[0].Message);
        
        connection.Dispose();
    }
 [Fact]
    public async Task Test_PaginatedPage_32Cheeps(){
         using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);

        CheepRepository cheepRepository = new CheepRepository(context);
        

        ICheepService service = new CheepService(cheepRepository);

        

        const int PageSize = 32;
        var totalCheeps = await service.GetTotalCheepCount();
        var TotalPages = (int)Math.Ceiling(totalCheeps / (double)PageSize);

        var list = service.GetPaginatedCheepsAsync(TotalPages,PageSize).Result;
        

        var userName = "Helge";
        var Message = "Hello, BDSA students!";
        var TimeStamp = "01/08/2023 12:16:48";

        Assert.Equal(userName,list[list.Count-1].Author);
        Assert.Equal(Message,list[list.Count-1].Message);
        

        list = service.GetPaginatedCheepsAsync(1,PageSize).Result;
        Assert.Equal(32,list.Count);

        connection.Dispose();
    }
}
}