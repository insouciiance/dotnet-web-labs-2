using Microsoft.AspNetCore.Builder;

namespace Orderly.WebAPI.Startup;

public class ApplicationStartupBuilder(IApplicationStartup startup)
{
    public WebApplicationBuilder CreateBuilder(string[]? args = null)
    {
        args ??= [];
        var builder = WebApplication.CreateBuilder(args);
        startup.ConfigureBuilder(builder);
        return builder;
    }

    public WebApplication BuildApplication(WebApplicationBuilder builder)
    {
        var app = builder.Build();
        startup.ConfigureApplication(app);
        return app;
    }
}
