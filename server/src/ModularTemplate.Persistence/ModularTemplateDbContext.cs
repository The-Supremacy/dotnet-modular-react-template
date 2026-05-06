using Microsoft.EntityFrameworkCore;
using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Infrastructure.Persistence;
using ModularTemplate.Identity.Users;

namespace ModularTemplate.Persistence;

public sealed class ModularTemplateDbContext(DbContextOptions<ModularTemplateDbContext> options)
    : DbContext(options), IIdentityDbContext
{
    public DbSet<LocalUser> LocalUsers => Set<LocalUser>();

    public DbSet<ApplicationAccessRecord> ApplicationAccessRecords => Set<ApplicationAccessRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IIdentityDbContext).Assembly);
    }
}
