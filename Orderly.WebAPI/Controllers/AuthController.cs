using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Orderly.Application.Entities;
using Orderly.Application.Identity;
using Orderly.Application.Models;
using Orderly.Application.Repositories;
using Orderly.Application.Specifications.AppUsers;
using Orderly.WebAPI.Identity;
using Crypto = BCrypt.Net.BCrypt;

namespace Orderly.WebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IRepository<AppUser, Guid> usersRepo, IJwtTokenGenerator tokenGenerator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public IActionResult Register([FromBody] AppUserCreateDto userDto)
    {
        if (usersRepo.List(new AppUserNameSpecification(userDto.Username)).Any())
            return BadRequest("User already exists");

        AppUser user = new()
        {
            Id = Guid.NewGuid(),
            Username = userDto.Username,
            PasswordHash = Crypto.HashPassword(userDto.Password)
        };

        usersRepo.Add(user);
    
        return Ok(mapper.Map<AppUserReadDto>(user));
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] AppUserLoginDto userDto)
    {
        if (usersRepo.List(new AppUserNameSpecification(userDto.Username)).FirstOrDefault() is not { } user)
            return BadRequest("User not found");

        if (!Crypto.Verify(userDto.Password, user.PasswordHash))
            return BadRequest("Invalid password");

        // assume "admin" is an admin
        string role = user.Username == IdentityRoles.ADMIN ? IdentityRoles.ADMIN : string.Empty;

        string jwt = tokenGenerator.GenerateToken(user, role);

        return Ok(jwt);
    }
}
