using ModularTemplate.SharedKernel.Domain;

namespace ModularTemplate.Identity.Users.Events;

[DomainEventType(
    "identity.local-user-created",
    "identity.local-user",
    1)]
public sealed record LocalUserCreatedDomainEvent(
    Guid LocalUserId,
    string Provider,
    string Subject) : DomainEvent;
