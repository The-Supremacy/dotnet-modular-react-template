using Microsoft.EntityFrameworkCore;
using ModularTemplate.Identity.Infrastructure.Persistence;
using ModularTemplate.Persistence;
using ModularTemplate.Identity.Infrastructure.Tests.Support;
using Shouldly;

namespace ModularTemplate.Identity.Infrastructure.Tests.Persistence;

public sealed class IdentityStoreTests(PostgreSqlFixture postgreSqlFixture)
    : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetOrCreateLocalUserAsync_reuses_existing_provider_subject()
    {
        await using var dbContext = CreateDbContext();
        await dbContext.Database.EnsureCreatedAsync(CancellationToken.None);
        var store = new IdentityStore(dbContext);

        var first = await store.GetOrCreateLocalUserAsync(
            "oidc",
            "subject-1",
            "Ada",
            "ada@example.test",
            CancellationToken.None);
        var second = await store.GetOrCreateLocalUserAsync(
            "oidc",
            "subject-1",
            "Ada Lovelace",
            "ada@example.test",
            CancellationToken.None);

        second.Id.ShouldBe(first.Id);
        (await dbContext.LocalUsers.CountAsync(CancellationToken.None)).ShouldBe(1);
    }

    private ModularTemplateDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ModularTemplateDbContext>()
            .UseNpgsql(postgreSqlFixture.ConnectionString)
            .Options;

        return new ModularTemplateDbContext(options);
    }
}
