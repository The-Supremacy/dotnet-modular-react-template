using System.Net;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ModularTemplate.Host.Configuration;
using ModularTemplate.Host.Features.Auth;
using Shouldly;

namespace ModularTemplate.Host.Tests.Authentication;

public sealed class AuthEndpointTests
{
    [Fact]
    [Trait("Category", "Application")]
    public async Task Login_route_starts_oidc_challenge()
    {
        using var app = await CreateAuthHostAsync();
        using var client = app.GetTestClient();

        HttpResponseMessage response = await client.GetAsync(
            "/auth/login?returnUrl=/after-login",
            CancellationToken.None);

        response.StatusCode.ShouldBe(HttpStatusCode.Redirect);
        response.Headers.Location.ShouldNotBeNull();
        response.Headers.Location.AbsoluteUri.ShouldStartWith("http://keycloak.test/authorize");
        response.Headers.Location.Query.ShouldContain("client_id=modular-template-host");
        response.Headers.Location.Query.ShouldContain("redirect_uri=");
    }

    [Fact]
    [Trait("Category", "Application")]
    public async Task Logout_route_starts_cookie_and_oidc_signout()
    {
        using var app = await CreateAuthHostAsync();
        using var client = app.GetTestClient();

        HttpResponseMessage response = await client.PostAsync(
            "/auth/logout",
            null,
            CancellationToken.None);

        response.StatusCode.ShouldBe(HttpStatusCode.Redirect);
        response.Headers.Location.ShouldNotBeNull();
        response.Headers.Location.AbsoluteUri.ShouldStartWith("http://keycloak.test/logout");
    }

    private static async Task<WebApplication> CreateAuthHostAsync()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Configuration["Authentication:Oidc:Authority"] =
            "http://localhost:8080/realms/modular-template";
        builder.AddHostAuthentication();
        builder.Services.RemoveAll<IDistributedCache>();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.PostConfigure<OpenIdConnectOptions>(
            HostAuthenticationConfiguration.OpenIdConnectScheme,
            options =>
            {
                var configuration = new OpenIdConnectConfiguration
                {
                    AuthorizationEndpoint = "http://keycloak.test/authorize",
                    EndSessionEndpoint = "http://keycloak.test/logout",
                };
                options.Configuration = configuration;
                options.ConfigurationManager =
                    new StaticConfigurationManager<OpenIdConnectConfiguration>(configuration);
            });

        var app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapAuthEndpoints();
        await app.StartAsync(CancellationToken.None);

        return app;
    }
}
