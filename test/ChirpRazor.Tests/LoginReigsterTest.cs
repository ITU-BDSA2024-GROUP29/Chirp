
using Chirp.Core.DomainModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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



        //this methods test the login in credentials using the objects our .identity migration gave
        [Fact]
        public async Task Test_Logincredentialstest_false_true(){
                using (var connection = new SqliteConnection("Filename=:memory:")){
                await connection.OpenAsync();
                var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);

                using (var context = new ChirpDBContext(builder.Options)){
                    await context.Database.EnsureCreatedAsync();

                    var userStore = new UserStore<ApplicationUser>(context);
                    var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());  // You can add other log providers like File, etc.
                    var logger = loggerFactory.CreateLogger<UserManager<ApplicationUser>>();

                    #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    var userManager = new UserManager<ApplicationUser>(
                    userStore, null, new PasswordHasher<ApplicationUser>(), null, null, null, null, null, logger);
                    #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                    var user = new ApplicationUser { UserName = "TestUser", Email = "testuser@example.com" };
                    var result = await userManager.CreateAsync(user, "TestPassword123!");

                    Assert.True(result.Succeeded);
                    Assert.NotNull(await userManager.FindByNameAsync("TestUser"));

                    await context.SaveChangesAsync();

                    
                    var loginTry = await userManager.CheckPasswordAsync(user,"WrongPassword!");
                    Assert.False(loginTry);

                    loginTry = await userManager.CheckPasswordAsync(user,"TestPassword123!");
                    Assert.True(loginTry);


                }
            }
        }

    }

}