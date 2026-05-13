# Identity Module

Identity owns template-level local identity and application authorization
behavior.

Identity owns:

- Local user aggregates mapped to OIDC subjects.
- Lazy local user creation after successful authentication.
- Application-owned application access aggregates.
- Current-user context contracts.
- Initial admin access grant behavior used by setup tooling.

The Host owns OIDC challenge/callback/logout mechanics, cookie configuration,
Redis-backed session ticket storage, and API authentication behavior.

Identity-provider roles, groups, and organization membership are not
authoritative product authorization sources.

The Host owns claim parsing. Identity current-user behavior consumes a
provider-neutral authenticated identity value and resolves local user plus
application access state from application-owned data.

Implementation progress for the shipped template lives in
[../current-state/identity.md](../current-state/identity.md).
