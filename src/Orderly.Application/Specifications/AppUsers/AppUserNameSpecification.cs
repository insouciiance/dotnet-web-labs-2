using System;
using System.Linq.Expressions;
using Orderly.Application.Entities;

namespace Orderly.Application.Specifications.AppUsers;

public class AppUserNameSpecification(string username) : ISpecification<AppUser>
{
    public Expression<Func<AppUser, bool>> Expression => x => x.Username == username;
}
