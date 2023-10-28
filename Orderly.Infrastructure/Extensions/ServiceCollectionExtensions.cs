using Microsoft.Extensions.DependencyInjection;
using Orderly.Application.Entities;
using Orderly.Application.Interfaces;
using Orderly.Infrastructure.Data.InMemory;

namespace Orderly.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInMemoryRepository<TEntity, TKey>(this IServiceCollection services)
        where TEntity : IEntity<TKey>
        where TKey : notnull
    {
        services.AddSingleton<IRepository<TEntity, TKey>, InMemoryRepository<TEntity, TKey>>();
    }
}
