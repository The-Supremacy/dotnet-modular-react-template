# Server Testing

The backend target uses xUnit, Shouldly, NSubstitute, Testcontainers, and
architecture tests.

Every backend test should declare a category trait so CI can choose test bands
explicitly:

```csharp
[Trait("Category", "Unit")]
```

Expected categories:

- `Unit`
- `Application`
- `Integration`
- `Eval`

Use `Unit` for isolated domain, application, helper, and component behavior
that can be covered without host composition or external IO. Simple
collaborator interactions should use substitutes.

Use `Application` only when the application or host harness is the useful thing
under test, such as dependency injection wiring, middleware behavior,
authentication/authorization configuration, endpoint composition, or a
stateful in-memory fake that gives fast PR feedback for behavior that would be
awkward to cover with pure unit tests. Application tests must not depend on
real external IO.

Use `Integration` for behavior that verifies persistence, network, or other
external infrastructure boundaries. Integration tests should use real IO
through Testcontainers rather than an in-memory persistence stack.

Backend test method names should use PascalCase scenario segments separated by
underscores:

```csharp
public async Task Endpoint_WhenCondition_ReturnsExpectedResult()
```

Use the first segment for the behavior or API under test, the middle segment
for the relevant condition, and the final segment for the observable outcome.
Prefer this shape for new tests instead of sentence-style names.

On Linux, Testcontainers integration fixtures default to the standard rootless
Podman socket at `${XDG_RUNTIME_DIR}/podman/podman.sock`, or
`/run/user/<uid>/podman/podman.sock`, when `DOCKER_HOST` is not already set.
This is mainly for VS Code test runs, where the editor may not inherit shell
environment exports. Developers still need the Podman user socket running:

```sh
systemctl --user enable --now podman.socket
```

The default CI backend job restores, builds, and runs the `Unit` and
`Application` test categories with `--collect:"XPlat Code Coverage"`. Coverage
output is uploaded as a workflow artifact. `Integration` tests remain a local or
explicit validation band because they can require heavier infrastructure such
as Testcontainers.
