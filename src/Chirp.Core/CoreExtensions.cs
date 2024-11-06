using Chirp.Core.DomainModel;

namespace Chirp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



public static class CoreExtensions {
    public static IServiceCollection Core(this IServiceCollection services) {
        services.AddDbContext<ChirpDBContext>( o =>
            o.UseSqlite("Data Source=Cheep.db"));
        return services;
    }
}
