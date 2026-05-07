# Scripts

Repository helper scripts live here.

## Available Scripts

- `setup-openspec.sh` installs the pinned OpenSpec CLI and initializes Codex
  support. It refuses to reuse an existing `openspec/` directory unless
  `--force` is passed.
- `generate-openapi.js` generates the Host OpenAPI document used by the
  frontend API client package.
- `generate-api-client.js` refreshes the Host OpenAPI document and generated
  frontend API client.
- `check-api-client.js` verifies that the checked-in OpenAPI document and
  generated frontend API client are current.
- `bootstrap-template.js` creates a product-named repository copy from this
  template.
- `verify-bootstrap.js` creates a temporary sample product repository and
  checks the rename/bootstrap path.

Script files are Node ES modules. The root package sets `"type": "module"` so
`.js` scripts use `import`/`export` syntax; `commitlint.config.cjs` remains
CommonJS explicitly because that tool expects it.

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
