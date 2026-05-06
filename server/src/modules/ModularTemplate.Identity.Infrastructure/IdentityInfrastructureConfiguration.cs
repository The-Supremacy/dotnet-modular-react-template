using Microsoft.Extensions.DependencyInjection;
using ModularTemplate.Identity.Infrastructure.Persistence;
using ModularTemplate.Identity.Persistence;

namespace ModularTemplate.Identity.Infrastructure;

public static class IdentityInfrastructureConfiguration
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IIdentityStore, IdentityStore>();

        return services;
    }
}
