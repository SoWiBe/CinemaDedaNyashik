namespace FirstMy.Bot.Commands.Core;

public abstract class TelegramBotCommand
{
    protected string CommandName;

    protected TelegramBotCommand(string commandName)
    {
        CommandName = commandName;
    }

    protected abstract Task CommandLogic();
}