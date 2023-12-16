using System;

namespace Orderly.Application.Models.AppUsers;

public class AppUserReadDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
}
