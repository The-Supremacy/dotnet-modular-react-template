using Microsoft.Extensions.Hosting;
using ModularTemplate.Migrator;
using ModularTemplate.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.AddMigratorComposition();

using IHost host = builder.Build();

return await MigratorRunner.RunAsync(
    args,
    builder.Configuration,
    host.Services,
    Console.Out,
    Console.Error,
    CancellationToken.None);
