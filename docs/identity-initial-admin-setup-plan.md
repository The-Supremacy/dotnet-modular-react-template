# Identity Initial Admin Setup Plan

This plan records the template direction for replacing Host-startup permission
mutation with explicit setup-time initial-admin provisioning. It is written so
an already-generated product repository can apply the same change.

## Target Shape

- The identity provider owns user authentication only.
- The application owns product access and admin authorization.
- The web Host never grants or repairs permissions during startup.
- The Migrator or cluster init job applies schema migrations, then runs an
  explicit initial-admin setup step.
- Setup identifies the initial admin by provider/issuer plus subject. It does
  not depend on Keycloak, Entra, Auth0, Okta, groups, roles, or provider SDKs.

The setup input is:

```sh
identity grant-admin --provider "<issuer>" --subject "<subject>" [--force]
```

Local Aspire can pass the local Keycloak issuer and known smoke-admin subject
through configuration:

```text
Identity:InitialAdmin:Provider
Identity:InitialAdmin:Subject
Identity:InitialAdmin:Force
```

## Idempotency Rules

- If the local user does not exist, create it with `(provider, subject)`.
- If active application access already exists, no-op successfully.
- If application access does not exist, create active access.
- If application access exists but is revoked, return a failure/result that
  says force is required.
- Only reactivate revoked access when `--force` or
  `Identity:InitialAdmin:Force=true` is explicitly supplied.

The current template still has a minimal `ApplicationAccess` aggregate rather
than full roles/permissions. Products that already have a role model should map
this command to a `SystemAdmin` or equivalent role assignment instead of a
generic access flag.

## Transfer Steps For An Existing Product

1. Move initial admin provisioning out of the Host.
   Remove any `IHostedService`, startup filter, or Host startup code that grants
   default permissions.

2. Add an Identity command such as `GrantInitialAdminAccessCommand`.
   The command should live in the Identity module and should use existing local
   user and authorization repositories. Keep the command provider-neutral:
   accept `provider`, `subject`, and `force`.

3. Make revoked access sticky.
   Re-running setup must not silently undo an operator revocation. Return a
   distinct result, exit code, or exception for "revoked; force required".

4. Add a Migrator/setup command.
   The Migrator should still apply EF migrations. After migrations, it may run
   initial-admin setup from command-line args or environment-backed
   configuration. Keep schema migration and admin setup visible as separate log
   steps even when one process performs both.

5. Wire local Aspire to the Migrator, not the Host.
   Pass the local Keycloak smoke admin issuer and subject to the Migrator with
   `Identity__InitialAdmin__Provider` and
   `Identity__InitialAdmin__Subject`.

6. Wire production cluster init similarly.
   A Kubernetes Job, Helm hook, Terraform-driven task, or deployment pipeline
   can run the Migrator with the real IdP issuer and subject. Treat the issuer
   and subject as environment configuration, not application code.

7. Update documentation.
   Product docs should say that IdP roles/groups are not authoritative for
   product authorization, and that initial admin setup is an explicit
   setup-time operation.

## Tests To Carry Over

- Unit test: setup creates a local user and grants access for a complete
  provider/subject pair.
- Unit test: repeated setup with active access is idempotent.
- Unit test: repeated setup after revocation does not reactivate access without
  force.
- Unit test: force reactivates revoked access.
- Integration test: the Migrator applies migrations against PostgreSQL and
  grants initial admin access from configuration.
- Integration test: the Migrator returns the expected failure/result after
  revocation and succeeds with command-line `--force`.

The template uses Testcontainers for the Migrator integration tests so the
setup path exercises EF migrations and PostgreSQL constraints rather than only
in-memory fakes.

## Follow-Up Direction

When the product grows beyond the current access flag, introduce local
application roles or permission assignments:

```text
LocalUser(provider, subject)
ApplicationRole(key)
ApplicationRoleAssignment(localUserId, roleKey, isActive, grantedAt, source)
```

At that point `identity grant-admin` should create or reactivate the
`SystemAdmin` assignment rather than a generic `ApplicationAccess` row.
