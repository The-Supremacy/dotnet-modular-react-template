using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularTemplate.Host.Tests.Authentication;
using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Persistence;
using ModularTemplate.Identity.Users;

namespace ModularTemplate.Host.Tests.Support;

public sealed class HostApplicationFactory(
    Action<IServiceCollection>? configureServices = null)
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthenticationHandler.Scheme;
                    options.DefaultChallengeScheme = TestAuthenticationHandler.Scheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                    TestAuthenticationHandler.Scheme,
                    _ => { });

            services.RemoveAll<IIdentityStore>();
            services.AddSingleton<IIdentityStore, HostTestIdentityStore>();

            configureServices?.Invoke(services);
        });
    }
}

internal sealed class HostTestIdentityStore : IIdentityStore
{
    private readonly List<ApplicationAccessRecord> _accessRecords = [];
    private readonly List<LocalUser> _users = [];

    public Task<LocalUser> GetOrCreateLocalUserAsync(
        string provider,
        string subject,
        string? displayName,
        string? email,
        CancellationToken cancellationToken)
    {
        LocalUser? user = _users.SingleOrDefault(x => x.Provider == provider && x.Subject == subject);
        if (user is null)
        {
            user = LocalUser.Create(provider, subject, displayName, email);
            _users.Add(user);
        }
        else
        {
            user.MarkSeen(displayName, email);
        }

        if (user.Subject.EndsWith("-with-access", StringComparison.Ordinal)
            && !_accessRecords.Any(x => x.LocalUserId == user.Id))
        {
            _accessRecords.Add(new ApplicationAccessRecord(Guid.NewGuid(), user.Id));
        }

        return Task.FromResult(user);
    }

    public Task<bool> HasActiveApplicationAccessAsync(
        Guid localUserId,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(_accessRecords.Any(x => x.LocalUserId == localUserId && x.IsActive));
    }

    public Task UpsertApplicationAccessAsync(
        ApplicationAccessRecord accessRecord,
        CancellationToken cancellationToken)
    {
        _accessRecords.RemoveAll(x => x.LocalUserId == accessRecord.LocalUserId);
        _accessRecords.Add(accessRecord);

        return Task.CompletedTask;
    }
}
