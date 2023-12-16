using System;
using System.Linq.Expressions;

namespace Orderly.Application.Specifications;

public class FalseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Expression => _ => false;
}
