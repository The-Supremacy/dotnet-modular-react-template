# OpenSpec Setup

OpenSpec is optional in this starter repository. Initialize it when the product
has real system behavior that benefits from accepted behavior specs.

## When To Use It

Use OpenSpec for system behavior that needs durable acceptance criteria,
user-visible semantics, cross-artifact planning, or accepted current-state
capability specs. Narrow documentation, governance, tooling, implementation
structure, and verification-only changes can be handled directly when they do
not define product behavior.

Do not add domain behavior, auth/session plumbing, generated migrations,
frontend apps, orchestration resources, CI workflows, generated clients, or
template automation without product-owned feature artifacts or a durable
architecture decision that states the scope.

## Preferred Flow

1. Install the pinned OpenSpec CLI with
   [setup-openspec.sh](../scripts/setup-openspec.sh).
2. Initialize OpenSpec in the product repository with Codex support:
   `openspec init --tools codex .`.
3. Read [governance.md](governance.md), relevant stable docs, and
   `../openspec/config.yaml`.
4. Create a change with `openspec new change <name>`.
5. Write `proposal.md`, behavior capability specs under `specs/`, `design.md`
   when the change is cross-cutting, and `tasks.md`.
6. Validate with `openspec validate <change-name> --strict`.
7. Implement and verify the task list.
8. Archive accepted changes with `openspec archive <change-name> --yes` so
   current behavior is merged into `openspec/specs/`.

## Accepted Specs

The template intentionally does not ship pre-populated OpenSpec specs or
archived changes. Once a product initializes OpenSpec, accepted behavior should
live under `openspec/specs/`, and active changes should live under
`openspec/changes/`.

## Tooling

Run [setup-openspec.sh](../scripts/setup-openspec.sh) to install the pinned
OpenSpec CLI globally.

OpenSpec is intentionally not installed from devcontainer lifecycle hooks.
