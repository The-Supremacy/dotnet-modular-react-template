namespace ModularTemplate.Identity.Contracts.CurrentUser;

public sealed record AuthenticatedIdentity(
    string Provider,
    string Subject,
    string? DisplayName,
    string? Email);
