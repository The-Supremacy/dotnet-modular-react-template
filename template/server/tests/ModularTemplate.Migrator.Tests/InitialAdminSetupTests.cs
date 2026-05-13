using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ModularTemplate.Identity.Access;
using ModularTemplate.Identity.Infrastructure.Persistence;
using ModularTemplate.Migrator.Tests.Support;
using Shouldly;

namespace ModularTemplate.Migrator.Tests;

public sealed class InitialAdminSetupTests(PostgreSqlFixture fixture)
    : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task RunAsync_WhenInitialAdminIsConfigured_MigratesAndGrantsAccessIdempotently()
    {
        using IHost host = CreateHost();
        using var output = new StringWriter();
        using var error = new StringWriter();

        int firstExitCode = await MigratorRunner.RunAsync(
            [],
            host.Services.GetRequiredService<IConfiguration>(),
            host.Services,
            output,
            error,
            CancellationToken.None);
        int secondExitCode = await MigratorRunner.RunAsync(
            [],
            host.Services.GetRequiredService<IConfiguration>(),
            host.Services,
            output,
            error,
            CancellationToken.None);

        firstExitCode.ShouldBe(0);
        secondExitCode.ShouldBe(0);
        await using AsyncServiceScope scope = host.Services.CreateAsyncScope();
        IIdentityDbContext dbContext = scope.ServiceProvider.GetRequiredService<IIdentityDbContext>();
        var user = dbContext.LocalUsers.Single(x => x.Provider == "oidc" && x.Subject == "subject-1");
        dbContext.ApplicationAccess.Count(x => x.LocalUserId == user.Id).ShouldBe(1);
        dbContext.ApplicationAccess.Single(x => x.LocalUserId == user.Id).IsActive.ShouldBeTrue();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task RunAsync_WhenInitialAdminWasRevoked_ReturnsFailureUntilForced()
    {
        using IHost host = CreateHost();
        using var output = new StringWriter();
        using var error = new StringWriter();
        await MigratorRunner.RunAsync(
            [],
            host.Services.GetRequiredService<IConfiguration>(),
            host.Services,
            output,
            error,
            CancellationToken.None);
        await using (AsyncServiceScope scope = host.Services.CreateAsyncScope())
        {
            IIdentityDbContext dbContext = scope.ServiceProvider.GetRequiredService<IIdentityDbContext>();
            ApplicationAccess access = dbContext.ApplicationAccess.Single();
            access.Revoke();
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        int blockedExitCode = await MigratorRunner.RunAsync(
            [],
            host.Services.GetRequiredService<IConfiguration>(),
            host.Services,
            output,
            error,
            CancellationToken.None);
        int forcedExitCode = await MigratorRunner.RunAsync(
            ["identity", "grant-admin", "--provider", "oidc", "--subject", "subject-1", "--force"],
            host.Services.GetRequiredService<IConfiguration>(),
            host.Services,
            output,
            error,
            CancellationToken.None);

        blockedExitCode.ShouldBe(1);
        forcedExitCode.ShouldBe(0);
        await using AsyncServiceScope verifyScope = host.Services.CreateAsyncScope();
        IIdentityDbContext verifyDbContext = verifyScope.ServiceProvider.GetRequiredService<IIdentityDbContext>();
        verifyDbContext.ApplicationAccess.Single().IsActive.ShouldBeTrue();
    }

    private IHost CreateHost()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Configuration["ConnectionStrings:modular-template-host"] = fixture.ConnectionString;
        builder.Configuration["Identity:InitialAdmin:Provider"] = "oidc";
        builder.Configuration["Identity:InitialAdmin:Subject"] = "subject-1";
        builder.AddMigratorComposition();

        return builder.Build();
    }
}
