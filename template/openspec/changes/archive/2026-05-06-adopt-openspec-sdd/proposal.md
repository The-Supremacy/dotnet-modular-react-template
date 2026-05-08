## Why

The first accepted behavior slice showed that Spec Kit's archive memory
preserved useful history but did not keep capability-oriented current specs as
the main source of truth. OpenSpec better matches the template's need to evolve
accepted behavior through deltas against current capability specs.

## What Changes

- Replace Spec Kit as the documented SDD workflow with OpenSpec.
- Move hard project rules from Spec Kit memory into durable
  `docs/governance.md`.
- Convert accepted auth/session, current-user, Host API, and persistence
  behavior into OpenSpec current-state specs.
- Add a pinned OpenSpec setup script and Codex OpenSpec support.
- Remove the old Spec Kit files after the migration change is archived.
- This change is behavior-preserving and does not change runtime code, API
  semantics, persistence behavior, frontend scope, orchestration resources, CI
  workflows, generated migrations, or template automation.

## Capabilities

### New Capabilities

- `governance`: Hard project rules and SDD workflow governance.
- `auth-session`: Accepted backend authentication/session behavior and deferred
  production session mechanics.
- `identity-current-user`: Identity-owned local user, application access, and
  current-user behavior.
- `host-api`: Host-owned API authentication/authorization response behavior and
  current-user route contract.
- `persistence`: Accepted persistence scope and generated migration boundaries.

### Modified Capabilities

- None.

## Impact

- Documentation: `README.md`, `AGENTS.md`, `docs/README.md`,
  `docs/governance.md`, `docs/openspec.md`, and template planning docs.
- Tooling: `openspec/`, `.codex/skills/`, and `scripts/setup-openspec.sh`.
- Removed workflow artifacts after archive: `.specify/`, Spec Kit skills,
  `scripts/setup-speckit.sh`, `docs/speckit.md`, and historical Spec Kit feature
  artifacts once represented in OpenSpec specs.
- Runtime code and behavior: no impact.
