using AutoMapper;
using Back.Api.Endpoints.v1.Requests.Users;
using Back.Api.Infrastructure.Dto.Users;
using Back.Api.Infrastructure.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Back.Api.Endpoints.v1.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }
    
    [HttpPost]
    public IActionResult Create(CreateUserRequest request)
    {
        var dto = _mapper.Map<UserDto>(request);
        var user = _userService.Create(dto);
        return Ok(user);
    }
}