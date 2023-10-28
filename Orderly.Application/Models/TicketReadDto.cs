using System;

namespace Orderly.Application.Models;

public class TicketReadDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime Created { get; set; }

    public DateTime Deadline { get; set; }
}
