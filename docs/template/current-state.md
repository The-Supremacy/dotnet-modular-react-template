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
  `scripts`, and the former `openspec` placeholder.

Note: `openspec/` was created as a Gate 1 placeholder before the SDD-tooling
pivot. The accepted direction is now Spec Kit with Codex, Archive, and Refine.

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

## Spec Kit Tooling Step

Completed after the devcontainer checkpoint.

Includes:

- `scripts/setup-speckit.sh`.
- `.specify/` initialized with Spec Kit `0.8.5`.
- Codex integration under `.agents/skills`.
- Archive extension pinned to `stn1slv/spec-kit-archive` `v1.0.0`.
- Refine extension pinned to `Quratulain-bilal/spec-kit-refine` `v1.0.0`.
- Initial `.specify/memory/constitution.md`.
- Removal of the obsolete `openspec/` placeholder.

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

In review.

Current standing:

- The slice is governed by `specs/001-auth-session-current-user/`.
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
- Plan and task artifacts may be stale after refinement; propagate the latest
  spec before continuing implementation changes.

## Next Intended Step

Review Gate 6 persistence foundation before proceeding to behavior work.

Recommended next gate: initial auth/session and current-user behavior slice.

Expected preparation:

- Use Spec Kit for the first behavior slice before code changes.
- Start from the backend auth/session context, Identity current-user context,
  and initial `/api/me` behavior.
- Keep application authorization and initial admin bootstrap explicit in the
  spec if they are included in the slice.
- Do not add generated migrations, frontend apps, Aspire resources, outbox/Rebus,
  or broad admin provisioning workflows as part of the first behavior slice.

Spec Kit note: this is the first upcoming scope that should use Spec Kit. Start
with `speckit-specify`, then plan and task artifacts before implementation.

## Fresh Agent Handoff

Start with this file, then read:

- `AGENTS.md`.
- `.specify/memory/constitution.md`.
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
- Spec Kit should not be installed automatically by devcontainer rebuilds.
- The initial constitution governs this template repository. Decide during the
  product-bootstrap work whether generated repositories inherit it unchanged or
  receive a product-specific bootstrap constitution.
- The first behavior gate should use Spec Kit rather than proceeding directly
  from planning notes.
