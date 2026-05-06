using ModularTemplate.Identity.Contracts.CurrentUser;

namespace ModularTemplate.Identity.Contracts.Authorization;

public interface IApplicationAccessAuthorizer
{
    Task<bool> HasApplicationAccessAsync(
        AuthenticatedIdentity? identity,
        CancellationToken cancellationToken);
}
