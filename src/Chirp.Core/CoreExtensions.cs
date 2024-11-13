using Chirp.Core.DomainModel;

namespace Chirp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



public static class CoreExtensions {
    public static IServiceCollection Core(this IServiceCollection services) {
        services.AddDbContext<ChirpDBContext>( o =>
            o.UseSqlite("Data Source=./wwwroot/Cheep.db"));         //Azure webservice needs to have write access to Database (which can only happen in wwwroot)
        return services;
    }
}
