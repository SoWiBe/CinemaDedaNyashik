using System.Text.Json.Serialization;

namespace FirstMy.Bot.Models.MediaContent;

public class MediaContentResponse
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("userId")] public long UserId { get; set; }
    [JsonPropertyName("status")] public MediaContentStatus? Status { get; set; }
}

public enum MediaContentStatus
{
    Success, Waiting, Deleted
}