using Microsoft.EntityFrameworkCore;
using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Infrastructure.Persistence;
using ModularTemplate.Identity.Users;
using ModularTemplate.Persistence.DomainEvents;
using ModularTemplate.SharedKernel.Domain;

namespace ModularTemplate.Persistence;

public sealed class ModularTemplateDbContext(DbContextOptions<ModularTemplateDbContext> options)
    : DbContext(options), IIdentityDbContext
{
    public DbSet<LocalUser> LocalUsers => Set<LocalUser>();

    public DbSet<ApplicationAccess> ApplicationAccess => Set<ApplicationAccess>();

    public DbSet<StoredDomainEvent> DomainEvents => Set<StoredDomainEvent>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        CaptureDomainEvents();

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IIdentityDbContext).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoredDomainEvent).Assembly);
    }

    private void CaptureDomainEvents()
    {
        var domainEventEntries = ChangeTracker.Entries<IAggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Count > 0)
            .Select(x => new
            {
                Aggregate = x.Entity,
                Events = x.Entity.DequeueDomainEvents()
            })
            .ToArray();

        foreach (var entry in domainEventEntries)
        {
            foreach (IDomainEvent domainEvent in entry.Events)
            {
                DomainEvents.Add(StoredDomainEvent.FromDomainEvent(
                    domainEvent,
                    entry.Aggregate.Id.ToString() ?? string.Empty));
            }
        }
    }
}
