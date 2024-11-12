using Chirp.Core;
using Chirp.Core.DomainModel;
using Chirp.Repository;
using Microsoft.EntityFrameworkCore;
using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace Chirp.Razor
{

    class Program
    {
        public static void Main(String[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load database connection via configuration
            builder.Services.AddDbContext<ChirpDBContext>(options =>
                options.UseSqlite(builder.Configuration
                .GetConnectionString("DefaultConnection")));
            var environment = builder.Environment.EnvironmentName;
            
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (environment == "Production")
            {
                // In Azure, use the writeable directory path for SQLite
                connectionString = "Data Source=D:\\home\\site\\wwwroot\\ChirpDatabase.db";
            }

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
            options.SignIn.RequireConfirmedAccount = false) 
            .AddEntityFrameworkStores<ChirpDBContext>(); 

            // Add services to the container.
            builder.Services.Core();
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<ICheepRepository, CheepRepository>();
            builder.Services.AddScoped<ICheepService, CheepService>();


            // builder.Services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //     options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = "GitHub";
            // })
            // .AddCookie()
            // .AddGitHub(o =>
            // {
            //     o.ClientId = builder.Configuration["authentication_github_clientId"];
            //     o.ClientSecret = builder.Configuration["authentication_github_clientSecret"];
            //     o.CallbackPath = "/signin-github";
            // });

            // Build
            // Once you are sure everything works, you might want to increase this value to up to 1 or 2 years
            // builder.Services.AddHsts(options => options.MaxAge = TimeSpan.FromHours(1));
            var app = builder.Build();
            _ = app.Services.GetService<IServiceCollection>(); //delete??






            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var context = services.GetRequiredService<ChirpDBContext>();
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
