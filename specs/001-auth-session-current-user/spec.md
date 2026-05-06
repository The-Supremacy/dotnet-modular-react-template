# Feature Specification: Authenticated Session And Current User

**Feature Branch**: `[001-auth-session-current-user]`  
**Created**: 2026-05-06  
**Status**: Refined  
**Refined**: 2026-05-06 — Clarified Identity persistence scope, minimal Host auth/session scope, `/api/me` unauthenticated semantics, neutral access response shape, and lazy local user creation.  
**Refined**: 2026-05-06 — Added explicit backend architecture requirements for vertical slices, DDD/CQRS consistency, persistence abstractions, and test-double choices.  
**Refined**: 2026-05-06 — Documented transitional auth-header and Identity store shortcuts, and clarified that Host should extract authenticated identity data before Identity resolves current-user state.  
**Input**: User description: "Create a feature specification for the first backend auth/session and current-user behavior slice. Establish the first backend behavior for authenticated browser sessions and current user context in the domain-neutral modular-monolith template. Scope includes authenticated session behavior for backend/API use, current-user behavior for Identity, GET /api/me behavior and response semantics, unauthenticated and forbidden API behavior, authenticated identities with no application access, and verification expectations. Out of scope: frontend apps, generated clients, Aspire topology except future verification context, broad admin provisioning workflows, product-specific roles, tenants, organizations, business workflows, durable messaging/outbox/Rebus, and template automation."

## User Scenarios & Testing _(mandatory)_

### User Story 1 - See Current Application User (Priority: P1)

As an authenticated browser user with application access, I need the backend to tell me who I am in application terms so browser clients and later modules can make access-aware decisions from one consistent current-user contract.

**Why this priority**: This is the first useful authenticated backend behavior and establishes the boundary between identity-provider authentication and application-owned access.

**Independent Test**: Can be fully tested by presenting an authenticated identity with application access, requesting the current-user endpoint, and verifying that the response identifies the local application user and access state without relying on provider authorization data.

**Acceptance Scenarios**:

1. **Given** an authenticated identity that maps to an application user with access, **When** the user requests `GET /api/me`, **Then** the response indicates the user is authenticated, has application access, and includes stable local current-user details needed by backend clients.
2. **Given** a valid authenticated identity from the configured identity provider that has not previously been seen locally, **When** the user requests `GET /api/me`, **Then** Identity creates or resolves a local user identity for that provider subject and the response reflects no application access unless an application-owned access record already exists.
3. **Given** an authenticated identity with provider roles, groups, or organization claims, **When** current-user access is evaluated, **Then** the response is based on application-owned access records rather than provider authorization claims.

---

### User Story 2 - Receive API Authentication Failures Without Redirects (Priority: P2)

As an API client or browser application calling backend API routes, I need unauthenticated requests to receive clear API responses rather than browser login redirects so failures are predictable and testable.

**Why this priority**: Auth/session behavior is not reliable for clients until unauthenticated API behavior is explicit.

**Independent Test**: Can be tested by calling protected API routes without a valid authenticated session and verifying that the API response is a non-redirect authentication failure.

**Acceptance Scenarios**:

1. **Given** no authenticated browser session, **When** a user requests `GET /api/me`, **Then** the API responds with `401 Unauthorized` and does not redirect to a login page.
2. **Given** an expired, missing, or invalid session, **When** an API route requiring authentication is requested, **Then** the API responds with `401 Unauthorized` using the repository's standard error semantics.
3. **Given** this first behavior slice, **When** API authentication behavior is verified, **Then** the Host provides the minimal authentication foundation needed for backend/API tests without requiring full OIDC challenge/callback/logout flows or Redis-backed ticket storage in this slice.

---

### User Story 3 - Distinguish No Application Access From Unauthenticated (Priority: P3)

As an authenticated person who has proven identity but has not been granted application access, I need the backend to report that state distinctly from being signed out so access can be requested, bootstrapped, or diagnosed later.

**Why this priority**: The template must demonstrate that identity providers prove identity while the application decides product access, without adding broad provisioning workflows in this slice.

**Independent Test**: Can be tested by presenting an authenticated identity with a local user record but no active application access and verifying that `GET /api/me` reports authenticated/no-access semantics while protected application behavior rejects the user as forbidden.

**Acceptance Scenarios**:

1. **Given** an authenticated identity with no application access record, **When** the user requests `GET /api/me`, **Then** the response indicates the user is authenticated but does not have application access.
2. **Given** an authenticated identity with no application access, **When** the user requests an API route that requires application access, **Then** the API responds with `403 Forbidden`.
3. **Given** an authenticated identity whose application access has been removed or disabled, **When** current-user behavior is evaluated, **Then** the user is treated as authenticated without application access.

### Edge Cases

- An authenticated identity is missing the stable provider subject needed to map to a local application user; the API treats the current-user request as an authentication failure rather than creating an ambiguous local user.
- The same provider subject is seen more than once; current-user behavior resolves the same local application user without creating duplicates.
- A session is valid for authentication but the local application access record is absent, inactive, or otherwise not usable; the user remains authenticated but has no application access.
- A forbidden API request uses `403 Forbidden`; an unauthenticated API request uses `401 Unauthorized`; neither case uses browser redirects for API routes.
- Provider-supplied authorization data changes between requests; application access decisions remain based on application-owned records.
- `GET /api/me` distinguishes unauthenticated users through a `401 Unauthorized` problem-details response, not through a successful unauthenticated response body.

## Requirements _(mandatory)_

### Functional Requirements

- **FR-001**: The backend MUST support the minimal Host authentication foundation needed for authenticated browser/API behavior in this slice, while full OIDC challenge/callback/logout mechanics and Redis-backed session ticket storage remain deferred unless accepted feature artifacts or durable architecture decisions bring them into scope.
- **FR-002**: API endpoints that require authentication MUST return `401 Unauthorized` for unauthenticated, missing, invalid, or expired sessions.
- **FR-003**: API endpoints MUST suppress browser login redirects for API requests and instead return API-appropriate authentication or authorization responses.
- **FR-004**: API endpoints that require application access MUST return `403 Forbidden` for authenticated users who lack active application-owned access.
- **FR-005**: Identity MUST provide current-user behavior that translates an authenticated identity into a stable local application user identity.
- **FR-006**: Identity MUST lazily create or resolve a local user identity after successful authentication when a stable provider subject is present, including users created directly in the external identity provider.
- **FR-007**: Identity MUST use application-owned authorization records, not identity-provider roles, groups, organizations, or provider-specific claims, to determine whether the current user has application access.
- **FR-008**: `GET /api/me` MUST be available as the initial current-user API behavior for authenticated browser/API use.
- **FR-009**: `GET /api/me` MUST return a successful response for an authenticated identity that can be mapped to a local user, even when that user has no application access.
- **FR-010**: `GET /api/me` MUST distinguish authenticated-with-access and authenticated-without-access states through successful responses, and MUST distinguish unauthenticated requests through `401 Unauthorized` problem-details responses.
- **FR-011**: The `GET /api/me` response MUST include stable local current-user details when authenticated and MUST avoid exposing raw provider tokens or provider-specific authorization payloads.
- **FR-012**: Identity persistence model, mappings, stores, and narrow DbContext surface needed for local user identity and application access records ARE in scope for this slice.
- **FR-013**: Generated migrations MUST NOT be added in this slice, even when Identity persistence model and mappings are introduced.
- **FR-014**: The current-user response MUST remain access-neutral for this slice by exposing whether application access exists without committing the public API contract to product-specific roles or broad provisioning semantics.
- **FR-015**: Minimal initial application access/admin bootstrap behavior is in scope only as needed to seed or verify application-owned access from configuration; it MUST be idempotent, identify one initial access subject/provider, create no product-specific role model, and expose no UI/API provisioning workflow.
- **FR-016**: The first slice MUST NOT include frontend applications, generated clients, broad admin provisioning workflows, product-specific roles, tenants, organizations, business workflows, durable messaging, outbox processing, orchestration resource topology, generated migrations, CI workflows, or template automation.
- **FR-017**: Verification MUST cover successful current-user responses, unauthenticated API behavior, forbidden API behavior, lazy local user creation, duplicate prevention for repeated provider subjects, Identity persistence behavior, and the authenticated-without-application-access state.
- **FR-018**: Backend tests for this slice MUST follow the repository testing guidance in `docs/testing.md` and `docs/testing/server.md`, including explicit category traits and real external IO for integration coverage when persistence behavior is exercised.
- **FR-019**: Backend code introduced by this slice MUST follow the repository's documented server architecture and any durable DDD/CQRS guidance before introducing new local patterns; deviations such as direct stores instead of repositories/query handlers MUST be justified in the feature artifacts or stable docs.
- **FR-020**: Host API behavior introduced by this slice MUST be organized as a vertical feature slice, keeping endpoint handlers, request/response models, and feature-local authorization/API composition close together unless a shared abstraction is required by another feature.
- **FR-021**: Identity domain model introduced by this slice MUST use explicit domain terminology for entities, aggregates, repositories/stores, and persistence boundaries so future modules can understand whether local users and access records are domain entities, aggregate roots, value objects, or infrastructure DTOs.
- **FR-022**: Tests SHOULD prefer mocks/substitutes for simple collaborator interactions and SHOULD use in-memory fakes only when stateful behavior is central to the test; any reusable fake store introduced by this slice MUST stay test-only and not become a second production persistence model.
- **FR-023**: Any custom request-header authentication used by this slice MUST be documented as temporary scaffolding for backend verification only; it MUST NOT be treated as production authentication, response-header state, or an optimization for avoiding repeated `GET /api/me` calls, and it MUST be removed when real Host authentication/session mechanics are introduced.
- **FR-024**: Current-user behavior MUST keep Host authentication mechanics separate from Identity application state: Host SHOULD parse `ClaimsPrincipal`/claims into a provider-neutral authenticated-identity DTO, and Identity SHOULD resolve local user and application-access state from that DTO rather than depending directly on Host claim shapes.
- **FR-025**: A direct `IIdentityStore` abstraction MAY remain only as transitional scaffolding until repository/query-handler conventions are documented and implemented; the feature artifacts MUST record that it is expected to be replaced or reshaped by the durable DDD/CQRS persistence pattern.

### Key Entities _(include if feature involves data)_

- **Authenticated Identity**: A browser/API caller whose identity has been proven by the configured identity provider and includes a stable provider subject.
- **Local User Identity**: The application-owned representation of a person, mapped to a stable provider subject and used by current-user behavior.
- **Application Access Record**: An application-owned authorization record that determines whether a local user has access to the template application's protected behavior without exposing product-specific roles in the public current-user contract for this slice.
- **Current User**: The request-time application view of the authenticated identity, local user identity, and application access state.

## Success Criteria _(mandatory)_

### Measurable Outcomes

- **SC-001**: In verification, 100% of current-user scenarios distinguish unauthenticated, authenticated-with-access, and authenticated-without-access states correctly.
- **SC-002**: In verification, 100% of unauthenticated API requests to protected routes return `401 Unauthorized` without browser redirects.
- **SC-003**: In verification, 100% of authenticated requests without application access to access-protected behavior return `403 Forbidden`.
- **SC-004**: Repeated current-user requests for the same authenticated identity resolve to one local user identity in verification.
- **SC-005**: Verification demonstrates that a valid external identity with no existing local Identity record is populated locally with no default application access.
- **SC-006**: The first backend auth/session behavior slice can be restored, built, and tested using documented repository commands without requiring frontend apps, generated clients, broad provisioning workflows, generated migrations, Redis ticket storage, or orchestration resources.

## Assumptions

- Architecture and platform responsibilities are governed by `.specify/memory/constitution.md`, `docs/architecture.md`, `docs/architecture/server.md`, `docs/platform/auth-and-authorization.md`, and `docs/modules/identity.md`; this spec references those sources rather than restating every rule.
- DDD, CQRS, vertical-slice organization, and persistence abstraction conventions are expected to live in durable repository documentation and be loaded by future Spec Kit plans through the constitution's repository-context rules.
- Custom request-header authentication and the direct Identity store are temporary scaffolding in this slice, retained only to keep verification possible before full Host auth/session mechanics and durable DDD/CQRS persistence conventions are implemented.
- The first slice defines and persists the minimum Identity data needed to recognize local users and application access, but it does not include broad admin provisioning workflows beyond narrow initial access/bootstrap behavior needed for verification.
- API behavior is the focus; browser login, callback, and logout pages may exist only as needed to support backend session behavior and are not frontend app work.
- Full OIDC browser-flow mechanics, Redis-backed ticket storage, and local orchestration resources are deferred for this slice unless accepted feature artifacts or durable architecture decisions change the scope.
- Generated migrations are deferred for this slice unless accepted feature artifacts or durable architecture decisions change the scope.
