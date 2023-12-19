using Orderly.Application.Entities;

namespace Orderly.Application.Identity;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser user, string role);
}
