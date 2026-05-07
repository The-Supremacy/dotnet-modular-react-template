# Template TODO

This file is the temporary planning list for finishing the template. Stable
documentation should describe only implemented behavior. Unaccepted ideas stay
here until they are either implemented or removed.

## Script Direction

Resolved:

- Keep template automation as root `pnpm` scripts.
- Keep template automation internals as Node `.js` files.
- Convert `generate-openapi`, `generate-api-client`, and `check-api-client`
  helpers from shell to Node `.js` scripts.
- Do not package a global npm CLI before repo-local scripts are stable and have
  focused tests.
- Document the one-input product-name naming model in
  `docs/template/template-decisions.md`.

## CI Direction

Resolved:

- Keep the workflow name `Verify`.
- Add explicit backend restore/build/test steps.
- Run only `Unit` and `Application` backend test categories in default CI,
  collect backend coverage through that .NET test pipeline, and upload the
  coverage artifact.
- Move OpenSpec validation into a separate `openspec` job.
- Keep bootstrap verification out of default CI as a local template-maintenance
  check.

## OpenSpec Scope Cleanup

Decision:

- Remove purely technical/automation current specs such as CI and
  rename/bootstrap from `openspec/specs/`.
- Keep archived OpenSpec changes in history unless we intentionally rewrite
  history outside normal repo workflow.
- Use OpenSpec current specs only for accepted behavior contracts that should
  guide future runtime/product-facing work.

Completed cleanup:

- Deleted `openspec/specs/template-ci/`.
- Deleted `openspec/specs/template-rename-bootstrap/`.
- Updated docs so CI/template automation is maintained as technical template
  behavior rather than current OpenSpec capability specs.
- Trimmed `docs/template/template-decisions.md` to decisions that explain
  implemented template behavior.

## Documentation Sweep

Goal:

- Stable docs should describe what exists now.
- Temporary plans, deferred ideas, future lanes, and MVP/v1 wording should not
  remain in stable docs.
- Unaccepted or optional ideas should either move here or be removed.

Sweep targets:

- `README.md`
- `AGENTS.md`
- `docs/README.md`
- `docs/governance.md`
- `docs/architecture.md`
- `docs/architecture/*.md`
- `docs/platform/*.md`
- `docs/modules/*.md`
- `docs/testing.md`
- `docs/testing/*.md`
- `docs/openspec.md`
- `docs/template/README.md`
- `docs/template/template-decisions.md`
- `scripts/README.md`
- `server/README.md`
- `web/README.md`
- `orchestration/README.md`

Cleanup rules:

- Remove `deferred`, `future`, `planned`, `MVP`, and `v1` language from stable
  docs unless it describes implemented behavior.
- Move still-interesting not-yet-implemented items into this file.
- Keep `template-decisions.md`, but trim it to decisions that still explain
  implemented template behavior.
- Make `docs/template/README.md` describe the template maintenance docs, not a
  forward plan.

## Deferred Mention Inventory

Gather every mention before deleting or moving anything. Initial search terms:

- `defer`
- `deferred`
- `future`
- `planned`
- `MVP`
- `v1`
- `TODO`
- `later`
- `when needed`
- `until`
- `should`

Expected items to classify:

- generated EF migrations
- generated TanStack Query helpers
- shared UI package conventions
- Mailpit or additional local services
- durable intermodule messaging and outbox processing
- identity-provider Admin API provisioning
- Scalar or interactive API docs
- full Aspire/Keycloak browser automation
- dependency automation
- Release Please
- template maintenance checks
- template packaging as an npm/global CLI
- explicit rename/bootstrap overrides
- template-change export/import packet schema
- frontend coverage

Each item should end in one of three places:

- implemented docs, if already implemented;
- this TODO file, if still worth discussing;
- deleted, if it is noise.

Current parking lot after stable-doc cleanup:

- generated EF migrations;
- generated TanStack Query helpers;
- shared UI package conventions;
- Mailpit or additional local services;
- durable intermodule messaging and outbox processing;
- identity-provider Admin API provisioning;
- Scalar or interactive API docs;
- full Aspire/Keycloak browser automation;
- dependency automation;
- Release Please;
- template maintenance checks;
- template packaging as an npm/global CLI;
- explicit rename/bootstrap overrides;
- template-change export/import packet schema;
- frontend coverage;
- local observability dashboards;
- AI services, evals, agents, or local model resources.

## Remaining Cleanup Order

1. Run final full verification.
2. Hand off for independent final review.
