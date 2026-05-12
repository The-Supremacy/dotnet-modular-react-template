using Shouldly;

namespace ModularTemplate.Migrator.Tests;

public sealed class MigratorCommandTests
{
    [Fact]
    [Trait("Category", "Unit")]
    public void TryParse_WhenNoArgs_UsesConfigurationOnly()
    {
        bool parsed = MigratorCommand.TryParse([], out MigratorCommand command, out string? error);

        parsed.ShouldBeTrue();
        error.ShouldBeNull();
        command.InitialAdmin.ShouldBeNull();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TryParse_WhenGrantAdminArgsAreComplete_ReturnsInitialAdmin()
    {
        bool parsed = MigratorCommand.TryParse(
            ["identity", "grant-admin", "--provider", "oidc", "--subject", "subject-1", "--force"],
            out MigratorCommand command,
            out string? error);

        parsed.ShouldBeTrue();
        error.ShouldBeNull();
        command.InitialAdmin.ShouldNotBeNull();
        command.InitialAdmin.Provider.ShouldBe("oidc");
        command.InitialAdmin.Subject.ShouldBe("subject-1");
        command.InitialAdmin.Force.ShouldBeTrue();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TryParse_WhenGrantAdminArgsAreIncomplete_ReturnsError()
    {
        bool parsed = MigratorCommand.TryParse(
            ["identity", "grant-admin", "--provider", "oidc"],
            out _,
            out string? error);

        parsed.ShouldBeFalse();
        error.ShouldBe("Both --provider and --subject are required.");
    }
}
