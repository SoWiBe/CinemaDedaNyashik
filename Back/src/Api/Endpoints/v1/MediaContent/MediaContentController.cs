using AutoMapper;
using Back.Api.Endpoints.v1.Requests.MediaContent;
using Back.Api.Infrastructure.Dto.MediaContent;
using Back.Api.Infrastructure.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Back.Api.Endpoints.v1.MediaContent;

[ApiController]
[Route("api/[controller]")]
public class MediaContentController : ControllerBase
{
    private readonly IMediaContentService _mediaContentService;
    private readonly IMapper _mapper;

    public MediaContentController(IMediaContentService mediaContentService, IMapper mapper)
    {
        _mediaContentService = mediaContentService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var mediaContents = await _mediaContentService.GetAll(cancellationToken);
        return Ok(mediaContents);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateMediaContentRequest request, CancellationToken cancellationToken = default)
    {
        var dto = _mapper.Map<MediaContentDto>(request);
        var mediaContent = await _mediaContentService.Create(dto, cancellationToken);
        return Ok(mediaContent);
    }
}