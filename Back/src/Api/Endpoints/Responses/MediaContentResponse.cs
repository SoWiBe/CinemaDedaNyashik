namespace Back.Api.Endpoints.Responses;

public class MediaContentResponse
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? Rating { get; set; }
    public string? Genres { get; set; }
    public MediaContentStatus? Status { get; set; }
    
    public long UserId { get; set; }
}

public enum MediaContentStatus
{
    Success, Waiting, Deleted
}