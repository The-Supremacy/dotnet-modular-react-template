## Purpose

This capability defines the template automation for creating product-named
repositories from the source template.

## Requirements

### Requirement: Product Bootstrap Command

The repository MUST provide a template bootstrap command that creates a
product-named repository copy from the current template source.

#### Scenario: Bootstrap command creates output repository

- **WHEN** a developer runs the bootstrap command with a product name and output
  path
- **THEN** the command creates a renamed repository at the output path
- **AND** the source template checkout remains unchanged

#### Scenario: Existing output is rejected

- **WHEN** the bootstrap command is run with an output path that already exists
- **THEN** the command fails without modifying that output path

### Requirement: Deterministic Name Derivation

The bootstrap command MUST derive all first-version rename values from one
required product name.

#### Scenario: Product name derives naming forms

- **WHEN** the product name is `Acme Desk`
- **THEN** the namespace/project prefix is derived as `AcmeDesk`
- **AND** the slug is derived as `acme-desk`
- **AND** the database slug is derived as `acme_desk`
- **AND** the npm scope is derived as `@acme-desk`
- **AND** the display name is derived as `Acme Desk`

#### Scenario: Empty product name is rejected

- **WHEN** the bootstrap command receives an empty or invalid product name
- **THEN** it fails with a clear validation message

### Requirement: Placeholder Rewrite Scope

The bootstrap command MUST rewrite known template placeholders across code,
configuration, docs, package metadata, and filesystem paths.

#### Scenario: Dotnet surfaces are renamed

- **WHEN** a generated repository is inspected after bootstrap
- **THEN** solution, project, namespace, project-reference, appsettings,
  OpenAPI metadata, and test literal surfaces use the derived product names
  instead of `ModularTemplate` placeholder names

#### Scenario: Frontend surfaces are renamed

- **WHEN** a generated repository is inspected after bootstrap
- **THEN** pnpm package names, workspace imports, frontend labels, package
  scripts, and lockfile references use the derived product names instead of
  `modular-template` placeholder names

#### Scenario: Local platform surfaces are renamed

- **WHEN** a generated repository is inspected after bootstrap
- **THEN** Aspire project/resource references, PostgreSQL database names,
  Keycloak realm/client names, and local OIDC configuration use the derived
  product names instead of template placeholder names

### Requirement: Template Planning Exclusion

Generated product repositories MUST NOT inherit template-only planning history
as product documentation.

#### Scenario: Template planning docs are excluded

- **WHEN** a generated product repository is inspected
- **THEN** `docs/template/` is absent
- **AND** stable docs do not link to removed template-planning documents

### Requirement: Bootstrap Verification

The repository MUST provide verification for the rename/bootstrap path.

#### Scenario: Generated repository verification runs

- **WHEN** bootstrap verification runs for a sample product name
- **THEN** it creates a temporary generated repository
- **AND** it verifies no known template placeholders remain in expected
  generated-repository paths
- **AND** it runs the accepted backend, frontend, generated-client, formatting,
  and OpenSpec checks or reports an explicit environment-dependent skip

### Requirement: Deferred Bootstrap Scope

The first bootstrap implementation MUST NOT introduce a full generator,
interactive wizard, explicit naming overrides, generated migrations, deployment
automation, dependency automation, release automation, or product-specific
domain behavior.

#### Scenario: Deferred scope remains absent

- **WHEN** the bootstrap implementation is inspected
- **THEN** it accepts one required product name rather than separate namespace,
  package, database, realm, or resource override inputs
- **AND** it does not generate EF migrations
- **AND** it does not add deployment, dependency-update, or release automation
- **AND** it does not add product-specific domain behavior
