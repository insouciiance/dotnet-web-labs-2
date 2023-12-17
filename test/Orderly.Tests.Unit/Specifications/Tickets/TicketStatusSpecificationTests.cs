using Orderly.Application.Entities;
using Orderly.Application.Specifications.Tickets;
using Xunit;

namespace Orderly.Tests.Unit.Specifications.Tickets;

public class TicketStatusSpecificationTests : SpecificationTestBase
{
    [Fact]
    public void IfStatusMatches_Satisfies()
    {
        TicketStatusSpecification spec = new(TicketStatus.Implemented);
        AssertSatisfied(spec, new() { Status = TicketStatus.Implemented });
    }

    [Fact]
    public void IfStatusDoesNotMatch_DoesNotSatisfy()
    {
        TicketStatusSpecification spec = new(TicketStatus.Implemented);
        AssertNotSatisfied(spec, new() { Status = TicketStatus.Todo });
    }
}
