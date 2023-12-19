using Orderly.Application.Models.Tickets;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using System;

namespace Orderly.Tests.Integration.Tickets;

public class GetTicketTests : TestBase
{
    private readonly HttpClient _client;

    public GetTicketTests()
    {
        InitTestServer();
        _client = GetClient();
    }

    [Fact]
    public async Task GetById_IfTicketExists_AndUserMatches_ReturnsOK()
    {
        var user = CreateUser("user", "password");
        var ticket = CreateTicket(user.Id, "foo");

        string token = GenerateToken(user);

        var message = new HttpRequestMessage(HttpMethod.Get, $"api/tickets/{ticket.Id}");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Content);

        var returnedTicket = Deserialize<TicketReadDto>(result.Content);

        Assert.NotNull(returnedTicket);

        Assert.Equal(ticket.Id, returnedTicket.Id);
    }

    [Fact]
    public async Task GetById_IfTicketExists_AndUserDifferent_ReturnsForbidden()
    {
        var user1 = CreateUser("user", "password");
        var user2 = CreateUser("user2", "password");
        var ticket = CreateTicket(user1.Id, "foo");

        string token = GenerateToken(user2);

        var message = new HttpRequestMessage(HttpMethod.Get, $"api/tickets/{ticket.Id}");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
    }

    [Fact]
    public async Task GetById_IfTicketDoesNotExist_ReturnsNotFound()
    {
        var user = CreateUser("user", "password");

        string token = GenerateToken(user);

        var message = new HttpRequestMessage(HttpMethod.Get, $"api/tickets/{Guid.NewGuid()}");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task GetById_IfTicketDoesNotExist_AndUserNotAuthenticated_ReturnsUnauthorized()
    {
        var user = CreateUser("user", "password");
        var ticket = CreateTicket(user.Id, "foo");

        var message = new HttpRequestMessage(HttpMethod.Get, $"api/tickets/{ticket.Id}");

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task GetById_IfTicketExists_AndUserNotAuthenticated_ReturnsUnauthorized()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, $"api/tickets/{Guid.NewGuid()}");

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }
}
