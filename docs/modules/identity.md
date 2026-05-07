# Identity Module

Identity owns template-level local identity and application authorization
behavior.

Identity owns:

- Local user identity records mapped to OIDC subjects.
- Lazy local user creation after successful authentication.
- Application-owned staff/admin authorization records.
- Current-user context contracts.
- Initial admin bootstrap records.

The Host owns OIDC challenge/callback/logout mechanics, cookie configuration,
Redis-backed session ticket storage, and API authentication behavior.

Identity-provider roles, groups, and organization membership are not
authoritative product authorization sources.

## Current Implementation Slice

Identity now defines provider-neutral current-user contracts, local user
entities, application access records, and a narrow persistence store. A valid
authenticated principal with a stable provider subject lazily creates or updates
one local user by `(provider, subject)`.

Application access is represented by active application-owned records. Missing
or inactive records mean the user is authenticated without product access; they
do not become unauthenticated. A minimal bootstrapper can seed one configured
provider/subject pair with active application access and is intentionally
idempotent.

The Host owns claim parsing. Identity current-user behavior consumes a
provider-neutral authenticated identity value and resolves local user plus
application access state from application-owned data.

`IIdentityStore` is the current narrow persistence boundary for the accepted
identity behavior.
