# OpenSpec Pivot Migration Plan

Status: Completed on 2026-05-06. This file is retained as historical migration
planning context; active workflow guidance now lives in `docs/openspec.md`,
hard governance lives in `docs/governance.md`, and accepted current behavior
lives under `openspec/specs/`.

## Goal

Pivot the repository's spec-driven development workflow from Spec Kit to
OpenSpec while preserving accepted runtime behavior and repository governance.

This migration is a tooling and documentation change only. It MUST NOT change
backend behavior, auth/session semantics, persistence behavior, frontend scope,
generated migrations, orchestration resources, CI workflows, or template
automation.

## Rationale

The first completed Spec Kit feature exposed friction in the archive/current
state model. Spec Kit produced useful reviewed artifacts, but the Archive
extension consolidated accepted behavior into coarse project-memory files under
`.specify/memory/` rather than maintaining capability-oriented current specs.

OpenSpec better matches the desired long-term workflow:

- active changes are represented as deltas under `openspec/changes/`;
- accepted behavior is merged into current specs under `openspec/specs/`;
- archived change artifacts remain available without becoming the main source
  of truth;
- future auth, identity, persistence, and module changes can modify existing
  capability specs instead of re-specifying whole features.

## Migration Gates

### Gate 1: Record The Tooling Decision

- [ ] Update `docs/template/template-decisions.md`.
- [ ] Mark the previous "Use Spec Kit For Greenfield SDD" decision as
      superseded, not deleted.
- [ ] Add a new decision: "Use OpenSpec For SDD".
- [ ] Record the reason: current-state/capability archive fit is more important
      than Spec Kit's stronger generated workflow for this template.
- [ ] Update `docs/template/current-state.md` with the migration status and
      next step.

### Gate 2: Move Governance Out Of Spec Kit Memory

- [ ] Create a durable governance document outside `.specify/`, likely
      `docs/governance.md`.
- [ ] Use `.specify/memory/constitution.md` as input.
- [ ] Convert Spec Kit-specific wording to OpenSpec/tool-neutral wording.
- [ ] Preserve hard project rules:
  - domain-neutral template first;
  - substantial runtime behavior requires accepted SDD artifacts or durable
    architecture decisions;
  - durable decisions live in repository docs;
  - explicit modular-monolith boundaries;
  - verification scales with risk.
- [ ] Update references in `README.md`, `AGENTS.md`, and stable docs to point to
      the new governance document.

### Gate 3: Install OpenSpec

- [ ] Add `scripts/setup-openspec.sh`.
- [ ] Require Node.js `>=20.19.0`.
- [ ] Install a pinned OpenSpec version, not floating `latest`.
- [ ] Prefer current known version `@fission-ai/openspec@1.3.1` unless a newer
      version is intentionally reviewed.
- [ ] Run `openspec init` with Codex support if the CLI supports a stable
      non-interactive option.
- [ ] Print `openspec --version`.
- [ ] Refuse to overwrite existing `openspec/` unless `--force` is passed.
- [ ] Update `scripts/README.md`.

Do not install OpenSpec automatically from devcontainer lifecycle hooks during
this migration.

### Gate 4: Create The OpenSpec Migration Change

- [ ] Initialize an OpenSpec change named `adopt-openspec-sdd`.
- [ ] Make the proposal explicitly behavior-preserving.
- [ ] Use existing artifacts as source material:
  - `specs/001-auth-session-current-user/spec.md`;
  - `specs/001-auth-session-current-user/plan.md`;
  - `specs/001-auth-session-current-user/research.md`;
  - `specs/001-auth-session-current-user/data-model.md`;
  - `specs/001-auth-session-current-user/contracts/api-me.md`;
  - `.specify/memory/spec.md`;
  - `.specify/memory/plan.md`;
  - `.specify/memory/changelog.md`;
  - `.specify/memory/constitution.md`.
- [ ] Produce OpenSpec current-state specs that describe accepted behavior as it
      exists now, not as historical implementation steps.

Suggested current specs:

```text
openspec/specs/
├── governance.md
├── auth-session.md
├── identity-current-user.md
├── host-api.md
└── persistence.md
```

### Gate 5: Archive The Migration Change

- [ ] Validate the OpenSpec change.
- [ ] Review the generated `openspec/specs/` files for accuracy.
- [ ] Archive the `adopt-openspec-sdd` change so accepted current specs live
      under `openspec/specs/`.
- [ ] Confirm future work instructions point agents to `openspec/specs/` before
      creating new changes.

### Gate 6: Remove Spec Kit

Only do this after Gate 5 passes.

- [ ] Delete `.specify/`.
- [ ] Delete Spec Kit skills under `.agents/skills/speckit-*`.
- [ ] Delete `scripts/setup-speckit.sh`.
- [ ] Delete or replace `docs/speckit.md` with `docs/openspec.md`.
- [ ] Remove or archive `specs/001-auth-session-current-user/` after its
      accepted knowledge is represented in `openspec/specs/`.
- [ ] Keep any OpenSpec-generated Codex skills or commands.
- [ ] Do not delete unrelated `.agents/skills/*` directories.

### Gate 7: Update Repository References

- [ ] Replace remaining references to Spec Kit, `.specify`, `speckit`, and
      `specify` where they refer to the old workflow.
- [ ] Keep historical references only when clearly marked as superseded
      template-building history.
- [ ] Update `AGENTS.md` with an OpenSpec block.
- [ ] Update `README.md`.
- [ ] Update `docs/README.md`.
- [ ] Update `docs/template/implementation-plan.md`.
- [ ] Update `docs/template/current-state.md`.
- [ ] Update `docs/template/template-decisions.md`.

Suggested search:

```sh
rg -n "Spec Kit|SpecKit|speckit|\\.specify|specify" README.md AGENTS.md docs scripts openspec .agents
```

## Verification

Run after the migration files are in place:

```sh
openspec --version
openspec validate
rg -n "Spec Kit|SpecKit|speckit|\\.specify|specify" README.md AGENTS.md docs scripts openspec .agents
dotnet restore ModularTemplate.slnx
dotnet build ModularTemplate.slnx --no-restore
dotnet test ModularTemplate.slnx --no-build
pnpm format:check
```

If the installed OpenSpec CLI exposes a different validation command, use the
installed CLI help as the source of truth and update this TODO.

## Acceptance Criteria

- [ ] OpenSpec is the documented SDD workflow.
- [ ] Accepted current behavior is represented under `openspec/specs/`.
- [ ] Historical Spec Kit artifacts are either converted, archived, or removed.
- [ ] `AGENTS.md` routes agents to OpenSpec current specs and stable docs.
- [ ] No runtime behavior changes are included in the migration.
- [ ] Verification commands pass or have explicit documented exceptions.

## Open Questions

- [ ] Should OpenSpec use the default `spec-driven` schema or a custom schema
      that keeps ADR-style architecture decisions outside archived changes?
- [ ] Should the converted current specs be one file per capability or grouped
      by backend surface?
- [ ] Should `docs/governance.md` be inherited by generated product
      repositories unchanged, or should product bootstrap create a product-specific
      governance document?
