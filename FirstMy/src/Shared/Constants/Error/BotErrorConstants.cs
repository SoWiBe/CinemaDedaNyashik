namespace FirstMy.Shared.Constants.Error;

public static class BotErrorConstants
{
    public const string FailedToDeconstruct 
        = "Failed to deconstruct Telegram update message. Message property is null or malformed.";
    public const string MissingText 
        = "Missing text content in Telegram message. Message.Text property is null.";
    public const string MissingSender 
        = "Missing sender information in Telegram message. Message.From property is null.";
    public const string RequestedAction
        = "The requested action is no longer available";
}