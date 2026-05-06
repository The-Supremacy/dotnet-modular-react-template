namespace ModularTemplate.Host.Configuration;

public static class HostAuthenticationConfiguration
{
    public static WebApplicationBuilder AddHostAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        return builder;
    }
}
