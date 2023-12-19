using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Orderly.Tests.Integration.Tickets;

public class DeleteTicketTests : TestBase
{
    private readonly HttpClient _client;

    public DeleteTicketTests()
    {
        InitTestServer();
        _client = GetClient();
    }

    [Fact]
    public async Task Delete_IfUserAuthenticated_AndTicketMatches_ReturnsForbidden()
    {
        var user = CreateUser("user", "password");
        var ticket = CreateTicket(user.Id, "foo");

        string token = GenerateToken(user);

        var message = new HttpRequestMessage(HttpMethod.Delete, $"api/tickets/{ticket.Id}");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
    }

    [Fact]
    public async Task Delete_IfUserNotAuthenticated_ReturnsUnauthorized()
    {
        var user = CreateUser("user", "password");
        var ticket = CreateTicket(user.Id, "foo");

        var message = new HttpRequestMessage(HttpMethod.Delete, $"api/tickets/{ticket.Id}");

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task Delete_IfUserAdmin_AndTicketMatches_ReturnsOK()
    {
        var user = CreateUser("user", "password");
        var ticket = CreateTicket(user.Id, "foo");

        string token = GenerateToken(user, "admin");

        var message = new HttpRequestMessage(HttpMethod.Delete, $"api/tickets/{ticket.Id}");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Content);

        Assert.Empty(_appDbContext.Tickets);
    }

    [Fact]
    public async Task Delete_IfUserAdmin_AndTicketDoesNotMatch_ReturnsOK()
    {
        var user1 = CreateUser("user", "password");
        var user2 = CreateUser("user", "password");
        var ticket = CreateTicket(user2.Id, "foo");

        string token = GenerateToken(user1, "admin");

        var message = new HttpRequestMessage(HttpMethod.Delete, $"api/tickets/{ticket.Id}");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Content);

        Assert.Empty(_appDbContext.Tickets);
    }
}
