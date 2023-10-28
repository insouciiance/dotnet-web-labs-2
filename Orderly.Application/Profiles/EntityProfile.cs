using AutoMapper;
using Orderly.Application.Entities;
using Orderly.Application.Models;

namespace Orderly.Application.Profiles;

file class EntityProfile : Profile
{
    public EntityProfile()
    {
        CreateMap<Ticket, TicketCreateDto>().ReverseMap();

        CreateMap<Ticket, TicketReadDto>().ReverseMap();
    }
}
