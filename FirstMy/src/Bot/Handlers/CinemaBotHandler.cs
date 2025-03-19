using FirstMy.Bot.Models;
using FirstMy.Bot.Services.Core;
using FirstMy.Bot.Services.Users;
using FirstMy.Shared.Constants;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace FirstMy.Bot.Handlers;

public class CinemaBotHandler : IUpdateHandler
{
    private readonly IUsersService _usersService;

    public CinemaBotHandler(IUsersService userService)
    {
        _usersService = userService;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message) return;
        if (message.Text is null ) return;

        switch (message.Text.ToLower())
        {
            case CommandConstants.Info:
                await SendInfoMessage(botClient, message.Chat.Id);
                break;
            case CommandConstants.Add:
                await SendMediaContent(botClient, message.Chat.Id);
                break;
            case CommandConstants.Secret:
                await SendSecret(botClient, message);
                break;
            default:
                await EchoMessage(botClient, message);
                break;
        }
        
        Console.WriteLine($"Received a {message.Text} in chat {message.Chat.Id}.");
    }

    private async Task SendSecret(ITelegramBotClient botClient, Message message)
    {
        var user = message.From;

        try
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
        catch (ApiRequestException ex)
        {
            await botClient.SendMessage(message.Chat.Id, ex.Message);
            return;
        }
        
        await botClient.SendMessage(message.Chat.Id, "Привет! Я твой бот. Доступные команды: /info, /add, /list, /secret");
    }

    private async Task SendMediaContent(ITelegramBotClient botClient, long chatId)
    {
        var welcomeText = "ДОБАВИЛ КОНТЕНТ!";
        await botClient.SendMessage(chatId, welcomeText);
    }

    private async Task SendInfoMessage(ITelegramBotClient botClient, long chatId)
    {
        var welcomeText = "Привет! Я твой бот. Доступные команды: /info, /add, /list, /secret";
        //TODO: get user request, if is not exist - create
        await botClient.SendMessage(chatId, welcomeText);
    }
    
    private async Task EchoMessage(ITelegramBotClient botClient, Message message)
    {
        await botClient.SendMessage(
            message.Chat.Id,
            $"Ты сказал(-а): {message.Text}"
        );
    }
    
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}