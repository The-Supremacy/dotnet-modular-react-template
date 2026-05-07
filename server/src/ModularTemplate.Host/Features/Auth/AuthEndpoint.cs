using Microsoft.AspNetCore.Authentication;
using ModularTemplate.Host.Configuration;

namespace ModularTemplate.Host.Features.Auth;

public static class AuthEndpoint
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/auth/login",
                (string? returnUrl, HttpRequest request) =>
                {
                    return TypedResults.Challenge(
                        new AuthenticationProperties { RedirectUri = returnUrl ?? "/" },
                        [HostAuthenticationConfiguration.OpenIdConnectScheme]);
                })
            .ExcludeFromDescription();

        endpoints.MapPost(
                "/auth/logout",
                () => TypedResults.SignOut(
                    new AuthenticationProperties { RedirectUri = "/" },
                    [
                        HostAuthenticationConfiguration.CookieScheme,
                        HostAuthenticationConfiguration.OpenIdConnectScheme,
                    ]))
            .ExcludeFromDescription();

        return endpoints;
    }
}
