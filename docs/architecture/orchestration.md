# Orchestration Architecture

The local platform target uses Aspire as the development entrypoint.

Expected local resources include:

- Host API
- Migrator
- PostgreSQL
- Redis for BFF session tickets
- Keycloak for local OIDC authentication
- Mailpit, deferred until mail workflows exist
- Vite frontend apps, deferred until frontend gates

Orchestration lives under the top-level `orchestration/` folder.

The current Aspire app host is
`orchestration/ModularTemplate.Orchestration/ModularTemplate.Orchestration.csproj`.
It defines PostgreSQL, Redis, Keycloak, Migrator, and Host resources. The Host
waits for the Migrator to complete before starting.

Keycloak uses a checked-in realm import JSON for deterministic local OIDC
client configuration. Persistent local service volumes are intentionally not
enabled in the initial topology; reset behavior should stay cheap while the
template is still under construction.
