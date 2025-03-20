using Back.Api.Data.Repositories.Core;
using Back.Api.Infrastructure.Dto.Users;
using Back.Api.Infrastructure.Services.Core;
using Back.Api.Models;

namespace Back.Api.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }

    public async Task<User> GetById(long telegramUserId, CancellationToken cancellationToken)
    {
        return await _userRepository.GetById(telegramUserId, cancellationToken);
    }

    public async Task<User> Create(UserDto dto, CancellationToken cancellationToken)
    {
        return await _userRepository.Create(dto, cancellationToken);
    }

    public async Task DeleteAll(CancellationToken cancellationToken)
    {
        await _userRepository.DeleteAll(cancellationToken);
    }

    public async Task DeleteById(long telegramUserId, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteById(telegramUserId, cancellationToken);
    }
}