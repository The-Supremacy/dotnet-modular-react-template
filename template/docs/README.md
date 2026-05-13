# Documentation

This folder contains product documentation. Stable architecture and product
docs describe durable intent, rules, and decision boundaries. Implementation
progress and shipped-template inventory belong under `current-state/` so stable
docs do not become stale status reports.

## Index

- [architecture.md](architecture.md) summarizes the intended system shape.
- [governance.md](governance.md) records hard project rules.
- [openspec.md](openspec.md) describes optional OpenSpec setup and the
  relationship between stable docs, proposed changes, and accepted specs.
- [architecture/server.md](architecture/server.md) records backend architecture
  guidance.
- [architecture/web.md](architecture/web.md) records frontend architecture
  guidance.
- [architecture/orchestration.md](architecture/orchestration.md) records local
  orchestration guidance.
- [architecture/workflows.md](architecture/workflows.md) records workflow
  architecture guidance.
- [current-state/README.md](current-state/README.md) indexes implementation
  progress notes for the generated template.
- [modules/README.md](modules/README.md) indexes module documentation.
- [platform/README.md](platform/README.md) indexes platform concerns.
- [testing.md](testing.md) summarizes the testing strategy.

Before substantial runtime behavior is proposed or implemented, read
[governance.md](governance.md), this index, the relevant area docs, and
`../openspec/config.yaml`. Agent indexes may summarize hard rules, but durable
rules belong in these versioned docs.
