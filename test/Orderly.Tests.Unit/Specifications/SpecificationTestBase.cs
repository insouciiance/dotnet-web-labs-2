using Orderly.Application.Specifications;
using Xunit;

namespace Orderly.Tests.Unit.Specifications;

public class SpecificationTestBase
{
    protected static TrueSpecification<T> True<T>() => new();

    protected static FalseSpecification<T> False<T>() => new();

    protected static void AssertSatisfied<T>(ISpecification<T> spec, T value)
    {
        bool satisfies = spec.IsSatisfiedBy(value);
        Assert.True(satisfies);
    }

    protected static void AssertSatisfied<T>(ISpecification<T> spec)
    {
        bool satisfies = spec.IsSatisfiedBy(default!);
        Assert.True(satisfies);
    }

    protected static void AssertNotSatisfied<T>(ISpecification<T> spec, T value)
    {
        bool satisfies = spec.IsSatisfiedBy(value);
        Assert.False(satisfies);
    }

    protected static void AssertNotSatisfied<T>(ISpecification<T> spec)
    {
        bool satisfies = spec.IsSatisfiedBy(default!);
        Assert.False(satisfies);
    }
}
