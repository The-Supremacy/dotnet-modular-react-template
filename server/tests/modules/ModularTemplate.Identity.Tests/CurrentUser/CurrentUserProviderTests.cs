using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Contracts.CurrentUser;
using ModularTemplate.Identity.CurrentUser;
using ModularTemplate.Identity.Tests.Support;
using Shouldly;

namespace ModularTemplate.Identity.Tests.CurrentUser;

public sealed class CurrentUserProviderTests
{
    [Fact]
    [Trait("Category", "Unit")]
    public async Task GetCurrentUserAsync_creates_local_user_with_no_default_access()
    {
        var store = new InMemoryIdentityStore();
        var provider = new CurrentUserProvider(store);

        CurrentUserContext currentUser = await provider.GetCurrentUserAsync(
            new AuthenticatedIdentity("oidc", "subject-1", "Ada", "ada@example.test"),
            CancellationToken.None);

        currentUser.IsAuthenticated.ShouldBeTrue();
        currentUser.LocalUserId.ShouldNotBeNull();
        currentUser.DisplayName.ShouldBe("Ada");
        currentUser.Email.ShouldBe("ada@example.test");
        currentUser.HasApplicationAccess.ShouldBeFalse();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GetCurrentUserAsync_uses_application_owned_access_records()
    {
        var store = new InMemoryIdentityStore();
        var provider = new CurrentUserProvider(store);
        var identity = new AuthenticatedIdentity("oidc", "subject-1", "Ada", "ada@example.test");
        CurrentUserContext created = await provider.GetCurrentUserAsync(
            identity,
            CancellationToken.None);

        await store.UpsertApplicationAccessAsync(
            new ApplicationAccessRecord(Guid.NewGuid(), created.LocalUserId!.Value),
            CancellationToken.None);

        CurrentUserContext currentUser = await provider.GetCurrentUserAsync(
            identity,
            CancellationToken.None);

        currentUser.HasApplicationAccess.ShouldBeTrue();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GetCurrentUserAsync_returns_unauthenticated_when_subject_is_missing()
    {
        var provider = new CurrentUserProvider(new InMemoryIdentityStore());

        CurrentUserContext currentUser = await provider.GetCurrentUserAsync(
            null,
            CancellationToken.None);

        currentUser.ShouldBe(CurrentUserContext.Unauthenticated);
    }
}
