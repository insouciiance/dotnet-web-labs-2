using System;
using System.Collections.Generic;
using System.Linq;
using Orderly.Application.Entities;
using Orderly.Application.Repositories;
using Orderly.Application.Specifications;

namespace Orderly.Infrastructure.Data.InMemory;

internal class InMemoryRepository<T, TKey> : IRepository<T, TKey>
    where T : IEntity<TKey>
    where TKey : notnull
{
    private readonly Dictionary<TKey, T> _entries = new();

    public void Add(T entity) => _entries.Add(entity.Id, entity);

    public T Get(TKey id) => _entries[id];

    public void Update(T entity) => _entries[entity.Id] = entity;

    public void Delete(TKey id) => _entries.Remove(id);

    public IEnumerable<T> List(ISpecification<T> specification)
        => _entries.Values.Where(specification.IsSatisfiedBy);
}
