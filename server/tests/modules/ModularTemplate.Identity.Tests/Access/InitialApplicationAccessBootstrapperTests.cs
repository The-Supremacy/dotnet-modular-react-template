using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Tests.Support;
using Shouldly;

namespace ModularTemplate.Identity.Tests.Access;

public sealed class InitialApplicationAccessBootstrapperTests
{
    [Fact]
    [Trait("Category", "Unit")]
    public async Task BootstrapAsync_creates_active_application_access_from_configuration()
    {
        var store = new InMemoryIdentityStore();
        var bootstrapper = new InitialApplicationAccessBootstrapper(
            store,
            new InitialApplicationAccessOptions("oidc", "subject-1"));

        await bootstrapper.BootstrapAsync(CancellationToken.None);
        var user = await store.GetOrCreateLocalUserAsync(
            "oidc",
            "subject-1",
            null,
            null,
            CancellationToken.None);

        bool hasAccess = await store.HasActiveApplicationAccessAsync(
            user.Id,
            CancellationToken.None);

        hasAccess.ShouldBeTrue();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task BootstrapAsync_is_idempotent()
    {
        var store = new InMemoryIdentityStore();
        var bootstrapper = new InitialApplicationAccessBootstrapper(
            store,
            new InitialApplicationAccessOptions("oidc", "subject-1"));

        await bootstrapper.BootstrapAsync(CancellationToken.None);
        await bootstrapper.BootstrapAsync(CancellationToken.None);
        var user = await store.GetOrCreateLocalUserAsync(
            "oidc",
            "subject-1",
            null,
            null,
            CancellationToken.None);

        bool hasAccess = await store.HasActiveApplicationAccessAsync(
            user.Id,
            CancellationToken.None);

        hasAccess.ShouldBeTrue();
    }
}
