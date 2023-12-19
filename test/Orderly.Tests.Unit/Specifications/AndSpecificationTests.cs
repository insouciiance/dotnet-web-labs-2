using Orderly.Application.Specifications;
using Xunit;

namespace Orderly.Tests.Unit.Specifications;

public class AndSpecificationTests : SpecificationTestBase
{
    [Fact]
    public void IfTrueAndTrue_Satisfies()
    {
        AndSpecification<object> spec = new(True<object>(), True<object>());
        AssertSatisfied(spec);
    }

    [Fact]
    public void IfTrueAndFalse_DoesNotSatisfy()
    {
        AndSpecification<object> spec = new(True<object>(), False<object>());
        AssertNotSatisfied(spec);
    }

    [Fact]
    public void IfFalseAndTrue_DoesNotSatisfy()
    {
        AndSpecification<object> spec = new(False<object>(), True<object>());
        AssertNotSatisfied(spec);
    }

    [Fact]
    public void IfFalseAndFalse_DoesNotSatisfy()
    {
        AndSpecification<object> spec = new(False<object>(), False<object>());
        AssertNotSatisfied(spec);
    }
}
