# Tasks: Authenticated Session And Current User

**Input**: Design documents from `specs/001-auth-session-current-user/`
**Prerequisites**: [plan.md](plan.md), [spec.md](spec.md), [research.md](research.md), [data-model.md](data-model.md), [contracts/api-me.md](contracts/api-me.md), [quickstart.md](quickstart.md)
**Propagated**: 2026-05-06 — Added follow-up tasks for transitional auth-header removal, Host/Identity current-user split, vertical-slice organization, and Identity store replacement planning.

**Tests**: Required by FR-017 and FR-018. Test tasks appear before implementation tasks within each user story.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on incomplete tasks.
- **[Story]**: User story label for story phases only.
- Every task includes an exact file path.

## Phase 1: Setup

**Purpose**: Add backend test project structure and solution entries needed by every story.

- [x] T001 Create Host test project in `server/tests/ModularTemplate.Host.Tests/ModularTemplate.Host.Tests.csproj`
- [x] T002 [P] Create Identity application test project in `server/tests/modules/ModularTemplate.Identity.Tests/ModularTemplate.Identity.Tests.csproj`
- [x] T003 [P] Create Identity infrastructure test project in `server/tests/modules/ModularTemplate.Identity.Infrastructure.Tests/ModularTemplate.Identity.Infrastructure.Tests.csproj`
- [x] T004 Add backend test projects to `ModularTemplate.slnx`
- [x] T005 Add shared test package references and category trait guidance in `server/tests/Directory.Build.props`

---

## Phase 2: Foundational

**Purpose**: Create shared contracts, persistence surface, and test infrastructure that block all user stories.

**Critical**: No user story implementation should begin until this phase is complete.

- [x] T006 [P] Define authenticated identity contract in `server/src/modules/ModularTemplate.Identity.Contracts/CurrentUser/AuthenticatedIdentity.cs`
- [x] T007 [P] Define current-user contract in `server/src/modules/ModularTemplate.Identity.Contracts/CurrentUser/CurrentUserContext.cs`
- [x] T008 [P] Define current-user provider abstraction in `server/src/modules/ModularTemplate.Identity.Contracts/CurrentUser/ICurrentUserProvider.cs`
- [x] T009 [P] Define application access abstraction in `server/src/modules/ModularTemplate.Identity.Contracts/Authorization/IApplicationAccessAuthorizer.cs`
- [x] T010 Create Identity module service registration in `server/src/modules/ModularTemplate.Identity/IdentityModuleConfiguration.cs`
- [x] T011 Create Identity infrastructure service registration in `server/src/modules/ModularTemplate.Identity.Infrastructure/IdentityInfrastructureConfiguration.cs`
- [x] T012 Expand Identity persistence interface in `server/src/modules/ModularTemplate.Identity.Infrastructure/Persistence/IIdentityDbContext.cs`
- [x] T013 Add Identity DbSet surface to shared context in `server/src/ModularTemplate.Persistence/ModularTemplateDbContext.cs`
- [x] T014 [P] Create Host test authentication fixture in `server/tests/ModularTemplate.Host.Tests/Authentication/TestAuthenticationHandler.cs`
- [x] T015 [P] Create Host web application factory in `server/tests/ModularTemplate.Host.Tests/Support/HostApplicationFactory.cs`
- [x] T016 [P] Create PostgreSQL integration fixture in `server/tests/modules/ModularTemplate.Identity.Infrastructure.Tests/Support/PostgreSqlFixture.cs`

**Checkpoint**: Shared contracts, service registration hooks, persistence surface, and test infrastructure are ready.

---

## Phase 3: User Story 1 - See Current Application User (Priority: P1) MVP

**Goal**: Authenticated users can call `GET /api/me` and receive stable local current-user details plus neutral application access state.

**Independent Test**: Present an authenticated identity with application access, request `GET /api/me`, and verify the response identifies the local user and `hasAccess` without provider authorization data.

### Tests for User Story 1

- [x] T017 [P] [US1] Add lazy local user creation tests in `server/tests/modules/ModularTemplate.Identity.Tests/CurrentUser/CurrentUserProviderTests.cs`
- [x] T018 [P] [US1] Add duplicate provider subject integration tests in `server/tests/modules/ModularTemplate.Identity.Infrastructure.Tests/Persistence/IdentityStoreTests.cs`
- [x] T019 [P] [US1] Add authenticated-with-access API contract tests in `server/tests/ModularTemplate.Host.Tests/CurrentUser/GetMeEndpointTests.cs`

### Implementation for User Story 1

- [x] T020 [P] [US1] Create local user entity in `server/src/modules/ModularTemplate.Identity/Users/LocalUser.cs`
- [x] T021 [P] [US1] Create application access entity in `server/src/modules/ModularTemplate.Identity/Access/ApplicationAccessRecord.cs`
- [x] T022 [US1] Add Identity EF mappings in `server/src/modules/ModularTemplate.Identity.Infrastructure/Persistence/IdentityEntityTypeConfigurations.cs`
- [x] T023 [US1] Implement Identity store in `server/src/modules/ModularTemplate.Identity.Infrastructure/Persistence/IdentityStore.cs`
- [x] T024 [US1] Implement current-user provider in `server/src/modules/ModularTemplate.Identity/CurrentUser/CurrentUserProvider.cs`
- [x] T025 [US1] Implement neutral current-user response models in `server/src/ModularTemplate.Host/CurrentUser/GetMeResponse.cs`
- [x] T026 [US1] Implement `GET /api/me` endpoint in `server/src/ModularTemplate.Host/CurrentUser/GetMeEndpoint.cs`
- [x] T027 [US1] Register Identity and current-user endpoint composition in `server/src/ModularTemplate.Host/Program.cs`

**Checkpoint**: User Story 1 works independently with authenticated current-user success behavior and lazy local user creation.

---

## Phase 4: User Story 2 - Receive API Authentication Failures Without Redirects (Priority: P2)

**Goal**: API callers receive `401 Unauthorized` problem-details responses for unauthenticated, invalid, expired, or unmappable authentication contexts without browser redirects.

**Independent Test**: Call protected API behavior without a valid authenticated session and verify `401 Unauthorized` with no redirect.

### Tests for User Story 2

- [x] T028 [P] [US2] Add unauthenticated `GET /api/me` tests in `server/tests/ModularTemplate.Host.Tests/Authentication/ApiAuthenticationFailureTests.cs`
- [x] T029 [P] [US2] Add missing provider subject tests in `server/tests/ModularTemplate.Host.Tests/CurrentUser/GetMeAuthenticationFailureTests.cs`
- [x] T030 [US2] Add invalid and expired authentication context tests in `server/tests/ModularTemplate.Host.Tests/Authentication/ApiAuthenticationFailureTests.cs`

### Implementation for User Story 2

- [x] T031 [US2] Implement minimal Host API authentication configuration in `server/src/ModularTemplate.Host/Configuration/HostAuthenticationConfiguration.cs`
- [x] T032 [US2] Implement API redirect suppression behavior in `server/src/ModularTemplate.Host/Configuration/HostAuthenticationConfiguration.cs`
- [x] T033 [US2] Update current-user endpoint authentication failure handling in `server/src/ModularTemplate.Host/CurrentUser/GetMeEndpoint.cs`
- [x] T034 [US2] Wire Host authentication configuration in `server/src/ModularTemplate.Host/Program.cs`

**Checkpoint**: User Story 2 works independently with API authentication failures returning `401` and no redirects.

---

## Phase 5: User Story 3 - Distinguish No Application Access From Unauthenticated (Priority: P3)

**Goal**: Authenticated users without active application access receive successful no-access current-user responses, while access-protected API behavior rejects them with `403 Forbidden`.

**Independent Test**: Present an authenticated identity with no active application access, verify `GET /api/me` returns authenticated/no-access semantics, and verify access-protected behavior returns `403 Forbidden`.

### Tests for User Story 3

- [x] T035 [P] [US3] Add authenticated-without-access API contract tests in `server/tests/ModularTemplate.Host.Tests/CurrentUser/GetMeNoAccessTests.cs`
- [x] T036 [P] [US3] Add application access authorization tests in `server/tests/modules/ModularTemplate.Identity.Tests/Authorization/ApplicationAccessAuthorizerTests.cs`
- [x] T037 [P] [US3] Add minimal access bootstrap tests in `server/tests/modules/ModularTemplate.Identity.Tests/Access/InitialApplicationAccessBootstrapperTests.cs`
- [x] T038 [P] [US3] Add forbidden API behavior tests in `server/tests/ModularTemplate.Host.Tests/Authorization/ApplicationAccessPolicyTests.cs`

### Implementation for User Story 3

- [x] T039 [US3] Implement application access authorizer in `server/src/modules/ModularTemplate.Identity/Authorization/ApplicationAccessAuthorizer.cs`
- [x] T040 [US3] Implement minimal config-driven access bootstrap service in `server/src/modules/ModularTemplate.Identity/Access/InitialApplicationAccessBootstrapper.cs`
- [x] T041 [US3] Update current-user provider no-access behavior in `server/src/modules/ModularTemplate.Identity/CurrentUser/CurrentUserProvider.cs`
- [x] T042 [US3] Implement Host application access authorization policy in `server/src/ModularTemplate.Host/Authorization/ApplicationAccessPolicyConfiguration.cs`
- [x] T043 [US3] Register application access authorization policy in `server/src/ModularTemplate.Host/Program.cs`

**Checkpoint**: User Story 3 works independently with authenticated/no-access `GET /api/me` semantics and `403` for access-protected API behavior.

---

## Phase 6: Polish & Cross-Cutting

**Purpose**: Verify boundaries, documentation, and repository health after the behavior slices are complete.

- [x] T044 Verify no generated migrations were added under `server/src/ModularTemplate.Persistence/Migrations/`
- [x] T045 [P] Update auth/session implementation notes in `docs/platform/auth-and-authorization.md`
- [x] T046 [P] Update Identity module notes in `docs/modules/identity.md`
- [x] T047 [P] Update server architecture implementation notes in `docs/architecture/server.md`
- [x] T048 Run restore verification command from quickstart in `specs/001-auth-session-current-user/quickstart.md`
- [x] T049 Run build verification command from quickstart in `specs/001-auth-session-current-user/quickstart.md`
- [x] T050 Run backend test verification command from quickstart in `specs/001-auth-session-current-user/quickstart.md`
- [x] T051 Run formatting verification command from quickstart in `specs/001-auth-session-current-user/quickstart.md`

---

## Phase 7: Architecture Refinement Follow-up

**Purpose**: Propagate refined architecture requirements that were identified after the initial behavior slice was implemented.

- [x] T052 [P] Document backend DDD/CQRS, vertical-slice, repository/query-handler, and test-double conventions in `docs/architecture/server.md`
- [x] T053 [P] Document custom Host request-header authentication as temporary removal-bound scaffolding in `docs/platform/auth-and-authorization.md`
- [x] T054 [P] Document Identity current-user ownership and transitional `IIdentityStore` replacement path in `docs/modules/identity.md`
- [x] T055 [US1] Move `GET /api/me` Host endpoint and response models into a vertical feature slice in `server/src/ModularTemplate.Host/Features/CurrentUser/GetMeEndpoint.cs`
- [x] T056 [US1] Split Host claim parsing into a provider-neutral authenticated identity adapter in `server/src/ModularTemplate.Host/Features/CurrentUser/AuthenticatedIdentityAdapter.cs`
- [x] T057 [US1] Update Identity current-user resolution to consume provider-neutral authenticated identity data in `server/src/modules/ModularTemplate.Identity/CurrentUser/CurrentUserProvider.cs`
- [x] T058 [US1] Replace or explicitly reshape `IIdentityStore` according to the documented repository/query-handler convention in `server/src/modules/ModularTemplate.Identity/Persistence/IIdentityStore.cs`
- [x] T059 [US2] Remove or isolate production custom request-header authentication scaffolding in `server/src/ModularTemplate.Host/Configuration/HostAuthenticationConfiguration.cs`
- [x] T060 [P] Update Host and Identity tests for the current-user split, vertical feature path, and temporary auth-header removal in `server/tests/`
- [x] T061 Run restore, build, backend tests, and formatting verification after architecture refinement in `specs/001-auth-session-current-user/quickstart.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies.
- **Foundational (Phase 2)**: Depends on Phase 1 and blocks all user stories.
- **User Story 1 (Phase 3)**: Depends on Phase 2 and is the MVP.
- **User Story 2 (Phase 4)**: Depends on Phase 2 and can run after or beside User Story 1 once shared Host files are coordinated.
- **User Story 3 (Phase 5)**: Depends on Phase 2 and uses Identity current-user/access behavior from User Story 1.
- **Polish (Phase 6)**: Depends on selected user stories being complete.
- **Architecture Refinement Follow-up (Phase 7)**: Depends on Phase 6 and the refined architecture requirements FR-019 through FR-025.

### User Story Dependencies

- **US1 (P1)**: Can start after Foundational phase; no dependency on US2 or US3.
- **US2 (P2)**: Can start after Foundational phase; touches some Host files also touched by US1, so coordinate `Program.cs` and `GetMeEndpoint.cs` edits.
- **US3 (P3)**: Can start after Foundational phase, but final no-access behavior depends on US1 current-user provider shape.
- **Architecture follow-up**: Can start after completed behavior verification; T055-T059 should be coordinated because they touch Host and Identity current-user seams.

### Requirement Traceability

- **FR-001, FR-002, FR-003**: T028-T034
- **FR-004**: T038, T042, T043
- **FR-005, FR-006, FR-007**: T017, T020-T024, T039
- **FR-008, FR-009, FR-010, FR-011, FR-014**: T019, T025-T027, T035, T041
- **FR-012, FR-013**: T012, T013, T020-T023, T044
- **FR-015**: T037, T040
- **FR-016**: T044-T051
- **FR-017, FR-018**: T017-T019, T028-T030, T035-T038, T048-T051
- **FR-019, FR-020, FR-021, FR-022**: T052, T055, T058, T060-T061
- **FR-023**: T053, T059-T061
- **FR-024**: T056, T057, T060-T061
- **FR-025**: T052, T054, T058, T060-T061

---

## Parallel Opportunities

- T002 and T003 can run in parallel after T001 is started because they create separate test projects.
- T006-T009 can run in parallel because they create separate contract files.
- T014-T016 can run in parallel because they create separate test support files.
- T017-T019 can run in parallel because they create separate US1 test files.
- T020 and T021 can run in parallel because they create separate Identity model files.
- T028 and T029 can run in parallel because they create separate US2 test files.
- T035-T038 can run in parallel because they create separate US3 test files.
- T045-T047 can run in parallel because they update separate docs.
- T052-T054 can run in parallel because they update separate docs.
- T055-T059 should run sequentially or with careful file ownership because they reshape the Host/Identity current-user boundary.
- T060 can run after T055-T059 because tests depend on the refined implementation shape.

## Parallel Example: User Story 1

```text
Task: "T017 [P] [US1] Add lazy local user creation tests in server/tests/modules/ModularTemplate.Identity.Tests/CurrentUser/CurrentUserProviderTests.cs"
Task: "T018 [P] [US1] Add duplicate provider subject integration tests in server/tests/modules/ModularTemplate.Identity.Infrastructure.Tests/Persistence/IdentityStoreTests.cs"
Task: "T019 [P] [US1] Add authenticated-with-access API contract tests in server/tests/ModularTemplate.Host.Tests/CurrentUser/GetMeEndpointTests.cs"
Task: "T020 [P] [US1] Create local user entity in server/src/modules/ModularTemplate.Identity/Users/LocalUser.cs"
Task: "T021 [P] [US1] Create application access entity in server/src/modules/ModularTemplate.Identity/Access/ApplicationAccessRecord.cs"
```

## Parallel Example: User Story 2

```text
Task: "T028 [P] [US2] Add unauthenticated GET /api/me tests in server/tests/ModularTemplate.Host.Tests/Authentication/ApiAuthenticationFailureTests.cs"
Task: "T029 [P] [US2] Add missing provider subject tests in server/tests/ModularTemplate.Host.Tests/CurrentUser/GetMeAuthenticationFailureTests.cs"
Task: "T030 [US2] Add invalid and expired authentication context tests in server/tests/ModularTemplate.Host.Tests/Authentication/ApiAuthenticationFailureTests.cs"
```

## Parallel Example: User Story 3

```text
Task: "T035 [P] [US3] Add authenticated-without-access API contract tests in server/tests/ModularTemplate.Host.Tests/CurrentUser/GetMeNoAccessTests.cs"
Task: "T036 [P] [US3] Add application access authorization tests in server/tests/modules/ModularTemplate.Identity.Tests/Authorization/ApplicationAccessAuthorizerTests.cs"
Task: "T037 [P] [US3] Add minimal access bootstrap tests in server/tests/modules/ModularTemplate.Identity.Tests/Access/InitialApplicationAccessBootstrapperTests.cs"
Task: "T038 [P] [US3] Add forbidden API behavior tests in server/tests/ModularTemplate.Host.Tests/Authorization/ApplicationAccessPolicyTests.cs"
```

---

## Implementation Strategy

### MVP First

1. Complete Phase 1 and Phase 2.
2. Complete Phase 3 for User Story 1.
3. Stop and validate `GET /api/me` for authenticated users with local current-user details and neutral access state.

### Incremental Delivery

1. Deliver US1 current-user success behavior.
2. Deliver US2 unauthenticated API failure behavior.
3. Deliver US3 authenticated/no-access and forbidden behavior.
4. Run Phase 6 verification and update stable docs.
5. Complete Phase 7 architecture refinement follow-up before accepting the slice as durable template guidance.

### Guardrails

- Do not create generated migrations.
- Do not add frontend apps, generated clients, Aspire resources, CI workflows, Redis ticket storage, full OIDC browser flow, outbox/Rebus, product roles, tenants, organizations, or business workflows.
- Keep provider roles, groups, organizations, and provider-specific claims out of product authorization decisions.
- Keep Host authentication mechanics separate from Identity current-user and application access behavior.
