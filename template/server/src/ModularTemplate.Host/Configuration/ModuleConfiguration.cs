using Mediator;
using ModularTemplate.Identity;
using ModularTemplate.Identity.CurrentUser;
using ModularTemplate.Identity.Infrastructure;
using ModularTemplate.Identity.Infrastructure.Persistence;
using ModularTemplate.Persistence.Transactions;

namespace ModularTemplate.Host.Configuration;

public static class ModuleConfiguration
{
    public static IServiceCollection AddConfiguredModules(this IServiceCollection services)
    {
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
            options.Assemblies =
            [
                typeof(ResolveCurrentUserCommand).Assembly,
                typeof(ApplicationAccessRepository).Assembly
            ];
            options.PipelineBehaviors =
            [
                typeof(CommandTransactionBehavior<,>)
            ];
        });

        services.AddIdentityModule();
        services.AddIdentityInfrastructure();

        return services;
    }
}
