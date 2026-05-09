## Context

The current Aspire app host composes PostgreSQL, Redis, Keycloak, the Migrator,
and the Host API. The web foundation now includes admin and web Vite apps with
shared auth helpers and local proxy configuration driven by `VITE_HOST_ORIGIN`,
but those browser apps are still launched outside the Aspire resource graph.

This change makes the local platform entrypoint include the browser apps while
preserving the template constraints: no product-specific domain behavior, no
generated clients, no CI workflow, no Mailpit resource, and no production
frontend deployment automation.

## Goals / Non-Goals

**Goals:**

- Add admin and web Vite dev servers as Aspire-managed Vite app resources.
- Expose stable local HTTP endpoints for both browser apps.
- Provide each browser app the Host origin used by its existing Vite proxy.
- Keep frontend app behavior domain-neutral and aligned with the accepted web
  app foundation.
- Update local-platform documentation and verification steps.

**Non-Goals:**

- Adding generated API clients.
- Adding shared UI package conventions.
- Adding Mailpit or mail workflows.
- Adding production frontend publishing, Dockerfiles, or deployment resources.
- Adding CI workflows or template bootstrap automation.

## Decisions

Use Aspire Vite app resources for Vite apps.

Aspire's JavaScript hosting integration provides `AddViteApp` for Vite
applications with Vite-specific development and build behavior. The app host
should use `AddViteApp` with pnpm rather than modeling the frontend dev servers
as generic executable resources.

Let Aspire assign app ports while preserving direct Vite defaults.

The existing Vite configs use `5173` for admin and `5174` for web outside
Aspire. Under Aspire, `AddViteApp` owns endpoint assignment and provides the
port through the `PORT` environment variable. The shared Vite config should
honor `PORT` when present and fall back to the existing per-app defaults.

Pass Host origin through environment.

The frontend config already reads `VITE_HOST_ORIGIN` and proxies `/api` and
`/auth` to that target. The Aspire resources should set that variable from the
Host HTTP endpoint so the browser apps stay free of identity-provider tokens and
continue to communicate through Host-owned BFF routes.

Do not accept a production frontend serving topology yet.

`AddViteApp` models the frontend apps as local Vite development servers in run
mode and as frontend build resources during publish inspection. This change
does not accept a production frontend serving topology, Dockerfile ownership
model, static-file hosting strategy, or deployment workflow because the template
has not accepted those decisions yet.

## Risks / Trade-offs

Package-manager availability -> The app host will depend on `pnpm` being
available in the development environment. Mitigate by documenting the
expectation and using the existing repository package scripts.

Endpoint timing -> Vite apps depend on the Host origin for proxy targets, but
the browser apps can start before a user makes API calls. Mitigate by wiring a
resource dependency on Host where Aspire supports it and by keeping frontend
auth flows resilient to unauthenticated or temporarily unavailable API calls.

Port conflicts -> Stable local ports are developer-friendly but can conflict
with other running worktrees. Mitigate by documenting `aspire start --isolated`
as the preferred local command and relying on Aspire endpoint reporting for the
actual resource URLs.
