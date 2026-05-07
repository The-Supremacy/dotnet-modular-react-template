## ADDED Requirements

### Requirement: Default Verification Workflow

The repository MUST provide a default CI workflow that runs on pull requests
and pushes to the default branch and verifies the accepted MVP 1 template
surface.

#### Scenario: Pull request validation runs

- **WHEN** a pull request targets the default branch
- **THEN** CI runs backend, frontend, generated-client, and OpenSpec validation
  jobs or steps

#### Scenario: Default branch validation runs

- **WHEN** changes are pushed to the default branch
- **THEN** CI runs the same default verification surface used for pull requests

### Requirement: Backend Verification

The default CI workflow MUST run the documented backend verification command
with repository-pinned .NET SDK configuration.

#### Scenario: Backend tests run in CI

- **WHEN** the backend CI job runs
- **THEN** it uses the .NET SDK version behavior from `global.json`
- **AND** it uses the Development ASP.NET Core environment for local template
  configuration
- **AND** it runs `dotnet test ModularTemplate.slnx`

#### Scenario: Integration tests are included

- **WHEN** backend tests include Testcontainers-backed integration tests
- **THEN** the default backend CI job runs on a runner with Docker support

### Requirement: Frontend Verification

The default CI workflow MUST run the documented frontend verification commands
with lockfile-based pnpm installation.

#### Scenario: Frontend workspace validates in CI

- **WHEN** the frontend CI job runs
- **THEN** it installs dependencies from `pnpm-lock.yaml`
- **AND** it runs `pnpm frontend:typecheck`
- **AND** it runs `pnpm frontend:test`
- **AND** it runs `pnpm frontend:build`
- **AND** it runs `pnpm frontend:lint`

### Requirement: Generated Client Drift Verification

The default CI workflow MUST fail when the generated Host API client is stale.

#### Scenario: Generated client check runs in CI

- **WHEN** the frontend CI job runs
- **THEN** it runs `pnpm api-client:check`
- **AND** generated OpenAPI or API client drift fails the workflow

### Requirement: OpenSpec Verification

The default CI workflow MUST validate OpenSpec artifacts.

#### Scenario: OpenSpec validation runs in CI

- **WHEN** the CI workflow runs
- **THEN** it runs `openspec validate --all --strict`

### Requirement: Deferred CI Automation

The first default CI workflow MUST NOT introduce deployment, release,
dependency-update, generated migration, or full Aspire/browser smoke
automation.

#### Scenario: Deferred automation stays out of first workflow

- **WHEN** the CI workflow is inspected
- **THEN** it does not publish deployments or releases
- **AND** it does not configure dependency update automation
- **AND** it does not generate or commit EF migrations
- **AND** it does not start the full Aspire platform or drive browser login
  automation
