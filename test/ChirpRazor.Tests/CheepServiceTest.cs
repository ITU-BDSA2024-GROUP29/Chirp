namespace Chirp.Razor.test.ChirpRazor.Tests{
    using System.Data.Common;
    using System.Reflection.Metadata;
    using System.Threading.Tasks;
using Chirp.Razor.CheepRepository;
using Chirp.Razor.DomainModel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;



public class CheepServiceTest(){
    [Fact]
    public async Task DbLoaderTest(){
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        ICheepRepository cheepRepository = new CheepRepository(context);
        

        ICheepService service = CheepService.GetInstance(cheepRepository);

        var list = service.GetCheepsAsync().Result;

        Assert.False(list.Count == 0);

        Assert.Same(cheepRepository, service.GetCheepRepository());
    }
[Fact]
    public async Task CheepCountTest(){
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        CheepRepository cheepRepository = new CheepRepository(context);
        

        ICheepService service = new CheepService(cheepRepository);

        var list = service.GetCheepsAsync().Result;

        
        Assert.Equal(service.GetTotalCheepCount().Result, list.Count);
    }

        [Fact]
    public async Task GetCheepstest(){
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        CheepRepository cheepRepository = new CheepRepository(context);
        

        ICheepService service = new CheepService(cheepRepository);

        var list = service.GetCheepsAsync().Result;

        var userName = "Helge";
        var Message = "Hello, BDSA students!";
        var TimeStamp = "01/08/2023 12:16:48";

        Assert.Equal(userName,list[list.Count-1].Author);
        Assert.Equal(Message,list[list.Count-1].Message);
        Assert.Equal(TimeStamp,list[list.Count-1].Timestamp);

    }

    [Fact]
    public async Task getCheepsByAuthorTest(){
         using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        CheepRepository cheepRepository = new CheepRepository(context);
        

        ICheepService service = new CheepService(cheepRepository);

        var list = service.GetCheepsFromAuthorAsync("Helge").Result;

        var userName = "Helge";
        var Message = "Hello, BDSA students!";
        var TimeStamp = "01/08/2023 12:16:48";

        Assert.Equal(userName,list[0].Author);
        Assert.Equal(Message,list[0].Message);
        Assert.Equal(TimeStamp,list[0].Timestamp);

    }
 [Fact]
    public async Task PaginatedPageTest(){
         using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

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
        Assert.Equal(TimeStamp,list[list.Count-1].Timestamp);

        list = service.GetPaginatedCheepsAsync(1,PageSize).Result;
        Assert.Equal(32,list.Count);
    }
}
}