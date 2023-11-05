using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orderly.Application.Entities;
using Orderly.Application.Extensions;
using Orderly.Application.Models.Tickets;
using Orderly.Application.Repositories;
using Orderly.Application.Specifications;
using Orderly.Application.Specifications.Tickets;
using Orderly.WebAPI.Extensions;
using Orderly.WebAPI.Identity;

namespace Orderly.WebAPI.Controllers;

[Route("api/tickets")]
[ApiController]
public class TicketsController(IRepository<Ticket, Guid> ticketsRepo, IMapper mapper) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult GetTickets([FromQuery] TicketStatus? status = null)
    {
        Guid userId = User.GetUserId();

        ISpecification<Ticket> filter = status switch
        {
            { } s => new TicketStatusSpecification(s),
            _ => new TrueSpecification<Ticket>()
        };

        filter = filter.And(new TicketUserIdSpecification(userId));

        var tickets = ticketsRepo.List(filter).Select(mapper.Map<TicketReadDto>);
        return Ok(tickets);
    }

    [Authorize]
    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetTicketById([FromRoute] Guid id)
    {
        Guid userId = User.GetUserId();

        var ticket = ticketsRepo.Get(id);

        if (userId != ticket.UserId)
            return Forbid();

        var readDto = mapper.Map<TicketReadDto>(ticket);
        return Ok(readDto);
    }

    [Authorize]
    [HttpPost]
    public IActionResult CreateTicket([FromBody] TicketCreateDto createDto)
    {
        Guid userId = User.GetUserId();

        var ticket = mapper.Map<Ticket>(createDto);
        ticket.Id = Guid.NewGuid();
        ticket.Created = DateTime.Now;
        ticket.UserId = userId;

        ticketsRepo.Add(ticket);

        var readDto = mapper.Map<TicketReadDto>(ticket);
        return Created(ticket.Id.ToString(), readDto);
    }

    [Authorize]
    [HttpPut]
    [Route("{id:guid}")]
    public IActionResult PatchTicket([FromRoute] Guid id, [FromBody] TicketUpdateDto updateDto)
    {
        Guid userId = User.GetUserId();

        var ticket = ticketsRepo.Get(id);

        if (userId != ticket.UserId)
            return Forbid();

        ticket.Title = updateDto.Title;
        ticket.Description = updateDto.Description;
        ticket.Status = updateDto.Status;
        ticket.Deadline = updateDto.Deadline;
        ticket.ParentId = updateDto.ParentId;

        ticketsRepo.Update(ticket);

        return Ok(mapper.Map<TicketReadDto>(ticket));
    }

    [Authorize(Roles = IdentityRoles.ADMIN)]
    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult DeleteTicket([FromRoute] Guid id)
    {
        // admin can delete any ticket, don't check that user id matches
        var ticket = ticketsRepo.Get(id);

        ticketsRepo.Delete(ticket);
        var readDto = mapper.Map<TicketReadDto>(ticket);
        return Ok(readDto);
    }
}
