using Back.Api.Endpoints.Responses;

namespace Back.Api.Endpoints.Requests.MediaContent;

public class CreateMediaContentRequest
{
    public string? Title { get; set; } 
    public string? Description { get; set; }
    public decimal? Rating { get; set; }
    public long? UserId { get; set; }

    public MediaContentStatus? Status { get; set; } = MediaContentStatus.Waiting;
}