using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orderly.Application.Entities;
using Orderly.Application.Identity;
using Orderly.Application.Repositories;
using Orderly.Infrastructure.Data.EntityFramework;
using Orderly.Infrastructure.Data.InMemory;
using Orderly.Infrastructure.Identity;

namespace Orderly.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryRepository<TEntity, TKey>(this IServiceCollection services)
        where TEntity : IEntity<TKey>
        where TKey : notnull
        => services.AddScoped<IRepository<TEntity, TKey>, InMemoryRepository<TEntity, TKey>>();

    public static IServiceCollection AddEfRepository<TEntity, TKey>(this IServiceCollection services)
        where TEntity : class, IEntity<TKey>, new()
        where TKey : notnull
        => services.AddScoped<IRepository<TEntity, TKey>, EntityFrameworkRepository<TEntity, TKey>>();

    public static IServiceCollection AddEfContext(this IServiceCollection services, string connectionString)
        => services.AddDbContext<DbContext, AppDbContext>(options => options.UseSqlServer(connectionString));

    public static IServiceCollection AddJwtGenerator(this IServiceCollection services)
        => services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
}
