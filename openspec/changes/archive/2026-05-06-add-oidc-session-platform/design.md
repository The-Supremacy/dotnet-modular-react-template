## Context

The template already has Identity-owned current-user behavior, local user creation, application-owned access records, Host API error handling, and tests around `GET /api/me`. Full Host OIDC, cookie sessions, Redis ticket storage, and Aspire resources are still deferred. The next frontend and generated-client work should sit on the real BFF-style browser session model, not on temporary backend verification scaffolding.

The durable boundary remains unchanged: Host owns authentication mechanics, Identity owns local identity and application access decisions, and browser code never receives or stores identity-provider tokens.

## Goals / Non-Goals

**Goals:**

- Configure Host-owned OpenID Connect and cookie authentication for browser sessions.
- Store authentication tickets and token material server-side in Redis.
- Add login, callback, and logout behavior suitable for same-origin browser apps.
- Preserve API `401` and `403` semantics without browser redirects.
- Add local Aspire orchestration for PostgreSQL, Redis, Keycloak, Migrator, and Host.
- Keep local identity-provider configuration deterministic enough for repeatable development and tests.

**Non-Goals:**

- Do not add frontend apps, route guards, or generated clients in this change.
- Do not add identity-provider Admin API provisioning or invitation workflows.
- Do not introduce product-specific roles, organizations, tenants, or domain permissions.
- Do not add generated EF migrations unless a later accepted task explicitly includes them.
- Do not add durable intermodule messaging, outbox processing, or CI automation.

## Decisions

### Host owns OIDC and cookie session mechanics

Use ASP.NET Core authentication in the Host with cookie authentication as the application session scheme and OpenID Connect as the challenge/sign-out scheme.

Rationale: auth/session plumbing is platform behavior. Identity should continue receiving provider-neutral authenticated identity values instead of `ClaimsPrincipal`, OIDC handler options, provider SDK types, or token payloads.

Alternative considered: create a separate Auth module. Rejected for this gate because it would blur the current boundary and add a module with no domain-owned data beyond Identity's existing responsibilities.

### Redis backs cookie ticket storage

Store authentication tickets server-side through a Redis-backed ticket store. The browser cookie should contain only an opaque session reference, not serialized OIDC tokens.

Rationale: the template targets BFF-style browser apps where frontend code calls same-origin APIs and never stores provider access or refresh tokens.

Alternative considered: default encrypted cookie ticket storage. Rejected for the template baseline because it makes token material browser-carried and conflicts with the accepted Redis session-ticket decision.

Start with a minimal Host-owned ticket-store implementation. Extract a reusable platform abstraction later only if additional session behavior needs it.

### API requests never redirect to login or access-denied pages

Cookie and OIDC events must suppress redirects for API routes and return `401` for unauthenticated API calls and `403` for authenticated callers without application access.

Rationale: frontend code and generated clients need machine-readable API semantics. Browser navigation can still start login explicitly through Host login routes.

Alternative considered: rely on ASP.NET Core default cookie redirects. Rejected because defaults are browser-page oriented and would break API behavior.

### Keycloak is local OIDC only

Use Keycloak as the local development identity provider and treat roles, groups, organizations, and provider-specific claims as non-authoritative diagnostics.

Rationale: Keycloak is a replaceable identity provider for local development. Product authorization must remain application-owned.

Alternative considered: map Keycloak roles or groups to application access. Rejected by governance and existing Identity specs.

Use a checked-in realm import JSON with the Aspire Keycloak integration so local realm/client setup is repeatable without manual console steps.

### Aspire introduces only the local platform resources needed for this slice

Add Aspire orchestration for PostgreSQL, Redis, Keycloak, Migrator, and Host. Mailpit and frontend Vite apps remain deferred until their own gates.

Rationale: this keeps the auth/session slice reviewable while still making the new Host behavior runnable locally.

Alternative considered: add the entire planned local topology now. Rejected because frontend apps and mail flows are outside this change.

## Risks / Trade-offs

- Local Keycloak setup can become brittle across container/image changes -> keep realm/client configuration documented and deterministic, and prefer checked-in development configuration over manual console steps.
- Redis ticket storage adds a local dependency before frontend work exists -> include Aspire wiring and focused Host tests so the dependency is visible and repeatable.
- OIDC integration tests can be expensive or flaky -> unit-test Host option wiring and API redirect behavior first; reserve full browser/provider smoke coverage for the later e2e gate.
- Provider claim mapping can accidentally leak provider assumptions into Identity -> keep claim parsing in Host and assert Identity contracts remain provider-neutral.

## Migration Plan

1. Add central package versions and Host configuration for cookie, OIDC, distributed Redis cache, and ticket storage.
2. Add login/logout endpoint mapping and API redirect suppression.
3. Add Aspire resources and local configuration for Host, Migrator, PostgreSQL, Redis, and Keycloak.
4. Update docs to describe local auth/session startup, reset behavior, and the temporary nature of test-only header authentication.
5. Run restore, build, formatting checks, and focused server tests.

Rollback is straightforward before this template is consumed: remove the change implementation and keep the accepted Gate 7 behavior. After archive, future changes should modify the accepted auth/session and local platform specs instead of reintroducing header-based production auth.

## Open Questions

- None for the proposal. Implementation may still adjust exact package names or option shapes to match the current Aspire and ASP.NET Core APIs.
