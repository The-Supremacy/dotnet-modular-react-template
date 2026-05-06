using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Persistence;
using ModularTemplate.Identity.Users;

namespace ModularTemplate.Identity.Tests.Support;

internal sealed class InMemoryIdentityStore : IIdentityStore
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
