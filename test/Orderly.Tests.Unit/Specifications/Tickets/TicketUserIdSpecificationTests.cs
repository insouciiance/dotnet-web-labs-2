using System;
using Orderly.Application.Specifications.Tickets;
using Xunit;

namespace Orderly.Tests.Unit.Specifications.Tickets;

public class TicketUserIdSpecificationTests : SpecificationTestBase
{
    [Fact]
    public void IfUserIdatches_Satisfies()
    {
        Guid guid = Guid.NewGuid();
        TicketUserIdSpecification spec = new(guid);
        AssertSatisfied(spec, new() { UserId = guid });
    }

    [Fact]
    public void IfUserIdDoesNotMatch_DoesNotSatisfy()
    {
        Guid guid = Guid.NewGuid();
        TicketUserIdSpecification spec = new(guid);
        AssertNotSatisfied(spec, new() { UserId = Guid.Empty });
    }
}
