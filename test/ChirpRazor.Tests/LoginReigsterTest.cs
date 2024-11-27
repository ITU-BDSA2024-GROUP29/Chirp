
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.test.ChirpRazor.Tests
{

    public class LoginRegisterTest{

        //This Test makes sure that our login system can actually create a user
        [Fact]
        public async Task Test_CreateUser_True_NotNull(){
            using (var connection = new SqliteConnection("Filename=:memory:")){
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

                using (var context = new ChirpDBContext(builder.Options)){
                    await context.Database.EnsureCreatedAsync();

                    var userStore = new UserStore<ApplicationUser>(context);


                    #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    var userManager = new UserManager<ApplicationUser>(
                    userStore, null, new PasswordHasher<ApplicationUser>(), null, null, null, null, null, null);
                    #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                    var user = new ApplicationUser { UserName = "TestUser", Email = "testuser@example.com" };
                    var result = await userManager.CreateAsync(user, "TestPassword123!");

                    Assert.True(result.Succeeded);
                    Assert.NotNull(await userManager.FindByNameAsync("TestUser"));
                }
            }
        }
    }

}