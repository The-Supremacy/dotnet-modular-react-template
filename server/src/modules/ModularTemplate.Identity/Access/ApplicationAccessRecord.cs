namespace ModularTemplate.Identity.Access;

public sealed class ApplicationAccessRecord
{
    public ApplicationAccessRecord(Guid id, Guid localUserId)
    {
        Id = id;
        LocalUserId = localUserId;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    private ApplicationAccessRecord()
    {
    }

    public Guid Id { get; private set; }

    public Guid LocalUserId { get; private set; }

    public bool IsActive { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? DisabledAt { get; private set; }

    public void Disable()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        DisabledAt = DateTimeOffset.UtcNow;
    }
}
