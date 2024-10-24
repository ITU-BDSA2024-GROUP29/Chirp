using Chirp.Razor.CheepRepository;
using Chirp.Razor.DomainModel;
using Microsoft.EntityFrameworkCore;

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
