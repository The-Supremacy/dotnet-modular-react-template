using Microsoft.EntityFrameworkCore;
using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Persistence;
using ModularTemplate.Identity.Users;

namespace ModularTemplate.Identity.Infrastructure.Persistence;

public sealed class IdentityStore(IIdentityDbContext dbContext) : IIdentityStore
{
    public async Task<LocalUser> GetOrCreateLocalUserAsync(
        string provider,
        string subject,
        string? displayName,
        string? email,
        CancellationToken cancellationToken)
    {
        LocalUser? user = await dbContext.LocalUsers
            .SingleOrDefaultAsync(
                x => x.Provider == provider && x.Subject == subject,
                cancellationToken);

        if (user is null)
        {
            user = LocalUser.Create(provider, subject, displayName, email);
            dbContext.LocalUsers.Add(user);
        }
        else
        {
            user.MarkSeen(displayName, email);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }

    public Task<bool> HasActiveApplicationAccessAsync(
        Guid localUserId,
        CancellationToken cancellationToken)
    {
        return dbContext.ApplicationAccessRecords.AnyAsync(
            x => x.LocalUserId == localUserId && x.IsActive,
            cancellationToken);
    }

    public async Task UpsertApplicationAccessAsync(
        ApplicationAccessRecord accessRecord,
        CancellationToken cancellationToken)
    {
        ApplicationAccessRecord? existing = await dbContext.ApplicationAccessRecords
            .SingleOrDefaultAsync(x => x.LocalUserId == accessRecord.LocalUserId, cancellationToken);

        if (existing is null)
        {
            dbContext.ApplicationAccessRecords.Add(accessRecord);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
