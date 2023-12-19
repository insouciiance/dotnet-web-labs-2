using EntityFrameworkCore.Testing.Common.Helpers;
using EntityFrameworkCore.Testing.NSubstitute.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orderly.Infrastructure.Data.EntityFramework;
using Orderly.WebAPI.Startup;
using System;

namespace Orderly.Tests.Integration;

public class TestApplicationStartup : ApplicationStartup
{
    protected override void ConfigureDb(WebApplicationBuilder builder)
    {
        var context = ConfigureDb<AppDbContext>();
        builder.Services.AddSingleton<DbContext, AppDbContext>(_ => context.MockedDbContext);
    }

    private static IMockedDbContextBuilder<T> ConfigureDb<T>()
        where T : DbContext
    {
        var options = new DbContextOptionsBuilder<T>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContextToMock = (T)Activator.CreateInstance(typeof(T), options)!;

        return new MockedDbContextBuilder<T>()
            .UseDbContext(dbContextToMock)
            .UseConstructorWithParameters(options);
    }
}
