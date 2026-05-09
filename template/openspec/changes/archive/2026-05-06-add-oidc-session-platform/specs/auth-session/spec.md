## ADDED Requirements

### Requirement: Production Authentication Composition

The Host MUST use OpenID Connect plus application cookie authentication for runtime browser authentication and MUST keep custom request-header authentication out of production Host composition.

#### Scenario: Host authentication is composed for runtime

- **WHEN** the Host starts outside test-only authentication configuration
- **THEN** runtime authentication is composed from OpenID Connect and application cookie authentication
- **AND** custom request-header authentication is not registered as a production authentication scheme

### Requirement: Server-Side Session Storage

The Host MUST store browser authentication ticket state server-side and MUST NOT require browser code to store identity-provider tokens.

#### Scenario: Browser API code uses authenticated session

- **WHEN** browser code calls same-origin API endpoints after login
- **THEN** authentication is based on the application session cookie
- **AND** browser code does not read, store, or send provider access or refresh tokens directly

## MODIFIED Requirements

### Requirement: API Authentication Failures

API endpoints that require authentication MUST return `401 Unauthorized` for unauthenticated, missing, invalid, expired, unmappable, or absent cookie-session authentication contexts and MUST NOT redirect API requests to browser login pages.

#### Scenario: API request has no authenticated session

- **WHEN** an unauthenticated caller requests an API endpoint requiring
  authentication
- **THEN** the response is `401 Unauthorized` using standard API error
  semantics without a browser redirect

#### Scenario: API request has expired browser session

- **WHEN** a caller requests an API endpoint with an expired or invalid application session cookie
- **THEN** the response is `401 Unauthorized`
- **AND** the response does not redirect to the OpenID Connect login route
