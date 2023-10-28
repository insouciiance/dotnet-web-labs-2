using System;
using Orderly.Application.Entities;

namespace Orderly.Application.Models;

public class TicketCreateDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TicketStatus Status { get; set; }

    public DateTime Deadline { get; set; }
}
