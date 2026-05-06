# Project Specification Memory

**Revision**: 2026-05-06 — Bootstrapped from first archived feature. [Source: specs/001-auth-session-current-user]

This file captures accepted feature behavior that should inform future Spec Kit
work. The constitution remains the governing source for hard project rules.

## Archived Features

### Authenticated Session And Current User

**Source**: `specs/001-auth-session-current-user`  
**Branch**: `001-auth-session-current-user`  
**Archived**: 2026-05-06

#### User Stories

1. **See Current Application User (P1)**: Authenticated browser/API users with
   application access can call `GET /api/me` and receive stable local
   current-user details plus application access state. [Source:
   specs/001-auth-session-current-user]
2. **Receive API Authentication Failures Without Redirects (P2)**: API callers
   receive `401 Unauthorized` for missing, invalid, expired, or unmappable
   authentication contexts without browser redirects. [Source:
   specs/001-auth-session-current-user]
3. **Distinguish No Application Access From Unauthenticated (P3)**:
   Authenticated users without active application access receive successful
   current-user responses with `hasAccess: false`, while access-protected API
   behavior returns `403 Forbidden`. [Source:
   specs/001-auth-session-current-user]

#### Functional Requirements

- **FR-001**: The backend MUST support the minimal Host authentication
  foundation needed for authenticated browser/API behavior in this slice, while
  full OIDC challenge/callback/logout mechanics and Redis-backed session ticket
  storage remain deferred unless accepted feature artifacts or durable
  architecture decisions bring them into scope. [Source:
  specs/001-auth-session-current-user]
- **FR-002**: API endpoints that require authentication MUST return
  `401 Unauthorized` for unauthenticated, missing, invalid, or expired sessions.
  [Source: specs/001-auth-session-current-user]
- **FR-003**: API endpoints MUST suppress browser login redirects for API
  requests and instead return API-appropriate authentication or authorization
  responses. [Source: specs/001-auth-session-current-user]
- **FR-004**: API endpoints that require application access MUST return
  `403 Forbidden` for authenticated users who lack active application-owned
  access. [Source: specs/001-auth-session-current-user]
- **FR-005**: Identity MUST provide current-user behavior that translates an
  authenticated identity into a stable local application user identity. [Source:
  specs/001-auth-session-current-user]
- **FR-006**: Identity MUST lazily create or resolve a local user identity after
  successful authentication when a stable provider subject is present, including
  users created directly in the external identity provider. [Source:
  specs/001-auth-session-current-user]
- **FR-007**: Identity MUST use application-owned authorization records, not
  identity-provider roles, groups, organizations, or provider-specific claims,
  to determine whether the current user has application access. [Source:
  specs/001-auth-session-current-user]
- **FR-008**: `GET /api/me` MUST be available as the initial current-user API
  behavior for authenticated browser/API use. [Source:
  specs/001-auth-session-current-user]
- **FR-009**: `GET /api/me` MUST return a successful response for an
  authenticated identity that can be mapped to a local user, even when that user
  has no application access. [Source: specs/001-auth-session-current-user]
- **FR-010**: `GET /api/me` MUST distinguish authenticated-with-access and
  authenticated-without-access states through successful responses, and MUST
  distinguish unauthenticated requests through `401 Unauthorized`
  problem-details responses. [Source: specs/001-auth-session-current-user]
- **FR-011**: The `GET /api/me` response MUST include stable local current-user
  details when authenticated and MUST avoid exposing raw provider tokens or
  provider-specific authorization payloads. [Source:
  specs/001-auth-session-current-user]
- **FR-012**: Identity persistence model, mappings, stores, and narrow DbContext
  surface needed for local user identity and application access records are in
  scope for this slice. [Source: specs/001-auth-session-current-user]
- **FR-013**: Generated migrations MUST NOT be added in this slice, even when
  Identity persistence model and mappings are introduced. [Source:
  specs/001-auth-session-current-user]
- **FR-014**: The current-user response MUST remain access-neutral by exposing
  whether application access exists without committing the public API contract
  to product-specific roles or broad provisioning semantics. [Source:
  specs/001-auth-session-current-user]
- **FR-015**: Minimal initial application access/admin bootstrap behavior is in
  scope only as needed to seed or verify application-owned access from
  configuration; it MUST be idempotent, identify one initial access
  subject/provider, create no product-specific role model, and expose no UI/API
  provisioning workflow. [Source: specs/001-auth-session-current-user]
- **FR-016**: This slice MUST NOT include frontend applications, generated
  clients, broad admin provisioning workflows, product-specific roles, tenants,
  organizations, business workflows, durable messaging, outbox processing,
  orchestration resource topology, generated migrations, CI workflows, or
  template automation. [Source: specs/001-auth-session-current-user]
- **FR-017**: Verification MUST cover successful current-user responses,
  unauthenticated API behavior, forbidden API behavior, lazy local user
  creation, duplicate prevention for repeated provider subjects, Identity
  persistence behavior, and the authenticated-without-application-access state.
  [Source: specs/001-auth-session-current-user]
- **FR-018**: Backend tests for this slice MUST follow repository testing
  guidance, including explicit category traits and real external IO for
  integration coverage when persistence behavior is exercised. [Source:
  specs/001-auth-session-current-user]
- **FR-019**: Backend code introduced by this slice MUST follow documented
  server architecture and durable DDD/CQRS guidance before introducing new local
  patterns; deviations such as direct stores instead of repositories/query
  handlers MUST be justified in feature artifacts or stable docs. [Source:
  specs/001-auth-session-current-user]
- **FR-020**: Host API behavior introduced by this slice MUST be organized as a
  vertical feature slice, keeping endpoint handlers, request/response models,
  and feature-local authorization/API composition close together unless a
  shared abstraction is required by another feature. [Source:
  specs/001-auth-session-current-user]
- **FR-021**: Identity domain model introduced by this slice MUST use explicit
  domain terminology for entities, aggregates, repositories/stores, and
  persistence boundaries. [Source: specs/001-auth-session-current-user]
- **FR-022**: Tests SHOULD prefer mocks/substitutes for simple collaborator
  interactions and SHOULD use in-memory fakes only when stateful behavior is
  central to the test; reusable fake stores MUST stay test-only. [Source:
  specs/001-auth-session-current-user]
- **FR-023**: Custom request-header authentication used by this slice MUST be
  documented as temporary scaffolding for backend verification only; it MUST NOT
  be treated as production authentication, response-header state, or an
  optimization for avoiding repeated `GET /api/me` calls, and it MUST be
  removed when real Host authentication/session mechanics are introduced.
  [Source: specs/001-auth-session-current-user]
- **FR-024**: Current-user behavior MUST keep Host authentication mechanics
  separate from Identity application state: Host SHOULD parse
  `ClaimsPrincipal`/claims into a provider-neutral authenticated-identity DTO,
  and Identity SHOULD resolve local user and application-access state from that
  DTO. [Source: specs/001-auth-session-current-user]
- **FR-025**: A direct `IIdentityStore` abstraction MAY remain only as
  transitional scaffolding until repository/query-handler conventions are
  documented and implemented; it is expected to be replaced or reshaped by the
  durable DDD/CQRS persistence pattern. [Source:
  specs/001-auth-session-current-user]

#### Key Entities

- **Authenticated Identity**: Provider-neutral request identity proven by the
  configured identity provider. Fields: provider, subject, displayName, email.
  Subject is required for local user mapping. [Source:
  specs/001-auth-session-current-user]
- **Local User Identity**: Application-owned representation of an authenticated
  person mapped to a unique `(provider, subject)` pair. Fields: id, provider,
  subject, displayName, email, createdAt, lastSeenAt. [Source:
  specs/001-auth-session-current-user]
- **Application Access Record**: Application-owned authorization record for a
  local user. Fields: id, localUserId, isActive, createdAt, disabledAt. Missing,
  inactive, or disabled records mean authenticated without application access.
  [Source: specs/001-auth-session-current-user]
- **Current User**: Request-time application view containing isAuthenticated,
  localUserId, displayName, email, and hasApplicationAccess. [Source:
  specs/001-auth-session-current-user]

#### API Contract

- `GET /api/me` requires authentication and returns `200 OK` with
  `isAuthenticated: true`, `user`, and `applicationAccess.hasAccess` for mapped
  authenticated users. [Source: specs/001-auth-session-current-user]
- Unauthenticated, invalid, expired, or unmappable current-user requests return
  `401 Unauthorized` without browser redirects. [Source:
  specs/001-auth-session-current-user]
- Access-protected API behavior returns `403 Forbidden` for authenticated users
  without active application-owned access. [Source:
  specs/001-auth-session-current-user]

#### Edge Cases

- Missing stable provider subject is an authentication failure, not an ambiguous
  local user creation path. [Source: specs/001-auth-session-current-user]
- Repeated provider subjects resolve the same local user identity. [Source:
  specs/001-auth-session-current-user]
- Provider roles, groups, organizations, and provider-specific claims are not
  authoritative for product authorization. [Source:
  specs/001-auth-session-current-user]
- API authentication and authorization failures must not redirect to browser
  login pages. [Source: specs/001-auth-session-current-user]

#### Success Criteria

- Verification distinguishes unauthenticated, authenticated-with-access, and
  authenticated-without-access states correctly. [Source:
  specs/001-auth-session-current-user]
- Protected API authentication failures return `401`; authenticated users
  without access receive `403` from access-protected behavior. [Source:
  specs/001-auth-session-current-user]
- Repeated current-user requests for the same authenticated identity resolve to
  one local user identity. [Source: specs/001-auth-session-current-user]
- The slice restores, builds, tests, and formats without requiring frontend
  apps, generated clients, broad provisioning workflows, generated migrations,
  Redis ticket storage, or orchestration resources. [Source:
  specs/001-auth-session-current-user]
