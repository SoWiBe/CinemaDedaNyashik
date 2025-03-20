using FirstMy.Bot.Models.User;
using FirstMy.Bot.Services.Core;
using Microsoft.Extensions.Configuration;

namespace FirstMy.Bot.Services.Users;

public class UsersService : ApiService, IUsersService
{
    public UsersService(IConfiguration configuration, HttpClient httpClient) : base(configuration, httpClient)
    {
    }
    
    public async Task<User?> GetUserAsync(long userId)
    {
        return await GetAsync<User?>($"/api/Users/{userId}");
    }

    public async Task<User?> CreateUserAsync(UserRequest? user)
    {
        return await PostAsync<User?>("/api/Users", user);
    }
}