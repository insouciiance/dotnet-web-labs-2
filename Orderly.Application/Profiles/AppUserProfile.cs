using AutoMapper;
using Orderly.Application.Entities;
using Orderly.Application.Models;

namespace Orderly.Application.Profiles;

file class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<AppUser, AppUserReadDto>();
    }
}
