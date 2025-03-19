using AutoMapper;
using Back.Api.Endpoints.Requests.MediaContent;
using Back.Api.Endpoints.Requests.Users;
using Back.Api.Endpoints.Responses;
using Back.Api.Infrastructure.Dto.MediaContent;
using Back.Api.Infrastructure.Dto.Users;
using Back.Api.Models;

namespace Back.Api.Infrastructure.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserDto, CreateUserRequest>().ReverseMap();
        CreateMap<MediaContent, MediaContentDto>().ReverseMap();
        CreateMap<MediaContentDto, CreateMediaContentRequest>().ReverseMap();
        CreateMap<MediaContentResponse, MediaContent>().ReverseMap();
        CreateMap<MediaContentResponse, MediaContentDto>().ReverseMap();
    }   
}