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

Integration tests should use real IO through Testcontainers rather than an
in-memory persistence stack.

The default CI backend job runs `dotnet test ModularTemplate.slnx` on a Linux
runner with Docker support so Testcontainers-backed integration tests are part
of the normal verification surface.
