using ModularTemplate.Identity.Persistence;

namespace ModularTemplate.Identity.Access;

public sealed record InitialApplicationAccessOptions(
    string? Provider,
    string? Subject);

public sealed class InitialApplicationAccessBootstrapper(
    IIdentityStore identityStore,
    InitialApplicationAccessOptions options)
{
    public async Task BootstrapAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(options.Provider)
            || string.IsNullOrWhiteSpace(options.Subject))
        {
            return;
        }

        var localUser = await identityStore.GetOrCreateLocalUserAsync(
            options.Provider,
            options.Subject,
            displayName: null,
            email: null,
            cancellationToken);

        await identityStore.UpsertApplicationAccessAsync(
            new ApplicationAccessRecord(Guid.NewGuid(), localUser.Id),
            cancellationToken);
    }
}
