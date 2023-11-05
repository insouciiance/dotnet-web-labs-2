using System;
using System.Linq.Expressions;
using Orderly.Application.Entities;

namespace Orderly.Application.Specifications.Tickets;

public class TicketUserIdSpecification(Guid userId) : ISpecification<Ticket>
{
    public Expression<Func<Ticket, bool>> Expression => x => x.UserId == userId;
}
