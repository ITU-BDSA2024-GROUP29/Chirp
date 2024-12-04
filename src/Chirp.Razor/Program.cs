using Chirp.Core;
using Chirp.Core.DomainModel;
using Chirp.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Chirp.Razor
{
    class Program
    {
        public static void Main(String[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load database connection via configuration
            builder.Services.AddDbContext<ChirpDBContext>(options => {
                options.EnableSensitiveDataLogging();
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
                
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
                options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ChirpDBContext>();


            builder.Services.AddAuthentication()
                .AddGitHub(o =>
                {
                    o.ClientId = builder.Configuration["authentication:github:clientId"] ?? Environment.GetEnvironmentVariable("GITHUBCLIENTID");
                    o.ClientSecret = builder.Configuration["authentication:github:clientSecret"] ?? Environment.GetEnvironmentVariable("GITHUBCLIENTSECRET");
                    o.CallbackPath = "/signin-github";
                });

            // Add services to the container.
            builder.Services.Core();
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<ICheepRepository, CheepRepository>();
            builder.Services.AddScoped<ICheepService, CheepService>();
            
            


            var app = builder.Build();

            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var context = services.GetRequiredService<ChirpDBContext>();
                context.Database.EnsureCreated();
                DbInitializer.SeedDatabase(context);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            else if (app.Environment.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.Run();
        }
    }
}
