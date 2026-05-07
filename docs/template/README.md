# Template Plan

This page tracks the remaining template lanes at a durable but lightweight
level. Accepted runtime behavior lives under `openspec/specs/`, proposed
behavior starts under `openspec/changes/`, and template-building decisions that
should not become inherited product ADR history live in
[template-decisions.md](template-decisions.md).

## Current Foundation

Accepted behavior currently covers:

- Host-owned OIDC browser authentication and Redis-backed session tickets.
- Provider-neutral current-user and application-access behavior.
- `GET /api/me` response semantics.
- Local Keycloak, Redis, PostgreSQL, Migrator, Host, admin frontend, and web
  frontend orchestration.
- Admin and web React app shells with shared browser-safe auth helpers.
- Frontend loading, unauthenticated, no-access, has-access, and error states.
- Same-origin frontend `/api/` and `/auth/` proxying for local development.
- Generated Host API client package consumed by frontend auth helpers.
- Domain-neutral browser-session smoke surface in both frontend apps.
- MVP 1 query helper decision: keep app-facing TanStack Query composition in
  `web/packages/auth` and defer Hey API generated query helpers.
- Default CI verification for backend, frontend, generated-client, and
  OpenSpec checks.
- Out-of-place template rename/bootstrap automation with focused and full
  generated-repository verification.

## MVP 1 Remainder

### Technical Cleanup And Verification

These changes can be handled directly when they do not alter runtime behavior:

- Documentation freshness and broken reference cleanup.
- Formatting, restore, build, test, lint, OpenSpec validation, and generated
  client drift fixes.
- Verification-only changes that do not add new behavior.
- Code review and feedback follow-ups when the finding is purely technical or
  documentation-only.

Recommended MVP 1 verification entrypoints:

- `dotnet test ModularTemplate.slnx`
- `pnpm frontend:typecheck`
- `pnpm frontend:test`
- `pnpm frontend:build`
- `pnpm frontend:lint`
- `pnpm api-client:check`
- `pnpm template:verify`
- `openspec validate --all --strict`

The default CI workflow now runs these checks for pull requests and pushes to
`main`, except bootstrap verification, which remains a local/template
maintenance check for now.

These lanes need accepted artifacts before implementation:

- Template maintenance checks or skills.
- Dependency automation such as Dependabot or Release Please.
- Generated TanStack Query helpers from Hey API, after additional Host API
  operations prove the app-facing query shape.
- Shared UI package conventions beyond simple scaffolding.
- Generated migrations.
- Mailpit or other local service resources.
- Durable intermodule messaging and outbox processing.

## Suggested Next Gates

Review checkpoint on 2026-05-07: the browser-session smoke surface and MVP 1
query helper decision are implemented through OpenSpec change
`add-browser-session-smoke-and-query-decision`.

The next gates are:

1. Run the final MVP 1 review and verification pass.
2. Add template maintenance checks only after a dedicated scope is accepted.

Generated migrations, generated TanStack Query helpers, shared UI conventions,
Mailpit, durable messaging/outbox processing, and identity-provider Admin API
provisioning remain deferred until a concrete accepted scope needs them.
