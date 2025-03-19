using FirstMy.Bot.Models;
using FirstMy.Bot.Models.User;

namespace FirstMy.Bot.Services.Users;

public interface IUsersService
{
    Task<UserResponse> GetUserAsync(long userId);
    Task<UserResponse> CreateUserAsync(UserRequest user);
}