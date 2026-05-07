# Testing

Testing is part of the template contract. The template should verify both
application behavior and template mechanics.

Area details:

- [Server testing](testing/server.md)
- [Web testing](testing/web.md)
- [E2E testing](testing/e2e.md)
- [Eval testing](testing/eval.md)

Current validation entrypoints:

- `dotnet test ModularTemplate.slnx`
- `pnpm frontend:typecheck`
- `pnpm frontend:test`
- `pnpm frontend:build`
- `pnpm frontend:lint`
- `pnpm api-client:check`
- `pnpm template:verify`

`pnpm api-client:check` is also wired into the local Husky pre-commit hook and
the default CI workflow.

The default CI workflow lives at `.github/workflows/verify.yml` and runs on
pull requests and pushes to `main`. It runs backend tests, frontend validation,
generated API client drift checks, and OpenSpec validation with
`ASPNETCORE_ENVIRONMENT=Development`, matching the local OpenAPI generation
configuration. Full Aspire/identity-provider browser automation remains
deferred until the e2e startup path is accepted as a CI responsibility.

Use `pnpm template:verify -- --full` before changing bootstrap behavior. It
generates a temporary product-named repository and runs the accepted validation
surface in that generated repository.
