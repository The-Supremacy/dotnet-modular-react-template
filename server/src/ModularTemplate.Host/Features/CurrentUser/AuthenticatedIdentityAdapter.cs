using System.Security.Claims;
using ModularTemplate.Identity.Contracts.CurrentUser;

namespace ModularTemplate.Host.Features.CurrentUser;

public static class AuthenticatedIdentityAdapter
{
    private const string ProviderClaimType = "provider";

    public static AuthenticatedIdentity? FromClaimsPrincipal(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        string? subject = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(subject))
        {
            return null;
        }

        return new AuthenticatedIdentity(
            principal.FindFirst(ProviderClaimType)?.Value ?? "default",
            subject,
            principal.FindFirst(ClaimTypes.Name)?.Value,
            principal.FindFirst(ClaimTypes.Email)?.Value);
    }
}
