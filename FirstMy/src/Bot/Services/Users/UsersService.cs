using FirstMy.Bot.Models;
using FirstMy.Bot.Models.User;
using FirstMy.Bot.Services.Core;
using Microsoft.Extensions.Configuration;

namespace FirstMy.Bot.Services.Users;

public class UsersService : ApiService, IUsersService
{
    public UsersService(IConfigurationRoot root, HttpClient httpClient) : base(root, httpClient)
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