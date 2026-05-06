using Microsoft.EntityFrameworkCore;
using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Users;

namespace ModularTemplate.Identity.Infrastructure.Persistence;

public interface IIdentityDbContext
{
    DbSet<LocalUser> LocalUsers { get; }

    DbSet<ApplicationAccessRecord> ApplicationAccessRecords { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
