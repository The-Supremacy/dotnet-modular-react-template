using ModularTemplate.SharedKernel.Domain;

namespace ModularTemplate.Identity.Access.Events;

[DomainEventType(
    "identity.application-access-revoked",
    "identity.application-access",
    1)]
public sealed record ApplicationAccessRevokedDomainEvent(
    Guid ApplicationAccessId,
    Guid LocalUserId) : DomainEvent;
