# Scripts

Repository helper scripts live here.

## Available Scripts

- `setup-openspec.sh` installs the pinned OpenSpec CLI and initializes Codex
  support. It refuses to reuse an existing `openspec/` directory unless
  `--force` is passed.
- `bootstrap-template.mjs` creates a product-named repository copy from this
  template.
- `verify-bootstrap.mjs` creates a temporary sample product repository and
  checks the rename/bootstrap path.

## Template Bootstrap

Create a product repository copy:

```sh
pnpm template:bootstrap -- --product-name "Acme Desk" --output ../acme-desk
```

The first bootstrap version accepts one product name and derives:

- namespace/project prefix: `AcmeDesk`
- slug: `acme-desk`
- database slug: `acme_desk`
- npm scope: `@acme-desk`
- display name: `Acme Desk`

Run focused bootstrap verification:

```sh
pnpm template:verify
```

Run full generated-repository verification:

```sh
pnpm template:verify -- --full
```
