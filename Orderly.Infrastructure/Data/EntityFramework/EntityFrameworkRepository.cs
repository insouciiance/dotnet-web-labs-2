using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Orderly.Application.Entities;
using Orderly.Application.Repositories;
using Orderly.Application.Specifications;

namespace Orderly.Infrastructure.Data.EntityFramework;

internal class EntityFrameworkRepository<T, TKey>(DbContext context) : IRepository<T, TKey>
    where T : class, IEntity<TKey>, new()
    where TKey : notnull
{
    public void Add(T entity)
    {
        context.Add(entity);
        context.SaveChanges();
    }

    public void Delete(TKey id)
    {
        T entity = new()
        {
            Id = id
        };

        context.Remove(entity);
        context.SaveChanges();
    }

    public T Get(TKey id)
    {
        T entity = context.Find<T>(id)!;
        return entity;
    }

    public IEnumerable<T> List(ISpecification<T> specification)
    {
        return context.Set<T>().Where(specification.Expression);
    }

    public void Update(T entity)
    {
        context.Update(entity);
        context.SaveChanges();
    }
}
