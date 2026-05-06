# Local Services

The template's current local platform uses Aspire and includes:

- PostgreSQL for the Host-owned database.
- Redis for BFF/session ticket storage.
- Keycloak for local OpenID Connect authentication.
- Migrator for applying Host-owned migrations.
- Host API.
- Admin Vite app.
- Web Vite app.

Mailpit remains deferred until mail workflows exist.

## Startup

Start the local platform from the repository root:

```sh
aspire start --apphost orchestration/ModularTemplate.Orchestration/ModularTemplate.Orchestration.csproj --isolated
```

Use `aspire describe` to inspect resource endpoints and status after startup.

## Development Defaults

Keycloak is configured through the checked-in realm import under
`orchestration/ModularTemplate.Orchestration/Realms/`. The local Keycloak
resource is exposed on port `8080`, and the Host uses
`http://localhost:8080/realms/modular-template` as its development OIDC
authority.

The Redis resource is referenced by the Host as
`ConnectionStrings:session-tickets`. The PostgreSQL database resource is
referenced as `ConnectionStrings:modular-template-host`.

The admin and web Vite app resources receive `VITE_HOST_ORIGIN` from the local
Host HTTP endpoint. Their Vite development servers continue to proxy `/api/`
and `/auth/` routes to the Host rather than calling identity-provider endpoints
directly from browser code.

## Reset Behavior

The initial local platform intentionally avoids persistent data volumes. Stopping
and recreating resources may reset PostgreSQL, Redis, and Keycloak state. The
checked-in Keycloak realm import is the source of repeatable local identity
provider client configuration.
