using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Orderly.Application.Models.Tickets;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Linq;

namespace Orderly.Tests.Integration.Tickets;

public class CreateTicketTests : TestBase
{
    private readonly HttpClient _client;

    public CreateTicketTests()
    {
        InitTestServer();
        _client = GetClient();
    }

    [Fact]
    public async Task Create_IfUserNotAuthenticated_ReturnsUnauthorized()
    {
        var inputModel = new TicketCreateDto
        {
            Title = "foo",
            Description = "baz"
        };

        var message = new HttpRequestMessage(HttpMethod.Post, "api/tickets")
        {
            Content = new StringContent(JsonSerializer.Serialize(inputModel), Encoding.UTF8, "application/json")
        };

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task Create_IfUserAuthenticated_SavesNewEntityInDb()
    {
        var user = CreateUser("user", "password");
        string token = GenerateToken(user);

        var inputModel = new TicketCreateDto
        {
            Title = "foo",
            Description = "baz"
        };

        var message = new HttpRequestMessage(HttpMethod.Post, "api/tickets")
        {
            Content = new StringContent(JsonSerializer.Serialize(inputModel), Encoding.UTF8, "application/json")
        };

        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.NotNull(result.Content);

        var ticket = JsonSerializer.Deserialize<TicketReadDto>(await result.Content.ReadAsStringAsync())!;
        Assert.NotNull(ticket);

        var savedEntity = _appDbContext.Tickets.FirstOrDefault(x => x.Title == inputModel.Title);
        Assert.NotNull(savedEntity);
    }
}
