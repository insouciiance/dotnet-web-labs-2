using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Orderly.Application.Entities;
using Orderly.Application.Identity;

namespace Orderly.Infrastructure.Identity;

internal class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    private static readonly TimeSpan ExpirationSpan = TimeSpan.FromHours(1);

    public string GenerateToken(AppUser user, string role)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, role)
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration[IdentityConstants.CONFIG_SECTION_KEY]!));

        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new(
            issuer: configuration[IdentityConstants.CONFIG_SECTION_ISSUER],
            audience: configuration[IdentityConstants.CONFIG_SECTION_AUDIENCE],
            claims: claims,
            expires: DateTime.Now + ExpirationSpan,
            signingCredentials: credentials);
    
        string jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}
