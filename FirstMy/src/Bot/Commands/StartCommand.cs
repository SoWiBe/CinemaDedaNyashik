using FirstMy.Bot.Commands.Core;
using FirstMy.Bot.Models.User;
using FirstMy.Bot.Services.Users;
using FirstMy.Shared.Constants.Info;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bots.Types;

namespace FirstMy.Bot.Commands;

public class StartCommand : TelegramBotCommand
{
    private readonly IUsersService _usersService;
    public StartCommand(string commandName, IUsersService usersService) : base(commandName)
    {
        _usersService = usersService;
    }

    protected override async Task CommandLogic(ITelegramBotClient botClient, Message message)
    {
        var user = message.From;
        
        try
        {
            var userResponse = await _usersService.GetUserAsync(user.Id);
            if (userResponse is null)
            {
                await _usersService.CreateUserAsync(new UserRequest
                {
                    Username = user?.Username,
                    FirstName = user?.FirstName,
                    LastName = user.LastName,
                    LastInteraction = DateTime.UtcNow,
                    TelegramUserId = user.Id

                });
            }
            
            await botClient.SendMessage(message.Chat.Id, InfoConstants.StartMessage);
        }
        catch (ApiRequestException ex)
        {
            await botClient.SendMessage(message.Chat.Id, ex.Message);
            Log.Error($"{nameof(StartCommand)} : {ex.Message}");
        }
    }
}