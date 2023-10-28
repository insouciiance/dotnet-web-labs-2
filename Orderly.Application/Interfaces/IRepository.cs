using System.Collections.Generic;
using Orderly.Application.Entities;
using Orderly.Application.Specifications;

namespace Orderly.Application.Interfaces;

public interface IRepository<T, TKey>
    where T : IEntity<TKey>
    where TKey : notnull
{
    T Get(TKey id);

    void Add(T entity);

    void Delete(T entity);

    void Update(T entity);

    IEnumerable<T> List(ISpecification<T> specification);
}
