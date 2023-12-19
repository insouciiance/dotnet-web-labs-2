using Orderly.Application.Specifications;
using Xunit;

namespace Orderly.Tests.Unit.Specifications;

public class OrSpecificationTests : SpecificationTestBase
{
    [Fact]
    public void IfTrueAndTrue_Satisfies()
    {
        OrSpecification<object> spec = new(True<object>(), True<object>());
        AssertSatisfied(spec);
    }

    [Fact]
    public void IfTrueAndFalse_Satisfies()
    {
        OrSpecification<object> spec = new(True<object>(), False<object>());
        AssertSatisfied(spec);
    }

    [Fact]
    public void IfFalseAndTrue_Satisfies()
    {
        OrSpecification<object> spec = new(False<object>(), True<object>());
        AssertSatisfied(spec);
    }

    [Fact]
    public void IfFalseAndFalse_DoesNotSatisfy()
    {
        OrSpecification<object> spec = new(False<object>(), False<object>());
        AssertNotSatisfied(spec);
    }
}
