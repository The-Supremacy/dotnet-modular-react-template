# Template Maintenance

This folder contains template-repository maintenance notes. Generated product
repositories should not inherit this folder as product documentation.

Files:

- [template-decisions.md](template-decisions.md) records template-building
  decisions that explain implemented repository behavior.
- [todo.md](todo.md) tracks temporary cleanup and packaging questions that are
  not part of stable generated-product documentation.

Current maintenance entrypoints:

- `pnpm template:bootstrap -- --product-name "Acme Desk" --output ../acme-desk`
- `pnpm template:verify`
- `pnpm template:verify -- --full`
