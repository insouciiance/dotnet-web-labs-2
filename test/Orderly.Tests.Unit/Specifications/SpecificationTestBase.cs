using Orderly.Application.Specifications;
using Xunit;

namespace Orderly.Tests.Unit.Specifications;

public class SpecificationTestBase
{
    protected TrueSpecification<T> True<T>() => new();

    protected FalseSpecification<T> False<T>() => new();

    protected void AssertSatisfied<T>(ISpecification<T> spec, T value)
    {
        bool satisfies = spec.IsSatisfiedBy(value);
        Assert.True(satisfies);
    }

    protected void AssertSatisfied<T>(ISpecification<T> spec)
    {
        bool satisfies = spec.IsSatisfiedBy(default!);
        Assert.True(satisfies);
    }

    protected void AssertNotSatisfied<T>(ISpecification<T> spec, T value)
    {
        bool satisfies = spec.IsSatisfiedBy(value);
        Assert.False(satisfies);
    }

    protected void AssertNotSatisfied<T>(ISpecification<T> spec)
    {
        bool satisfies = spec.IsSatisfiedBy(default!);
        Assert.False(satisfies);
    }
}
