﻿using Orderly.Application.Entities;

namespace Orderly.Application.Specifications;

public class AppUserNameSpecification(string username) : ISpecification<AppUser>
{
    public bool IsSatisfiedBy(AppUser user) => user.Username == username;
}
