# API Contract: Current User

## GET /api/me

Returns the current authenticated user's application context.

### Authentication

Requires an authenticated browser/API session.

### Successful Response: Authenticated With Application Access

Status: `200 OK`

```json
{
  "isAuthenticated": true,
  "user": {
    "id": "local-user-id",
    "displayName": "Example User",
    "email": "user@example.test"
  },
  "applicationAccess": {
    "hasAccess": true
  }
}
```

### Successful Response: Authenticated Without Application Access

Status: `200 OK`

```json
{
  "isAuthenticated": true,
  "user": {
    "id": "local-user-id",
    "displayName": "Example User",
    "email": "user@example.test"
  },
  "applicationAccess": {
    "hasAccess": false
  }
}
```

### Unauthenticated Response

Status: `401 Unauthorized`

Rules:

- Returned when the request has no valid authenticated session.
- Returned when an authenticated identity cannot provide a stable provider subject for local user mapping.
- Must not redirect to a browser login page for API requests.
- Uses the repository's standard problem-details/error semantics.

### Forbidden Response For Access-Protected API Behavior

Status: `403 Forbidden`

Rules:

- Applies to API behavior that requires application access.
- Returned when the request is authenticated but the local user has no active application access.
- Must not redirect to a browser login page for API requests.
- Uses the repository's standard problem-details/error semantics.

## Response Field Semantics

- `isAuthenticated`: always true in successful `GET /api/me` responses because unauthenticated requests receive `401`.
- `user.id`: stable local application user identifier.
- `user.displayName`: optional client-safe display name.
- `user.email`: optional client-safe email.
- `applicationAccess.hasAccess`: true only when an active application-owned access record grants access.

## Non-Exposure Rules

- Responses do not expose raw provider tokens.
- Responses do not expose provider authorization payloads.
- Provider roles, groups, organizations, and provider-specific claims are not authoritative for `applicationAccess`.
- Product-specific roles and access levels are deferred from the public current-user contract for this slice.
