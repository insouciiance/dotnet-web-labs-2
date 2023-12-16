using System;
using System.Collections.Generic;

namespace Orderly.Application.Entities;

public class Ticket : IEntity<Guid>
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TicketStatus Status { get; set; }

    public DateTime Created { get; set; }

    public DateTime Deadline { get; set; }

    public Guid UserId { get; set; }

    public Guid? ParentId { get; set; }

    public virtual AppUser User { get; set; } = null!;

    public virtual Ticket Parent { get; set; } = null!;

    public virtual List<Ticket> Children { get; set; } = null!;
}
