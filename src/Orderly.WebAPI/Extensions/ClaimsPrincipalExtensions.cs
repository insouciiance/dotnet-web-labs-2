using System;
using System.Linq;
using System.Security.Claims;

namespace Orderly.WebAPI.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;

        string identifierName = identity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return Guid.Parse(identifierName);
    }
}
