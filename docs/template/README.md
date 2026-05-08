# Template Maintenance

This folder contains template-repository maintenance notes. Generated product
repositories should not inherit this folder as product documentation.

Files:

- [template-decisions.md](template-decisions.md) records template-building
  decisions that explain implemented repository behavior.
- [release-readiness.md](release-readiness.md) tracks the release and
  bootstrap-population plan for template-maintenance work.

Current maintenance entrypoints:

- `pnpm template:bootstrap -- --product-name "Acme Desk" --output ../acme-desk`
- `pnpm template:verify`
- `pnpm template:verify -- --full`
