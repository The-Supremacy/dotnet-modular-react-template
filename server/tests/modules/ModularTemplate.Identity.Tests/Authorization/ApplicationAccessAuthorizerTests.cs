using NSubstitute;
using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Authorization;
using ModularTemplate.Identity.Contracts.CurrentUser;
using ModularTemplate.Identity.Tests.Support;
using Shouldly;

namespace ModularTemplate.Identity.Tests.Authorization;

public sealed class ApplicationAccessAuthorizerTests
{
    [Fact]
    [Trait("Category", "Unit")]
    public async Task HasApplicationAccessAsync_WhenAccessIsActive_ReturnsTrue()
    {
        var provider = Substitute.For<ICurrentUserProvider>();
        var identity = new AuthenticatedIdentity("oidc", "subject-1", null, null);
        provider.GetCurrentUserAsync(identity, CancellationToken.None)
            .Returns(new CurrentUserContext(true, Guid.NewGuid(), null, null, true));
        var authorizer = new ApplicationAccessAuthorizer(provider);

        bool hasAccess = await authorizer.HasApplicationAccessAsync(
            identity,
            CancellationToken.None);

        hasAccess.ShouldBeTrue();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task HasApplicationAccessAsync_WhenAccessIsMissing_ReturnsFalse()
    {
        var provider = Substitute.For<ICurrentUserProvider>();
        var identity = new AuthenticatedIdentity("oidc", "subject-1", null, null);
        provider.GetCurrentUserAsync(identity, CancellationToken.None)
            .Returns(new CurrentUserContext(true, Guid.NewGuid(), null, null, false));
        var authorizer = new ApplicationAccessAuthorizer(provider);

        bool hasAccess = await authorizer.HasApplicationAccessAsync(
            identity,
            CancellationToken.None);

        hasAccess.ShouldBeFalse();
    }
}
