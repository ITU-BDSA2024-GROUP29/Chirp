using Chirp.Core;
using Chirp.Core.DomainModel;
using Chirp.Repository;
using Microsoft.EntityFrameworkCore;
using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication.Cookies;
using Chirp.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace Chirp.Razor
{

    class Program
    {
        public static void Main(String[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Ensure secrets and environment variables are added
            builder.Configuration.AddUserSecrets<Program>(optional: true);

            // Load database connection via configuration
            builder.Services.AddDbContext<ChirpDBContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ChirpDBContext>();

            // Add services to the container.
            builder.Services.Core();
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<ICheepRepository, CheepRepository>();
            builder.Services.AddScoped<ICheepService, CheepService>();

            /*
            // Add Authentication with GitHub
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "GitHub";
            })
            .AddCookie()
            .AddGitHub(options =>
            {
                options.ClientId = builder.Configuration["authentication:github:clientId"];
                options.ClientSecret = builder.Configuration["authentication:github:clientSecret"];
                if (string.IsNullOrEmpty(options.ClientId) || string.IsNullOrEmpty(options.ClientSecret))
                {
                    throw new InvalidOperationException("GitHub ClientId or ClientSecret is missing.");
                }
                options.CallbackPath = "/signin-github";
            }); */


            // Build
            // Once you are sure everything works, you might want to increase this value to up to 1 or 2 years
            // builder.Services.AddHsts(options => options.MaxAge = TimeSpan.FromHours(1));
            var app = builder.Build();
            _ = app.Services.GetService<IServiceCollection>(); //delete??

            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var context = services.GetRequiredService<ChirpDBContext>();
                context.Database.EnsureCreated();
                DbInitializer.SeedDatabase(context);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }
            // else if(app.Environment.IsProduction()) {
            //     app.UseHsts(); // Send HSTS headers, but only in production
            // }

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
