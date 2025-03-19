using FirstMy.Bot.Models.MediaContent;
using FirstMy.Bot.Services.Core;
using Microsoft.Extensions.Configuration;

namespace FirstMy.Bot.Services.MediaService;

public class MediaContentService : ApiService, IMediaContentService
{
    public MediaContentService(IConfiguration configuration, HttpClient httpClient) : base(configuration, httpClient)
    {
    }

    public async Task<IEnumerable<MediaContentResponse>> GetMyList(long userId)
    {
        return await GetAsync<IEnumerable<MediaContentResponse>>($"/api/MediaContent/{userId}/list");
    }

    public async Task<MediaContentResponse> GetMyRandom(long userId)
    {
        return await GetAsync<MediaContentResponse>($"/api/MediaContent/{userId}/random");
    }

    public async Task<MediaContentResponse> GetRandom()
    {
        return await GetAsync<MediaContentResponse>($"/api/MediaContent/all/list");
    }

    public async Task<MediaContentResponse> CreateContent(MediaContentRequest request)
    {
        return await PostAsync<MediaContentResponse>($"/api/MediaContent", request);
    }
}