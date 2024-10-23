namespace Chirp.Razor.test.ChirpRazor.Tests;

using System.Threading.Tasks;
using Chirp.Razor.CheepRepository;
using Chirp.Razor.DomainModel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;



public class Test(){

    [Fact]
    public async Task testCheepRepository(){
        //Arange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync();

        ICheepRepository cheepRepository = new CheepRepository(context);

        var userName = "Helge";
        var Message = "Hello, BDSA students!";
        var CheepId = 656;
        var AuthorId = 11;


        var list = cheepRepository.ReadCheeps().Result;
        
        var result = list[list.Count - 2];

        
        
        Assert.True(result.CheepId == CheepId);
        Assert.True(result.AuthorId == AuthorId);
        Assert.True(result.Author.Name == userName);
        Assert.True(result.Text == Message);


    }
}