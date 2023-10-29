using System;
using Orderly.Application.Entities;

namespace Orderly.Application.Specifications.Tickets;

public class TicketUserIdSpecification(Guid userId) : ISpecification<Ticket>
{
    public bool IsSatisfiedBy(Ticket ticket) => ticket.UserId == userId;
}
