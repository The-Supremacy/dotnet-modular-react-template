using Testcontainers.PostgreSql;

using ModularTemplate.Tests.Support;

namespace ModularTemplate.Migrator.Tests.Support;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public PostgreSqlFixture()
    {
        ContainerRuntimeDefaults.Apply();

        _container = new PostgreSqlBuilder("docker.io/library/postgres:17-alpine")
            .WithDatabase("modular_template_migrator_tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public string ConnectionString => _container.GetConnectionString();

    public Task InitializeAsync()
    {
        return _container.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _container.DisposeAsync().AsTask();
    }
}
