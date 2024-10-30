using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Chirp.Repository;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class RepositoryExtensions {
    public static IServiceCollection Repository(this IServiceCollection repositories) {
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<ICheepRepository, CheepRepository>();
        ChirpDBContext _dbContext = services.AddScoped<ChirpDBContext>();
        
        return repositories;
    }
}

