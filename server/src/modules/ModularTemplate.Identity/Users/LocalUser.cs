namespace ModularTemplate.Identity.Users;

public sealed class LocalUser
{
    private LocalUser(
        Guid id,
        string provider,
        string subject,
        string? displayName,
        string? email,
        DateTimeOffset createdAt)
    {
        Id = id;
        Provider = provider;
        Subject = subject;
        DisplayName = displayName;
        Email = email;
        CreatedAt = createdAt;
        LastSeenAt = createdAt;
    }

    private LocalUser()
    {
    }

    public Guid Id { get; private set; }

    public string Provider { get; private set; } = string.Empty;

    public string Subject { get; private set; } = string.Empty;

    public string? DisplayName { get; private set; }

    public string? Email { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset LastSeenAt { get; private set; }

    public static LocalUser Create(
        string provider,
        string subject,
        string? displayName,
        string? email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);
        ArgumentException.ThrowIfNullOrWhiteSpace(subject);

        return new LocalUser(
            Guid.NewGuid(),
            provider,
            subject,
            displayName,
            email,
            DateTimeOffset.UtcNow);
    }

    public void MarkSeen(string? displayName, string? email)
    {
        DisplayName = displayName;
        Email = email;
        LastSeenAt = DateTimeOffset.UtcNow;
    }
}
