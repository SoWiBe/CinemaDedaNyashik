using FirstMy.Bot.Models.MediaContent;

namespace FirstMy.Bot.Services.MediaService;

public interface IMediaContentService
{
    Task<IEnumerable<MediaContentResponse>?> GetMyList(long userId);
    Task<MediaContentResponse?> GetMyRandom(long userId);
    Task<MediaContentResponse?> GetRandom();
    Task<bool> CreateContent(MediaContentRequest? request);
    Task<bool> ClearMediaContent(long userId);
}