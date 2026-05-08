## ADDED Requirements

### Requirement: Browser OIDC Login

The Host MUST provide browser login behavior that challenges the configured local OpenID Connect provider and establishes an application cookie session after a successful provider callback.

#### Scenario: Browser starts login

- **WHEN** a browser requests the Host login route
- **THEN** the Host starts an OpenID Connect challenge using the configured local identity provider

#### Scenario: Provider callback succeeds

- **WHEN** the local identity provider returns a successful callback with a stable subject
- **THEN** the Host establishes an authenticated application cookie session

### Requirement: Redis-Backed Session Tickets

The Host MUST store authentication ticket state and identity-provider token material server-side in Redis and MUST NOT expose provider access or refresh tokens to browser code.

#### Scenario: Authenticated browser receives session cookie

- **WHEN** the Host completes a successful browser sign-in
- **THEN** the browser receives only an opaque application session cookie
- **AND** provider token material is stored server-side

### Requirement: Browser Logout

The Host MUST provide browser logout behavior that clears the local application session and initiates provider sign-out when provider sign-out is configured.

#### Scenario: Browser logs out

- **WHEN** an authenticated browser requests the Host logout route
- **THEN** the Host clears the local cookie session
- **AND** the Host initiates OpenID Connect sign-out with the configured provider when supported

### Requirement: Local Identity Provider Orchestration

The local platform MUST provide a repeatable OpenID Connect development identity provider configuration for the Host.

#### Scenario: Local platform starts

- **WHEN** the Aspire local platform starts for this template
- **THEN** Keycloak is available to the Host as the configured OpenID Connect provider
- **AND** the Host can resolve its configured authority, client identifier, and callback routes without manual console setup for each run

#### Scenario: Local realm import is configured

- **WHEN** the local Keycloak resource starts through Aspire
- **THEN** Keycloak imports the checked-in template realm/client configuration
- **AND** the imported configuration supports the Host login, callback, and logout routes

### Requirement: Local Auth Platform Resources

The local Aspire platform MUST include the Host, Migrator, PostgreSQL, Redis, and local identity provider resources required by the auth/session flow.

#### Scenario: Auth platform starts locally

- **WHEN** a developer starts the local Aspire platform for this change
- **THEN** PostgreSQL, Redis, Keycloak, Migrator, and Host resources are defined in the app model
- **AND** the Host receives the connection/configuration values needed for persistence, session tickets, and OpenID Connect authentication

### Requirement: Provider-Neutral Identity Boundary

The Host MUST map successful OpenID Connect authentication to provider-neutral authenticated identity values before calling Identity behavior.

#### Scenario: Provider includes authorization claims

- **WHEN** an authenticated provider principal includes roles, groups, organizations, or provider-specific authorization claims
- **THEN** the Host does not treat those claims as product authorization
- **AND** Identity current-user behavior still receives only provider-neutral identity data needed for local user resolution
