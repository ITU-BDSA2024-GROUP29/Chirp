using Chirp.Core.DomainModel;
using Chirp.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.test.ChirpRazor.Tests {
    public class CheepServiceTest() {
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

        [Fact]
        public async Task Test_GetOwnCheeps() {
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
        
        [Fact]
        public async Task Test_FollowAuthor() {
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
    }
}