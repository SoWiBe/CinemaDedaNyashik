using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Back.Api.Endpoints.Requests.MediaContent;
using Back.Api.Endpoints.Responses;
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
    
    [HttpGet("/api/MediaContent/{telegramUserId}/list")]
    public async Task<ActionResult<IEnumerable<MediaContentResponse>>> GetMyList([FromRoute, Required] long telegramUserId, 
        CancellationToken cancellationToken = default)
    {
        var mediaContents = await _mediaContentService.GetMyList(telegramUserId, cancellationToken);
        var result = mediaContents.Select(x => _mapper.Map<MediaContentResponse>(x));
        return Ok(result);
    }
    
    [HttpGet("/api/MediaContent/{telegramUserId}/random")]
    public async Task<ActionResult<MediaContentResponse>> GetMyRandom([FromRoute, Required] long telegramUserId, CancellationToken cancellationToken = default)
    {
        var mediaContent = await _mediaContentService.GetMyRandom(telegramUserId, cancellationToken);
        var response = _mapper.Map<MediaContentResponse>(mediaContent);
        return Ok(response);
    }
    
    [HttpGet("/api/MediaContent/all/random")]
    public async Task<ActionResult<MediaContentResponse>> GetRandom(CancellationToken cancellationToken = default)
    {
        var mediaContent = await _mediaContentService.GetRandom(cancellationToken);
        var response = _mapper.Map<MediaContentResponse>(mediaContent);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMediaContentRequest request, CancellationToken cancellationToken = default)
    {
        var dto = _mapper.Map<MediaContentDto>(request);
        await _mediaContentService.Create(dto, cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("/api/MediaContent/{mediaContentId}")]
    public async Task<IActionResult> DeleteMediaContent(long mediaContentId, CancellationToken cancellationToken = default)
    {
        var isDeleted = await _mediaContentService.DeleteMediaContent(mediaContentId, cancellationToken);
        return isDeleted ? Ok(isDeleted) : StatusCode(500);
    }
    
    [HttpDelete("/api/MediaContent/user/{telegramUserId}")]
    public async Task<IActionResult> DeleteAllMediaContent(long telegramUserId, CancellationToken cancellationToken = default)
    {
        var isDeleted = await _mediaContentService.DeleteAllMediaContent(telegramUserId, cancellationToken);
        return isDeleted ? Ok(isDeleted) : StatusCode(500);
    }
    
    [HttpPatch("/api/MediaContent/status/{mediaContentId}")]
    public async Task<IActionResult> SetInverseStatus(long mediaContentId, CancellationToken cancellationToken = default)
    {
        var isInversed = await _mediaContentService.SetInverseStatusContent(mediaContentId, cancellationToken);
        return isInversed ? Ok(isInversed) : StatusCode(500);
    }
}