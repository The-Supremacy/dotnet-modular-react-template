using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularTemplate.Persistence;
using ModularTemplate.Persistence.Configuration;
using ModularTemplate.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.AddPersistence();

using IHost host = builder.Build();
using IServiceScope scope = host.Services.CreateScope();

ModularTemplateDbContext dbContext = scope.ServiceProvider.GetRequiredService<ModularTemplateDbContext>();
await dbContext.Database.MigrateAsync();
