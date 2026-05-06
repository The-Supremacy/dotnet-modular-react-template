# Research: Authenticated Session And Current User

## Decision: Use the documented backend stack for this slice

The implementation target is the existing .NET 10 backend modular monolith, with ASP.NET Core Minimal APIs in the Host, EF Core with PostgreSQL through the Host-owned persistence project, and the existing Identity Contracts/Module/Infrastructure project split.

**Rationale**: `docs/architecture/server.md`, `Directory.Build.props`, and the current solution already establish this stack and project layout. The constitution requires Spec Kit artifacts to follow canonical repository docs unless the feature explicitly proposes and justifies a change.

**Alternatives considered**:

- Introduce a separate auth service: rejected because the repository is a modular monolith template and durable intermodule/distributed patterns are deferred.
- Add frontend or generated-client scope: rejected because the feature spec explicitly excludes these.

## Decision: Keep minimal authentication mechanics in Host and application access in Identity

Host planning will cover the minimal API authentication foundation needed for this slice, API `401`/`403` behavior, and redirect suppression for API routes. Full OIDC challenge/callback/logout mechanics and Redis-backed server-side ticket storage are deferred. Identity planning will cover local user identity mapping, application-owned access records, current-user contracts, persistence model/mappings/stores, and narrow bootstrap behavior needed to seed or verify application-owned access.

**Rationale**: `docs/platform/auth-and-authorization.md` and `docs/modules/identity.md` define this responsibility split. It preserves modular boundaries and prevents provider roles, groups, organizations, or provider-specific claims from becoming authoritative for product authorization.

**Alternatives considered**:

- Let the Host own user/application access data: rejected because Identity owns authorization data, persistence model, and current-user context.
- Treat identity-provider roles or groups as product access: rejected by the constitution and platform docs.
- Implement full OIDC browser flow and Redis ticket storage now: rejected because the refined spec limits this slice to a minimal Host auth/API foundation.

## Decision: Model `GET /api/me` as an authenticated current-user endpoint

`GET /api/me` will require an authenticated API request. Authenticated users that map to a local user receive a successful response indicating whether they have application access. Unauthenticated requests receive `401 Unauthorized`. Application-access-protected behavior returns `403 Forbidden` for authenticated users without application access.

**Rationale**: This satisfies the spec's distinction between authentication and application access while keeping no-access diagnosis available to authenticated users. It also matches the repository rule that API auth failures do not redirect to browser login.

**Alternatives considered**:

- Return a successful unauthenticated shape from `GET /api/me`: rejected because the spec requires API authentication failures to return `401`.
- Return `403 Forbidden` from `GET /api/me` for authenticated users without access: rejected because the spec requires the endpoint to report authenticated/no-access semantics distinctly.

## Decision: Add Identity persistence model/mappings/stores, but defer generated migrations

The implementation scope includes Identity entities/model, persistence mappings, stores, and narrow DbContext surface needed for local user identity and application access records. Implementation tasks must not create generated migrations unless accepted feature artifacts or durable architecture decisions change the scope.

**Rationale**: The refined spec requires lazy local user creation and duplicate prevention for repeated provider subjects. Those behaviors need real Identity persistence in the module boundary, while generated migrations remain explicitly out of scope.

**Alternatives considered**:

- Include migrations as part of this behavior slice: rejected because the refined spec defers generated migrations.
- Define only contracts with no real persistence: rejected because lazy local user creation and duplicate prevention require stores/mappings to be implementable and verifiable.

## Decision: Verification includes unit/application tests plus integration coverage where real IO is exercised

Plan for backend tests using xUnit, Shouldly, NSubstitute, and Testcontainers when Identity persistence behavior is covered. All tests should use explicit category traits. Baseline verification remains restore, build, and formatting.

**Rationale**: `docs/testing.md` and `docs/testing/server.md` define backend testing expectations, and the constitution requires tests proportionate to auth/session behavior risk.

**Alternatives considered**:

- Only rely on restore/build: rejected because auth/session runtime behavior has a larger blast radius.
- Use in-memory persistence for integration coverage: rejected because server testing guidance requires real IO through Testcontainers for integration tests.
- Require Redis session storage integration in this slice: rejected because Redis-backed ticket storage is deferred.

## Decision: Do not add Aspire topology or CI as part of this plan

Aspire/local orchestration and CI workflows may be noted as future verification context, but this feature plan does not add resources or workflows.

**Rationale**: The spec excludes Aspire topology and CI workflows, and the constitution requires accepted feature artifacts or durable architecture decisions before adding orchestration resources.

**Alternatives considered**:

- Add Redis/PostgreSQL Aspire resources now: rejected as out of scope for this slice.
- Add CI test bands now: rejected because CI workflows are deferred.
