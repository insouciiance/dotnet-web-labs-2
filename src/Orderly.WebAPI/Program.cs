using Orderly.WebAPI.Startup;

namespace Orderly.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        ApplicationStartup startup = new();
        ApplicationStartupBuilder startupBuilder = new(startup);
        
        var appBuilder = startupBuilder.CreateBuilder(args);
        
        var app = startupBuilder.BuildApplication(appBuilder);
        
        app.Run();
    }
}
