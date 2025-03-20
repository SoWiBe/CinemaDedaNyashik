using FirstMy.Bot.Models;
using FirstMy.Bot.Models.User;

namespace FirstMy.Bot.Services.Users;

public interface IUsersService
{
    Task<User?> GetUserAsync(long userId);
    Task<User?> CreateUserAsync(UserRequest? user);
}