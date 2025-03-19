using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Back.Api.Endpoints.Requests.Users;
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
    
    [HttpGet("/api/Users/{telegramUserId}")]
    public async Task<IActionResult> GetUser([FromRoute, Required] long telegramUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetById(telegramUserId, cancellationToken);
        return Ok(user);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var users = await _userService.GetAll(cancellationToken);
        return Ok(users);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var dto = _mapper.Map<UserDto>(request);
        var user = await _userService.Create(dto, cancellationToken);
        return Ok(user);
    }
    
    [HttpDelete("/api/Users/all")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAll(CancellationToken cancellationToken = default)
    {
        await _userService.DeleteAll(cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("/api/Users/{telegramUserId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute, Required] long telegramUserId, CancellationToken cancellationToken = default)
    {
        await _userService.DeleteById(telegramUserId, cancellationToken);
        return NoContent();
    }
}