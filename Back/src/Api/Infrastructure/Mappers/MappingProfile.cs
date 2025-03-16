using AutoMapper;
using Back.Api.Endpoints.v1.Requests.Users;
using Back.Api.Infrastructure.Dto.Users;
using Back.Api.Models;

namespace Back.Api.Infrastructure.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, CreateUserRequest>();
    }   
}