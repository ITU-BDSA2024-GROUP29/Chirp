namespace Chirp.Razor.test.ChirpRazor.Tests{


using System.Threading.Tasks;
using Chirp.Razor.CheepRepository;
using Chirp.Razor.DomainModel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;



public class Test(){

   public ICheepRepository CheepRepository;


   
    [Fact] 
    public async Task canInitializeCheepRepository(){
        //Arange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        CheepRepository cheepRepository = new CheepRepository(context);
        //Test
        Assert.NotNull(cheepRepository);

        this.CheepRepository = cheepRepository;

    }

    [Fact]
    public async Task testCheepRepositoryAsync(){
         //Arange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        CheepRepository cheepRepository = new CheepRepository(context);

        var userName = "Helge";
        var Message = "Hello, BDSA students!";
        var TimeStamp = "01-08-2023 12:16:48";
        var CheepId = 656;
        var AuthorId = 11;


        var list = cheepRepository.ReadCheeps().Result;

        var result = list[list.Count - 2];



        Assert.True(result.CheepId == CheepId);
        Assert.True(result.AuthorId == AuthorId);
        Assert.True(result.Author.Name == userName);
        Assert.True(result.Text == Message);
        Assert.True(result.TimeStamp.ToString("dd-MM-yyyy HH:mm:ss")== TimeStamp);


    }
}
}
