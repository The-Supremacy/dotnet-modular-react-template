# Implementation Plan: Authenticated Session And Current User

**Branch**: `[001-auth-session-current-user]` | **Date**: 2026-05-06 | **Spec**: [spec.md](spec.md)
**Propagated**: 2026-05-06 — Updated from spec.md refinement
**Propagated**: 2026-05-06 — Propagated transitional auth-header removal, Host/Identity current-user split, vertical-slice, and Identity store replacement requirements.
**Input**: Feature specification from `specs/001-auth-session-current-user/spec.md`

## Summary

Establish the first backend auth/session behavior slice for authenticated browser/API behavior and Identity current-user context. The plan keeps a minimal Host authentication foundation in scope, defers full OIDC browser flow and Redis-backed ticket storage, adds Identity persistence model/mappings/stores without generated migrations, and exposes `GET /api/me` as the first observable current-user API contract. Unauthenticated API calls return `401`, authenticated users without application access are distinguishable from signed-out users through a successful current-user response, and access-protected API behavior returns `403`.

Transitional standing: custom request-header authentication exists only as backend verification scaffolding and should be removed when real Host auth/session mechanics are introduced. Current-user behavior should split Host claim parsing from Identity local-user/access resolution. The direct `IIdentityStore` abstraction is also transitional and should be replaced or reshaped once repository/query-handler conventions are documented for the template.

## Technical Context

Repository context loaded from `.specify/memory/constitution.md`, `docs/architecture.md`, `docs/architecture/server.md`, `docs/platform/auth-and-authorization.md`, `docs/modules/identity.md`, `docs/testing.md`, and `docs/testing/server.md`.

**Language/Version**: C# on .NET `net10.0`; `global.json` pins SDK `10.0.107` with latest-feature roll-forward  
**Primary Dependencies**: ASP.NET Core Minimal APIs; EF Core `10.0.7`; Npgsql EF Core provider `10.0.1`; Mediator packages already centrally versioned; OpenTelemetry and ServiceDefaults already present  
**Storage**: PostgreSQL through `ModularTemplate.Persistence` and `ModularTemplateDbContext`; Identity persistence model/mappings/stores and narrow DbContext surface are in scope; generated migrations and Redis-backed session ticket storage remain deferred  
**Testing**: xUnit, Shouldly, NSubstitute, Testcontainers PostgreSQL; every backend test uses explicit category traits per `docs/testing/server.md`  
**Target Platform**: Backend web service in the existing modular-monolith solution  
**Project Type**: Backend/API behavior slice for a domain-neutral modular monolith  
**Performance Goals**: No performance target is introduced for this slice  
**Constraints**: Preserve Host/Identity responsibility split; product authorization must be application-owned and provider-neutral; API auth failures must not redirect; no frontend apps, generated clients, generated migrations, orchestration resources, CI workflows, outbox/Rebus, product-specific roles, tenants, organizations, or business workflows  
**Scale/Scope**: First backend behavior slice only: minimal Host API authentication foundation, local current-user mapping, Identity persistence model, application access state, `GET /api/me`, and proportionate backend verification
**Transitional Follow-up**: Current custom request-header authentication is not durable production auth; it must be removed when real Host auth/session mechanics land. Current-user resolution should split Host claim parsing into a provider-neutral authenticated identity value before Identity resolves local user/access state. `IIdentityStore` may remain temporarily, but it must be replaced or reshaped by the template's durable DDD/CQRS repository/query-handler conventions.

## Constitution Check

_CHECK: Must pass before Phase 0 research. Re-check after Phase 1 design._

- **Domain-Neutral Template First**: PASS. The plan uses generic local users and neutral application access records only; it does not introduce product workflows, tenants, organizations, provider-bound authorization, or product-specific public roles.
- **Spec Kit Before Runtime Surface**: PASS. This runtime behavior starts from reviewed Spec Kit artifacts, and the plan keeps excluded surfaces deferred.
- **Durable Decisions Live In The Repository**: PASS. Decisions are captured in this plan plus `research.md`, `data-model.md`, `contracts/api-me.md`, and `quickstart.md`.
- **Explicit Modular-Monolith Boundaries**: PASS. Host owns minimal HTTP/API authentication mechanics; Identity owns local identity, application-owned access records, persistence model, stores, and current-user contracts. No module exposes EF entities or provider SDK types in contracts.
- **Verification Scales With Risk**: PASS. The plan requires backend behavior tests for auth/session/current-user flows, plus integration coverage with real IO when Identity persistence behavior is exercised.

**Post-Design Re-check**: PASS with follow-up. Phase 0 and Phase 1 artifacts preserve the refined scope boundaries, keep generated migrations, Redis ticket storage, and orchestration out of scope, and define verification expectations proportionate to auth/session and Identity persistence behavior. The latest refinement adds explicit cleanup and architecture alignment tasks for temporary auth-header scaffolding, Host/Identity current-user separation, vertical-slice organization, and the transitional Identity store.

## Project Structure

### Documentation (this feature)

```text
specs/001-auth-session-current-user/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── api-me.md
├── checklists/
│   └── requirements.md
└── tasks.md
```

### Source Code (repository root)

```text
server/
├── src/
│   ├── ModularTemplate.Host/
│   │   ├── Configuration/
│   │   ├── Features/
│   │   └── Program.cs
│   ├── ModularTemplate.Persistence/
│   │   ├── Configuration/
│   │   └── ModularTemplateDbContext.cs
│   └── modules/
│       ├── ModularTemplate.Identity.Contracts/
│       ├── ModularTemplate.Identity/
│       └── ModularTemplate.Identity.Infrastructure/
└── tests/
    ├── ModularTemplate.Host.Tests/
    └── modules/
        ├── ModularTemplate.Identity.Tests/
        └── ModularTemplate.Identity.Infrastructure.Tests/
```

**Structure Decision**: Use the existing backend modular-monolith structure. Host changes are limited to minimal API authentication foundation and API behavior. Identity Contracts define provider-neutral current-user abstractions. Identity owns application behavior and local user/access model. Identity Infrastructure owns persistence mappings and stores through narrow module DbContext contracts. Test projects may be introduced under `server/tests` because no backend test projects exist yet and this runtime behavior requires proportionate verification.

**Refined Structure Decision**: Host API behavior should move toward vertical feature slices under `ModularTemplate.Host/Features`. The `GET /api/me` response models should be feature-local unless reused elsewhere. Host should own conversion from `ClaimsPrincipal` to a provider-neutral authenticated identity value; Identity should own local-user and application-access resolution from that value.

## Phase 0: Research

Completed in [research.md](research.md).

Key resolved decisions:

- Use the documented .NET 10 backend stack and existing modular-monolith project layout.
- Keep minimal Host authentication mechanics separate from Identity current-user, persistence, and application access behavior.
- Treat custom request-header authentication and direct Identity stores as temporary scaffolding that must be removed or reshaped by later auth/session and DDD/CQRS gates.
- Treat `GET /api/me` as authenticated-only: `401` when signed out, `200` when authenticated with or without application access.
- Keep generated migrations, Redis ticket storage, full OIDC browser flow, frontend apps, generated clients, orchestration resources, CI workflows, and durable messaging out of scope.
- Use backend behavior tests and real-IO integration coverage where Identity persistence behavior is exercised.

## Phase 1: Design & Contracts

Completed artifacts:

- [data-model.md](data-model.md)
- [contracts/api-me.md](contracts/api-me.md)
- [quickstart.md](quickstart.md)

Design notes:

- Local user identity uniqueness is `(provider, subject)`.
- Application access is application-owned and independent from identity-provider authorization payloads.
- Current-user responses expose `hasAccess` without committing the public API contract to product-specific roles or access levels.
- Host should extract provider-neutral authenticated identity data from claims before Identity resolves local user and application access state.
- The direct `IIdentityStore` shape is a transitional persistence boundary until repository/query-handler guidance is propagated.
- Missing, invalid, expired, or unmappable authenticated context results in API `401`.
- Access-protected API behavior returns `403` for authenticated users without active application access.

## Complexity Tracking

No constitution violations were introduced. No complexity exceptions are required.

Temporary scaffolding to retire:

- Custom request-header authentication in Host is temporary verification scaffolding and must not be treated as production authentication.
- Direct `IIdentityStore` usage is transitional until DDD/CQRS repository/query-handler conventions are documented and propagated.
