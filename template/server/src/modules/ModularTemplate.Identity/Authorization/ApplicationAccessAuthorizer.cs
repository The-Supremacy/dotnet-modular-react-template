using ModularTemplate.Identity.Contracts.Authorization;
using ModularTemplate.Identity.Contracts.CurrentUser;

namespace ModularTemplate.Identity.Authorization;

public sealed class ApplicationAccessAuthorizer(ICurrentUserProvider currentUserProvider)
    : IApplicationAccessAuthorizer
{
    public async Task<bool> HasApplicationAccessAsync(
        AuthenticatedIdentity? identity,
        CancellationToken cancellationToken)
    {
        CurrentUserContext currentUser = await currentUserProvider.GetCurrentUserAsync(
            identity,
            cancellationToken);

        return currentUser.IsAuthenticated && currentUser.HasApplicationAccess;
    }
}
