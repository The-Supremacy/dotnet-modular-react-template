## Why

The accepted current-user slice still relies on deferred Host authentication mechanics, which leaves the template with correct Identity semantics but no realistic browser session platform. Adding local OIDC, cookie sessions, and session ticket storage now turns the temporary backend slice into the intended BFF-style foundation before frontend apps and generated clients build on it.

## What Changes

- Add Host-owned OIDC authentication using a local identity provider for development.
- Add secure cookie session behavior with API-safe redirect suppression.
- Store server-side authentication tickets in Redis so browser tokens are not stored in frontend code.
- Add local Aspire orchestration resources for the Host, Migrator, PostgreSQL, Redis, and Keycloak needed by the auth/session flow.
- Add Host login and logout endpoints/routes for browser flows.
- Keep Identity provider-neutral: Identity continues to consume authenticated identity values and application-owned access records, not provider roles, groups, organizations, or raw provider tokens.
- Keep custom request-header authentication as test-only verification scaffolding; it must not be wired into production Host composition.

## Capabilities

### New Capabilities

- `local-oidc-session-platform`: Browser login, callback, logout, cookie session, Redis ticket storage, and local identity-provider orchestration behavior.

### Modified Capabilities

- `auth-session`: Expands the accepted authentication/session boundary from API failure semantics and temporary scaffolding to the real Host-owned OIDC/cookie session behavior.
- `host-api`: Clarifies how API authentication and authorization responses behave when browser OIDC/cookie authentication is active.

## Impact

- Host authentication configuration, login/logout route mapping, cookie/OIDC options, and API redirect behavior.
- Service registration for Redis-backed authentication ticket storage.
- Aspire orchestration for PostgreSQL, Redis, Keycloak, Host, and Migrator.
- Development configuration and documentation for local auth/session setup.
- Server tests for API `401`/`403` semantics, login/logout route behavior, ticket-store wiring, and current-user behavior under cookie-authenticated requests.
