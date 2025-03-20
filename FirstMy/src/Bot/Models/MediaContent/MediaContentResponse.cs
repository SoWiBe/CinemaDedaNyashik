using System.Text.Json.Serialization;

namespace FirstMy.Bot.Models.MediaContent;

public class MediaContentResponse
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("userId")] public long UserId { get; set; }
}