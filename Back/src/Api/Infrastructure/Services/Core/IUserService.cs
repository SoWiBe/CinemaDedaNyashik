using Back.Api.Infrastructure.Dto.Users;
using Back.Api.Models;

namespace Back.Api.Infrastructure.Services.Core;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User Create(UserDto dto);
}