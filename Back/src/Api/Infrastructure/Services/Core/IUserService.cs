using Back.Api.Infrastructure.Dto.Users;
using Back.Api.Models;

namespace Back.Api.Infrastructure.Services.Core;

public interface IUserService
{
    Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken);
    Task<User> GetById(long telegramUserId, CancellationToken cancellationToken);
    Task<User> Create(UserDto dto, CancellationToken cancellationToken);
    Task DeleteAll(CancellationToken cancellationToken);
    Task DeleteById(long telegramUserId, CancellationToken cancellationToken);
}