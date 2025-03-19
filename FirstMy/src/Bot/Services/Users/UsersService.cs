using FirstMy.Bot.Models;
using FirstMy.Bot.Services.Core;
using Microsoft.Extensions.Configuration;

namespace FirstMy.Bot.Services.Users;

public class UsersService : ApiService, IUsersService
{
    public UsersService(IConfiguration configuration, HttpClient httpClient) : base(configuration, httpClient)
    {
    }
    
    
    
    // public async Task<User> GetUserAsync(long userId)
    // {
    //     return await GetAsync<User>($"/users/{userId}");
    // }
    //
    public async Task<UserResponse> CreateUserAsync(UserRequest user)
    {
        return await PostAsync<UserResponse>("/api/Users", user);
    }
}