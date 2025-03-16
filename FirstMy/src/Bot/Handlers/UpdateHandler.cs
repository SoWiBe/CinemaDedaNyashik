using FirstMy.Shared.Constants;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FirstMy.src.Bot.Handlers;

public class UpdateHandler : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message) return;
        if (message.Text is null ) return;
        if (message is not { Type: MessageType.Text } ) return;

        switch (message.Text.ToLower())
        {
            case CommandConstants.Info:
                await SendInfoMessage(botClient, message.Chat.Id);
                break;
            default:
                await EchoMessage(botClient, message);
                break;
        }
        
        Console.WriteLine($"Received a {message.Text} in chat {message.Chat.Id}.");
    }
    
    private async Task SendInfoMessage(ITelegramBotClient botClient, long chatId)
    {
        var welcomeText = "Привет! Я твой бот. Доступные команды: /info, /add, /list";
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