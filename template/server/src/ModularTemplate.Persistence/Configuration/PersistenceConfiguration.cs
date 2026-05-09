using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularTemplate.Identity.Infrastructure.Persistence;
using ModularTemplate.Persistence;

namespace ModularTemplate.Persistence.Configuration;

public static class PersistenceConfiguration
{
    private const string ConnectionStringName = "modular-template-host";

    public static TBuilder AddPersistence<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        string connectionString = builder.Configuration[$"ConnectionStrings:{ConnectionStringName}"]
            ?? throw new InvalidOperationException(
                $"Connection string 'ConnectionStrings:{ConnectionStringName}' is required.");

        builder.Services.AddDbContext<ModularTemplateDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "host"));
        });
        builder.Services.AddScoped<IIdentityDbContext>(
            services => services.GetRequiredService<ModularTemplateDbContext>());

        return builder;
    }
}
