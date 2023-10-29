using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orderly.Application.Entities;
using Orderly.Application.Models;
using Orderly.Application.Repositories;
using Orderly.Application.Specifications;

namespace Orderly.WebAPI.Controllers;

[Route("api/tickets")]
[ApiController]
public class TicketsController(IRepository<Ticket, Guid> ticketsRepo, IMapper mapper) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult GetTickets([FromQuery] TicketStatus? status = null)
    {
        ISpecification<Ticket> filter = status switch
        {
            { } s => new TicketStatusSpecification(s),
            _ => new TrueSpecification<Ticket>()
        };

        var tickets = ticketsRepo.List(filter).Select(mapper.Map<TicketReadDto>);
        return Ok(tickets);
    }

    [Authorize]
    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetTicketById([FromRoute] Guid id)
    {
        var ticket = ticketsRepo.Get(id);
        var readDto = mapper.Map<TicketReadDto>(ticket);
        return Ok(readDto);
    }

    [Authorize]
    [HttpPost]
    public IActionResult CreateTicket([FromBody] TicketCreateDto createDto)
    {
        var ticket = mapper.Map<Ticket>(createDto);
        ticket.Id = Guid.NewGuid();
        ticket.Created = DateTime.Now;

        ticketsRepo.Add(ticket);

        var readDto = mapper.Map<TicketReadDto>(ticket);
        return Created(ticket.Id.ToString(), readDto);
    }
}
