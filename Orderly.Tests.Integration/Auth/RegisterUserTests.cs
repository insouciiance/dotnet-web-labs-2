using Orderly.Application.Models.AppUsers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Orderly.Tests.Integration.Auth;

public class RegisterUserTests : TestBase
{
    private readonly HttpClient _client;

    public RegisterUserTests()
    {
        InitTestServer();
        _client = GetClient();
    }

    [Fact]
    public async Task Register_IfCredentialsValid_SavesNewEntityInDb()
    {
        var inputModel = new AppUserCreateDto
        {
            Username = "foo",
            Password = "bar"
        };

        var message = new HttpRequestMessage(HttpMethod.Post, "api/auth/register")
        {
            Content = new StringContent(JsonSerializer.Serialize(inputModel), Encoding.UTF8, "application/json")
        };

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Content);

        var responseModel = JsonSerializer.Deserialize<AppUserReadDto>(await result.Content.ReadAsStringAsync())!;
        Assert.NotNull(responseModel);
        
        var savedEntity = _appDbContext.Users.FirstOrDefault(x => x.Username == inputModel.Username);
        Assert.NotNull(savedEntity);
    }

    [Fact]
    public async Task Register_IfUsernameExists_ReturnsBadRequest()
    {
        var user = CreateUser("foo", "");

        var inputModel = new AppUserCreateDto
        {
            Username = "foo",
            Password = "bar"
        };

        var message = new HttpRequestMessage(HttpMethod.Post, "api/auth/register")
        {
            Content = new StringContent(JsonSerializer.Serialize(inputModel), Encoding.UTF8, "application/json")
        };

        var result = await _client.SendAsync(message);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        var savedEntity = _appDbContext.Users.FirstOrDefault(x => x.Username == inputModel.Username);
        Assert.NotNull(savedEntity);
        Assert.Equal(user.Id, savedEntity.Id);
    }
}
