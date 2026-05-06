# Template Plan

This page tracks the next template lanes at a durable but lightweight level.
Accepted runtime behavior still lives under `openspec/specs/`, and proposed
behavior starts under `openspec/changes/`.

## Current Foundation

Accepted behavior currently covers:

- Host-owned OIDC browser authentication and Redis-backed session tickets.
- Provider-neutral current-user and application-access behavior.
- `GET /api/me` response semantics.
- Local Keycloak, Redis, PostgreSQL, Migrator, Host, admin frontend, and web
  frontend orchestration.
- Admin and web React app shells with shared browser-safe auth helpers.

## Next Lanes

### Technical Cleanup

These changes can usually be handled directly when they do not alter runtime
behavior:

- Documentation freshness and broken reference cleanup.
- Formatting, restore, build, test, and lint fixes.
- Verification-only changes that do not add new behavior.

### Requires OpenSpec Or Durable Architecture Decision

These lanes need accepted artifacts before implementation:

- Generated API clients.
- Shared UI package conventions beyond simple scaffolding.
- CI workflow definition.
- Generated migrations.
- Template automation for creating product repositories.
- Mailpit or other local service resources.
- Durable intermodule messaging and outbox processing.

## Suggested Next Gate

The next runtime gate should be generated API clients: decide the OpenAPI
generation boundary, package shape, update workflow, and verification that keeps
browser code provider-neutral and same-origin.
