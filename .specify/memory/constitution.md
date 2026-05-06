<!--
Sync Impact Report
Version change: 1.0.1 -> 2.0.0
Modified principles:
- Domain-Neutral Template First: removed temporary-scope exception wording.
- Reviewed Runtime Surface -> Spec Kit Before Runtime Surface:
  removed temporary construction-state dependency and retained reviewed
  feature-artifact requirement for runtime behavior.
- Durable Decisions Live In The Repository: removed temporary-scope wording.
- Verification Scales With Risk: removed temporary-scope wording.
- Template Constraints: removed temporary construction-state document
  dependency.
- Development Workflow: removed temporary construction-state workflow
  dependencies.
Added sections: none
Removed sections: none
Templates requiring updates:
- .specify/templates/plan-template.md: updated
- .specify/templates/spec-template.md: reviewed, no change required
- .specify/templates/tasks-template.md: reviewed, no change required
- AGENTS.md: updated
- README.md: updated
- docs/speckit.md: updated
Follow-up TODOs:
- Decide whether generated product repositories inherit this constitution
  unchanged or receive a product-bootstrap constitution during rename/setup.
-->

# Modular Template Constitution

## Core Principles

### I. Domain-Neutral Template First

This repository MUST remain a reusable .NET and React modular-monolith template.
Template work MUST NOT introduce product-specific domain behavior, sample
business workflows, generated migrations, production secrets, or provider-bound
authorization decisions.

Rationale: future products should inherit a clean starting point, not hidden
assumptions from an imagined sample domain.

### II. Spec Kit Before Runtime Surface

Substantial runtime behavior MUST start from reviewed Spec Kit artifacts before
code is added. Auth/session plumbing, persistence behavior, frontend apps,
orchestration resources, CI workflows, generated clients, and template
automation MUST NOT be introduced without accepted feature artifacts or a
durable architecture decision that states the scope.

Rationale: the template is intended to be inspectable and teachable, and broad
unreviewed scaffolding makes later decisions depend on accidental structure.

### III. Durable Decisions Live In The Repository

Durable project knowledge MUST live in versioned repository files. Stable
architecture rules belong under `docs/`; template-construction reasoning belongs
under `docs/template/`; accepted feature behavior and task plans belong in Spec
Kit artifacts. Agent instructions MAY summarize or route to those files, but
MUST NOT be the only source of an important rule.

The constitution SHOULD remain a compact governance document. It MUST contain
the hard rules needed for Spec Kit work and MAY reference canonical repository
docs for detailed architecture, platform, testing, and construction context.
Detailed rules SHOULD NOT be duplicated in the constitution when a stable doc
can own the detail without weakening the governance rule.

Rationale: humans and agents need the same durable source of truth after tool
state, containers, or local sessions are rebuilt.

### IV. Explicit Modular-Monolith Boundaries

Backend modules MUST preserve visible dependency direction. Module contracts
MUST NOT expose EF entities, aggregate internals, provider SDK types, or
infrastructure details. Module implementations MUST NOT use another module's
DbSet surface directly. Host composition MAY wire modules together, but business
behavior MUST stay in module-owned contracts and feature slices.

Rationale: the template should demonstrate modular-monolith benefits without
pretending the system is already a distributed architecture.

### V. Verification Scales With Risk

Every behavior or infrastructure change MUST leave the repository in a
verifiable state. Narrow infrastructure or documentation changes MAY rely on
restore, build, and formatting checks. Runtime behavior, shared abstractions,
persistence, auth/session flows, frontend workflows, and generated clients MUST
include tests or explicit verification steps proportionate to their blast
radius.

Rationale: the template is a learning and restartability tool; broken baseline
verification undermines every generated product.

## Template Constraints

- Specs and plans MUST load repository context before making design choices:
  `.specify/memory/constitution.md`, `docs/architecture.md`, relevant
  `docs/architecture/*.md`, relevant `docs/platform/*.md`, and `docs/testing.md`
  plus relevant testing detail docs when verification is in scope.
- The canonical stack and architecture defaults live in `docs/architecture`
  and related platform docs. Spec Kit artifacts MUST follow those docs unless
  the feature explicitly proposes and justifies a change.
- Product authorization MUST be application-owned and provider-neutral.
  Identity-provider roles, groups, organizations, or provider-specific claims
  MUST NOT be authoritative for product authorization.
- Durable intermodule messaging, outbox processing, orchestration resources,
  CI workflows, generated migrations, and template automation MUST remain
  deferred until accepted feature artifacts or durable architecture decisions
  state their scope.
- Spec Kit is the default SDD tool. The approved initial extension set is
  Archive and Refine only.

## Development Workflow

Substantial behavior MUST start from Spec Kit feature artifacts before code is
added. Architecture and template-building decisions MUST first be recorded in
`docs/template/template-decisions.md` or the relevant stable architecture doc.
Spec Kit planning MUST prefer repository docs as context over duplicating long
architecture guidance in prompts.
When a feature is completed and accepted, durable lessons SHOULD be archived
into Spec Kit memory before the next related feature begins.

Generated product repositories MAY amend this constitution after bootstrap, but
any amendment MUST preserve an explicit decision trail and update affected docs,
templates, and task workflows in the same change.

## Governance

This constitution supersedes conflicting generated instructions and local agent
memory for template work. If a Spec Kit spec, plan, task list, or implementation
conflicts with a MUST rule here, the artifact or implementation MUST change
unless this constitution is explicitly amended first.

Amendments require:

- a documented rationale;
- a semantic version bump;
- updates to affected docs, Spec Kit templates, and agent instructions;
- a verification note describing which checks were run or why checks were not
  applicable.

Versioning policy:

- MAJOR: removes or redefines a core principle.
- MINOR: adds a principle, required workflow, or new governed surface.
- PATCH: clarifies wording without changing obligations.

**Version**: 2.0.0 | **Ratified**: 2026-05-05 | **Last Amended**: 2026-05-06
