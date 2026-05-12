using System.Net;
using System.Net.Http.Json;
using ModularTemplate.Host.Tests.Authentication;
using ModularTemplate.Host.Tests.Support;
using Shouldly;

namespace ModularTemplate.Host.Tests.CurrentUser;

public sealed class GetMeNoAccessTests
{
    [Fact]
    [Trait("Category", "Application")]
    public async Task GetMe_WhenUserLacksApplicationAccess_ReturnsAuthenticatedCurrentUser()
    {
        await using var factory = new HostApplicationFactory();
        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/me");
        request.Headers.Add(TestAuthenticationHandler.ProviderHeader, "oidc");
        request.Headers.Add(TestAuthenticationHandler.SubjectHeader, "subject-without-access");
        request.Headers.Add(TestAuthenticationHandler.DisplayNameHeader, "Ada");
        request.Headers.Add(TestAuthenticationHandler.EmailHeader, "ada@example.test");

        HttpResponseMessage response = await client.SendAsync(
            request,
            CancellationToken.None);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<GetMeResponseBody>(
            cancellationToken: CancellationToken.None);
        content.ShouldNotBeNull();
        content.IsAuthenticated.ShouldBeTrue();
        content.User.DisplayName.ShouldBe("Ada");
        content.User.Email.ShouldBe("ada@example.test");
        content.ApplicationAccess.HasAccess.ShouldBeFalse();
    }

    private sealed record GetMeResponseBody(
        bool IsAuthenticated,
        UserBody User,
        ApplicationAccessBody ApplicationAccess);

    private sealed record UserBody(string Id, string? DisplayName, string? Email);

    private sealed record ApplicationAccessBody(bool HasAccess);
}
