using System;
using System.Linq.Expressions;

namespace Orderly.Application.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Expression { get; }

    bool IsSatisfiedBy(T entity) => Expression.Compile().Invoke(entity);
}
