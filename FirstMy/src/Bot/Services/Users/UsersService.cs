using FirstMy.Bot.Models;
using FirstMy.Bot.Models.User;
using FirstMy.Bot.Services.Core;
using Microsoft.Extensions.Configuration;

namespace FirstMy.Bot.Services.Users;

public class UsersService : ApiService, IUsersService
{
    public UsersService(IConfiguration configuration, HttpClient httpClient) : base(configuration, httpClient)
    {
    }
    
    public async Task<UserResponse> GetUserAsync(long userId)
    {
        return await GetAsync<UserResponse>($"/api/Users/{userId}");
    }

    public async Task<UserResponse> CreateUserAsync(UserRequest user)
    {
        return await PostAsync<UserResponse>("/api/Users", user);
    }
}