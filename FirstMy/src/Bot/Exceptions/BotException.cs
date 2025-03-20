namespace FirstMy.Bot.Exceptions;

public class BotException : Exception
{
    public BotException()
    {
    }

    public BotException(string? message) : base(message)
    {
    }
}