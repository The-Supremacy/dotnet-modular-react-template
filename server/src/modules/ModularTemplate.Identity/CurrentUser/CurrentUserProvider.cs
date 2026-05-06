using ModularTemplate.Identity.Contracts.CurrentUser;
using ModularTemplate.Identity.Persistence;

namespace ModularTemplate.Identity.CurrentUser;

public sealed class CurrentUserProvider(IIdentityStore identityStore) : ICurrentUserProvider
{
    public async Task<CurrentUserContext> GetCurrentUserAsync(
        AuthenticatedIdentity? identity,
        CancellationToken cancellationToken)
    {
        if (identity is null || string.IsNullOrWhiteSpace(identity.Subject))
        {
            return CurrentUserContext.Unauthenticated;
        }

        var localUser = await identityStore.GetOrCreateLocalUserAsync(
            identity.Provider,
            identity.Subject,
            identity.DisplayName,
            identity.Email,
            cancellationToken);

        bool hasAccess = await identityStore.HasActiveApplicationAccessAsync(
            localUser.Id,
            cancellationToken);

        return new CurrentUserContext(
            true,
            localUser.Id,
            localUser.DisplayName,
            localUser.Email,
            hasAccess);
    }
}
