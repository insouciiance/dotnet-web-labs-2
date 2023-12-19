using Microsoft.AspNetCore.Builder;

namespace Orderly.WebAPI.Startup;

public interface IApplicationStartup
{
    void ConfigureBuilder(WebApplicationBuilder builder);

    void ConfigureApplication(WebApplication app);
}
