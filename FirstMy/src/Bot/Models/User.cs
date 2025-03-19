namespace FirstMy.Bot.Models;

public class UserRequest
{
    public long TelegramUserId { get; set; }
    public string? Username { get; set; } 
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public DateTime? LastInteraction { get; set; }
}

public class UserResponse
{
    public long TelegramUserId { get; set; }
    public string? Username { get; set; } 
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public DateTime? LastInteraction { get; set; }
}