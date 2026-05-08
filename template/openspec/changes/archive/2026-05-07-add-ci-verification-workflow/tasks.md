## 1. Workflow Implementation

- [x] 1.1 Add a GitHub Actions verification workflow for pull requests and
      pushes to the default branch.
- [x] 1.2 Configure the backend job to use `global.json` and run
      `dotnet test ModularTemplate.slnx` on a Docker-capable Linux runner.
- [x] 1.3 Configure the frontend job to install pnpm dependencies from the
      lockfile and run typecheck, tests, build, lint, API client drift check,
      and OpenSpec validation.

## 2. Documentation

- [x] 2.1 Document the default CI validation surface in testing docs.
- [x] 2.2 Update template planning and decision docs so CI is no longer listed
      as deferred after implementation.

## 3. Validation

- [x] 3.1 Validate the OpenSpec change with
      `openspec validate add-ci-verification-workflow --strict`.
- [x] 3.2 Run or account for the workflow-equivalent local verification
      commands.
