
using Chirp.Core.DomainModel;
using Chirp.Razor.Pages;
using Chirp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chirp.Razor.test.ChirpRazor.Tests
{

    public class SharedFunctionTest
    {
        /*
        This test, tests to make sure our program actually safeguards againts cheeps being over 160 charaters
        */
        [Fact]
        public async Task Test_CreateUser_IsTypeBadRequest_IdenticalString()
        {
            using (var connection = new SqliteConnection("Filename=:memory:"))
            {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

                using (var context = new ChirpDBContext(builder.Options))
                {
                    Thread.Sleep(5000); //To Insure no collition with other tests
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    ICheepRepository cheepRepository = new CheepRepository(context);

                    ICheepService service = CheepService.GetInstance(cheepRepository);

                    SharedFuncs shared = new SharedFuncs(service);
                    var toLongCheep = new string('a', 161); // 161 char String
                    var error = await shared.OnPostCheepAsync(toLongCheep);



                    Assert.IsType<BadRequestObjectResult>(error);
                    var BadRequestResult = error as BadRequestObjectResult;
#pragma warning disable CS8602
                    Assert.Equal("Cheep Length too long > 160", BadRequestResult.Value);
#pragma warning restore CS8602
                }
            }
        }

        /*
        Here we make sure that you are uable to delete cheeps from user backend without having Authorization 
        */
        [Fact]
        public async Task Test_OnPostDelete_Identical_UableToDeleteWithoutAuthorization()
        {
            using (var connection = new SqliteConnection("Filename=:memory:"))
            {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

                using (var context = new ChirpDBContext(builder.Options))
                {

                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);
                    Thread.Sleep(5000); //To Insure no collition with other tests
                    ICheepRepository cheepRepository = new CheepRepository(context);

                    ICheepService service = CheepService.GetInstance(cheepRepository);

                    // Create TempData with a real TempDataProvider  
                    var tempDataProvider = new FakeTempDataProvider();
                    var tempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProvider);


                    SharedFuncs shared = new SharedFuncs(service)
                    {
                        TempData = tempData
                    };


                    // Cheep ID656
                    // Author ID 11 Helge
                    Assert.True(service.getAuthorCheepCount("Helge") == 1);





                    var result = await shared.OnPostDelete(656);

                    Console.WriteLine(shared.TempData.First().ToString());
                    Console.WriteLine("wow");

                    Assert.Equal("[ErrorMessage, An error occurred while attempting to delete the cheep.]", shared.TempData.First().ToString());

                }
            }

        }


    }

    public class FakeTempDataProvider : ITempDataProvider
    {
        private readonly Dictionary<string, object> _tempData = new();

        public IDictionary<string, object> LoadTempData(HttpContext context)
        {
            return _tempData;
        }
        /*
        Required by the interface
        */
        public void SaveTempData(HttpContext context, IDictionary<string, object> values)
        {
            foreach (var item in values)
            {
                _tempData[item.Key] = item.Value;
            }
        }
    }

}