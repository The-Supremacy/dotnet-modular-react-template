# Quickstart: Authenticated Session And Current User

## Prerequisites

- Use the repository toolchain documented in `global.json`, `Directory.Build.props`, and `Directory.Packages.props`.
- Keep the current persistence foundation changes intact.
- Add only the Identity persistence model/mappings/stores needed for local user identity and application access records.
- Do not add frontend apps, generated clients, generated migrations, Redis-backed ticket storage, full OIDC browser flow, Aspire resources, CI workflows, outbox/Rebus, or broad admin provisioning workflows for this slice.

## Implementation Orientation

1. Read the feature spec and this plan directory:
   - `specs/001-auth-session-current-user/spec.md`
   - `specs/001-auth-session-current-user/plan.md`
   - `specs/001-auth-session-current-user/research.md`
   - `specs/001-auth-session-current-user/data-model.md`
   - `specs/001-auth-session-current-user/contracts/api-me.md`
2. Keep Host responsibilities aligned with `docs/platform/auth-and-authorization.md`.
3. Keep Identity responsibilities aligned with `docs/modules/identity.md`.
4. Keep module boundaries aligned with `docs/architecture/server.md`.
5. Keep temporary template-building context out of durable product behavior.

## Expected Verification

Run baseline checks:

```sh
dotnet restore ModularTemplate.slnx
dotnet build ModularTemplate.slnx --no-restore
pnpm format:check
```

Add and run backend tests proportionate to the slice:

```sh
dotnet test ModularTemplate.slnx --no-build
```

Test coverage should include:

- `GET /api/me` returns current-user details for authenticated users with application access.
- `GET /api/me` returns authenticated/no-access semantics for authenticated users without application access.
- Unauthenticated API requests receive `401 Unauthorized` without redirects.
- Authenticated users without application access receive `403 Forbidden` for access-protected behavior.
- Repeated provider subjects resolve the same local user identity.
- Valid external IdP identities with no local user are created locally with no default application access.
- Missing provider subjects do not create ambiguous local users.
- Identity persistence behavior is covered with real IO where integration tests are needed.

## Future Context

Local orchestration may eventually provide supporting PostgreSQL and Redis resources, full OIDC browser-flow mechanics, and Redis-backed ticket storage, but this feature plan does not add Aspire topology. Generated migrations remain deferred unless accepted feature artifacts or durable architecture decisions change scope.
