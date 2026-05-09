## 1. Bootstrap Script

- [x] 1.1 Add an out-of-place bootstrap script that accepts `--product-name`
      and `--output`.
- [x] 1.2 Implement deterministic name derivation for namespace/project,
      kebab-case slug, snake_case database slug, npm scope, and display name.
- [x] 1.3 Copy the repository to a new output path while excluding git,
      dependency, build, coverage, and template-only planning artifacts.
- [x] 1.4 Rewrite known placeholder text and filesystem paths across backend,
      frontend, orchestration, scripts, docs, tests, and lockfiles.
- [x] 1.5 Fail safely for invalid product names or existing output paths.

## 2. Bootstrap Verification

- [x] 2.1 Add a verification command for generating a temporary product
      repository.
- [x] 2.2 Check that known template placeholders are absent from expected
      generated-repository paths.
- [x] 2.3 Run or document the accepted generated-repository validation commands.

## 3. Documentation

- [x] 3.1 Document bootstrap usage and naming derivation.
- [x] 3.2 Update template decisions and planning docs with the accepted
      rename/bootstrap scope.

## 4. Validation

- [x] 4.1 Validate the OpenSpec change with
      `openspec validate add-template-rename-bootstrap --strict`.
- [x] 4.2 Run formatting and focused bootstrap verification.
