# identity-current-user Specification

## Purpose

Defines Identity-owned current-user behavior, local user resolution, and application-owned access semantics.

## Requirements

### Requirement: Local User Resolution

Identity MUST translate an authenticated identity with a stable provider subject into a stable local application user identity.

#### Scenario: Known provider subject requests current user

- **WHEN** an authenticated identity with a known provider and subject requests
  current-user behavior
- **THEN** Identity resolves the existing local application user identity

### Requirement: Lazy Local User Creation

Identity MUST lazily create a local user identity after successful authentication when a stable provider subject is present and no matching local identity exists.

#### Scenario: New provider subject requests current user

- **WHEN** a valid authenticated identity with a stable provider subject has no
  existing local Identity record
- **THEN** Identity creates one local user identity with no default application
  access

### Requirement: Duplicate Prevention

Identity MUST keep `(provider, subject)` unique and repeated current-user requests for the same provider subject MUST resolve one local user identity.

#### Scenario: Provider subject is seen repeatedly

- **WHEN** current-user behavior runs more than once for the same provider and
  subject
- **THEN** the same local user identity is resolved without duplicates

### Requirement: Application-Owned Access

Identity MUST use application-owned authorization records, not identity-provider roles, groups, organizations, or provider-specific claims, to determine whether the current user has application access.

#### Scenario: Provider authorization claims change

- **WHEN** provider roles, groups, organizations, or provider-specific claims
  change between requests
- **THEN** current-user application access is still based on application-owned
  access records

### Requirement: Authenticated Without Access

Identity MUST distinguish authenticated users without active application access from unauthenticated users.

#### Scenario: Local user has no active application access

- **WHEN** an authenticated local user has no active application access record
- **THEN** current-user behavior reports authenticated-without-access state

### Requirement: Minimal Initial Access Bootstrap

Minimal initial application access behavior MUST be idempotent, identify one
configured provider/subject pair, create no product-specific role model, expose
no UI or API provisioning workflow, and remain usable as an
operator-controlled production bootstrap path.

#### Scenario: Bootstrap runs multiple times

- **WHEN** initial application access bootstrap runs repeatedly for the same
  configured provider and subject
- **THEN** the same application-owned access state exists without duplicate
  access records or product-specific roles

#### Scenario: Bootstrap configuration is absent

- **WHEN** initial application access bootstrap runs without a configured
  provider and subject
- **THEN** no local user or application access state is created
