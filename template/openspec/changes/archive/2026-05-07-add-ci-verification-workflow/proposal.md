## Why

The template has repeatable backend, frontend, OpenSpec, and generated-client
validation commands, but no CI workflow that runs them before changes merge.
Adding the first CI workflow makes the accepted MVP 1 surface continuously
verifiable and gives later template automation a stable safety net.

## What Changes

- Add a repository CI workflow for pull requests and pushes to the default
  branch.
- Run the existing validation entrypoints for .NET tests, frontend typecheck,
  frontend tests, frontend build, frontend lint, generated API client drift, and
  OpenSpec validation.
- Keep CI focused on current repository checks and avoid adding deployment,
  release, dependency update, generated migration, or full Aspire/browser smoke
  automation in this change.
- Document the CI validation boundary in stable testing/template docs.

## Capabilities

### New Capabilities

- `template-ci`: Repository CI behavior for validating the template's accepted
  backend, frontend, OpenSpec, and generated-client contracts.

### Modified Capabilities

- None.

## Impact

- GitHub Actions workflow files under `.github/workflows/`.
- Testing documentation under `docs/testing.md` and area testing docs if needed.
- Template planning and decision docs under `docs/template/`.
- No runtime API, authentication, persistence, orchestration, or frontend user
  behavior changes.
