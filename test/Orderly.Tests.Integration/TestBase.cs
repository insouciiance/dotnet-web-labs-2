using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orderly.Application.Entities;
using Orderly.Application.Identity;
using Orderly.Infrastructure.Data.EntityFramework;
using Orderly.WebAPI.Identity;
using Orderly.WebAPI.Startup;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Crypto = BCrypt.Net.BCrypt;

namespace Orderly.Tests.Integration;

public abstract class TestBase : IAsyncDisposable
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

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

    protected AppUser CreateUser(string username, string password)
    {
        Guid id = Guid.NewGuid();

        AppUser user = new()
        {
            Id = id,
            Username = username,
            PasswordHash = Crypto.HashPassword(password)
        };

        _appDbContext.Users.Add(user);
        _appDbContext.SaveChanges();

        return user;
    }

    protected Ticket CreateTicket(Guid userId, string title, string description = "", TicketStatus status = TicketStatus.Todo)
    {
        Guid id = Guid.NewGuid();

        Ticket ticket = new()
        {
            Id = id,
            Created = DateTime.UtcNow,
            Title = title,
            Description = description,
            Status = status,
            UserId = userId
        };

        _appDbContext.Tickets.Add(ticket);
        _appDbContext.SaveChanges();

        return ticket;
    }

    protected string GenerateToken(AppUser user, string role = "")
    {
        var generator = _host.Services.GetService<IJwtTokenGenerator>()!;
        string token = generator.GenerateToken(user, role);
        return token;
    }

    protected static JwtSecurityToken DecodeJwt(string token)
    {
        JwtSecurityTokenHandler handler = new();
        var decoded = handler.ReadToken(token);
        return (JwtSecurityToken)decoded;
    }

    protected static string? GetClaim(JwtSecurityToken token, string type)
    {
        return token.Claims.FirstOrDefault(x => x.Type == type)?.Value;
    }

    protected static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value);
    }

    protected static T Deserialize<T>(HttpContent content)
    {
        return JsonSerializer.Deserialize<T>(content.ReadAsStringAsync().Result, _options)!;
    }
}
