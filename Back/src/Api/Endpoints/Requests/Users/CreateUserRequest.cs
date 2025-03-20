namespace Back.Api.Endpoints.Requests.Users;

public class CreateUserRequest
{
    public long TelegramUserId { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? LastInteraction { get; set; }
}