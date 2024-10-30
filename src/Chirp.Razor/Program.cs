using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chirp.Razor {
    
    class Program {
        public static void Main(String[] args) {
            var builder = WebApplication.CreateBuilder(args);

            

            ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
            
            // Add services to the container.
            builder.Services.AddRazorPages();
            //builder.Services.AddScoped<ICheepRepository, CheepRepository.CheepRepository>();

            builder.Services.AddScoped<ICheepService, CheepService>();
            builder.Services.BuildServiceProvider();
            //builder.Services.AddDbContext<ChirpDBContext>();

            // Build
            var app = builder.Build();
            //_ = app.Services.GetService<IServiceCollection>(); //delete??

            using (var serviceScope = app.Services.CreateScope()) {
                var services = serviceScope.ServiceProvider;
                var context = services.GetRequiredService<ChirpDBContext>();
                DbInitializer.SeedDatabase(context);
            }

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
