using Orderly.Application.Specifications;

namespace Orderly.Application.Extensions;

public static class SpecificationExtensions
{
    public static ISpecification<T> And<T>(this ISpecification<T> specification, ISpecification<T> other)
        => new AndSpecification<T>(specification, other);

    public static ISpecification<T> Or<T>(this ISpecification<T> specification, ISpecification<T> other)
        => new OrSpecification<T>(specification, other);
}
