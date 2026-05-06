# Auth And Authorization

Authentication mechanics are a Host/platform responsibility. Identity and
application authorization data are module responsibilities.

Host responsibilities:

- Configure ASP.NET Core authentication.
- Handle OIDC challenge, callback, and logout routes.
- Store server-side session tickets in Redis.
- Ensure unauthenticated API requests return `401`.
- Ensure forbidden API requests return `403`.
- Suppress browser redirects for API endpoints.

Identity module responsibilities:

- Translate authenticated OIDC principals into local user identities.
- Store app-owned authorization records.
- Provide current-user context.
- Bootstrap one initial application admin.

The identity provider proves identity. The application decides product access.

## Current Implementation Slice

The template currently includes a minimal backend API authentication foundation
for `GET /api/me`. Full OIDC browser flow and Redis-backed ticket storage are
still deferred. The Host resolves a request principal, requires authentication
for the current-user endpoint, and exposes application-access authorization as a
Host policy backed by Identity contracts.

API authentication failures return `401` without browser redirects. API
authorization failures for authenticated users without active application-owned
access return `403`.

Custom request-header authentication is not production authentication. It exists
only in backend tests as temporary verification scaffolding while full Host
OIDC/session mechanics remain deferred. It must not be wired into production
Host composition, emitted as response state, or used as a replacement for
calling `GET /api/me`.
