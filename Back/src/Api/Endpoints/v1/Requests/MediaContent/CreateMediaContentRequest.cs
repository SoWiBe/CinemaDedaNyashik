using System.ComponentModel.DataAnnotations;

namespace Back.Api.Endpoints.v1.Requests.MediaContent;

public class CreateMediaContentRequest
{
    [Required] [MaxLength(200)] public string Title { get; set; } = null!;
    [MaxLength(1000)] public string? Description { get; set; }
    public decimal? Rating { get; set; }
}