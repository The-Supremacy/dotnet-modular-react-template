using Microsoft.Extensions.DependencyInjection;
using ModularTemplate.Identity.Infrastructure.Persistence;
using ModularTemplate.Identity.Users;
using ModularTemplate.Identity.Access;

namespace ModularTemplate.Identity.Infrastructure;

public static class IdentityInfrastructureConfiguration
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ILocalUserRepository, LocalUserRepository>();
        services.AddScoped<IApplicationAccessRepository, ApplicationAccessRepository>();

        return services;
    }
}
