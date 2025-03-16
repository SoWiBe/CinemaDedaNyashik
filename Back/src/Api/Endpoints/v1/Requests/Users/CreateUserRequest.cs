using System.ComponentModel.DataAnnotations;

namespace Back.Api.Endpoints.v1.Requests.Users;

public class CreateUserRequest
{
    [Required] public long TelegramUserId { get; set; }
    [Required] [MaxLength(50)] public string? Username { get; set; }
    [Required] public string FirstName { get; set; } = null!;
    [Required] [MaxLength(100)] public string? LastName { get; set; }
    [Required] public DateTime? LastInteraction { get; set; }
}