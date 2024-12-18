using Chirp.Core.DomainModel;
using Chirp.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.test.ChirpRazor.Tests {
    public class CheepServiceTest() {

        /*
        This Test Case makes sure that CheepService takes the correct object "CheepRepository" as an depency injection
        */
        [Fact]
        public async Task Test_CheepServiceDependencyInjection_NotNull_NotEmpty() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    ICheepRepository cheepRepository = new CheepRepository(context);


                    ICheepService service = CheepService.GetInstance(cheepRepository);

                    var list = await service.GetCheepsAsync();

                    Assert.False(list.Count == 0);

                    Assert.Same(cheepRepository, service.GetCheepRepository());

                }
            }
        }

        /*
        This Test, Tests getTotalCheepCount to make sure it returns propper values
        */
        [Fact]
        public async Task Test_CheepCount_Equal() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);


                    ICheepService service = new CheepService(cheepRepository);

                    var list = await service.GetCheepsAsync();


                    Assert.Equal(await service.GetTotalCheepCount(), list.Count);

                }
            }

        }

        
        /*
        This test, Test the getCheep method i should return a specefic cheep
        */
        [Fact]
        public async Task Test_GetCheeps_IdenticalCheep() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);


                    ICheepService service = new CheepService(cheepRepository);

                    var list = await service.GetCheepsAsync();

                    var userName = "Helge";
                    var Message = "Hello, BDSA students!";

                    Assert.Equal(userName, list[list.Count - 1].Author);
                    Assert.Equal(Message, list[list.Count - 1].Message);

                }
            }
        }

        /*
        This test, tests our getCheepsByAuthor method, we make sure correct cheeps are returned based on 
        DBinitilizer
        */
        [Fact]
        public async Task Test_getCheepsByAuthor_IdenticalCheep() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);


                    ICheepService service = new CheepService(cheepRepository);

                    var list = await service.GetCheepsFromAuthorAsync("Helge");

                    var userName = "Helge";
                    var Message = "Hello, BDSA students!";

                    Assert.Equal(userName, list[0].Author);
                    Assert.Equal(Message, list[0].Message);
                }
            }
        }
        
        /*
        This Method makes sure our PaginatedPage returns 32_cheeps
        */
        [Fact]
        public async Task Test_PaginatedPage_32Cheeps() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);


                    ICheepService service = new CheepService(cheepRepository);



                    const int PageSize = 32;
                    var totalCheeps = await service.GetTotalCheepCount();
                    var TotalPages = (int)Math.Ceiling(totalCheeps / (double)PageSize);

                    var list = await service.GetPaginatedCheepsAsync(TotalPages, PageSize);


                    var userName = "Helge";
                    var Message = "Hello, BDSA students!";

                    Assert.Equal(userName, list[list.Count - 1].Author);
                    Assert.Equal(Message, list[list.Count - 1].Message);


                    list = await service.GetPaginatedCheepsAsync(1, PageSize);
                    Assert.Equal(32, list.Count);

                }
            }
        }

        /*
        This test, tests getTimelineCheeps we make sure that a specefic cheep shows up
        */
        [Fact]
        public async Task Test_GetTimelineCheeps_SpeceficAuthor() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);


                    ICheepService service = new CheepService(cheepRepository);

                    var jac = await service.GetOwnCheepsAsync("Jacqualine Gilcoine");

                    foreach (var cheep in jac) {
                        if (cheep.Author == "Roger Histand") {
                            Assert.True(true);
                            return;
                        }
                    }
                    Assert.True(false);
                }
            }
        }
        
        /*
        This test, Test the FollowAuthor Method, we expect 2 authors to follow each other 
        */
        [Fact]
        public async Task Test_FollowAuthor_AuthorFollow() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);


                    ICheepService service = new CheepService(cheepRepository);

                    //Jacqualine Gilcoine starts following Luanna Muro
                    await service.FollowAuthor("Luanna Muro", "Jacqualine Gilcoine");

                    Assert.True(await service.IsUserFollowing("Jacqualine Gilcoine","Luanna Muro"));

                }
            }
        }


        /*
        This test, Test that our createCheepAsync function, creates a rendered CheepViewModel
        */
        [Fact]
        public async Task Test_ReturnsRenderedMessage_RenderedMessageEquals_Test_() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);



                    Cheep cheep = new Cheep();
                    Author author = new Author();
                    author.AuthorId = 10000;
                    cheep.Author = author;
                    cheep.AuthorId = 10000;
                    cheep.CheepId = 100000;
                    cheep.Text = "Test";
                    cheep.TimeStamp = new DateTime(2024,12,16);
                    await cheepRepository.CreateCheepAsync(cheep);

                    ICheepService service = new CheepService(cheepRepository);

                    List<CheepViewModel> data = await service.GetCheepsAsync();

                    Assert.True(data[0].RenderedMessage == "Test");
                }
            }
        }

        /*
        This test checks that the createCheepAsync function prevents SQL injection by ensuring that malicious inputs are sanitized.
        */
        [Fact]
        public async Task Test_PreventsSqlInjection_AttemptsAreSanitized() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);

                    Cheep cheep = new Cheep();
                    Author author = new Author();
                    author.AuthorId = 10000;
                    cheep.Author = author;
                    cheep.AuthorId = 10000;
                    cheep.CheepId = 100001;
                    cheep.Text = "Test'; DROP TABLE Cheeps; --";
                    cheep.TimeStamp = new DateTime(2024, 12, 16);

                    // Attempt to save the malicious input
                    await cheepRepository.CreateCheepAsync(cheep);

                    ICheepService service = new CheepService(cheepRepository);

                    List<CheepViewModel> data = await service.GetCheepsAsync();

                    // Verify the injected SQL was not executed
                    Assert.True(data.Any(c => c.RenderedMessage == "Test'; DROP TABLE Cheeps; --"));
                }
            }
        }


        /*
        This test, Test to make sure our messageRender can render in bold
        */
        [Fact]
        public async Task Test_ReturnsRenderedMessageBold_RenderedInBold() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);

                    Cheep cheep = new Cheep();
                    Author author = new Author();
                    author.AuthorId = 10000;
                    cheep.Author = author;
                    cheep.AuthorId = 10000;
                    cheep.CheepId = 100000;
                    cheep.Text = "**Test**";
                    cheep.TimeStamp = new DateTime(2024,12,16);
                    await cheepRepository.CreateCheepAsync(cheep);

                    ICheepService service = new CheepService(cheepRepository);
                    

                    List<CheepViewModel> data = await service.GetCheepsAsync();

                    Assert.True(data[0].RenderedMessage == "<strong>Test</strong>");
                }
            }
        }

        /*
        this test, Test to make sure our messageRender can render in Italic
        */
        [Fact]
        public async Task Test_ReturnsRenderedMessageItalic_RenderedInItalic() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);                    


                    Cheep cheep = new Cheep();
                    Author author = new Author();
                    author.AuthorId = 10000;
                    cheep.Author = author;
                    cheep.AuthorId = 10000;
                    cheep.CheepId = 100000;
                    cheep.Text = "*Test*";
                    cheep.TimeStamp = new DateTime(2024,12,16);
                    await cheepRepository.CreateCheepAsync(cheep);

                    ICheepService service = new CheepService(cheepRepository);

                    List<CheepViewModel> data = await service.GetCheepsAsync();

                    Assert.True(data[0].RenderedMessage == "<em>Test</em>");
                }
            }
        }

        /*
        this test, Test to make sure our messageRender can render a link
        */
        [Fact]
        public async Task Test_ReturnsRenderedMessageLink_RenderedToRefLink() {
            using (var connection = new SqliteConnection("Filename=:memory:")) {
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
                using (var context = new ChirpDBContext(builder.Options)) {
                    await context.Database.EnsureCreatedAsync();
                    DbInitializer.SeedDatabase(context);

                    CheepRepository cheepRepository = new CheepRepository(context);

                    Cheep cheep = new Cheep();
                    Author author = new Author();
                    author.AuthorId = 10000;
                    cheep.Author = author;
                    cheep.AuthorId = 10000;
                    cheep.CheepId = 100000;
                    cheep.Text = "[Test Name](https://TestLink)";
                    cheep.TimeStamp = new DateTime(2024,12,16);
                    await cheepRepository.CreateCheepAsync(cheep);

                    ICheepService service = new CheepService(cheepRepository);

                    List<CheepViewModel> data = await service.GetCheepsAsync();

                    Assert.True(data[0].RenderedMessage == "<a href=\"https://TestLink\">Test Name</a>");
                }
            }
        }
    }
}