using Orderly.Application.Specifications.AppUsers;
using Xunit;

namespace Orderly.Tests.Unit.Specifications.AppUsers;

public class AppUserNameSpecificationTests : SpecificationTestBase
{
    [Fact]
    public void IfNameMatches_Satisfies()
    {
        AppUserNameSpecification spec = new("John");
        AssertSatisfied(spec, new() { Username = "John" });
    }

    [Fact]
    public void IfNameDoesNotMatch_DoesNotSatisfy()
    {
        AppUserNameSpecification spec = new("John");
        AssertNotSatisfied(spec, new() { Username = "Doe" });
    }
}
