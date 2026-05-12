using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using ModularTemplate.Host.Tests.Authentication;
using ModularTemplate.Host.Tests.Support;
using ModularTemplate.Identity.Contracts.CurrentUser;
using NSubstitute;
using Shouldly;

namespace ModularTemplate.Host.Tests.CurrentUser;

public sealed class GetMeEndpointTests
{
    [Fact]
    [Trait("Category", "Application")]
    public async Task GetMe_WhenUserHasApplicationAccess_ReturnsAuthenticatedCurrentUser()
    {
        var currentUserProvider = Substitute.For<ICurrentUserProvider>();
        currentUserProvider.GetCurrentUserAsync(
                Arg.Is<AuthenticatedIdentity>(x => x.Provider == "test" && x.Subject == "subject-1"),
                Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(new CurrentUserContext(
                true,
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Ada",
                "ada@example.test",
                true));

        await using var factory = new HostApplicationFactory(services =>
        {
            services.AddScoped(_ => currentUserProvider);
        });
        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/me");
        request.Headers.Add(TestAuthenticationHandler.SubjectHeader, "subject-1");

        HttpResponseMessage response = await client.SendAsync(
            request,
            CancellationToken.None);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<GetMeResponseBody>(
            cancellationToken: CancellationToken.None);
        content.ShouldNotBeNull();
        content.IsAuthenticated.ShouldBeTrue();
        content.User.Id.ShouldBe("11111111-1111-1111-1111-111111111111");
        content.ApplicationAccess.HasAccess.ShouldBeTrue();
    }

    private sealed record GetMeResponseBody(
        bool IsAuthenticated,
        UserBody User,
        ApplicationAccessBody ApplicationAccess);

    private sealed record UserBody(string Id, string? DisplayName, string? Email);

    private sealed record ApplicationAccessBody(bool HasAccess);
}
