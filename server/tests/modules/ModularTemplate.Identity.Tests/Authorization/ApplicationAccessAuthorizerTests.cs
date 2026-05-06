using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Authorization;
using ModularTemplate.Identity.Contracts.CurrentUser;
using ModularTemplate.Identity.CurrentUser;
using ModularTemplate.Identity.Tests.Support;
using Shouldly;

namespace ModularTemplate.Identity.Tests.Authorization;

public sealed class ApplicationAccessAuthorizerTests
{
    [Fact]
    [Trait("Category", "Unit")]
    public async Task HasApplicationAccessAsync_returns_true_for_active_application_access_record()
    {
        var store = new InMemoryIdentityStore();
        var provider = new CurrentUserProvider(store);
        var identity = new AuthenticatedIdentity("oidc", "subject-1", null, null);
        var currentUser = await provider.GetCurrentUserAsync(
            identity,
            CancellationToken.None);
        await store.UpsertApplicationAccessAsync(
            new ApplicationAccessRecord(Guid.NewGuid(), currentUser.LocalUserId!.Value),
            CancellationToken.None);
        var authorizer = new ApplicationAccessAuthorizer(provider);

        bool hasAccess = await authorizer.HasApplicationAccessAsync(
            identity,
            CancellationToken.None);

        hasAccess.ShouldBeTrue();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task HasApplicationAccessAsync_returns_false_without_application_access_record()
    {
        var provider = new CurrentUserProvider(new InMemoryIdentityStore());
        var authorizer = new ApplicationAccessAuthorizer(provider);

        bool hasAccess = await authorizer.HasApplicationAccessAsync(
            new AuthenticatedIdentity("oidc", "subject-1", null, null),
            CancellationToken.None);

        hasAccess.ShouldBeFalse();
    }
}
