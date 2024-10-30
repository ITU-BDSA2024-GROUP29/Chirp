namespace Chirp.Razor.test{


using System.Threading.Tasks;
    using Chirp.Core.DomainModel;
    using Chirp.Repository;
    using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class Test(){

    //This test makes sure our method for putting the database into memory works
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

    }

    //This test makes sure we are getting actuall data output from our database
    [Fact]
    public async Task testCheepRepositoryReadCheeps(){
         //Arange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);
        CheepRepository cheepRepository = new CheepRepository(context);

        var userName = "Helge";
        var Message = "Hello, BDSA students!";
        var TimeStamp = "01-08-2023 12:16:48";
        var CheepId = 656;
        var AuthorId = 11;


        var list = cheepRepository.ReadCheeps().Result;

        var result = list[list.Count - 2];


         //Test
        Assert.True(result.CheepId == CheepId);
        Assert.True(result.AuthorId == AuthorId);
        Assert.True(result.Author.Name == userName);
        Assert.True(result.Text == Message);
        Assert.True(result.TimeStamp.ToString("dd-MM-yyyy HH:mm:ss")== TimeStamp);


    }


    //this test makes sure our pagnation function works as intended
    [Fact]
    public async Task testCheepRepositoryPagnationAsync()
    {

        //Arange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        
        using var context = new ChirpDBContext(builder.Options);
        
        await context.Database.EnsureCreatedAsync();
        
        DbInitializer.SeedDatabase(context);
        CheepRepository cheepRepository = new CheepRepository(context);


            var list = cheepRepository.GetPaginatedCheeps(3).Result;

             //Test
            Assert.True(list.Count == 32);
    }

    //This test makes sure our cheepcount method for calculating the amount of pages returns the correct value
    [Fact]
    public async Task testCheepRepositoryGetCheepCount(){
         //Arange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);
        CheepRepository cheepRepository = new CheepRepository(context);

        var list = cheepRepository.ReadCheeps().Result;

        //Test
        Assert.True(list.Count == cheepRepository.GetTotalCheepCount().Result);


    }

    //This is a test where we make sure that we get the cheeps when specefied only by an author
    [Fact]
    public async Task testCheepRepositoryGetCheepByAuthor(){
           //Arange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);
        CheepRepository cheepRepository = new CheepRepository(context);

        var userName = "Adrian";
        var Message = "Hej, velkommen til kurset.";
        var TimeStamp = "01-08-2023 13:08:28";
        var CheepId = 657;
        var AuthorId = 12;


        var list = cheepRepository.GetTotalCheepsFromAuthorCount("Adrian").Result;

        var result = list[0];


         //Test
        Assert.True(result.CheepId == CheepId);
        Assert.True(result.AuthorId == AuthorId);
        Assert.True(result.Author.Name == userName);
        Assert.True(result.Text == Message);
        Assert.True(result.TimeStamp.ToString("dd-MM-yyyy HH:mm:ss")== TimeStamp);
    }


    //this is an integration test, which is resposible for testing both add cheeps and read cheeps, this also servers as the only test for add cheeps to the database
     [Fact]
    public async Task testCheepRepositoryCreatCheepIntegrationTest(){
              //Arange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        CheepRepository cheepRepository = new CheepRepository(context);
        Author author = new Author();
        author.AuthorId = 9999;
        author.Email = "test@test.com";
        author.Name = "Test";
        Cheep cheep = new Cheep();
        cheep.Author = author;
        cheep.AuthorId = author.AuthorId;
        cheep.CheepId = 9999;
        cheep.Text = "This is a Test Cheep";
        DateTime time = new DateTime(2000,04,30);
        cheep.TimeStamp = time;

         _ = cheepRepository.CreateCheepAsync(cheep);

        var list = cheepRepository.ReadCheeps().Result;

        var cheepFromDb = list[list.Count-1];


        //test
        Assert.True(cheepFromDb.Author == cheep.Author);
        Assert.True(cheepFromDb.AuthorId == cheep.AuthorId);
        Assert.True(cheepFromDb.CheepId == cheep.CheepId);
        Assert.True(cheepFromDb.Text == cheep.Text);
        Assert.True(cheepFromDb.TimeStamp == cheep.TimeStamp);



    }

}
}
