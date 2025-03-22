using FirstMy.Bot.Models;
using FirstMy.Bot.Models.MediaContent;
using FirstMy.Bot.Services.Core;
using Microsoft.Extensions.Configuration;

namespace FirstMy.Bot.Services.MediaService;

public class MediaContentService : ApiService, IMediaContentService
{
    public MediaContentService(IConfigurationRoot root, HttpClient httpClient) : base(root, httpClient) { }

    public async Task<IEnumerable<MediaContentResponse>?> GetMyList(long userId)
        => await GetAsync<IEnumerable<MediaContentResponse>>($"/api/MediaContent/{userId}/list");

    public async Task<MediaContentResponse?> GetMyRandom(long userId)
        => await GetAsync<MediaContentResponse>($"/api/MediaContent/{userId}/random");

    public async Task<MediaContentResponse?> GetRandom()
        => await GetAsync<MediaContentResponse>($"/api/MediaContent/all/random");

    public async Task<bool> CreateContent(MediaContentRequest? request)
        => await PostAsync("/api/MediaContent", request);

    public Task<bool> ClearMediaContent(long userId)
        => throw new NotImplementedException();

    public Task<bool> RemoveAtMediaContent(long contentId)
        => throw new NotImplementedException();
}