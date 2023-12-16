using System;
using System.Collections.Generic;

namespace Orderly.Application.Entities;

public class AppUser : IEntity<Guid>
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public virtual List<Ticket> Tickets { get; set; } = null!;
}
