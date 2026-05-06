using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Users;

namespace ModularTemplate.Identity.Persistence;

/// <summary>
/// Transitional persistence boundary for the first auth/current-user slice.
/// Replace or reshape this with the durable repository/query-handler pattern
/// before accepting the slice as final template guidance.
/// </summary>
public interface IIdentityStore
{
    Task<LocalUser> GetOrCreateLocalUserAsync(
        string provider,
        string subject,
        string? displayName,
        string? email,
        CancellationToken cancellationToken);

    Task<bool> HasActiveApplicationAccessAsync(
        Guid localUserId,
        CancellationToken cancellationToken);

    Task UpsertApplicationAccessAsync(
        ApplicationAccessRecord accessRecord,
        CancellationToken cancellationToken);
}
