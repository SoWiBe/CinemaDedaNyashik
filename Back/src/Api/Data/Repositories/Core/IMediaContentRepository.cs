using Back.Api.Infrastructure.Dto.MediaContent;
using Back.Api.Infrastructure.Repository;
using Back.Api.Models;

namespace Back.Api.Data.Repositories.Core;

public interface IMediaContentRepository : IRepositoryBase<MediaContent>
{
    Task<MediaContent> Create(MediaContentDto dto, CancellationToken cancellationToken);
    Task<MediaContent> GetMyRandom(long telegramUserId, CancellationToken cancellationToken);
    Task<IEnumerable<MediaContent>> GetMyList(long telegramUserId, CancellationToken cancellationToken);
    Task<MediaContent> GetRandom(CancellationToken cancellationToken);
    Task<bool> DeleteMediaContent(long mediaContentId, CancellationToken cancellationToken);
    Task<bool> DeleteAllMediaContent(long telegramUserId, CancellationToken cancellationToken);
    Task<bool> SetInverseStatusContent(long mediaContentId, CancellationToken cancellationToken);
}