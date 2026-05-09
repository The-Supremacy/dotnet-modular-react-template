## Context

The repository already records local validation entrypoints in `docs/testing.md`
and package scripts. Backend tests include Testcontainers-backed PostgreSQL
integration tests, frontend checks run through pnpm workspace scripts, and
generated API client freshness is enforced locally by Husky. Governance requires
accepted scope before CI workflows are introduced.

## Goals / Non-Goals

**Goals:**

- Add the first CI workflow for pull requests and pushes to the default branch.
- Run the same MVP 1 validation commands documented for local verification.
- Use pinned runtime setup from repository files where practical: .NET from
  `global.json` and pnpm from the package-manager toolchain.
- Keep generated API client drift visible in CI.
- Document which checks run in default CI.

**Non-Goals:**

- Add deployment, release publishing, Dependabot, Release Please, or scheduled
  automation.
- Add generated EF migrations or migration-generation checks.
- Add full Aspire/Keycloak browser automation.
- Add provider secrets, production cloud resources, or environment-specific
  deployment configuration.

## Decisions

### Use One Verification Workflow With Backend And Frontend Jobs

Create a single GitHub Actions workflow such as `.github/workflows/verify.yml`
with separate backend and frontend jobs.

Rationale: the first workflow should be easy to inspect and map directly to the
current validation surface. Separate jobs keep .NET and pnpm setup independent
while still making the workflow read as one verification gate.

Alternative considered: split into many workflows. That adds naming and trigger
surface before the template has enough automation variety to justify it.

### Run Full `dotnet test ModularTemplate.slnx`

Run the accepted backend test entrypoint, including integration tests.

Rationale: the current backend validation contract already includes
Testcontainers-backed persistence tests. GitHub-hosted Linux runners provide
Docker, so excluding integration tests would make CI weaker than local MVP
verification.

Alternative considered: run only non-integration test categories by default.
That can be revisited if runtime or flakiness becomes a real issue.

### Keep Browser Platform Smoke Manual For This Gate

Do not start Aspire or automate Keycloak login flows in the first CI workflow.

Rationale: the browser-session smoke surface is covered by component tests and
manual local platform documentation. Full Aspire/browser automation needs a
separate e2e scope that defines platform startup expectations, credentials, and
failure diagnostics.

Alternative considered: add Playwright plus Aspire startup immediately. That
would expand this CI change into orchestration and identity-provider automation.

### Treat Generated Client Freshness As A First-Class Check

Run `pnpm api-client:check` in CI.

Rationale: the generated Host client is an accepted contract between backend
OpenAPI output and frontend browser consumers. Drift should block merges.

Alternative considered: regenerate clients automatically in CI. That hides
source drift instead of making contributors commit the generated result.

## Risks / Trade-offs

- Testcontainers can be slower or fail if Docker is unavailable on a runner ->
  use a Linux hosted runner with Docker support and keep the command explicit.
- CI may duplicate local validation time -> split backend and frontend jobs so
  failures report independently.
- OpenAPI generation may be sensitive to SDK/tooling versions -> use
  `global.json`, lockfile installation, and the existing generation scripts.
- Full browser smoke is still manual -> keep the e2e gap documented and defer
  platform automation to a later accepted change.
