# Orchestration

Aspire orchestration lives here.

Current project:

- `ModularTemplate.Orchestration`

The app host currently composes PostgreSQL, Redis session-ticket storage,
Keycloak local OIDC, the Migrator, the Host API, and the admin and web Vite
apps. The frontend resources receive `VITE_HOST_ORIGIN` from the Host HTTP
endpoint so their local `/api/` and `/auth/` proxy routes target the Host.

Mailpit remains deferred until its feature gate.
