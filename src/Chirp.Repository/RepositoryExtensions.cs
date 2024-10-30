
using Chirp.Core.DomainModel;
using Microsoft.Extensions.DependencyInjection;

namespace Chirp.Repository;

public static class RepositoryExtensions {
    public static IServiceCollection Repository(this IServiceCollection repositories) {
        IServiceCollection services = new ServiceCollection();
        
        return repositories;
    }
}

