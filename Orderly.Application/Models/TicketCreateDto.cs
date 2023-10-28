using System;

namespace Orderly.Application.Models;

public class TicketCreateDto
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime Deadline { get; set; }
}
