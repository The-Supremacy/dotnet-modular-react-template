using System.Net;
using ModularTemplate.Host.Tests.Authentication;
using ModularTemplate.Host.Tests.Support;
using Shouldly;

namespace ModularTemplate.Host.Tests.CurrentUser;

public sealed class GetMeAuthenticationFailureTests
{
    [Fact]
    [Trait("Category", "Application")]
    public async Task GetMe_returns_unauthorized_when_provider_subject_is_blank()
    {
        await using var factory = new HostApplicationFactory();
        using var client = factory.CreateClient(new() { AllowAutoRedirect = false });
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/me");
        request.Headers.Add(TestAuthenticationHandler.SubjectHeader, " ");

        HttpResponseMessage response = await client.SendAsync(
            request,
            CancellationToken.None);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        response.Headers.Location.ShouldBeNull();
    }
}
