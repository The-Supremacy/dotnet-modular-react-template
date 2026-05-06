using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ModularTemplate.Host.Authorization;
using ModularTemplate.Host.Tests.Authentication;
using ModularTemplate.Identity;
using ModularTemplate.Identity.Persistence;
using ModularTemplate.Host.Tests.Support;
using Shouldly;

namespace ModularTemplate.Host.Tests.Authorization;

public sealed class ApplicationAccessPolicyTests
{
    [Fact]
    [Trait("Category", "Application")]
    public async Task Protected_api_returns_forbidden_for_authenticated_user_without_application_access()
    {
        using var host = await CreateHostAsync();
        using var client = host.GetTestClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/protected");
        request.Headers.Add(TestAuthenticationHandler.SubjectHeader, "subject-without-access");

        HttpResponseMessage response = await client.SendAsync(
            request,
            CancellationToken.None);

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        response.Headers.Location.ShouldBeNull();
    }

    private static async Task<WebApplication> CreateHostAsync()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthenticationHandler.Scheme;
                options.DefaultChallengeScheme = TestAuthenticationHandler.Scheme;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                TestAuthenticationHandler.Scheme,
                _ => { });
        builder.Services.AddSingleton<IIdentityStore, HostTestIdentityStore>();
        builder.Services.AddIdentityModule();
        builder.Services.AddApplicationAccessAuthorization();

        var app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapGet("/api/protected", () => Results.Ok())
            .RequireAuthorization(ApplicationAccessPolicyConfiguration.PolicyName);
        await app.StartAsync(CancellationToken.None);

        return app;
    }
}
