## Why

The template now has accepted admin and web React app shells, but the local
Aspire platform still starts only the backend services. Adding frontend
orchestration closes the local development loop so the Host, identity platform,
and browser apps can run together through one entrypoint.

## What Changes

- Add local Aspire orchestration for the admin and web Vite apps.
- Pass each frontend app the local Host origin needed for same-origin proxying.
- Document the frontend app resources and local startup expectations.
- Verify that the composed local platform exposes Host, admin, and web
  development endpoints without adding product-specific domain behavior.

## Capabilities

### New Capabilities

- `frontend-orchestration`: Local Aspire orchestration for the template's admin
  and web Vite apps.

### Modified Capabilities

- None.

## Impact

- Affects the Aspire app host under `orchestration/`.
- Affects frontend app package scripts or workspace metadata only as needed for
  predictable orchestration.
- Affects stable local-platform and orchestration documentation.
- Does not add generated clients, CI workflows, migrations, Mailpit, durable
  messaging, template automation, or product-specific domain behavior.
