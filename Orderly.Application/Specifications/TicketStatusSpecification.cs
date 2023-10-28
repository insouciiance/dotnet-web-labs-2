using Orderly.Application.Entities;

namespace Orderly.Application.Specifications;

public class TicketStatusSpecification(TicketStatus status) : ISpecification<Ticket>
{
    public bool IsSatisfiedBy(Ticket ticket) => ticket.Status == status;
}
