using Back.Api.Infrastructure.Dto.Users;
using Back.Api.Infrastructure.Repository;
using Back.Api.Models;

namespace Back.Api.Data.Repositories.Core;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User> Create(UserDto dto, CancellationToken cancellationToken);
    Task<User> GetById(long telegramUserId, CancellationToken cancellationToken);
    Task DeleteAll(CancellationToken cancellationToken);
    Task DeleteById(long telegramUserId, CancellationToken cancellationToken);
}