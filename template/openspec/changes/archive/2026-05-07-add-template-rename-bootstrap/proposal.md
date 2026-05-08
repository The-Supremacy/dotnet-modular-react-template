## Why

The template is close to MVP 1 but still depends on manual find-and-replace to
create a product repository. A repeatable rename/bootstrap entrypoint is needed
so copied repositories can become buildable, product-named starting points
without relying on ad hoc edits.

## What Changes

- Add a template rename/bootstrap script that copies or rewrites the template
  with a product name.
- Derive deterministic namespace, package, slug, database, local-service, OIDC
  realm/client, and display-name values from one required product name.
- Rename source paths, project files, solution references, package names,
  Aspire resource names, local service configuration, generated-client package
  references, and stable docs where they intentionally contain the template
  placeholder.
- Define verification for a renamed temporary repository using the accepted
  build/test/frontend/generated-client/OpenSpec checks.
- Decide which template-only planning files are excluded or rewritten during
  bootstrap.
- Do not add a full generator, interactive wizard, deployment automation,
  dependency automation, generated migrations, or product-specific domain
  behavior in this change.

## Capabilities

### New Capabilities

- `template-rename-bootstrap`: Template automation for creating a product-named
  repository from the current template.

### Modified Capabilities

- None.

## Impact

- Scripts under `scripts/`.
- Template docs under `docs/template/` and stable bootstrap/testing docs.
- File and directory naming conventions across .NET projects, pnpm packages,
  Aspire configuration, Keycloak realm import, appsettings, generated OpenAPI
  metadata, and frontend app labels.
- CI may consume the verification command in a later maintenance-check change,
  but this change does not add scheduled or dependency automation.
