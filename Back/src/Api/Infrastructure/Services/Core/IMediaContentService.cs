using Back.Api.Infrastructure.Dto.MediaContent;
using Back.Api.Models;

namespace Back.Api.Infrastructure.Services.Core;

public interface IMediaContentService
{
    Task<IEnumerable<MediaContent>> GetAll(CancellationToken cancellationToken);
    Task<MediaContent> Create(MediaContentDto dto, CancellationToken cancellationToken);
    Task<MediaContent> GetMyRandom(long telegramUserId, CancellationToken cancellationToken);
    Task<IEnumerable<MediaContent>> GetMyList(long telegramUserId, CancellationToken cancellationToken);
    Task<MediaContent> GetRandom(CancellationToken cancellationToken);
    
}