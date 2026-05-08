## ADDED Requirements

### Requirement: Minimal Auth Foundation

The backend MUST support the minimal Host authentication foundation needed for authenticated browser/API behavior while full OIDC challenge/callback/logout mechanics and Redis-backed session ticket storage remain deferred until accepted scope states otherwise.

#### Scenario: Backend API authentication is verified

- **WHEN** backend authentication behavior is tested before full OIDC session
  mechanics exist
- **THEN** the Host provides only the minimal authentication foundation needed
  for API verification

### Requirement: Temporary Header Auth

Any custom request-header authentication MUST be temporary backend verification scaffolding and MUST NOT be treated as production authentication, response state, or an optimization for avoiding repeated current-user requests.

#### Scenario: Production auth mechanics are introduced

- **WHEN** real Host authentication/session mechanics are accepted and
  implemented
- **THEN** custom request-header authentication scaffolding is removed

### Requirement: API Authentication Failures

API endpoints that require authentication MUST return `401 Unauthorized` for unauthenticated, missing, invalid, expired, or unmappable authentication contexts and MUST NOT redirect API requests to browser login pages.

#### Scenario: API request has no authenticated session

- **WHEN** an unauthenticated caller requests an API endpoint requiring
  authentication
- **THEN** the response is `401 Unauthorized` using standard API error
  semantics without a browser redirect

### Requirement: Provider-Neutral Authentication Boundary

Host authentication mechanics MUST remain separate from Identity application state by translating request claims into provider-neutral authenticated identity data before Identity resolves local user and application access state.

#### Scenario: Current user is resolved

- **WHEN** a request contains an authenticated principal
- **THEN** Host extracts provider-neutral identity data and Identity resolves
  local application state from that data
