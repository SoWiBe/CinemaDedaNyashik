using FirstMy.Bot.Commands.Core;

namespace FirstMy.Bot.Commands;

public class ClearCommand : TelegramBotCommand
{
    protected override async Task CommandLogic()
    {
        return;
    }

    public ClearCommand(string commandName) : base(commandName){}
}