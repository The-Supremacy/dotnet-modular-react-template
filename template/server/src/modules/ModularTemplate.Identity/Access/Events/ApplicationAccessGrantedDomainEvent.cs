using ModularTemplate.SharedKernel.Domain;

namespace ModularTemplate.Identity.Access.Events;

[DomainEventType(
    "identity.application-access-granted",
    "identity.application-access",
    1)]
public sealed record ApplicationAccessGrantedDomainEvent(
    Guid ApplicationAccessId,
    Guid LocalUserId) : DomainEvent;
