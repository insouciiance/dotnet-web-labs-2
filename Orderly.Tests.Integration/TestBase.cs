using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orderly.Infrastructure.Data.EntityFramework;
using Orderly.WebAPI.Startup;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Orderly.Tests.Integration;

public abstract class TestBase : IAsyncDisposable
{
    private ApplicationStartupBuilder _startupBuilder = null!;

    protected WebApplication _host = null!;

    protected WebApplicationBuilder _server = null!;

    private protected AppDbContext _appDbContext = null!;

    public async ValueTask DisposeAsync()
    {
        await _host.StopAsync();
        await _host.DisposeAsync();
    }

    public virtual HttpClient GetClient()
    {
        Assert.NotNull(_server);

        _host = _startupBuilder.BuildApplication(_server);
        _host.Start();
        _appDbContext = (AppDbContext)_host.Services.GetService<DbContext>()!;
        return _host.GetTestClient();
    }

    protected void InitTestServer()
    {
        _startupBuilder = new(new TestApplicationStartup());
        _server = _startupBuilder.CreateBuilder();
        _server.WebHost.UseTestServer();
    }
}
