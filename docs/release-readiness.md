# Release Readiness

This is template-maintenance planning context. Generated product repositories
should not inherit these root docs as product documentation.

The repository root is the template factory. The generated-product payload lives
under `template/`.

## Bootstrap Flow

The intended release path is an npm CLI package named
`dotnet-modular-react-template`.

After publication, create a product repository with:

```sh
pnpm dlx dotnet-modular-react-template -- --product-name "Acme Desk" --output ../acme-desk
```

For local factory development, create a product repository with:

```sh
pnpm template:bootstrap -- --product-name "Acme Desk" --output ../acme-desk
```

The bootstrap command accepts one display-oriented product name, derives the
namespace/package/service/database naming forms, copies `template/`
out-of-place, rewrites known placeholders, renames paths, and verifies the
generated repository.

Preview without writing files:

```sh
pnpm template:bootstrap -- --product-name "Acme Desk" --output ../acme-desk --dry-run
```

The root package also exposes the same CLI when installed globally:

```sh
dotnet-modular-react-template --product-name "Acme Desk" --output ../acme-desk
```

Local package testing should exercise the packed artifact, not only the working
tree:

```sh
mkdir -p /tmp/dotnet-modular-react-template-pack
pnpm pack --pack-destination /tmp/dotnet-modular-react-template-pack
pnpm dlx /tmp/dotnet-modular-react-template-pack/dotnet-modular-react-template-0.0.0.tgz -- --product-name "Acme Desk" --output /tmp/acme-desk
```

## Release Checks

Run normal factory verification for every bootstrap or payload-layout change:

```sh
pnpm verify
```

Before cutting a release, run full generated-product verification:

```sh
pnpm template:verify:full
```

Before testing the template payload directly from `template/`, remove any
template-local ignored EF migrations from previous manual runs:

```sh
rm -rf template/server/src/ModularTemplate.Persistence/Migrations
```

## Runtime Baseline

- Node is pinned through pnpm `devEngines.runtime` in both the factory root and
  `template/`.
- `packageManager` pins pnpm 10.33.x in both package manifests.
- `.node-version` is present for editor/version-manager hints, but pnpm is the
  authoritative script runtime.
- Full generated-product verification can run against Docker or a
  Docker-API-compatible Podman socket. The factory verifier auto-detects the
  rootless Linux Podman socket when `DOCKER_HOST` is not already set.
- The factory root is a publishable npm CLI package. `package.json` includes
  only the root README/license/docs, bootstrap script, and generated-product
  payload in the packed artifact.

## Ownership

- Root files are factory-maintenance files unless they are explicitly inside
  `template/`.
- Product files live under `template/` and are copied into generated
  repositories.
- Productized transforms should be rare because `template/` can contain the
  generated-product version of root-level files directly.
- Product `.github`, `.husky`, `.vscode`, and product docs belong in
  `template/`. Generated repositories do not inherit pre-populated OpenSpec
  specs or skills; products can initialize OpenSpec after bootstrap.
- Future release automation such as Dependabot and Release Please should be
  inherited by generated products once added, with publishing/deployment steps
  kept product-owned.

## V1 Checklist

Already implemented for the first delivery:

1. Focused script tests for name derivation, manifest rules, placeholder
   rewriting, path renaming, and generated sample checks.
2. Dependabot at the factory root for factory dependencies and GitHub Actions.
3. Dependabot under `template/.github/` for generated-product npm, NuGet, and
   GitHub Actions dependencies.
4. Release Please at the factory root for template-factory releases.
5. Release Please under `template/.github/` so generated product repositories
   inherit product-owned release automation.
6. Factory-root npm CLI packaging that ships the generated-product payload.

Required before v1:

1. Verify the packed tarball can bootstrap a generated product.
2. Run `pnpm template:verify:full` in a clean Node 24 environment.
3. Bootstrap a throwaway product repository and manually confirm the first-use
   path: install, initial EF migration, Aspire startup, generated product
   hooks, and inherited CI files.
4. Publish from the GitHub release workflow, then verify the npm CLI from a
   separate throwaway directory.

Good candidates for post-v1:

- Add publishing or deployment workflow examples. Keep concrete deploy jobs
  product-owned.
- Add richer bootstrap options after the one-name flow has proven stable in
  real generated repositories.
