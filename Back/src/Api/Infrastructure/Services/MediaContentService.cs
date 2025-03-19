using Back.Api.Data.Repositories.Core;
using Back.Api.Infrastructure.Dto.MediaContent;
using Back.Api.Infrastructure.Services.Core;
using Back.Api.Models;

namespace Back.Api.Infrastructure.Services;

public class MediaContentService : IMediaContentService
{
    private readonly IMediaContentRepository _mediaContentRepository;

    public MediaContentService(IMediaContentRepository mediaContentRepository)
    {
        _mediaContentRepository = mediaContentRepository;
    }

    public async Task<IEnumerable<MediaContent>> GetAll(CancellationToken cancellationToken)
        => await _mediaContentRepository.GetAllAsync(cancellationToken);

    public async Task<MediaContent> Create(MediaContentDto dto, CancellationToken cancellationToken)
        => await _mediaContentRepository.Create(dto, cancellationToken);
}