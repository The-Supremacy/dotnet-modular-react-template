# auth-session Specification

## Purpose

Defines the accepted backend authentication/session boundaries for API behavior
in the template, including production browser session mechanics and temporary
test-only verification scaffolding.

## Requirements

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

### Requirement: Login Return URL Safety

The Host login route MUST use only root-relative return URLs or absolute return
URLs that match the current request origin, and MUST fall back to `/` for
external or malformed return URLs.

#### Scenario: Login receives external return URL

- **WHEN** a browser starts login with an external return URL
- **THEN** the Host does not persist that external URL as the post-login
  redirect target
- **AND** the post-login redirect target falls back to `/`

#### Scenario: Login receives proxied frontend origin

- **WHEN** a frontend app starts login through its same-origin proxy
- **THEN** the Host may preserve the frontend origin return URL when it matches
  the current request origin

### Requirement: Logout Request Method

The Host logout route MUST use POST for local session logout.

#### Scenario: Browser starts logout

- **WHEN** browser code starts logout
- **THEN** it submits a POST to the same-origin Host logout route
