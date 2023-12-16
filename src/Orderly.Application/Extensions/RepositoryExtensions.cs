using System.Collections.Generic;
using Orderly.Application.Entities;
using Orderly.Application.Repositories;
using Orderly.Application.Specifications;

namespace Orderly.Application.Extensions;

public static class RepositoryExtensions
{
    public static IEnumerable<T> List<T, TKey>(this IRepository<T, TKey> repository)
        where T : IEntity<TKey>
        where TKey : notnull
        => repository.List(new TrueSpecification<T>());
}
