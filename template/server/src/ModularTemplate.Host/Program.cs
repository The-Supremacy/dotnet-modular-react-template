using Mediator;
using ModularTemplate.Host.Configuration;
using ModularTemplate.Host.Authorization;
using ModularTemplate.Host.Features.Auth;
using ModularTemplate.Host.Features.CurrentUser;
using ModularTemplate.Host.HostedServices;
using ModularTemplate.Identity;
using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.CurrentUser;
using ModularTemplate.Identity.Infrastructure;
using ModularTemplate.Identity.Infrastructure.Persistence;
using ModularTemplate.Persistence.Configuration;
using ModularTemplate.Persistence.Transactions;
using ModularTemplate.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddPersistence();
builder.AddHostAuthentication();
builder.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddMediator(options =>
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
builder.Services
    .AddOptions<InitialApplicationAccessOptions>()
    .BindConfiguration("Identity:InitialApplicationAccess");
builder.Services.AddHostedService<InitialApplicationAccessHostedService>();
builder.Services.AddIdentityModule();
builder.Services.AddIdentityInfrastructure();
builder.Services.AddApplicationAccessAuthorization();

var app = builder.Build();
app.UseProblemDetails();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.MapAuthEndpoints();
app.MapCurrentUserEndpoint();

app.Run();

public partial class Program;
