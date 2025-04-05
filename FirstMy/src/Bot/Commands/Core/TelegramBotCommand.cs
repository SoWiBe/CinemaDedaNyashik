using Telegram.Bot;
using Telegram.Bots.Types;

namespace FirstMy.Bot.Commands.Core;

public abstract class TelegramBotCommand
{
    protected string CommandName;

    protected TelegramBotCommand(string commandName)
    {
        CommandName = commandName;
    }

    protected virtual Task CommandLogic(ITelegramBotClient botClient, long userId, long chatId)
    {
        return Task.Delay(1);
    }
    
    protected virtual Task CommandLogic(ITelegramBotClient botClient, Message message)
    {
        return Task.Delay(1);
    }
}