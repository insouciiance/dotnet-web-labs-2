using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Orderly.Application.Entities;
using Orderly.Application.Models;
using Orderly.Application.Repositories;
using Orderly.Application.Specifications;
using Crypto = BCrypt.Net.BCrypt;

namespace Orderly.WebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IRepository<AppUser, Guid> usersRepo, IMapper mapper) : ControllerBase
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

        return Ok();
    }
}
