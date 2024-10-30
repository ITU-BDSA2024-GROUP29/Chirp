using Chirp.Razor.CheepRepository;
using Chirp.Razor.DomainModel;
using Microsoft.EntityFrameworkCore;
using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Chirp.Razor {
    class Program {
        public static void Main(String[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Load database connection via configuration
            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));

            ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
            //DbInitializer.SeedDatabase(serviceProvider.GetService(ChirpDBContext));

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<ICheepRepository, CheepRepository.CheepRepository>();
            builder.Services.AddScoped<ICheepService, CheepService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "GitHub";
            })
            .AddCookie()
            .AddGitHub(o =>
            {
                o.ClientId = builder.Configuration["authentication_github_clientId"];
                o.ClientSecret = builder.Configuration["authentication_github_clientSecret"];
                o.CallbackPath = "/signin-github";
            });

            // Build
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();

            app.Run();

        }
    }



}
