namespace FirstMy.Bot.Models.MediaContent;

public class MediaContentRequest
{
    public string? Title { get; set; } 
    public string? Description { get; set; }
    public decimal? Rating { get; set; }
    public long? UserId { get; set; }
}