using Orderly.Application.Models.Tickets;
using Orderly.Application.Entities;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using System;

namespace Orderly.Tests.Integration.Tickets;

public class GetTicketsTests : TestBase
{
    private readonly HttpClient _client;

    public GetTicketsTests()
    {
        InitTestServer();
        _client = GetClient();
    }

    [Fact]
    public async Task Get_IfUserAuthenticated_ReturnsAllTickets()
    {
        var user = CreateUser("user", "password");
        CreateTicket(user.Id, "foo");
        CreateTicket(user.Id, "baz");

        string token = GenerateToken(user);

        var message = new HttpRequestMessage(HttpMethod.Get, $"api/tickets");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Content);

        var returnedTickets = Deserialize<TicketReadDto[]>(result.Content);

        Assert.NotNull(returnedTickets);
        Assert.Equal(2, returnedTickets.Length);
    }

    [Fact]
    public async Task Get_IfUserNotAuthenticated_ReturnsUnauthorized()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, $"api/tickets");

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task GetWithFilter_IfUserAuthenticated_ReturnsFilteredTickets()
    {
        var user = CreateUser("user", "password");
        CreateTicket(user.Id, "foo", status: TicketStatus.Todo);
        CreateTicket(user.Id, "foo", status: TicketStatus.InProgress);
        CreateTicket(user.Id, "foo", status: TicketStatus.InProgress);
        CreateTicket(user.Id, "foo", status: TicketStatus.Implemented);

        string token = GenerateToken(user);

        var message = new HttpRequestMessage(HttpMethod.Get, $"api/tickets?status={nameof(TicketStatus.InProgress)}");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Content);

        var returnedTickets = Deserialize<TicketReadDto[]>(result.Content);

        Assert.NotNull(returnedTickets);
        Assert.Equal(2, returnedTickets.Length);
        Assert.True(Array.TrueForAll(returnedTickets, x => x.Status == TicketStatus.InProgress));
    }
}
