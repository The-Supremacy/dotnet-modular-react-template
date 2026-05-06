## MODIFIED Requirements

### Requirement: Current User Endpoint

The Host MUST expose `GET /api/me` as the initial current-user API behavior for authenticated browser/API use, including requests authenticated through the application cookie session.

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

#### Scenario: Cookie-authenticated browser requests current user

- **WHEN** a browser with a valid application session cookie requests `GET /api/me`
- **THEN** the Host maps the session principal to a provider-neutral authenticated identity
- **AND** the response follows the current-user response rules for the resolved local user

### Requirement: Current User Unauthenticated Response

`GET /api/me` MUST return `401 Unauthorized` for unauthenticated, invalid, expired, unmappable, or absent cookie-session authentication contexts.

#### Scenario: Missing stable subject

- **WHEN** an authenticated context lacks the stable provider subject required
  for local user mapping
- **THEN** `GET /api/me` returns `401 Unauthorized` without creating an
  ambiguous local user

#### Scenario: Missing or expired cookie session

- **WHEN** a caller requests `GET /api/me` without a valid application session cookie
- **THEN** `GET /api/me` returns `401 Unauthorized`
- **AND** the response does not redirect to browser login

### Requirement: Access-Protected Forbidden Response

API behavior that requires application access MUST return `403 Forbidden` for authenticated users who lack active application-owned access, including users authenticated through the application cookie session.

#### Scenario: Authenticated user without access calls protected behavior

- **WHEN** an authenticated local user without active application access
  requests API behavior requiring application access
- **THEN** the response is `403 Forbidden`

#### Scenario: Cookie-authenticated user without access calls protected behavior

- **WHEN** a browser with a valid application session cookie but no active application access requests API behavior requiring application access
- **THEN** the response is `403 Forbidden`
- **AND** the response does not redirect to an access-denied page
