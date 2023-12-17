using Orderly.Application.Models.AppUsers;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Orderly.Tests.Integration.Auth;

public class LoginUserTests : TestBase
{
    private readonly HttpClient _client;

    public LoginUserTests()
    {
        InitTestServer();
        _client = GetClient();
    }

    [Fact]
    public async Task Login_IfCredentialsValid_ReturnsValidJwt()
    {
        var user = CreateUser("foo", "baz");

        var inputModel = new AppUserLoginDto
        {
            Username = "foo",
            Password = "baz"
        };

        var message = new HttpRequestMessage(HttpMethod.Post, "api/auth/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(inputModel), Encoding.UTF8, "application/json")
        };

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Content);

        string jwtString = await result.Content.ReadAsStringAsync();
        var jwt = DecodeJwt(jwtString);

        Assert.Equal(user.Id.ToString(), GetClaim(jwt, ClaimTypes.NameIdentifier));
        Assert.Equal(user.Username, GetClaim(jwt, ClaimTypes.Name));
    }

    [Fact]
    public async Task Login_IfUserDoesNotExist_ReturnsBadRequest()
    {
        var inputModel = new AppUserLoginDto
        {
            Username = "foo",
            Password = "baz"
        };

        var message = new HttpRequestMessage(HttpMethod.Post, "api/auth/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(inputModel), Encoding.UTF8, "application/json")
        };

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task Login_IfPassswordInvalid_ReturnsBadRequest()
    {
        CreateUser("foo", "bar");

        var inputModel = new AppUserLoginDto
        {
            Username = "foo",
            Password = "baz"
        };

        var message = new HttpRequestMessage(HttpMethod.Post, "api/auth/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(inputModel), Encoding.UTF8, "application/json")
        };

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }
}
