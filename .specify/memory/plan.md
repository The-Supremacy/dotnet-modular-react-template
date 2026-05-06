# Project Plan Memory

**Revision**: 2026-05-06 — Bootstrapped from first archived feature. [Source: specs/001-auth-session-current-user]

This file captures accepted implementation architecture and operational lessons
from archived features. The constitution remains the governing source for hard
project rules.

## Current Backend Architecture

### Stack And Dependencies

- C# on .NET `net10.0`; `global.json` pins SDK `10.0.107` with latest-feature
  roll-forward. [Source: specs/001-auth-session-current-user]
- ASP.NET Core Minimal APIs in the Host. [Source:
  specs/001-auth-session-current-user]
- EF Core `10.0.7` with Npgsql EF Core provider `10.0.1`; generated migrations
  remain deferred for the auth/current-user slice. [Source:
  specs/001-auth-session-current-user]
- Backend tests use xUnit, Shouldly, NSubstitute, and Testcontainers PostgreSQL
  for real-IO persistence coverage. [Source:
  specs/001-auth-session-current-user]
- Mediator packages are centrally versioned and remain the durable direction for
  command/query handlers as behavior grows. [Source:
  specs/001-auth-session-current-user]

### Project Structure

```text
server/
├── src/
│   ├── ModularTemplate.Host/
│   │   ├── Authorization/
│   │   ├── Configuration/
│   │   ├── Features/
│   │   │   └── CurrentUser/
│   │   └── Program.cs
│   ├── ModularTemplate.Persistence/
│   ├── ModularTemplate.ServiceDefaults/
│   ├── ModularTemplate.SharedKernel/
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

[Source: specs/001-auth-session-current-user]

### Host Composition

- Host owns HTTP composition, platform authentication mechanics, problem
  details, status-code behavior, shared persistence composition, and
  application-access authorization policy wiring. [Source:
  specs/001-auth-session-current-user]
- Host API behavior should be organized as vertical feature slices under
  `ModularTemplate.Host/Features/{FeatureName}`. The `GET /api/me` endpoint and
  response models live under `Features/CurrentUser`. [Source:
  specs/001-auth-session-current-user]
- Host parses `ClaimsPrincipal` into provider-neutral `AuthenticatedIdentity`;
  Identity resolves local user and application access from that DTO. [Source:
  specs/001-auth-session-current-user]
- Custom request-header authentication is test-only scaffolding and must not be
  treated as production authentication. [Source:
  specs/001-auth-session-current-user]

### Identity Module

- Identity Contracts expose provider-neutral current-user and authorization
  abstractions. They must not expose EF entities, aggregate internals,
  `ClaimsPrincipal`, provider SDK types, or infrastructure details. [Source:
  specs/001-auth-session-current-user]
- Identity owns local user identity, application-owned access records,
  current-user resolution, initial application access bootstrap behavior, and
  provider-neutral authorization decisions. [Source:
  specs/001-auth-session-current-user]
- Identity Infrastructure implements persistence mappings and transitional
  store behavior through narrow module DbContext interfaces. [Source:
  specs/001-auth-session-current-user]
- Direct `IIdentityStore` is transitional scaffolding pending durable DDD/CQRS
  repository/query-handler conventions. [Source:
  specs/001-auth-session-current-user]

### Persistence

- `ModularTemplate.Persistence` is the Host-owned EF Core composition project
  with concrete `ModularTemplateDbContext`. [Source:
  specs/001-auth-session-current-user]
- `ModularTemplateDbContext` implements module-owned narrow DbContext
  interfaces, including Identity's local-user and application-access surface.
  [Source: specs/001-auth-session-current-user]
- Identity local user uniqueness is `(provider, subject)`. [Source:
  specs/001-auth-session-current-user]
- Generated migrations were intentionally not added for this slice. [Source:
  specs/001-auth-session-current-user]

### Routing And API Behavior

- `GET /api/me` is the first current-user API contract. [Source:
  specs/001-auth-session-current-user]
- Successful current-user responses include `isAuthenticated`, `user`, and
  `applicationAccess.hasAccess`. [Source:
  specs/001-auth-session-current-user]
- Unauthenticated, invalid, expired, or unmappable API authentication contexts
  return `401 Unauthorized` without browser redirects. [Source:
  specs/001-auth-session-current-user]
- Authenticated users without active application access receive `403 Forbidden`
  from API behavior that requires application access. [Source:
  specs/001-auth-session-current-user]

### Testing Strategy

- Backend tests use explicit category traits. [Source:
  specs/001-auth-session-current-user]
- Identity persistence behavior is covered with real IO through Testcontainers
  PostgreSQL. [Source: specs/001-auth-session-current-user]
- In-memory fakes are allowed only for stateful behavior central to a test and
  must stay test-only; substitutes are preferred for simple collaborator
  interactions. [Source: specs/001-auth-session-current-user]
- Baseline verification commands:

```sh
dotnet restore ModularTemplate.slnx
dotnet build ModularTemplate.slnx --no-restore
dotnet test ModularTemplate.slnx --no-build
pnpm format:check
```

[Source: specs/001-auth-session-current-user]

## Deferred Work

- Full OIDC challenge/callback/logout mechanics. [Source:
  specs/001-auth-session-current-user]
- Redis-backed server-side ticket storage. [Source:
  specs/001-auth-session-current-user]
- Generated migrations. [Source: specs/001-auth-session-current-user]
- Frontend apps, generated clients, broad admin provisioning workflows,
  product-specific roles, tenants, organizations, business workflows, durable
  messaging, outbox/Rebus, orchestration resource topology, CI workflows, and
  template automation. [Source: specs/001-auth-session-current-user]
