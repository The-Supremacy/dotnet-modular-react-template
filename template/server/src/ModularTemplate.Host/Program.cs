using ModularTemplate.Host.Configuration;
using ModularTemplate.Host.Authorization;
using ModularTemplate.Host.Features.Auth;
using ModularTemplate.Host.Features.CurrentUser;
using ModularTemplate.Persistence.Configuration;
using ModularTemplate.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddPersistence();
builder.AddHostAuthentication();
builder.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddConfiguredModules();
builder.Services.AddApplicationAccessAuthorization();

var app = builder.Build();
app.UseProblemDetails();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.MapAuthEndpoints();
app.MapCurrentUserEndpoint();

app.Run();

public partial class Program;
