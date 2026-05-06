# Template Current State

Status: Gate checkpoint for the next implementation session.

This file is planning context for building the template itself. It is not
intended to be inherited as stable product documentation.

## Completed Gates

### Gate 1: Repo Skeleton And Documentation Indexes

Completed.

Includes:

- Root README and AGENTS index.
- Stable documentation indexes under `docs/`.
- Top-level skeleton folders for `server`, `web`, `orchestration`, `deploy`,
  and `scripts`.

### Gate 2: Solution And Repository Infrastructure

Completed.

Includes:

- `ModularTemplate.slnx`.
- `global.json`.
- `Directory.Build.props`.
- `Directory.Packages.props`.
- Root pnpm workspace files.
- Empty .NET project shells for Host, Migrator, ServiceDefaults, SharedKernel,
  Identity contracts/module/infrastructure, and Orchestration.

Gate 2 intentionally does not include domain behavior, persistence,
auth/session plumbing, frontend apps, Aspire resources, CI workflows, generated
migrations, tests, or template automation.

## Last Verified Tool Versions

- .NET SDK: `10.0.203`.
- Node: `v24.15.0`.
- pnpm: `10.33.3`.

## Last Verification Commands

These passed after Gate 6:

```sh
dotnet restore ModularTemplate.slnx
dotnet build ModularTemplate.slnx --no-restore
pnpm format:check
```

## Devcontainer Tooling Step

Completed after Gate 2.

Includes:

- `.devcontainer/devcontainer.json`.
- `.devcontainer/README.md`.

The devcontainer is based on the saved
[devcontainer-baseline.md](devcontainer-baseline.md), with pnpm provided by a
devcontainer feature rather than a post-create script.

This was a tooling checkpoint, not Gate 3.

## OpenSpec Tooling Step

Completed after the first accepted behavior slice.

Includes:

- `scripts/setup-openspec.sh`.
- `openspec/` initialized with OpenSpec `1.3.1`.
- Codex OpenSpec skills under `.codex/skills`.
- Durable governance moved to `docs/governance.md`.
- Accepted current behavior represented under `openspec/specs/`.

This was an SDD tooling checkpoint, not Gate 3.

### Gate 3: SharedKernel Primitives

Completed.

Includes:

- `Entity<TId>` base type with identity equality.
- `AggregateRoot<TId>` base type with pending domain-event collection.
- `IDomainEvent` and `DomainEvent` metadata primitives.
- `ValueObject` structural equality base type.
- `DomainException` base type.

Gate 3 intentionally does not include persistence, EF Core mappings, Mediator
pipeline behavior, outbox storage, module behavior, or generated tests.

### Gate 4: ServiceDefaults

Completed.

Includes:

- `ModularTemplate.ServiceDefaults` extension methods for OpenTelemetry,
  service discovery, default HTTP resilience, health checks, and development
  health endpoints.
- Central package versions for ServiceDefaults dependencies.
- Host calls to `AddServiceDefaults` and `MapDefaultEndpoints`.

Gate 4 intentionally does not include feature endpoints, persistence,
auth/session plumbing, Aspire resource topology, or production health-check
policy.

### Gate 5: Host Foundation

Completed.

Includes:

- Host problem-details registration with trace identifiers.
- Host exception handler and status-code-pages middleware.
- Small Host error-handling configuration extension methods to keep
  `Program.cs` thin.

Gate 5 intentionally does not include feature endpoints, OpenAPI generation,
auth/session plumbing, persistence, module registration behavior, or frontend
integration.

### Gate 6: Persistence Foundation

Completed.

Includes:

- `ModularTemplate.Persistence` Host-owned composition project.
- Concrete EF Core `ModularTemplateDbContext` shell.
- Empty `IIdentityDbContext` module persistence interface to establish the
  narrow module DbContext pattern.
- Shared PostgreSQL persistence registration through `AddPersistence`.
- DI registration that resolves `IIdentityDbContext` to the shared concrete
  DbContext.
- Development connection string key `ConnectionStrings:modular-template-host`
  for the Host-owned database.
- Host calls to `AddPersistence`.
- Migrator wiring that resolves `ModularTemplateDbContext` and calls
  `Database.MigrateAsync`.
- Central package versions for EF Core, EF Core Relational, EF Core Design,
  Npgsql, and required Microsoft.Extensions abstractions.

Gate 6 intentionally does not include entities, EF mappings, generated
migrations, domain-event persistence, transaction pipeline behavior, module
stores, auth/session plumbing, API endpoints, outbox/Rebus, Aspire resource
topology, or frontend integration.

### Gate 7: Auth/Session And Current User Slice

Accepted into OpenSpec current specs.

Current standing:

- Behavioral current specs are `openspec/specs/auth-session/spec.md`,
  `openspec/specs/identity-current-user/spec.md`, and
  `openspec/specs/host-api/spec.md`.
- Technical migration, governance, persistence-scope, and verification-only
  details remain in stable docs or the archived OpenSpec migration change, not
  as active behavior capabilities.
- Custom request-header authentication is temporary backend verification
  scaffolding. It should be removed when real Host auth/session mechanics are
  introduced and must not become production authentication or response state.
- Current-user behavior should split Host claim parsing from Identity
  local-user/application-access resolution. Host should produce a
  provider-neutral authenticated identity value; Identity should resolve
  current-user state from that value.
- The direct `IIdentityStore` abstraction is transitional. It can remain until
  DDD/CQRS persistence conventions are documented and implemented, but it should
  be replaced or reshaped into the durable repository/query-handler pattern.
- Future changes should modify the current OpenSpec capability specs rather
  than historical feature artifacts.

## Next Intended Step

Use OpenSpec for the next substantial behavior or platform slice. Start by
reading `docs/governance.md`, stable architecture/platform/testing docs, and
the current specs under `openspec/specs/`.

## Fresh Agent Handoff

Start with this file, then read:

- `AGENTS.md`.
- `docs/governance.md`.
- `docs/openspec.md`.
- `openspec/specs/`.
- `docs/architecture/server.md`.
- `docs/template/template-decisions.md`.
- `docs/template/implementation-plan.md` for historical planning context only.

Treat this file as the current gate source of truth when it differs from older
planning notes.

Before editing, inspect the dirty worktree and do not revert existing changes.
The current uncommitted change set may include the completed Gate 6 persistence
foundation until it is reviewed and committed.

## Notes

- `.github/skills/aspire/SKILL.md` is an Aspire-installed skill artifact. It was
  not part of Gate 1 or Gate 2 template scaffolding.
- OpenSpec should not be installed automatically by devcontainer rebuilds.
- `docs/governance.md` governs this template repository. Decide during the
  product-bootstrap work whether generated repositories inherit it unchanged or
  receive a product-specific bootstrap governance document.
