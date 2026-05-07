## Context

The template uses `ModularTemplate`, `modular-template`,
`@modular-template`, `modular_template`, and display text such as
`Modular Template` across .NET projects, pnpm packages, Aspire resources,
Keycloak realm import, appsettings, scripts, docs, and tests. Today a copied
product repository must rename those surfaces manually. That is fragile because
missed names can leave generated repositories half-template and hard to verify.

## Goals / Non-Goals

**Goals:**

- Provide a repeatable bootstrap command that creates a product-named copy of
  the repository.
- Keep the template checkout unchanged by default.
- Derive the first version from one required product name.
- Rewrite text content and filesystem paths that intentionally use the template
  placeholder.
- Remove or rewrite template-only planning docs so generated product
  repositories do not inherit stale template-building history.
- Verify the generated repository with the accepted MVP 1 checks.

**Non-Goals:**

- Build a full project generator, wizard, package template, or scaffolding
  marketplace.
- Support explicit namespace/package/resource slug overrides in the first
  version.
- Generate EF migrations.
- Add product domain behavior, provider-specific authorization policy, CI
  dependency automation, deployment configuration, or release automation.

## Decisions

### Bootstrap Out Of Place

Add a script that takes `--product-name` and `--output` and writes a renamed
copy to a new output directory. The command fails when the output directory
already exists unless a later accepted change adds an explicit overwrite mode.

Rationale: out-of-place bootstrap avoids accidentally renaming the template
checkout and gives verification a clean generated repository.

Alternative considered: rename the current checkout in place. That is useful
after a repository has already been copied manually, but it is too easy to run
against the template source by mistake.

### Derive Names From One Product Name

For the first version, derive:

- PascalCase namespace/project prefix, for example `AcmeDesk`.
- kebab-case slug, for example `acme-desk`.
- snake_case database slug, for example `acme_desk`.
- npm scope, for example `@acme-desk`.
- display name, preserving normalized product words, for example `Acme Desk`.

Rationale: one required input keeps the script teachable and repeatable. Real
override needs can be added later once there are examples that prove the
default derivation is insufficient.

Alternative considered: accept explicit overrides for every naming surface from
day one. That creates more validation work and more ways to produce incoherent
repositories before there is pressure for it.

### Rewrite Known Placeholder Surfaces

Use an explicit mapping for template placeholder forms and known path segments
instead of broad fuzzy replacement. The first implementation should cover .NET
project names, namespaces, solution references, pnpm package names/imports,
Aspire project/resource configuration, Keycloak realm/client names,
appsettings, scripts, generated OpenAPI metadata, README/docs references,
frontend app labels, and test literals.

Rationale: explicit mappings are reviewable and reduce the risk of accidental
changes to unrelated text.

Alternative considered: use a broad case-insensitive text replacement. That is
fast but likely to damage docs or third-party data that happen to contain
similar words.

### Remove Template-Only Planning Docs From Generated Products

Generated product repositories should not inherit `docs/template/` by default.
The bootstrap script should remove that folder and update `docs/README.md`
references. Product repositories can add their own planning or ADR structure
after bootstrap.

Rationale: `docs/template/` records decisions made while building this source
template. Carrying it into products would make stale template context look like
product history.

Alternative considered: rewrite `docs/template/` into product planning docs.
That can be useful later, but it needs a stronger opinion about product
planning workflow than this template currently owns.

### Verify In A Temporary Generated Repository

Add a verification entrypoint that creates a temporary product repository and
runs accepted checks there. The first version should run formatting,
OpenSpec validation, backend tests, frontend typecheck/test/build/lint, and
generated-client drift checks, or clearly document any environment-dependent
check that cannot run.

Rationale: the bootstrap path is part of the template contract. A renamed
repository that cannot build is a template defect.

Alternative considered: inspect only that no placeholder strings remain. That
is necessary but not enough to prove the generated repository works.

## Risks / Trade-offs

- Derived naming may not match every real product brand -> keep overrides
  deferred but make the derivation code isolated and documented.
- Placeholder scanning may miss a new file type -> add a verification check for
  remaining known placeholders in expected generated-repo paths.
- Out-of-place copy can be slower than in-place rename -> correctness and
  recoverability matter more for the first version.
- Generated product docs may need product-specific governance later -> remove
  template planning docs now and keep governance inheritance as an explicit
  bootstrap decision.
