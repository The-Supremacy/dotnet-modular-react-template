# Changelog Memory

**Revision**: 2026-05-06 — Created merged features log. [Source: specs/001-auth-session-current-user]

## Merged Features Log

### Authenticated Session And Current User — 2026-05-06

**Branch:** `001-auth-session-current-user`  
**Spec:** `specs/001-auth-session-current-user`

**What was added:**

- Minimal backend auth/session behavior for API authentication and
  authorization outcomes.
- `GET /api/me` current-user API with authenticated-with-access and
  authenticated-without-access semantics.
- API `401 Unauthorized` behavior without redirects for unauthenticated,
  invalid, expired, or unmappable authentication contexts.
- API `403 Forbidden` behavior for authenticated users without active
  application access on access-protected behavior.
- Identity local user resolution, lazy local user creation, application-owned
  access records, and minimal initial access bootstrap behavior.
- Host/Identity current-user separation using provider-neutral authenticated
  identity data.

**New Components:**

- `ModularTemplate.Persistence` concrete EF Core composition project and
  `ModularTemplateDbContext`.
- Identity Contracts, Identity Module, and Identity Infrastructure current-user
  and persistence behavior.
- Host current-user vertical slice under `ModularTemplate.Host/Features`.
- Backend test projects for Host, Identity, and Identity Infrastructure.

**Tasks Completed:** 61/61 tasks
