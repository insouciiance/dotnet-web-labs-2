using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Orderly.Application.Entities;
using Orderly.Application.Extensions;
using Orderly.Application.Interfaces;
using Orderly.Application.Models;

namespace Orderly.WebAPI.Controllers;

[Route("api/tickets")]
[ApiController]
public class TicketsController(IRepository<Ticket, Guid> ticketsRepo, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetTickets()
    {
        var tickets = ticketsRepo.List().Select(mapper.Map<TicketReadDto>);
        return Ok(tickets);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetTicketById([FromRoute] Guid id)
    {
        var ticket = ticketsRepo.Get(id);
        var readDto = mapper.Map<TicketReadDto>(ticket);
        return Ok(readDto);
    }

    [HttpPost]
    public IActionResult PostTicket([FromBody] TicketCreateDto createDto)
    {
        var ticket = mapper.Map<Ticket>(createDto);
        ticket.Id = Guid.NewGuid();
        ticket.Created = DateTime.Now;

        ticketsRepo.Add(ticket);

        var readDto = mapper.Map<TicketReadDto>(ticket);
        return Created(ticket.Id.ToString(), readDto);
    }
}
