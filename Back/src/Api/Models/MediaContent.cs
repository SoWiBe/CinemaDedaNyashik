using Back.Api.Endpoints.Responses;
using Back.Api.Models.Core;

namespace Back.Api.Models;

public class MediaContent : BaseModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? Rating { get; set; }
    public string? Genres { get; set; }
    public MediaContentStatus? Status { get; set; }
    
    public long UserId { get; set; }
    public User? User { get; set; }
}