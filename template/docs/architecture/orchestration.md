# Orchestration Architecture

The local platform target uses Aspire as the development entrypoint.

Expected local resources include:

- Host API
- Migrator
- PostgreSQL
- Redis for BFF session tickets
- Keycloak for local OIDC authentication
- Vite frontend apps for the admin and web portals

Orchestration lives under the top-level `orchestration/` folder. App host
composition should keep service dependencies explicit, pass frontend apps only
same-origin Host endpoints, and preserve the Host-owned BFF session boundary.

Local identity-provider configuration should be deterministic and checked in
when products rely on local login smoke tests. Stateful local resources may use
named data volumes, but docs must explain how to reset them.
