using FirstMy.Bot.Models;

namespace FirstMy.Bot.Services.Users;

public interface IUsersService
{
    Task<User> GetUserAsync(long userId);
    Task<User> CreateUserAsync(User user);
}