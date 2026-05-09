## 1. AppHost Integration

- [x] 1.1 Add Aspire-managed Vite app resources for the admin and web
      development servers.
- [x] 1.2 Expose local HTTP endpoints for the admin and web resources while
      preserving the existing direct Vite port defaults.
- [x] 1.3 Pass the Host HTTP origin to both frontend resources through
      `VITE_HOST_ORIGIN`.
- [x] 1.4 Preserve the existing backend resource dependencies and startup order.

## 2. Documentation

- [x] 2.1 Update orchestration and local-services docs to list the frontend
      resources as part of the local platform.
- [x] 2.2 Document how frontend resources receive the Host origin for `/api/`
      and `/auth/` proxying.
- [x] 2.3 Update the template plan to mark frontend orchestration as accepted or
      move the next suggested gate forward.

## 3. Verification

- [x] 3.1 Run OpenSpec strict validation for this change.
- [x] 3.2 Run server/app-host validation with `dotnet test ModularTemplate.slnx`
      or an equivalent build/test command.
- [x] 3.3 Run frontend validation with `pnpm frontend:typecheck`,
      `pnpm frontend:test`, and `pnpm frontend:build`.
- [x] 3.4 Inspect the Aspire resource graph and verify Host, admin frontend, and
      web frontend resources expose HTTP endpoints.
