
using Chirp.Core.DomainModel;
using Chirp.Razor.Pages;
using Chirp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chirp.Razor.test.ChirpRazor.Tests
{

    public class SharedFunctionTest
    {
        
        [Fact]
         public async Task Test_CreateUser_True_NotNull()
        {
            using (var connection = new SqliteConnection("Filename=:memory:"))
            {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

                using (var context = new ChirpDBContext(builder.Options))
                {
                    
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    ICheepRepository cheepRepository = new CheepRepository(context);

                    ICheepService service = CheepService.GetInstance(cheepRepository);

                    SharedFuncs shared = new SharedFuncs(service);
                    var toLongCheep = new string('a', 161);
                    var error = await shared.OnPostCheepAsync(toLongCheep);


                    
                    Assert.IsType<BadRequestObjectResult>(error);
                    var BadRequestResult  = error as BadRequestObjectResult;
                    Assert.Equal("Cheep Length too long > 160", BadRequestResult.Value);
                }
            }
        }

        

    }

}