using System;
using System.Linq.Expressions;

namespace Orderly.Application.Specifications;

public class TrueSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Expression => _ => true;
}
