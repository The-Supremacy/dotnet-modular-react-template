# Repository Governance

This document contains the durable project rules for this repository. It
replaces the former Spec Kit constitution as the tool-neutral governance source.

## Core Principles

### I. Domain-Neutral Product First

This repository MUST remain a reusable .NET and React modular-monolith
foundation. Changes MUST NOT introduce unrelated sample business workflows,
unchecked generated migrations, production secrets, or provider-bound
authorization decisions.

### II. Reviewed Runtime Surface

Substantial runtime behavior MUST start from product-owned feature artifacts or
a durable architecture decision before code is added.
Auth/session plumbing, persistence behavior, frontend apps, orchestration
resources, CI workflows, and generated clients MUST NOT be introduced without
feature artifacts or a durable architecture decision that states the
scope.

### III. Durable Decisions Live In The Repository

Durable project knowledge MUST live in versioned repository files. Stable
architecture rules belong under `docs/`. Agent instructions MAY summarize or
route to those files, but MUST NOT be the only source of an important rule.
Stable architecture and product docs MUST describe durable intent, rules, and
decision boundaries instead of transient implementation status. Implementation
progress MUST be tracked under `docs/current-state/` or product-owned feature
artifacts.

### IV. Explicit Modular-Monolith Boundaries

Backend modules MUST preserve visible dependency direction. Module contracts
MUST NOT expose EF entities, aggregate internals, provider SDK types, or
infrastructure details. Module implementations MUST NOT use another module's
DbSet surface directly. Host composition MAY wire modules together, but
business behavior MUST stay in module-owned contracts and feature slices.
Aggregates MUST own domain transitions and raise relevant domain events.
Command-side repositories MUST represent domain persistence, while query
handlers and read models MUST represent provider-neutral projections.

### V. Verification Scales With Risk

Every behavior or infrastructure change MUST leave the repository in a
verifiable state. Narrow infrastructure or documentation changes MAY rely on
restore, build, and formatting checks. Runtime behavior, shared abstractions,
persistence, auth/session flows, frontend workflows, and generated clients MUST
include tests or explicit verification steps proportionate to their blast
radius.

## Repository Constraints

- Specs and plans MUST load repository context before making design choices:
  this document, `docs/architecture.md`, relevant `docs/architecture/*.md`,
  relevant `docs/platform/*.md`, `docs/testing.md`, and relevant testing detail
  docs when verification is in scope.
- The canonical stack and architecture defaults live in `docs/architecture`
  and related platform docs. Feature artifacts MUST follow those docs unless the
  change explicitly proposes and justifies a different durable direction.
- Product authorization MUST be application-owned and provider-neutral.
  Identity-provider roles, groups, organizations, or provider-specific claims
  MUST NOT be authoritative for product authorization.
- Durable intermodule messaging, outbox processing, orchestration resources,
  CI workflows, and generated migrations MUST remain out of scope unless
  feature artifacts or durable architecture decisions state their scope.

## Development Workflow

Substantial behavior MUST start from product-owned feature artifacts before
code is added. Durable architecture decisions MUST first be recorded in the
relevant stable architecture, platform, testing, module, or governance doc.
Feature planning MUST prefer repository docs as context over duplicating long
architecture guidance in prompts.

Product repositories MAY amend this governance document, but any amendment MUST
preserve an explicit decision trail and update affected docs, templates, and
task workflows in the same change.

## Governance Changes

This document supersedes conflicting generated instructions and local agent
memory for repository work. If a feature artifact, task list, or implementation
conflicts with a MUST rule here, the artifact or implementation MUST change
unless this document is explicitly amended first.

Amendments require:

- a documented rationale;
- updates to affected docs, feature artifacts, and agent instructions;
- a verification note describing which checks were run or why checks were not
  applicable.
