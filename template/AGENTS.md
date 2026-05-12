# Agent Index

This repository is a domain-neutral .NET + React modular-monolith template.

Start with:

- [docs/README.md](docs/README.md) for stable documentation.
- [docs/governance.md](docs/governance.md) for hard project rules.
- [docs/openspec.md](docs/openspec.md) and
  [openspec/config.yaml](openspec/config.yaml) before creating or changing
  OpenSpec artifacts.

Do not add domain behavior, auth/session plumbing, generated migrations,
frontend apps, orchestration resources, CI workflows, or template automation
unless product-owned feature artifacts or durable architecture decisions state
the scope.

Use [docs/governance.md](docs/governance.md) for hard project rules. Before
proposing or implementing substantial runtime behavior, read
`docs/governance.md`, `docs/openspec.md`, `openspec/config.yaml`, the relevant
stable docs under `docs/`, and existing OpenSpec specs or changes when present.
