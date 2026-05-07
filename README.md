# net-react-modular-template

Domain-neutral .NET + React modular-monolith template.

Substantial runtime behavior starts from accepted OpenSpec artifacts or durable
architecture decisions. Stable governance, architecture, platform, testing, and
module guidance lives under `docs/`.

Start with:

- [docs/README.md](docs/README.md) for stable product-facing documentation.
- [docs/governance.md](docs/governance.md) for hard project rules.
- [docs/openspec.md](docs/openspec.md) for the spec-driven development
  workflow.

## Use The Template

Create a product-named repository copy:

```sh
pnpm template:bootstrap -- --product-name "Acme Desk" --output ../acme-desk
```

The bootstrap command accepts one display-oriented product name and derives the
.NET namespace/project prefix, npm scope, local service slugs, database names,
and visible display text. Template automation is exposed through root `pnpm`
scripts and implemented as Node `.js` scripts under `scripts/` so the helpers
can be tested and packaged later.

Useful maintenance commands:

- `pnpm api-client:generate`
- `pnpm api-client:check`
- `pnpm scripts:lint`
- `pnpm template:verify`
- `pnpm template:verify -- --full`
