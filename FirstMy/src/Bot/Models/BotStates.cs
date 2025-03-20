namespace FirstMy.Bot.Models;

public enum BotStateType
{
    WaitingForCommand, WaitingForText
}

public class BotState
{
    public long ChatId { get; set; }
    public BotStateType CurrentState { get; set; } = BotStateType.WaitingForCommand;
}