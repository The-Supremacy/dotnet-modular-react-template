## Context

This repository is a domain-neutral .NET and React modular-monolith template.
Its first accepted Spec Kit feature archived current behavior into coarse
memory files under `.specify/memory/`. The desired future workflow needs active
changes as deltas and accepted behavior as capability-oriented current specs.

The migration must preserve runtime behavior. It only changes documentation,
tooling, and spec location.

## Goals / Non-Goals

**Goals:**

- Make OpenSpec the documented SDD workflow.
- Preserve hard governance rules outside any generated tool memory.
- Convert accepted auth/session and current-user behavior into current specs.
- Keep historical migration evidence available in archived OpenSpec change
  artifacts.
- Remove Spec Kit once OpenSpec current specs validate.

**Non-Goals:**

- No backend behavior changes.
- No auth/session semantic changes.
- No persistence behavior changes or generated migrations.
- No frontend apps, orchestration resources, CI workflows, generated clients,
  or template automation.
- No custom OpenSpec schema in this migration.

## Decisions

Use the default `spec-driven` OpenSpec schema.

Rationale: the migration is primarily a workflow pivot. A custom schema can be
introduced later if real OpenSpec use shows that architecture decisions need
first-class schema support beyond repository docs.

Represent current behavior as one spec per capability.

Rationale: capability specs let future changes modify `auth-session`,
`identity-current-user`, `host-api`, or `persistence` independently instead of
reworking one historical feature archive.

Move governance to `docs/governance.md`.

Rationale: hard repository rules should remain durable even if SDD tooling
changes again. OpenSpec specs can point to governance, but governance should not
depend on an archived change folder.

Install OpenSpec with a pinned script but not from devcontainer lifecycle hooks.

Rationale: generated setup remains reviewable and reproducible without adding
implicit mutations during container rebuilds.

## Risks / Trade-offs

- Spec conversion may lose nuance from the historical Spec Kit feature.
  Mitigation: use the archived spec, plan, data model, contract, memory, and
  changelog as source material and validate converted current specs.
- Removing Spec Kit could hide useful history. Mitigation: archive this
  OpenSpec migration change and preserve only clearly superseded references in
  template planning docs.
- Current OpenSpec defaults may not match every future governance need.
  Mitigation: keep durable architecture decisions in docs and revisit schema
  customization only after concrete friction appears.

## Migration Plan

1. Initialize OpenSpec with Codex support.
2. Add durable governance and OpenSpec usage documentation.
3. Create this behavior-preserving migration change and current-state spec
   deltas.
4. Validate and archive the change so `openspec/specs/` owns accepted behavior.
5. Remove Spec Kit artifacts and update repository references.
6. Run verification and document any exceptions.

Rollback is documentation/tooling-only: restore the removed Spec Kit files and
references from version control if OpenSpec validation fails before acceptance.

## Open Questions

- Future product bootstrap may decide whether generated repositories inherit
  `docs/governance.md` unchanged or receive a product-specific bootstrap
  governance document.
