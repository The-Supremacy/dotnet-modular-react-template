## ADDED Requirements

### Requirement: Identity Persistence Scope

Identity persistence model, mappings, stores, and narrow DbContext surface needed for local user identity and application access records MUST remain in scope for the accepted current-user slice.

#### Scenario: Current-user persistence is implemented

- **WHEN** Identity resolves local user identity or application access state
- **THEN** the behavior uses Identity-owned persistence model and narrow
  DbContext boundaries

### Requirement: Generated Migrations Deferred

Generated migrations MUST NOT be added for the accepted current-user slice unless a later accepted OpenSpec change or durable architecture decision explicitly brings them into scope.

#### Scenario: Identity mappings are added

- **WHEN** Identity persistence mappings exist for local users and application
  access records
- **THEN** generated EF migrations remain absent unless accepted scope changes

### Requirement: Local User Uniqueness

The persistence model MUST enforce or preserve unique local user identity mapping by `(provider, subject)`.

#### Scenario: Same provider subject is persisted twice

- **WHEN** Identity persistence handles repeated resolution for the same
  provider and subject
- **THEN** one local user identity exists for that pair

### Requirement: Real IO Persistence Verification

Persistence behavior MUST be verified with real external IO for integration coverage when the change exercises database behavior.

#### Scenario: Identity persistence behavior is tested

- **WHEN** verification covers lazy local user creation, duplicate prevention,
  or application access persistence
- **THEN** integration coverage uses real database IO according to repository
  testing guidance
