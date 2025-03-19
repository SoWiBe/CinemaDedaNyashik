using FirstMy.Bot.Models;

namespace FirstMy.Bot.Services.Users;

public interface IUsersService
{
    // Task<UserRequest> GetUserAsync(long userId);
    Task<UserResponse> CreateUserAsync(UserRequest user);
}