using System;
using System.Linq.Expressions;
using Orderly.Application.Entities;

namespace Orderly.Application.Specifications.Tickets;

public class TicketStatusSpecification(TicketStatus status) : ISpecification<Ticket>
{
    public Expression<Func<Ticket, bool>> Expression => x => x.Status == status;
}
