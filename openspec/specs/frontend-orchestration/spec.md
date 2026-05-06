# frontend-orchestration Specification

## Purpose

Defines the accepted local Aspire orchestration behavior for the template's
admin and web Vite apps.

## Requirements

### Requirement: Frontend App Resources

The local Aspire app host MUST include resources for the template's admin and
web Vite development servers.

#### Scenario: Frontend resources are described

- **WHEN** a developer inspects the Aspire app host resource graph
- **THEN** the graph includes one resource for the admin Vite app
- **AND** the graph includes one resource for the web Vite app
- **AND** each resource exposes an HTTP endpoint for local browser access

#### Scenario: Frontend resources use existing app scripts

- **WHEN** the Aspire app host starts frontend resources
- **THEN** the admin resource runs the existing admin frontend development
  script
- **AND** the web resource runs the existing web frontend development script

### Requirement: Host Origin Propagation

The local Aspire app host MUST provide each frontend app the local Host origin
used for Vite proxying of Host-owned API and auth routes.

#### Scenario: Frontend receives Host origin

- **WHEN** either frontend resource starts under Aspire
- **THEN** the resource receives `VITE_HOST_ORIGIN` from the local Host HTTP
  endpoint

#### Scenario: Browser auth boundary is preserved

- **WHEN** frontend apps run through Aspire
- **THEN** browser code still calls same-origin `/api/` and `/auth/` routes
- **AND** browser code does not receive identity-provider access tokens or
  refresh tokens from orchestration configuration

### Requirement: Local Platform Documentation

The template documentation MUST describe the frontend resources as part of the
local Aspire platform once this gate is implemented.

#### Scenario: Local startup docs are read

- **WHEN** a developer reads the local services or orchestration documentation
- **THEN** the docs identify the Host, Migrator, PostgreSQL, Redis, Keycloak,
  admin frontend, and web frontend resources
- **AND** the docs explain how frontend apps receive the Host origin for local
  proxying

### Requirement: Frontend Orchestration Verification

The frontend orchestration change MUST include verification that the app host
and frontend apps remain buildable and testable.

#### Scenario: Orchestration validation runs

- **WHEN** implementation verification runs for this change
- **THEN** the Aspire app host builds successfully
- **AND** frontend typecheck, test, and build validation pass

#### Scenario: Resource graph validation runs

- **WHEN** implementation verification inspects the Aspire resource graph
- **THEN** Host, admin frontend, and web frontend resources are present
- **AND** the frontend resources expose HTTP endpoints
