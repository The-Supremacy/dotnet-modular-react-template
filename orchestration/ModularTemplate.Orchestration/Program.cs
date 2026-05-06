var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var database = postgres.AddDatabase("modular-template-host", "modular_template");
var sessionTickets = builder.AddRedis("session-tickets");

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithRealmImport("./Realms");

var migrator = builder.AddProject<Projects.ModularTemplate_Migrator>("migrator")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.ModularTemplate_Host>("host")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WithReference(sessionTickets)
    .WithEnvironment("Authentication__Oidc__Authority", "http://localhost:8080/realms/modular-template")
    .WithEnvironment("Authentication__Oidc__ClientId", "modular-template-host")
    .WithEnvironment("Authentication__Oidc__CallbackPath", "/auth/callback")
    .WithEnvironment("Authentication__Oidc__SignedOutCallbackPath", "/auth/signed-out")
    .WithEnvironment("Authentication__Oidc__RequireHttpsMetadata", "false")
    .WaitFor(database)
    .WaitFor(sessionTickets)
    .WaitFor(keycloak)
    .WaitForCompletion(migrator);

builder.Build().Run();
