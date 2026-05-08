## ADDED Requirements

### Requirement: Current User Endpoint

The Host MUST expose `GET /api/me` as the initial current-user API behavior for authenticated browser/API use.

#### Scenario: Authenticated user with application access requests current user

- **WHEN** an authenticated identity maps to a local user with active
  application access and requests `GET /api/me`
- **THEN** the response is `200 OK` and includes authenticated current-user
  details with `applicationAccess.hasAccess` set to true

#### Scenario: Authenticated user without application access requests current user

- **WHEN** an authenticated identity maps to a local user without active
  application access and requests `GET /api/me`
- **THEN** the response is `200 OK` and includes authenticated current-user
  details with `applicationAccess.hasAccess` set to false

### Requirement: Current User Unauthenticated Response

`GET /api/me` MUST return `401 Unauthorized` for unauthenticated, invalid, expired, or unmappable authenticated contexts.

#### Scenario: Missing stable subject

- **WHEN** an authenticated context lacks the stable provider subject required
  for local user mapping
- **THEN** `GET /api/me` returns `401 Unauthorized` without creating an
  ambiguous local user

### Requirement: Access-Protected Forbidden Response

API behavior that requires application access MUST return `403 Forbidden` for authenticated users who lack active application-owned access.

#### Scenario: Authenticated user without access calls protected behavior

- **WHEN** an authenticated local user without active application access
  requests API behavior requiring application access
- **THEN** the response is `403 Forbidden`

### Requirement: Current User Response Shape

Successful current-user responses MUST include `isAuthenticated`, local `user` details, and `applicationAccess.hasAccess`, and MUST avoid raw provider tokens, provider authorization payloads, product-specific roles, and broad provisioning semantics.

#### Scenario: Current-user response is serialized

- **WHEN** `GET /api/me` returns a successful response
- **THEN** the response contains only client-safe local user details and neutral
  application access state

### Requirement: Host Feature Slice

Host API behavior introduced for current-user semantics MUST be organized as a vertical feature slice unless a shared abstraction is required by another feature.

#### Scenario: Current-user Host API behavior is changed

- **WHEN** current-user Host API behavior is changed
- **THEN** endpoint handlers, response models, and feature-local composition
  stay close to the current-user feature slice
