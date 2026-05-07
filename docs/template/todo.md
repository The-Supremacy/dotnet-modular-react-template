# Template TODO

This file captures the agreed first-delivery direction and the development
plan for the next implementation pass. It is template-maintenance context, not
generated product documentation. Runtime behavior changes should still go
through OpenSpec before implementation.

## First-Delivery Decisions

### Generated EF Migrations

Decision:

- Keep generated product migration history clean.
- Do not check generated EF migrations into the template as product history.
- Do not add database stubs or tricks to make the app work without migrations.
  The app expects the real database schema to exist.
- The Migrator project remains the entrypoint that applies migrations when
  Aspire starts.
- Generated product users should regenerate their own initial migration after
  bootstrap.

Implementation notes:

- Document the post-bootstrap migration command clearly in the root README and
  server/local-platform docs.
- Template verification can generate a temporary migration in a scratch copy if
  we want proof that the model is migratable, but that generated migration
  should not become template source.

### Generated TanStack Query Helpers

Decision:

- Include Hey API generated TanStack Query helpers in first delivery.
- Keep `web/packages/auth` as the app-facing wrapper for auth/current-user
  behavior.
- Do not add import restrictions that forbid apps from importing the generated
  API-client package directly.

Implementation notes:

- Enable the Hey API TanStack Query plugin in the API-client generator.
- Update `web/packages/auth` to wrap generated query helpers where useful.
- Update generated-client specs and web architecture docs to reflect the new
  boundary: wrappers are preferred for auth, direct generated-client imports
  are allowed.

### Shared UI Package

Decision:

- Add a narrow real `web/packages/ui`.
- Keep it focused on shadcn + Tailwind primitives.
- Do not move app shell components into the shared package.
- Allow a small exception for shared brand/product primitives such as a product
  logo component.
- Bias the UI toward B2B utility, density, accessibility, and repeatable
  workflow clarity over visual flourish.

Implementation notes:

- Start with low-level primitives only.
- Keep app-specific layout, navigation, dashboards, and shell composition in
  `web/apps/admin` and `web/apps/web`.
- Document when a component belongs in `web/packages/ui` versus an app.

### Mailpit And Local Services

Decision:

- Include Mailpit in local Aspire orchestration.
- Configure Keycloak SMTP through the realm import to send mail to Mailpit.
- If straightforward, enable Keycloak email verification and password-reset
  behavior alongside SMTP.
- Use Mailpit default ports and document them explicitly.
- Do not add a backend mail module yet, but keep the local-service shape ready
  for backend mail workflows later.
- In Aspire, specify ports explicitly for apps/resources even when using
  default ports.
- Add named data volumes with `WithDataVolume` for resources that support it.

Implementation notes:

- Use OpenSpec only for system behavior or accepted restore/verification
  behavior. Do not create OpenSpec changes for pure technical wiring that does
  not define product/runtime behavior.
- Update Aspire orchestration, Keycloak realm import, local-service docs, and
  bootstrap verification expectations.
- Verify whether the Aspire Mailpit integration exists in the current Aspire
  packages; if not, add the simplest container-backed Mailpit resource.

### Durable Intermodule Messaging And Outbox

Decision:

- Defer durable intermodule messaging and outbox processing.
- Add a short template-only deferred considerations section to
  `docs/template/template-decisions.md`.
- Ensure generated product repositories do not inherit that planning note.

Implementation notes:

- Do not mention durable messaging/outbox in stable generated-product docs
  until accepted product/runtime scope exists.

### Identity-Provider Admin API Provisioning

Decision:

- Include only lightweight local Keycloak provisioning in first delivery.
- Define the main Keycloak admin through Aspire if supported by the Aspire
  Keycloak resource.
- Add a couple of local users in the realm import, including at least one user
  suitable for smoke/e2e flows.
- Do not build deeper user-management abstractions or client libraries in the
  template.

Honest feedback:

- This is the right level for the template. Keycloak user management strategy
  varies a lot by product: some products manage users directly in Keycloak,
  some mirror users locally, some use organizations, and some outsource the
  identity provider entirely. A template-owned Admin API client would become
  opinionated quickly.
- Keeping realm-import users plus explicit admin configuration gives us enough
  local repeatability without pretending the template knows every product's
  identity-governance model.

Implementation notes:

- Keep application authorization app-owned and provider-neutral.
- Do not use Keycloak roles/groups/organizations as product authorization.
- Realm-import users should support local login and browser automation only.

### Interactive API Docs

Decision:

- Defer Scalar or other interactive API documentation UI.
- Keep only a template-maintenance mention of interactive API docs in
  `docs/template/template-decisions.md`.
- Keep simple .NET OpenAPI mapping/generation in first delivery.

Current state:

- Host already calls `builder.Services.AddOpenApi()`.
- Host already maps OpenAPI metadata needed by build-time OpenAPI document
  generation.
- The API-client generation path already depends on that OpenAPI document.

Implementation notes:

- Preserve OpenAPI generation as a required template capability.
- Do not add an interactive docs UI until a later accepted scope defines auth
  behavior for docs.

### Full Aspire/Keycloak Browser Automation

Decision:

- Include local-only full browser automation.
- Do not run it in the default `Verify` workflow.
- Start with happy-path coverage only.
- Extend the local realm import for e2e users if it stays simple.
- Use both Aspire CLI and Playwright: Aspire CLI starts/describes/waits for
  resources; Playwright drives browser behavior.

Aspire skill guidance:

- Start the app with `aspire start`, preferably `aspire start --isolated` in
  agent/worktree environments.
- Use `aspire describe --format Json` to discover resource endpoints.
- Use `aspire wait <resource>` to block until resources are healthy.
- For AppHost changes, restart with `aspire start`.
- For .NET project-resource changes, prefer
  `aspire resource <name> rebuild`.
- Use `aspire logs`, `aspire otel logs`, and `aspire otel traces` for
  diagnostics.

Honest feedback:

- Extending the realm import for local e2e users is a good tradeoff here. It is
  slightly test-shaped infrastructure in the local identity provider, but it
  gives a lot of confidence with little moving machinery.
- Keep it local/manual until the startup path and diagnostics are boring. A
  flaky full-platform auth test in default CI would slow down normal template
  work too much.

Implementation notes:

- Add a local command such as `pnpm e2e:local` only after the implementation
  shape is accepted.
- The command should start Aspire or assume Aspire is started, discover
  endpoints, and run Playwright happy-path login/current-user/logout tests.
- Document that this is not part of default `Verify`.

### Dependency Automation

Decision:

- Include Dependabot in first delivery.
- Target all relevant ecosystems: GitHub Actions, NuGet central package
  management, npm/pnpm frontend workspace, and root JS tooling.
- Do not enable auto-merge.

Recommendation:

- Use grouped updates by ecosystem and update class. This keeps PRs reviewable
  without flooding the repository.
- Suggested grouping:
  - GitHub Actions together;
  - NuGet patch/minor together, with majors separate;
  - npm/pnpm patch/minor together by workspace/tooling area, with majors
    separate;
  - Aspire-related packages may deserve their own group because orchestration
    breakage can be subtle.

Honest feedback:

- Avoid auto-merge for now. Since full browser automation is local-only and not
  in `Verify`, a human should have the chance to checkout dependency branches
  and run the local platform path before merging.

### Release Please

Decision:

- Include Release Please in first delivery.
- Use semantic/conventional commits as the versioning input.
- Aim for one global repository version covering backend and frontend together.
- Generated product repositories should inherit the Release Please setup.
- Publishing actual release artifacts can remain product-owned follow-up work.

Recommendation:

- Configure Release Please for a simple monorepo-wide release first, not
  independent package releases.
- Let Release Please maintain changelog/version files and GitHub releases.
- Keep deployment/publishing hooks out of the template until product-specific
  delivery targets exist.
- Keep the "publish" part separate from version/release creation. For the
  template repository, publishing can initially mean only creating a GitHub
  release. Later, the template may publish an npm package if bootstrap moves to
  an `npx`/package flow. Generated modular-monolith apps will likely publish
  several deployable artifacts such as Docker images, so those publish jobs
  should be product-owned.

Honest feedback:

- Your instinct is sound: a single global version is easier for modular
  monoliths than trying to version backend/frontend packages independently.
- The template can bootstrap release discipline without pretending to know how
  each product deploys.
- Release Please should be inherited by bootstrapped apps for version and
  changelog discipline, while publish/deploy workflows remain intentionally
  absent or skeletal until the product chooses its artifact strategy.

### Template Maintenance Checks

Decision:

- Defer broad template-maintenance checks or skills until a concrete missing
  check appears.
- Add only a short note in `docs/template/template-decisions.md` about the
  manual sync approach.
- Do not build template-change sync automation now.

Honest feedback:

- Manual import via agent-supplied context is the right default. Automated
  template-to-child synchronization is complex and likely not worth it until a
  real child repo produces a repeatable update pattern.

### Template Packaging As NPM/Global CLI

Decision:

- Do not publish a global CLI in first delivery.
- Optimize for the fresh-repo scenario:
  - user creates a new repo with basic files such as `README.md`, `.gitignore`,
    and `LICENSE`;
  - template files are copied in;
  - bootstrap/rename cleanup is run once.

Recommendation:

- Keep root `pnpm template:*` as the public template interface for now.
- Keep scripts as packageable Node `.js` internals.
- Add focused script tests before packaging.
- If we later package, prefer a local or `npx`-style command that can copy from
  a template source into the current fresh repo and then run the same bootstrap
  logic.

Honest feedback:

- A global CLI is premature until the fresh-repo copy/bootstrap path has been
  used a few times. The important work now is making the repo-local bootstrap
  deterministic, tested, and well documented.

### Explicit Rename/Bootstrap Overrides

Decision:

- Keep the simplest one-scenario bootstrap.
- Target a fresh repository only.
- Do not support merging into an existing app.
- Do not add explicit naming overrides in first delivery.

Implementation notes:

- Continue deriving namespace, slugs, database names, npm scope, and display
  text from one product name.
- Document that advanced merges/renames are outside template scope.

### Template-Change Export/Import Packet Schema

Decision:

- Drop this idea from first-delivery and future planned scope.
- Do not keep it as a deferred feature.

Implementation notes:

- Remove it from TODO parking lots once docs are updated.

### Frontend Coverage

Decision:

- Skip frontend coverage for first delivery.

Implementation notes:

- Keep frontend validation to typecheck, tests, build, and lint.
- Revisit coverage only if shared frontend packages become complex enough to
  justify coverage artifacts or thresholds.

### DDD + CQRS Identity Reference Module

Decision:

- Include DDD + CQRS conventions in first delivery.
- Identity is the canonical module sample because it is the only delivered
  module.
- Replace `IIdentityStore`.
- Do not add a generic shared `IRepository<T>` abstraction.
- Application services/commands should use the Mediator library's command and
  query abstractions.
- Discuss before introducing domain services; Identity likely does not need
  domain services for the current scope.
- Start with meaningful domain events:
  - `LocalUserCreated`;
  - `LocalUserSeen`;
  - `ApplicationAccessGranted`;
  - `ApplicationAccessRevoked`.
- Keep `EmailAddress` in Identity for now.
- Use simple email validation that rejects obviously invalid addresses without
  trying to implement full RFC validation.
- Treat application access as part of the Identity module's authorization
  model. Prefer dropping the `Record` suffix unless the type remains explicitly
  persistence-shaped.
- Decide during implementation whether application access is its own aggregate
  or an entity managed through a local-user/application-access aggregate. The
  current scope probably works best with application access as its own small
  aggregate because it has an independent grant/revoke lifecycle.
- Keep initial access as a Host-composed startup behavior that sends the
  Identity command only when configured.

Implementation notes:

- `LocalUser` should become an aggregate root using SharedKernel primitives.
- Use repositories for command/write behavior.
- Use Mediator query handlers for application read/query behavior.
- Infrastructure implements module-owned repositories/query handlers through
  narrow DbContext interfaces.
- Domain events should represent meaningful domain transitions only.
- Update architecture docs and module docs so future modules copy this pattern.
- Rename tests touched by this work to
  `{MethodUnderTest}_{ConditionOrInput}_{ExpectedResult}`.

### Domain Event Persistence

Decision:

- Include persistence of domain events in first delivery.
- Persist events into a `domain_events` table.
- The domain event store is platform-owned. Prefer a shared table such as
  `platform.domain_events` if schemas are used.
- Domain event type names remain module-owned stable identifiers.
- The table should record:
  - event id;
  - occurred timestamp;
  - aggregate type;
  - aggregate id;
  - event type;
  - serialized event payload;
  - optional metadata needed for diagnostics.
- Use a `DomainEventType` attribute or equivalent explicit mapping so stored
  event type, aggregate type, and version are stable and not tied to CLR rename
  churn.
- Persist only for now. Do not implement dispatch, durable messaging, inbox,
  or outbox processing.

Recommendation:

- Persist domain events in the root `ModularTemplateDbContext.SaveChangesAsync`
  override or a SaveChanges interceptor.
- For this template, a DbContext override is simpler and easier to inspect.
  It can gather domain events from tracked aggregate roots, create
  `StoredDomainEvent` rows in the Host-owned persistence model, then save both
  aggregate changes and event records in the same database transaction.
- Clear or dequeue aggregate domain events only after they have been converted
  to persisted rows.

Honest feedback:

- This is not an outbox yet. It is an audit/event-log table. That is still
  useful, but docs should avoid implying that events are durably dispatched to
  other modules or external systems.
- Stable event type naming is worth doing immediately. A
  `[DomainEventType("identity.local-user-created")]` style attribute is better
  than storing CLR full names.
- I would not prefix Identity events with `platform` unless the event truly
  belongs to a platform module. The event stream can infer module from the
  stable event type prefix. Good first names would be
  `identity.local-user-created`, `identity.local-user-seen`,
  `identity.application-access-granted`, and
  `identity.application-access-revoked`.
- Use `platform` for the shared store/table ownership, not for module event
  type names. For example: table `platform.domain_events`, event type
  `identity.local-user-created`, aggregate type `identity.local-user`.
- Storing an explicit `module` column is optional. With a single root DbContext,
  forcing every event through separate module storage adds complexity. A
  stable event type prefix plus aggregate type/id is enough for first delivery.
  If querying by module becomes important, add a denormalized module column
  derived from the event type mapping rather than making modules own separate
  event tables.

### Command Transactions And SaveChanges

Decision:

- Include a consistent command-side transaction/save boundary in first
  delivery.
- Use Mediator pipeline behavior for command handlers if the command shape is
  implemented with Mediator.
- Wrap command handling in a transaction and call `SaveChangesAsync` once after
  the handler succeeds.
- Commands handled through `ICommandHandler` save automatically through the
  Mediator pipeline.

Recommendation:

- Make `ICommandHandler` part of the Mediator-based application pattern.
- Query handlers should not save changes.
- Command handlers should mutate aggregates through repositories and avoid
  calling `SaveChangesAsync` directly.
- The transaction behavior should live at the application boundary, not inside
  repositories.

Honest feedback:

- A Mediator pipeline is a good fit for consistency between modules, but only
  if command/query boundaries are explicit. If every Mediator request goes
  through the transaction behavior, read paths may accidentally start
  transactions or save work they should not save.
- Keep the first implementation narrow: one DbContext transaction, one save,
  command requests only.
- Startup/provisioning code should prefer sending explicit commands instead of
  bypassing the Mediator command pipeline.

### App Auth Flow Demonstration

Decision:

- Both frontend apps should ship with a small testable auth/session flow.
- The flow should include Sign in and Sign out controls that exercise the
  Host-owned OIDC routes.
- The flow should load session/current-user state and show
  unauthenticated/authenticated/access states.
- This is both a template smoke surface and the canonical example of how apps
  consume the auth package.

Current state:

- Both apps already render `BrowserSessionSmokeSurface`.
- `web/packages/auth` already exposes login/logout navigation helpers,
  current-user loading, access-state resolution, and React Query options.

Implementation notes:

- Keep and strengthen the current smoke surface rather than replacing it.
- Consider adding a small app-facing auth hook/facade in `web/packages/auth`
  that covers basic operations:
  - sign in;
  - sign out;
  - get session/current-user data;
  - resolve access state.
- Keep apps thin: apps should compose the package-provided hook/facade and
  render their own shell around it.
- Local browser automation should drive this flow.

### Backend Test Naming

Decision:

- Backend tests should use
  `{MethodUnderTest}_{ConditionOrInput}_{ExpectedResult}` naming.

Honest feedback:

- I agree. The current tests are close but inconsistent. The proposed pattern
  is explicit, searchable, and works well for command/query/domain tests where
  the method or handler under test matters.
- For endpoint tests, use the endpoint method/route behavior as the method
  under test, for example
  `GetMe_WhenSessionIsMissing_ReturnsUnauthorizedWithoutRedirect`.

Implementation notes:

- Apply the naming convention to tests touched by upcoming DDD/CQRS work.
- Avoid a repo-wide rename-only churn pass unless it is paired with nearby
  behavioral test edits.

### Configuration Strategy

Decision:

- Aspire should provide runtime configuration for the local platform.
- Project `appsettings.json` / `appsettings.Development.json` should keep
  minimal local defaults needed by tools that run outside Aspire, especially
  OpenAPI generation and EF migration creation.
- Do not duplicate every Aspire-injected value in appsettings.
- Avoid secrets in checked-in appsettings.

Recommendation:

- Treat Aspire as the source of composed runtime configuration.
- Treat appsettings as tool-friendly local defaults and fallback documentation.
- Keep connection strings and OIDC defaults in development appsettings only
  when they are needed for CLI tooling or local non-Aspire commands.
- Document which commands expect Aspire-provided configuration and which can
  run from project appsettings.

Honest feedback:

- Full de-duplication sounds attractive but tends to break CLI tools. Full
  duplication gets stale. The practical middle path is best: Aspire owns real
  composition; appsettings carries the minimum stable local values required by
  design-time/build-time tools.

## Development Plan For Handoff

### Change 1: Architecture Decisions And OpenSpec Scope

Goal:

- Convert these decisions into accepted OpenSpec artifacts and update
  template-only decision docs before runtime implementation.

Tasks:

- Update `docs/template/template-decisions.md` with deferred considerations:
  durable messaging/outbox, interactive API docs, manual child-repo sync.
- Remove forgotten items from TODO parking lots:
  template-change export/import packet schema and frontend coverage.
- Create OpenSpec changes for runtime-affecting work:
  - DDD + CQRS Identity reference module;
  - domain event persistence;
  - local Mailpit/Keycloak provisioning/browser automation;
  - generated TanStack Query helpers;
  - shared UI package;
  - migration handoff docs.

Verification:

- `openspec validate <change> --strict`

### Change 2: DDD + CQRS Identity Reference Module

Goal:

- Make Identity the canonical module architecture example.

Tasks:

- Use Mediator command/query abstractions.
- Replace `IIdentityStore` with repositories and query handlers.
- Convert `LocalUser` to an aggregate root.
- Model `EmailAddress` in Identity with pragmatic validation.
- Add meaningful Identity domain events.
- Replace `ApplicationAccessRecord` with application access aggregate shape.
- Move initial access startup execution to Host composition while Identity owns
  the grant command.
- Add/adjust EF configurations for value objects and aggregate roots.
- Update tests to cover domain behavior, command handlers, query handlers, and
  persistence mappings.
- Update module and architecture docs.

Verification:

- `dotnet test ModularTemplate.slnx --filter "Category=Unit|Category=Application"`
- Integration tests for persistence mappings as needed.

### Change 3: Domain Event Persistence And Command Transaction Boundary

Goal:

- Persist domain events in the same database transaction as aggregate changes
  and centralize command save behavior.

Tasks:

- Add `StoredDomainEvent` persistence model and `domain_events` mapping.
- Add `DomainEventType` attribute or equivalent stable type registry.
- Override `ModularTemplateDbContext.SaveChangesAsync` or add an interceptor to
  collect aggregate domain events and persist event records.
- Add command transaction/save pipeline for command handlers only.
- Ensure repositories do not save directly.
- Add tests for event persistence, stable event type mapping, and transaction
  behavior.

Verification:

- Backend unit/application tests.
- Persistence integration test for event rows.

### Change 4: Migration Handoff And Local Startup

Goal:

- Keep migration history clean while making generated-product setup obvious.

Tasks:

- Document that generated products create their own initial migration after
  bootstrap.
- Document Migrator behavior and Aspire startup expectations.
- Optionally add a template verification path that generates a temporary
  migration in a scratch repo and proves the model is migratable.

Verification:

- Documentation review.
- Optional scratch migration verification.

### Change 5: Local Platform Mailpit, Keycloak Users, And Volumes

Goal:

- Make local identity/email flows repeatable.

Tasks:

- Add Mailpit to Aspire with explicit documented ports.
- Configure Keycloak SMTP in realm import.
- Enable verification/reset if straightforward and supported by realm import.
- Define Keycloak admin through Aspire where supported.
- Add local realm-import users for smoke/e2e.
- Add explicit ports for apps/resources even when default.
- Add named data volumes for resources that support `WithDataVolume`.
- Update local service docs.

Verification:

- `aspire start --isolated`
- `aspire describe --format Json`
- Manual Keycloak login and Mailpit email inspection.

### Change 6: Local Browser Automation

Goal:

- Provide a local happy-path full-platform smoke command.

Tasks:

- Add Playwright configuration for local Aspire e2e.
- Use Aspire CLI to start or describe the app and discover endpoints.
- Drive happy-path login/current-user/logout through Keycloak and frontend app.
- Reuse the app auth/session smoke surface as the browser target.
- Keep this command out of default `Verify`.
- Document diagnostics using Aspire logs/otel commands.

Verification:

- Local e2e command passes against `aspire start --isolated`.

### Change 7: Generated TanStack Query Helpers

Goal:

- Generate TanStack Query helpers while preserving auth package wrappers.

Tasks:

- Enable Hey API TanStack Query plugin.
- Regenerate API client.
- Update `web/packages/auth` wrappers.
- Update tests and docs.

Verification:

- `pnpm api-client:check`
- `pnpm frontend:typecheck`
- `pnpm frontend:test`

### Change 8: Shared UI Package

Goal:

- Add narrow shared UI primitives for B2B app surfaces.

Tasks:

- Add `web/packages/ui`.
- Add shadcn/Tailwind primitive conventions.
- Add minimal primitives and shared product logo component.
- Keep app shells app-owned.
- Add tests/typecheck/build wiring.
- Update docs.

Verification:

- `pnpm frontend:typecheck`
- `pnpm frontend:test`
- `pnpm frontend:build`
- `pnpm frontend:lint`

### Change 9: Dependency Automation And Release Please

Goal:

- Add repository automation that generated products inherit.

Tasks:

- Add Dependabot config for GitHub Actions, NuGet, pnpm/npm.
- Group updates by ecosystem/update class.
- Keep auto-merge disabled.
- Add Release Please config for one global repository version.
- Ensure conventional/semantic commit expectations are documented.
- Keep deployment/publishing hooks product-owned.

Verification:

- YAML/config validation where available.
- Documentation review.

### Change 10: Bootstrap And Product Handoff Polish

Goal:

- Keep the fresh-repo bootstrap story simple and explicit.

Tasks:

- Keep one product-name input and no overrides.
- Document fresh-repo-only support.
- Ensure bootstrap removes template-only planning docs and deferred
  considerations.
- Update root README "Use the template" section with migration and local-start
  steps.
- Keep global CLI packaging out of scope.

Verification:

- `pnpm template:verify`
- `pnpm template:verify -- --full` when environment allows.

### Change 11: Backend Test Naming Cleanup

Goal:

- Make backend tests follow the agreed naming convention as nearby behavior is
  changed.

Tasks:

- Use `{MethodUnderTest}_{ConditionOrInput}_{ExpectedResult}` for new and
  touched backend tests.
- Rename existing tests opportunistically when their files are edited for
  DDD/CQRS, domain-event persistence, command pipeline, or auth-flow work.
- Document the convention in server testing docs.

Verification:

- Backend test run for affected categories.

## Suggested Implementation Order

1. DDD + CQRS Identity reference module.
2. Domain event persistence and command transaction boundary.
3. Migration handoff docs/verification.
4. Local platform Mailpit, Keycloak users/admin, explicit ports, and volumes.
5. Local Aspire/Keycloak browser automation.
6. Generated TanStack Query helpers.
7. Shared UI package.
8. Dependency automation and Release Please.
9. Bootstrap/product handoff polish.
10. Backend test naming cleanup as part of touched backend test work.

Rationale:

- Identity architecture and persistence shape affect migrations, domain events,
  tests, and docs, so they should come first.
- Local platform changes should happen before browser automation.
- Generated frontend helpers and UI package can proceed after backend API
  contracts are stable enough.
