using AutoMapper;
using Back.Api.Data.Repositories.Core;
using Back.Api.Infrastructure.Dto.Users;
using Back.Api.Infrastructure.Services.Core;
using Back.Api.Models;

namespace Back.Api.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public IEnumerable<User> GetAll()
    {
        return _userRepository.GetAll();
    }

    public User Create(UserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        _userRepository.Add(user);
        
        return user;
    }
}