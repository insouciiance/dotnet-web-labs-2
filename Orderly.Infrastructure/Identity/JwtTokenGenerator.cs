using Microsoft.Extensions.Configuration;
using Orderly.Application.Entities;
using Orderly.Application.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace Orderly.Infrastructure.Identity;

internal class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    public string GenerateToken(AppUser user)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        ];

        
    }
}
