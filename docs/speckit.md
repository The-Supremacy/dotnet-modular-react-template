# Spec Kit Usage

Spec Kit is the template's default tool for greenfield spec-driven behavior
work. It is not required for every narrow architecture or tooling change.

## When To Use It

Use Spec Kit for behavior that needs durable acceptance criteria, user-visible
semantics, or cross-artifact planning. The first expected use is the
auth/session and `/api/me` behavior slice.

Do not use Spec Kit by default for narrow infrastructure changes such as
project shells, SharedKernel primitives, ServiceDefaults, or basic Host
composition.

## Preferred Flow

1. Explore in normal discussion first.
2. Run `specify` once the intent is clear enough to preserve.
3. Review the spec before planning.
4. Run `clarify` only when meaningful ambiguity remains.
5. Run `plan`.
6. Review the plan before task generation.
7. Run `tasks`.
8. Run `analyze`.
9. Review tasks and analysis before implementation.
10. Run `implement`.
11. Verify with normal repo commands and review.
12. Run Archive after the feature is accepted.

## Extensions

- Refine is for controlled changes to existing spec artifacts.
- Archive is for merging accepted feature knowledge into project memory.

Keep extension hooks optional. Do not enable broad automatic mutation until a
workflow has proven useful.

## Deferred

Do not use `taskstoissues` by default. Revisit it only if GitHub Issues become
the active work tracker for larger product or multi-agent work.
